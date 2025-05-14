using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace TPlusModule.Repository.Abstractions.Interfaces
{
    public interface IBaseRepository<TEntity>
            where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Пакетное создание записей
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

        Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default);

        Task Update(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdatePropertyAsync(Func<TEntity, bool> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default);

        Task Delete(TEntity entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entity, bool useBulkUpdate = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Пакетное обновление записей
        /// <returns></returns>
        Task UpdateBatch(TEntity entity, object updateValues, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Пакетное обновление записей
        /// <returns></returns>
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entity, bool useBulkUpdate = true, CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();
    }
}
