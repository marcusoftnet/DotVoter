using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DotVoter.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace DotVoter.Infrastructure
{
    public interface IMongoRepository<T> where T :class
    {
        T Add(T entity);
        T GetById(int id);
        T GetById(string id);
        void Add(IEnumerable<T> entities);
        T Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(string id);
        IQueryable<T> All(Expression<Func<T, bool>> criteria);
        IQueryable<T> All();
    }

    public class MongoRepository<T> :IMongoRepository<T> where T : class
    {
        private readonly MongoCollection<T> _collection;

        public MongoRepository()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["MONGOHQ_URL"].ConnectionString;
            var connectionString = ConfigurationManager.AppSettings.Get("MONGOHQ_URL");

            var client = new MongoClient(connectionString); // connect to localhost
            var server = client.GetServer();

            var con = new MongoUrlBuilder(connectionString);
            var db = server.GetDatabase(con.DatabaseName);
            _collection = db.GetCollection<T>(typeof(T).Name.ToLower());
        }
        public MongoCollection<T> Collection
        {
            get
            {
                return _collection;
            }
        }

        
        public T GetById(int id)
        {
            return Collection.FindOneById(id);
        }

        public T GetById(string id)
        {
            return Collection.FindOneById(id);
        }

        public T Add(T entity)
        {
            _collection.Insert<T>(entity);

            return entity;
        }

        public void Add(IEnumerable<T> entities)
        {
            _collection.InsertBatch<T>(entities);
        }
        
        public T Update(T entity)
        {
            _collection.Save<T>(entity);

            return entity;
        }
        
        public void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _collection.Save<T>(entity);
            }
        }
      
        public void Delete(string id)
        {    
                _collection.Remove(Query.EQ("_id", id));
        }
        public IQueryable<T> All(Expression<Func<T, bool>> criteria)
        {
            return _collection.AsQueryable<T>().Where(criteria);
        }

        public IQueryable<T> All()
        {
            return _collection.AsQueryable<T>();
        }
    }


}