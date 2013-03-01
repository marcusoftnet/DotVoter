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

        public EventModule(WorkshopEventRepository eventRepository)
            : base("/event")
        {
            _eventRepository = eventRepository;
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
            _eventRepository.Add(wsevent);
            return wsevent;
        }
    }

    public class WorkshopEventRepository : MongoRepository<WorkShopEvent> { }
}