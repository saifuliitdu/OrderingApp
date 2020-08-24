using MongoDB.Bson;
using MongoDB.Driver;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceStack;

namespace OrderingApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {

        private OrderingAppDbContext _dbContext;
        public IMongoCollection<T> Collection { get; private set; }
        public GenericRepository(OrderingAppDbContext dbContext)
        {
            this._dbContext = dbContext;
            Collection = _dbContext.DbSet<T>();
        }

        public async Task<T> Find(object id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                return null;
            }
            var filterId = Builders<T>.Filter.Eq("_id", objectId);
            var model = await Collection.Find(filterId).FirstOrDefaultAsync();
            return model;
        }

        public async Task<bool> Update(T model)
        {
            var filterId = Builders<T>.Filter.Eq("_id", model.GetId());
            var updated = await Collection.ReplaceOneAsync(filterId, model);
            return updated != null && updated.ModifiedCount > 0;
        }

        public async Task<T> Create(T model)
        {
            try
            {
                await Collection.InsertOneAsync(model);
                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> Delete(object id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                return false;
            }
            var filterId = Builders<T>.Filter.Eq("_id", objectId);
            var deleted = await Collection.DeleteOneAsync(filterId);
            return deleted != null && deleted.DeletedCount > 0;
        }

        //public IEnumerable<T> Query()
        //{
        //    return Collection.Find(FilterDefinition<T>.Empty).ToList();
        //}
        //public IEnumerable<T> Query(Expression<Func<T, bool>> filter)
        //{
        //    return Collection.Find(filter).ToList();
        //}
        //public IEnumerable<T> Get()
        //{
        //    return Collection.Find(x => true).ToList();
        //}
        //public T Get(string id)
        //{
        //    return Collection.Find<T>(x => x.Id == id).FirstOrDefault();
        //}
        //public bool Delete(T model)
        //{
        //    var filterId = Builders<T>.Filter.Eq("_id", model.Id);
        //    var deleted = Collection.DeleteOne(filterId);
        //    return deleted != null;
        //}

        public async Task<IEnumerable<T>> FindAll()
        {
            var all = await Collection.FindAsync(Builders<T>.Filter.Empty);
            return await all.ToListAsync();
        }
    }
}
