using DotVoter.Infrastructure;
using DotVoter.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using MongoRepository;

namespace DotVoter.Modules
{
    public class EventModule : NancyModule
    {
        private readonly WorkshopEventRepository _eventRepository;
        private readonly ICounterGenerator _counterGenerator;

        public EventModule(WorkshopEventRepository eventRepository,ICounterGenerator counterGenerator)
            : base("/event")
        {
            _eventRepository = eventRepository;
            _counterGenerator = counterGenerator;


            Post["/"] = _ =>
                {
                    var e = SaveEvent();
                    return new RedirectResponse("/event/" + e.Id);
                };

            Get["/{id}"] = p => View["event", GetWsEvent(p.Id)];
        }

        private WorkShopEvent GetWsEvent(string id)
        {
            return _eventRepository.GetById(id);
        }

        private WorkShopEvent SaveEvent()
        {
            var wsevent = this.Bind<WorkShopEvent>();
            wsevent.WorshopEventId = _counterGenerator.GenerateId<WorkShopEvent>(wsevent);

            _eventRepository.Add(wsevent);
            return wsevent;
        }
    }    
}