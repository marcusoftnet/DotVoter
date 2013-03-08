using System;
using System.Collections.Generic;
using DotVoter.Models;
using DotVoter.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using StructureMap;
using Xunit;
using System.Linq;

namespace DotVoter
{
    //public class EventRepoTest : IDisposable
    //{
    //    private readonly MongoRepository<WorkShopEvent> _repo;

    //    public EventRepoTest()
    //    {
    //        var container = ObjectFactory.Container;

    //        container.Configure(config => config.For<IIdentityGenerator>().Use<IdentityGenerator>());

    //        NancyBootstrapper.RegisterMongoMappings(container);

    //        _repo = new MongoRepository<WorkShopEvent>();
    //    }

    //    [Fact]
    //    public void Test()
    //    {
    //        var userIdentfier = "Marcus";
    //        // adding new entity
    //        var newEvent = new WorkShopEvent()
    //            {
    //                Name = "MyEvent",
    //                Description = "SuperduperEvent",
    //                Topics = new List<Topic>()
    //            };


    //        // adding a Topic 
    //        var newTopic = new Topic
    //            {
    //                Id = 16,
    //                Name = "Ny lapp",
    //                Votes = new List<Vote>() {new Vote() {UserIdentfier = userIdentfier}}
    //            };


    //        newEvent.Topics.Add(newTopic);

    //        _repo.Add(newEvent);

    //        // searching
    //        var result = _repo.All(c => c.Name == "MyEvent").First();

    //        // updating 
    //        newEvent.Description = "Just as good one!";
    //        _repo.Update(newEvent);
    //        RemoveVoteFromTopic(result.Id, result.Topics.First().Id, userIdentfier);
    //    }

    //    protected void RemoveVoteFromTopic(int eventId, int topicId, string uniqueIdentifier)
    //    {
    //        var vote = new Vote {UserIdentfier = uniqueIdentifier};

    //        var query = Query.And(Query.EQ("_id", eventId), Query.EQ("Topics._id", topicId));

    //        var update = Update.PullWrapped("Topics.$.Votes", vote.ToBsonDocument());

    //        _repo.Collection.Update(query, update);
    //    }

    //    public void Dispose()
    //    {
    //        Console.WriteLine("We are currently disposing.");
    //    }
    //}
}