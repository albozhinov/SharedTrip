using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;

namespace SharedTrip.Controllers
{

    public class HomeController : Controller
    {
        public HomeController(Request request)
            : base(request)
        {

        }

        
        public Response Index()
        {
            var isAuthorized = User.IsAuthenticated;

            if (isAuthorized)
            {
                return Redirect("/Trips/All");
            }

            return this.View();
        }
    }
}