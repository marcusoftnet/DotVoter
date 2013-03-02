using System.Collections.Generic;
using DotVoter.Models;
using DotVoter.Infrastructure;
using StructureMap;

namespace DotVoter
{
    public class EventRepoTest
    {
        public void Test()
        {
            var container = ObjectFactory.Container;

                container.Configure(config => config.For<IIdentityGenerator>().Use<IdentityGenerator>());

            NancyBootstrapper.RegisterMongoMappings(container);


            var repo = new MongoRepository<WorkShopEvent>();

            // adding new entity
            var newEvent= new WorkShopEvent()
                {
                    Name = "MyEvent",
                    Description = "SuperduperEvent",
                    Topics  = new List<Topic>()
                };

            
            // adding a Topic 
            var newTopic= new Topic
                {
                    Name   = "Ny lapp",
                    Votes = new List<Vote>() { new Vote() { UserIdentfier = "Marcus" } }
                };


            newEvent.Topics.Add(newTopic);

            repo.Add(newEvent);

            // searching
            var result = repo.All(c => c.Name == "MyEvent");

            // updating 
            newEvent.Description = "Just as good one!";
            repo.Update(newEvent);
        }
    }
}