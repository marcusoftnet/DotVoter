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
            var topicId = (int)p["topicid"];
            var wsEventId = (int)p["id"];
        
            _eventRepository.Collection.Update(Query.EQ("_id", wsEventId),
                  Update.PullWrapped("Topics", Query.EQ("_id", topicId)));
        }

        
        private void SaveTopic(int id)
        {
            var topic = this.Bind<Topic>();
            
            topic.Id = _identityGenerator.GenerateId<Topic>(topic);
            topic.WorkshopEventId = id;
            topic.CreatedDate = DateTime.Now;
            var query = Query.EQ("_id", id);

            var update = Update.PushWrapped("Topics", topic);

            _eventRepository.Collection.Update(query, update);
        }

        protected void RemoveVoteFromTopic(dynamic p)
        {

            var topicId = (int) p["topicid"];
            var wsEventId = (int)p["id"];
            var vote = new Vote(){Id = (int)p["voteId"],UserIdentfier = CurrentUserIdentifier};

            var query = Query.And(Query.EQ("_id", wsEventId), Query.EQ("Topics._id", topicId));

            var update = Update.PullWrapped("Topics.$.Votes", vote);

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

            var update = Update.PushAllWrapped("Topics.$.Votes", vote);

            _eventRepository.Collection.Update(query, update);
        }

    }
}