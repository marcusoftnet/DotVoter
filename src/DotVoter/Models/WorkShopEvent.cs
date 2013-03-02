using System.Collections.Generic;
using MongoRepository;

namespace DotVoter.Models
{

    public class WorkShopEvent : Entity
    {
        public int WorshopEventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxVotes { get; set; }


        private List<Topic> _topics;

        public List<Topic> Topics
        {
            get
            {
                if (_topics == null)
                    _topics = new List<Topic>();

                return _topics;
            }
            set { _topics = value; }
        }

    }
}