using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotVoter.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DotVoter.Modules
{
    public class TopicModule : NancyModule
    {
        private readonly WorkshopEventRepository _eventRepository;

        public TopicModule(WorkshopEventRepository eventRepository)
            : base("/event/{id}/topic")
        {
            _eventRepository = eventRepository;
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
            wsEvent.Topics.Add(topic);
            _eventRepository.Update(wsEvent);

        }
    }
}