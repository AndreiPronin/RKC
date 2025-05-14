using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using TPlusModule.Repository.Abstractions.Interfaces;
using EFCore.BulkExtensions;

namespace TPlusModule.Repository.Abstractions
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> where TEntity : class where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly TContext _context;

        public BaseRepository(TContext context)
        {
            _context = context;
        }

        public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().AsNoTracking();
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
        {
            //await _context.Set<TEntity>().AddRangeAsync(entity, cancellationToken);
            await _context.BulkInsertAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entity, bool useBulkUpdate = true, CancellationToken cancellationToken = default)
        {
            if (useBulkUpdate)
                await _context.BulkDeleteAsync(entity, cancellationToken: cancellationToken);
            else
            {
                _context.RemoveRange(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entity, bool useBulkUpdate = true, CancellationToken cancellationToken = default)
        {
            if (useBulkUpdate)
                await _context.BulkUpdateAsync(entity, cancellationToken: cancellationToken);
            else
            {
                _context.UpdateRange(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task UpdatePropertyAsync(Func<TEntity, bool> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Where(predicate).AsQueryable().ExecuteUpdate(setPropertyCalls);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBatch(TEntity entity, object updateValues, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            await _context.Set<TEntity>().Where<TEntity>(predicate).BatchUpdateAsync(updateValues, null, cancellationToken);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
