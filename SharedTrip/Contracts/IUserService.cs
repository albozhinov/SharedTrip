using SharedTrip.ViewModels;
using SharedTrip.ViewModels.Users;
using System.Collections.Generic;

namespace SharedTrip.Contracts
{
    public interface IUserService
    {
        (bool isValid, IEnumerable<ErrorViewModel> errors) ValidateModel(RegisterViewModel model);
        void RegisterUser(RegisterViewModel model);
        (string userId, bool isCorrect) IsLoginCorrect(LoginViewModel model);
    }
}
