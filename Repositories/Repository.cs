using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;
using ms_usuario.Interface;
using System.Data.Common;
using System.Linq.Expressions;

namespace ms_usuario.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly IDbContext _context;

        public Repository(UsuarioDbContext context)
        {
            _context = context;
        }

        protected IQueryable<T> Query(params Expression<Func<T, object>>[] joins)
        {
            var query = _context
                .Set<T>()
                .AsQueryable();
            return joins == null ? query : joins.Aggregate(query, (current, include) => current.Include(include));
        }

        public virtual async Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins).ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins)
                .Where(lambda)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins) => await Query(joins).FirstOrDefaultAsync(lambda, cancellationToken);

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins) => await Query(joins).SingleOrDefaultAsync(lambda, cancellationToken);

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken) => await Query().AnyAsync(lambda, cancellationToken);


        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context
                .Set<T>()
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual async Task AddCollectionAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _context
                .Set<T>()
                .AddRangeAsync(entities, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _context
                .Set<T>()
                .Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateCollectionAsync(IEnumerable<T> entities)
        {
            _context
                .Set<T>()
                .UpdateRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(T entity)
        {
            _context
                .Set<T>()
                .Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public DbConnection Connection => _context.Connection;
    }
}
