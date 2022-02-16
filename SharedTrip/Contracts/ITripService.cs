using SharedTrip.Models;
using SharedTrip.ViewModels;
using System.Collections.Generic;

namespace SharedTrip.Contracts
{
    public interface ITripService
    {
        (bool isValid, IEnumerable<ErrorViewModel>) ValidateModel(TripViewModel model);

        void AddTrip(TripViewModel model);

        IEnumerable<TripListViewModel> GetAllTrips();
        TripDetailsViewModel GetTripDetails(string tripId);
        void AddUserToTrip(string userId, string tripId);
    }
}
