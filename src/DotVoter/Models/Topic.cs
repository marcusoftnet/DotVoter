using System;
using System.Collections.Generic;

namespace DotVoter.Models
{
    public class Topic 
    {
        public int Id { get; set; }
        public int WorkshopEventId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        private List<Vote> _votes;

        public List<Vote> Votes
        {
            get
            {
                if (_votes == null)
                    _votes = new List<Vote>();

                return _votes;
            }
            set { _votes = value; }
        }

        public DateTime CreatedDate { get; set; }

        
    }
}