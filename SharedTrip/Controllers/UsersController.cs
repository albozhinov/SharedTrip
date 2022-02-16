namespace SharedTrip.Controllers
{
    using BasicWebServer.Server.Attributes;
    using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;
    using SharedTrip.Contracts;
    using SharedTrip.ViewModels;
    using SharedTrip.ViewModels.Users;
    using System;
    using System.Collections.Generic;

    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(
            Request request,
            IUserService _userService) 
            : base(request)
        {
            userService = _userService;
        }

        [HttpPost]
        public Response Login(LoginViewModel model)
        {
            var isAuthorized = User.IsAuthenticated;

            if (isAuthorized)
            {
                return Redirect("/Trips/All");
            }

            Request.Session.Clear();

            (string userId, bool isCorrect) = userService.IsLoginCorrect(model);

            if (isCorrect)
            {
                SignIn(userId);

                CookieCollection cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName,
                    Request.Session.Id);

                return Redirect("/Trips/All");
            }

            return View(new List<ErrorViewModel>() { new ErrorViewModel("Login incorrect") }, "/Error");
        }

        [HttpGet]
        public Response Login()
        {
            var isAuthorized = User.IsAuthenticated;

            if (isAuthorized)
            {
                return Redirect("/Trips/All");
            }

            return View();
        }

        [HttpGet]
        public Response Register()
        {
            var isAuthorized = User.IsAuthenticated;

            if (isAuthorized)
            {
                return Redirect("/Trips/All");
            }

            return View();
        }

        [HttpPost]
        public Response Register(RegisterViewModel model)
        {
            var isAuthorized = User.IsAuthenticated;

            if (isAuthorized)
            {
                return Redirect("/Trips/All");
            }

            var (isValid, errors) = userService.ValidateModel(model);

            if (!isValid)
            {
                return View(errors, "/Error");
            }

            try
            {
                userService.RegisterUser(model);
            }
            catch (ArgumentException aex)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel(aex.Message) }, "/Error");
            }
            catch (Exception)
            {

                return View(new List<ErrorViewModel>() { new ErrorViewModel("Unexpected Error") }, "/Error");
            }

            return Redirect("/Users/Login");
        }
                
        public Response Logout() 
        {
            var isAuthorized = User.IsAuthenticated;

            if (!isAuthorized)
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("You are not logged in, please login or register.") }, "/Error");
            }

            SignOut();

            return Redirect("/");
        }
    }
}
