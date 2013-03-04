using System;
using DotVoter.Infrastructure;
using DotVoter.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DotVoter.Modules
{
    public class EventModule : NancyModule
    {
        private readonly WorkshopEventRepository _eventRepository;
        private readonly IIdentityGenerator _identityGenerator;

        public EventModule(WorkshopEventRepository eventRepository,IIdentityGenerator identityGenerator)
            : base("/event")
        {
            _eventRepository = eventRepository;
            _identityGenerator = identityGenerator;


            Post["/"] = _ =>
                {
                    var e = SaveEvent();
                    return new RedirectResponse("/event/" + e.Id);
                };
            Get[@"/(?<id>[\d]+)"] = p => View["event", GetWsEvent(p.Id)];
            
            Get[@"/{id}/sorted"] = p => View["event", GetWsEventWithTopicsSortedByNumberOfVotes(p.Id)];
        }

        private WorkShopEvent GetWsEvent(int id)
        {
            return _eventRepository.GetById(id);
        }

        private WorkShopEvent SaveEvent()
        {
            var wsevent = this.Bind<WorkShopEvent>();
            wsevent.CreatedDate = DateTime.Now;
            _eventRepository.Add(wsevent);
            return wsevent;
        }


        private WorkShopEvent GetWsEventWithTopicsSortedByNumberOfVotes(int id)
        {
            var wsEvent = GetWsEvent(id);
       
            wsEvent.Topics.SortDescending(t => t.Votes.Count);
            return wsEvent;
        }
    }
}