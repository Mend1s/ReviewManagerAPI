using ReviewManager.Application.InputModels;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;

namespace ReviewManager.Application.Services.Interfaces;

public interface IUserService
{
    Task<List<UserViewModel>> GetAllUsers();
    Task<UserViewModel> GetUserById(int id);
    Task<User> CreateUser(CreateUserInputModel createUserInputModel);
    Task<User> UpdateUser(int id, UpdateUserInputModel updateUserInputModel);
    Task<bool> DeleteUser(int id);
}
