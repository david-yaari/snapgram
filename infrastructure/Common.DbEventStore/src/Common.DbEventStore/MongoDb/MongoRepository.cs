using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Common.DbEventStore.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        private readonly ILogger<MongoRepository<T>> _logger;

        public MongoRepository(
            IMongoDatabase database,
            string collectionName,
            ILogger<MongoRepository<T>> logger
        )
        {
            _collection = database.GetCollection<T>(collectionName);
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Guid tenantId)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.tenantId, tenantId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Guid tenantId, Expression<Func<T, bool>> filter)
        {
            var tenantFilter = Builders<T>.Filter.Eq(e => e.tenantId, tenantId);
            var combinedFilter = Builders<T>.Filter.And(tenantFilter, filter);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid tenantId, Guid id)
        {
            var tenantFilter = Builders<T>.Filter.Eq(e => e.tenantId, tenantId);
            var idFilter = Builders<T>.Filter.Eq(e => e.id, id);
            var combinedFilter = Builders<T>.Filter.And(tenantFilter, idFilter);

            return await _collection.Find(combinedFilter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Guid tenantId, Expression<Func<T, bool>> filter)
        {
            var tenantFilter = Builders<T>.Filter.Eq(e => e.tenantId, tenantId);
            var combinedFilter = Builders<T>.Filter.And(tenantFilter, filter);

            return await _collection.Find(combinedFilter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _collection.InsertOneAsync(entity);
        }

        public async Task CreateManyAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _collection.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.id, entity.id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid tenantId, Guid id)
        {
            if (id == Guid.Empty || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var tenantFilter = Builders<T>.Filter.Eq(entity => entity.tenantId, tenantId);
            var idFilter = Builders<T>.Filter.Eq(entity => entity.id, id);
            var combinedFilter = Builders<T>.Filter.And(tenantFilter, idFilter);

            await _collection.DeleteOneAsync(combinedFilter);
        }
    }

}
