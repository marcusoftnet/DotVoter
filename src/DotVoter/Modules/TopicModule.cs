using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotVoter.Infrastructure;
using DotVoter.Models;
using MongoDB.Bson;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DotVoter.Modules
{
    public class TopicModule : NancyModule
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
                    Vote(p);
                    return new RedirectResponse("/event/" + p.Id);
                };

            Get["/{topicid}/unvote"] = p =>
            {
               return UnVote(p);
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
            var uniqueId = this.Request.Cookies.FirstOrDefault(k => k.Key == "NCSRF").Value;
            var wsEvent = GetWorkShopEventById((int)p["id"]);
  
            var voteToRemove =   wsEvent.Topics.FirstOrDefault(t => t.Id == topicId).Votes.FirstOrDefault(c=>c.UserIdentfier == uniqueId);
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
            var uniqueId = this.Request.Cookies.FirstOrDefault(k=>k.Key =="NCSRF" ).Value;
            var wsEvent = GetWorkShopEventById((int) p["id"]);
            var vote = new Vote();

            vote.Id = _identityGenerator.GenerateId<Vote>(vote);
            vote.UserIdentfier = uniqueId;
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
    }
}