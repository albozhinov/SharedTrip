namespace SharedTrip.Services
{
    using SharedTrip.Contracts;
    using SharedTrip.Data.Common;
    using SharedTrip.Data.DBModels;
    using SharedTrip.Models;
    using SharedTrip.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class TripService : ITripService
    {
        private readonly IRepository repo;

        public TripService(IRepository _repo)
        {
            repo = _repo;
        }

        public void AddTrip(TripViewModel model)
        {
            Trip trip = new Trip()
            {
                Description = model.Description,
                EndPoint = model.EndPoint,
                Seats = model.Seats,
                StartPoint = model.StartPoint,
                ImagePath = model.ImagePath
            };

            DateTime date;

            DateTime.TryParseExact(model.DepartureTime,
                                            "dd.MM.yyyy HH:mm",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date);
            trip.DepartureTime = date;

            repo.Add(trip);
            repo.SaveChanges();
        }

        public void AddUserToTrip(string userId, string tripId)
        {
            var userTrip = repo
                                .All<UserTrip>()
                                .FirstOrDefault(ut => ut.TripId == tripId && ut.UserId == userId);

            if (userTrip == null)
            {

                userTrip = new UserTrip()
                {
                    UserId = userId,
                    TripId = tripId
                };

                var trip = repo
                               .All<Trip>()
                               .FirstOrDefault(t => t.Id == tripId);


                trip.Seats -= 1;
            }
            else
            {
                throw new ArgumentException("User allready added to this trip.");
            }

            repo.Add(userTrip);
            repo.SaveChanges();
        }

        public IEnumerable<TripListViewModel> GetAllTrips()
        {
            return repo.All<Trip>()
                            .Select(t => new TripListViewModel()
                            {
                                DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                                EndPoint = t.EndPoint,
                                Id = t.Id,
                                Seats = t.Seats,
                                StartPoint = t.StartPoint
                            });
        }

        public TripDetailsViewModel GetTripDetails(string tripId)
        {
            return repo.All<Trip>()
                       .Where(t => t.Id == tripId)
                       .Select(t => new TripDetailsViewModel()
                       {
                           DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                           EndPoint = t.EndPoint,
                           Id = t.Id,
                           Seats = t.Seats,
                           StartPoint = t.StartPoint,
                           ImagePath = t.ImagePath,
                           Description = t.Description
                       }).FirstOrDefault();
        }

        public (bool isValid, IEnumerable<ErrorViewModel>) ValidateModel(TripViewModel model)
        {
            bool isValid = true;
            List<ErrorViewModel> errors = new List<ErrorViewModel>();

            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                isValid = false;
                errors.Add(new ErrorViewModel("StartPoint is required"));
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                isValid = false;
                errors.Add(new ErrorViewModel("EndPoint is required"));
            }

            if (string.IsNullOrWhiteSpace(model.Description) ||
                model.Description.Length > 80)
            {
                isValid = false;
                errors.Add(new ErrorViewModel("Description is required and must not be more than 80 characters long"));
            }

            if (model.Seats < 2 || model.Seats > 6)
            {
                isValid = false;
                errors.Add(new ErrorViewModel("Seats must be between 2 and 6"));
            }


            return (isValid, errors);
        }
    }
}


