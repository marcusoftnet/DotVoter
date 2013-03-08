using System;
using DotVoter.Infrastructure;
using DotVoter.Models;
using DotVoter.ViewModels;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DotVoter.Modules
{
    public class EventModule : DotVoterModule
    {

        public const string CSSActive = "active";
        private readonly WorkshopEventRepository _eventRepository;
        private readonly IIdentityGenerator _identityGenerator;

        public EventModule(WorkshopEventRepository eventRepository, IIdentityGenerator identityGenerator)
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

            Get[@"/{id}/showresult"] = p => View["event", GetWsEventWithTopicsSortedByNumberOfVotes(p.Id)];
        }

        private WorkshopEventViewModel GetWsEvent(int id)
        {
            return CastToEventViewModel( DisplayModes.Voting,   _eventRepository.GetById(id));
        }

        private WorkshopEventViewModel SaveEvent()
        {
            var wsevent = this.Bind<WorkShopEvent>();
            wsevent.CreatedDate = DateTime.Now;
            _eventRepository.Add(wsevent);

            return CastToEventViewModel(DisplayModes.Voting,  wsevent);
        }


        private WorkshopEventViewModel GetWsEventWithTopicsSortedByNumberOfVotes(int id)
        {
            var wsEvent = GetWsEvent(id);

            wsEvent.Topics.SortDescending(t => t.Votes.Count);
            return CastToEventViewModel(DisplayModes.ShowResult, wsEvent);
        }



        private WorkshopEventViewModel CastToEventViewModel(DisplayModes mode, IWorkShopEvent source)
        {

            var destination = AutoMapper.Mapper.DynamicMap<WorkshopEventViewModel>(source);
            
            destination.CurrentUserIdentifier = CurrentUserIdentifier;
            destination.DisplayMode = mode;

            if (destination.DisplayMode == DisplayModes.Voting)
            {
                destination.CSSVoting = CSSActive;
                destination.CSSResult = "";
            }
            else
            {
                destination.CSSResult = CSSActive;
                destination.CSSVoting = "";
            }
            
            return destination;
        }

    
    }
}