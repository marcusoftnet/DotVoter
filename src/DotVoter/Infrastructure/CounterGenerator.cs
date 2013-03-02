using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;

namespace DotVoter.Infrastructure
{
    public class CounterGenerator : ICounterGenerator
    {
        public int GenerateId<T>(T entity)
        {
            var repo = new MongoRepository<Counter>();


            var query = Query.EQ("_id", entity.GetType().Name);

            return repo.Collection
                .FindAndModify(query, null, Update.Inc("seq", 1), true, true)
                .ModifiedDocument["seq"]
                .AsInt32;


        }
    }

    public interface ICounterGenerator
    {
        int GenerateId<T>(T entity);
    }
}