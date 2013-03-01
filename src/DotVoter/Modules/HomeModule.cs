namespace DotVoter.Modules
{
    public class HomeModule :Nancy.NancyModule
    {

        public HomeModule()
        {
            Get["/"] = _ => { return View["index"]; };
        }
    }
}