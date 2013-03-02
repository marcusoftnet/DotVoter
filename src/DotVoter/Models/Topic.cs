using System.Collections.Generic;
using MongoRepository;

namespace DotVoter.Models
{
    public class Topic 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<Vote> Votes { get; set; }
        
    }
}