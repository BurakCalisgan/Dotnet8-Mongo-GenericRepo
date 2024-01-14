using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmsManager.Domain.Documents.Common;
using SmsManager.Domain.Repository.Common;
using SmsManager.Infrastructure.Settings;

namespace SmsManager.Infrastructure.Repository.Common;

public class BaseRepository<T> : IBaseRepository<T, string> where T : Document, new()
{
    private readonly IMongoCollection<T> _collection;
    private readonly MongoDbSettings _settings;

    protected BaseRepository(IOptions<MongoDbSettings> options)
    {
        _settings = options.Value;
        var client = new MongoClient(_settings.ConnectionString);
        var db = client.GetDatabase(_settings.Database);
        this._collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null
            ? _collection.AsQueryable()
            : _collection.AsQueryable().Where(predicate);
    }

    public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public virtual Task<T> GetByIdAsync(string id)
    {
        return _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T document)
    {
        var options = new InsertOneOptions { BypassDocumentValidation = false };
        await _collection.InsertOneAsync(document, options);
        return document;
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<T> documents)
    {
        var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
        return (await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)documents, options)).IsAcknowledged;
    }

    public virtual async Task<T> UpdateAsync(string id, T document)
    {
        return await _collection.FindOneAndReplaceAsync(x => x.Id == id, document);
    }

    public virtual async Task<T> UpdateAsync(T document, Expression<Func<T, bool>> predicate)
    {
        return await _collection.FindOneAndReplaceAsync(predicate, document);
    }

    public virtual async Task<T> DeleteAsync(T document)
    {
        return await _collection.FindOneAndDeleteAsync(x => x.Id == document.Id);
    }

    public virtual async Task<T> DeleteAsync(string id)
    {
        return await _collection.FindOneAndDeleteAsync(x => x.Id == id);
    }

    public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.FindOneAndDeleteAsync(predicate);
    }
}