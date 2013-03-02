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
        private readonly ICounterGenerator _counterGenerator;

        public TopicModule(WorkshopEventRepository eventRepository, ICounterGenerator counterGenerator)
            : base("/event/{id}/topic")
        {
            _eventRepository = eventRepository;
            _counterGenerator = counterGenerator;
            Post["/"] = p =>
                {
                    SaveTopic(p.Id);
                    return new RedirectResponse("/event/" + p.Id);
                };
        }

        private void SaveTopic(string id)
        {
            var wsEvent = _eventRepository.GetById(id);
            var topic = this.Bind<Topic>();
            topic.Id = ObjectId.GenerateNewId().ToString();
            topic.TopicId = _counterGenerator.GenerateId<Topic>(topic);

            wsEvent.Topics.Add(topic);

            _eventRepository.Update(wsEvent);

        }
    }
}