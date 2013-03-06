using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DotVoter.Infrastructure
{
    public class IdentityGenerator : IIdentityGenerator , IIdGenerator
    {
        public int GenerateId<T>(T entity)
        {
            var repo = new MongoRepository<Counter>();

            var query = Query.EQ("_id", entity.GetType().Name);

            return (int) GenerateId(repo.Collection, query);
        }

        public object GenerateId(object container, object document)
        {
            var idSequenceCollection = ((MongoCollection)container).Database.GetCollection("IDSequence");

            var query = Query.EQ("_id", ((MongoCollection)container).Name);

            return GenerateId(idSequenceCollection, query);
        }

        private static int GenerateId(MongoCollection<BsonDocument> idSequenceCollection, IMongoQuery query)
        {
            return idSequenceCollection
                .FindAndModify(query, null, Update.Inc("seq", 1), true, true)
                .ModifiedDocument["seq"]
                .AsInt32;
        }

        public bool IsEmpty(object id)
        {
            return (int)id == 0;
        }
    }

    public interface IIdentityGenerator
    {
        int GenerateId<T>(T entity);
        object GenerateId(object container, object document);
        bool IsEmpty(object id);
    }
}