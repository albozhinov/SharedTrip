namespace SharedTrip.Controllers
{
using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;

    public class TripsController : Controller
    {
        public TripsController(Request request) 
            : base(request)
        {
        }

        public Response All()
            => View();

        public Response Add()
            => View();

        public Response Details()
            => View();
    }
}
