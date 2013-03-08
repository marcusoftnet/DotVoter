using System;
using System.Collections.Generic;

namespace DotVoter.Models
{
    public interface IWorkShopEvent
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int MaxVotes { get; set; }
        DateTime CreatedDate { get; set; }
        List<Topic> Topics { get; set; }
    }
}