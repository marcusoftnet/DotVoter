using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotVoter.Models;

namespace DotVoter.ViewModels
{
    public class WorkshopEventViewModel : IWorkShopEvent
    {
        public string CurrentUserIdentifier { get; set; }
        public DisplayModes DisplayMode { get; set; }
        public string CSSVoting { get; set; }
        public string CSSResult { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxVotes { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Topic> Topics { get; set; }
    }

    public enum DisplayModes
    {
        Voting = 0,
        ShowResult =1
    }
}