using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class QueueSynchronizationContext : SynchronizationContext, IDisposable
    {
        struct PostData
        {
            public SendOrPostCallback d;
            public object state;
        }

        private BlockingCollection<PostData> queue = new BlockingCollection<PostData>(new ConcurrentQueue<PostData>());
        private readonly SynchronizationContext parent = SynchronizationContext.Current;

        public QueueSynchronizationContext()
        {
            SynchronizationContext.SetSynchronizationContext(this);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            var q = queue;
            try
            {
                if (q != null)
                    q.Add(new PostData { d = d, state = state });
            }
            catch (InvalidOperationException)
            {
                // Мы можем сюда попасть после вызова CompleteAdding, если он произошел только что (гонка) или если измененная queue еще не видна в текущем потоке
                // ObjectDisposedException попадает сюда же
                q = null;
            }
            if (q == null) PostToParent(d, state);
        }

        private void PostToParent(SendOrPostCallback d, object state)
        {
            if (parent == null)
                d.BeginInvoke(state, d.EndInvoke, null);
            else
                parent.Post(d, state);
        }

        public void Dispose()
        {
            using (var queue_local = Interlocked.Exchange(ref queue, null))
                if (queue_local != null)
                {
                    queue_local.CompleteAdding();

                    foreach (var data in queue_local.GetConsumingEnumerable())
                        PostToParent(data.d, data.state);
                }

            SynchronizationContext.SetSynchronizationContext(parent);
        }

        public void RunLoop(CancellationToken token)
        {
            var queue_local = queue;
            if (queue_local == null) throw new ObjectDisposedException("QueueSynchronizationContext");

            try
            {
                foreach (var data in queue_local.GetConsumingEnumerable(token))
                    data.d(data.state);
            }
            catch (OperationCanceledException)
            {
                // Это нормальный выход из queue_local.GetConsumingEnumerable(token) при отмене токена
                return;
            }

            // А если мы добрались сюда - значит, кто-то нас уже закрыл
            throw new ObjectDisposedException("QueueSynchronizationContext");
        }

        public void WaitFor(Task task, int timeout = Timeout.Infinite)
        {
            using (var source = new CancellationTokenSource(timeout))
            {
                task.ContinueWith(_ => source.Cancel(), TaskContinuationOptions.ExecuteSynchronously);
                RunLoop(source.Token);
            }
        }

        public void WaitFor(Task task, TimeSpan timeout)
        {
            using (var source = new CancellationTokenSource(timeout))
            {
                task.ContinueWith(_ => source.Cancel(), TaskContinuationOptions.ExecuteSynchronously);
                RunLoop(source.Token);
            }
        }

        public void WaitFor(Task task, CancellationToken token)
        {
            using (var source = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                task.ContinueWith(_ => source.Cancel(), TaskContinuationOptions.ExecuteSynchronously);
                RunLoop(source.Token);
            }
        }
    }
}
