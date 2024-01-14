using System.Linq.Expressions;
using SmsManager.Domain.Documents.Common.Interface;

namespace SmsManager.Domain.Repository.Common;

public interface IBaseRepository<T, in TKey> where T : class, IDocument, new() where TKey : IEquatable<TKey>
{
    IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(TKey id);
    Task<T> AddAsync(T document);
    Task<bool> AddRangeAsync(IEnumerable<T> documents);
    Task<T> UpdateAsync(TKey id, T document);
    Task<T> UpdateAsync(T document, Expression<Func<T, bool>> predicate);
    Task<T> DeleteAsync(T document);
    Task<T> DeleteAsync(TKey id);
    Task<T> DeleteAsync(Expression<Func<T, bool>> predicate);
}