namespace SharedTrip.Controllers
{
    using BasicWebServer.Server.Attributes;
    using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;
    using SharedTrip.Contracts;
    using SharedTrip.ViewModels;
    using System;
    using System.Collections.Generic;

    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(Request request, ITripService _tripService) 
            : base(request)
        {
            tripService = _tripService;
        }

        /// <summary>
        /// All validations can separated in class to we have high quality code
        /// </summary>
        /// <returns></returns>
        /// 
        public Response All() 
        {
            var isAuthorized = User.IsAuthenticated;

            if (!isAuthorized)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Please login with username and password or register to have permission to this page.") }, "/Error");
            }

            IEnumerable<TripListViewModel> trips = tripService.GetAllTrips();

            return View(trips);
        }

        [HttpGet]
        public Response Add()
        {
            var isAuthorized = User.IsAuthenticated;

            if (!isAuthorized)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Please login with username and password or register to have permission to this page.") }, "/Error");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public Response Add(TripViewModel model)
        {
            var (isValid, errors) = tripService.ValidateModel(model);

            if (!isValid)
            {
                return View(errors, "/Error");
            }

            try
            {
                tripService.AddTrip(model);
            }
            catch (Exception)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Unexpected Error") }, "/Error");
            }

            return Redirect("/Trips/All");
        }


        [HttpGet]
        public Response Details(string tripId)
        {
            var isAuthorized = User.IsAuthenticated;

            if (!isAuthorized)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Please login with username and password or register to have permission to this page.") }, "/Error");
            }

            TripDetailsViewModel tripDetailsViewModel = tripService.GetTripDetails(tripId);

            return View(tripDetailsViewModel);
        }


        public Response AddUserToTrip(string tripId)
        {
            var isAuthorized = User.IsAuthenticated;

            if (!isAuthorized)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Please login with username and password or register to have permission to this page.") }, "/Error");
            }

            var userId = User.Id;

            try
            {
                tripService.AddUserToTrip(userId, tripId);
            }
            catch (Exception)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Unexpected Error") }, "/Error");
            }

            return Redirect("/Trips/All");
        }
    }
}
