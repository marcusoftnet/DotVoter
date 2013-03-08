using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotVoter.Infrastructure;
using DotVoter.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DotVoter.Modules
{
    public class TopicModule : DotVoterModule
    {
        private readonly WorkshopEventRepository _eventRepository;
        private readonly IIdentityGenerator _identityGenerator;

        public TopicModule(WorkshopEventRepository eventRepository, IIdentityGenerator identityGenerator)
            : base("/event/{id}/topic")
        {
            _eventRepository = eventRepository;
            _identityGenerator = identityGenerator;
            Post["/"] = p =>
                {
                    SaveTopic(p.Id);
                    return new RedirectResponse("/event/" + p.Id);
                };
            
            Post["/{topicid}/delete"] = p =>
            {
                DeletTopic(p);
                return new RedirectResponse("/event/" + p.Id);
            };

            Post["/{topicid}/vote"] = p =>
                {
                    AddVoteToTopic(p);
                    return new RedirectResponse("/event/" + p.Id);
                };

            Get["/{topicid}/unvote/{voteid}"] = p =>
            {
               RemoveVoteFromTopic(p);
               return new RedirectResponse("/event/" + p.Id);
            };
        }

        private void DeletTopic(dynamic p)
        {
            var topicId = p["topicid"];
            var wsEvent = GetWorkShopEventById((int)p["id"]);

            var topicToRemove = wsEvent.Topics.FirstOrDefault(t => t.Id == topicId);
            if (topicToRemove != null)
            {
                var topic = wsEvent.Topics.FirstOrDefault(t => t.Id == topicId);
                if (topic != null)
                    wsEvent.Topics.Remove(topicToRemove);
            }
            Update(wsEvent);
        }

        private RedirectResponse UnVote(dynamic p)
        {
            var topicId = p["topicid"];
            var wsEvent = GetWorkShopEventById((int)p["id"]);

  
            var voteToRemove =   wsEvent.Topics.FirstOrDefault(t => t.Id == topicId).Votes.FirstOrDefault(c=>c.UserIdentfier == CurrentUserIdentifier);
            if (voteToRemove == null)
                return new RedirectResponse(this.Request.Headers.Referrer);
         
            var topic = wsEvent.Topics.FirstOrDefault(t => t.Id == topicId);
             if (topic == null)
                 return new RedirectResponse(this.Request.Headers.Referrer);
 
            topic.Votes.Remove(voteToRemove);
            
            Update(wsEvent);

            return new RedirectResponse("/event/" + p.Id);
  
        }

        private void Vote(dynamic p)
        {
            var topicId = p["topicid"];
            var wsEvent = GetWorkShopEventById((int) p["id"]);
            var vote = new Vote();

            vote.Id = _identityGenerator.GenerateId<Vote>(vote);
            vote.UserIdentfier = CurrentUserIdentifier;
            wsEvent.Topics.FirstOrDefault(t=>t.Id == topicId).Votes.Add(vote);
            Update(wsEvent);
        }

        private void SaveTopic(int id)
        {
            var wsEvent = GetWorkShopEventById(id);
            var topic = this.Bind<Topic>();
            
            topic.Id = _identityGenerator.GenerateId<Topic>(topic);
            topic.WorkshopEventId = id;
            topic.CreatedDate = DateTime.Now;

            wsEvent.Topics.Add(topic);
            Update(wsEvent);
        }

        private void Update(WorkShopEvent wsEvent)
        {
            _eventRepository.Update(wsEvent);
        }

        private WorkShopEvent GetWorkShopEventById(int id)
        {
            return _eventRepository.GetById(id);
        }

        protected void RemoveVoteFromTopic(dynamic p)
        {

            var topicId = (int) p["topicid"];
            var wsEventId = (int)p["id"];
            var vote = new Vote(){Id = (int)p["voteId"],UserIdentfier = CurrentUserIdentifier};

            var query = Query.And(Query.EQ("_id", wsEventId), Query.EQ("Topics._id", topicId));

            var update = MongoDB.Driver.Builders.Update.PullWrapped("Topics.$.Votes", vote);

            _eventRepository.Collection.Update(query, update);
        }


        protected void AddVoteToTopic(dynamic p)
        {
            var topicId = (int) p["topicid"];
            var wsEventId = (int)p["id"];
            var vote = new Vote();
            vote.Id = _identityGenerator.GenerateId<Vote>(vote);
            vote.UserIdentfier = CurrentUserIdentifier;
           
            var query = Query.And(Query.EQ("_id", wsEventId), Query.EQ("Topics._id", topicId));

            var update = MongoDB.Driver.Builders.Update.PushAllWrapped("Topics.$.Votes", vote);

            _eventRepository.Collection.Update(query, update);
        }


        private void UpdateChildDocument(Topic topic)
        {
            /*
            //var vote = new Vote();
            //var query = Query.EQ("Topics._id", topic.Id); 
            //Update.Push()
            //var update = Update.Set("Topics.$.Votes", vote); 
            //collection.Update(query, update); 

//You would probably want to do the update based on the student Id 
//instead of Name, in case there was more than one Mark. 
        
                        // add it as a new attribute because it didn't exist
            query = Query.EQ("_id", id);
            update = Update.PushWrapped("bAttributeArray", new BAttribute { attribute = attribute, attributeValue = increment });
            collection.Update(query, update);

            
            */

        }

    }
}