using System.Linq;
using Nancy;

namespace DotVoter.Modules
{
    public abstract class DotVoterModule :NancyModule
    {
        
        public string CurrentUserIdentifier
        {
            get { return this.Request.Cookies.FirstOrDefault(k => k.Key == "NCSRF").Value; }
        }

        
        protected DotVoterModule(string modulePath):base(modulePath)
        {
        
        }

    }
}