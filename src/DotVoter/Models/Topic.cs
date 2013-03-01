using System.Collections.Generic;
using MongoRepository;

namespace DotVoter.Models
{
    public class Topic : Entity
    {
        public string Name { get; set; }
        public List<Vote> Votes { get; set; }
        public string Category { get; set; }
    }
}