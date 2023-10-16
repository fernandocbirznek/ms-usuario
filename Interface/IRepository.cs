using ms_usuario.Domains;
using System.Data.Common;
using System.Linq.Expressions;

namespace ms_usuario.Interface
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task AddCollectionAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task UpdateAsync(T entity);
        Task UpdateCollectionAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        DbConnection Connection { get; }
    }
}
