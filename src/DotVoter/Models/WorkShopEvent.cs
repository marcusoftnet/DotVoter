using System;
using System.Collections.Generic;

namespace DotVoter.Models
{

    public class WorkShopEvent : IWorkShopEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxVotes { get; set; }
        public DateTime CreatedDate { get; set; }


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