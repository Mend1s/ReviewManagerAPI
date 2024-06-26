using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly ReviewDbContext _dbContext;
    public UserService(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> CreateUser(CreateUserInputModel createUserInputModel)
    {
        var user = new User(createUserInputModel.Name, createUserInputModel.Email);

        if (ExistsByEmail(createUserInputModel.Email)) throw new Exception("Esse e-mail já está cadastrado!.");

        await _dbContext.Users.AddAsync(user);

        await _dbContext.SaveChangesAsync();

        // TODO: alterar retorno para nao exibir listagem de avaliações -- colocar viewmodel

        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

        if (user is null) throw new Exception("Usuário não encontrado.");

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<UserViewModel>> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();

        if (users is null) throw new Exception("Usuários não encontrado.");

        var userViewModel = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        }).ToList();

        return userViewModel;
    }

    public async Task<UserViewModel> GetUserById(int id)
    {
        var user = await _dbContext.Users
            .Include(r => r.Reviews)
            .SingleOrDefaultAsync(u => u.Id == id);

        if (user is null) throw new Exception("Usuário não encontrado.");

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Reviews = user.Reviews.Select(review => new ReviewViewModel
            {
                Id = review.Id,
                Note = review.Note,
                Description = review.Description,
                IdBook = review.IdBook,
                CreateDate = review.CreateDate
            }).ToList(),
        };

        return userViewModel;
    }

    public async Task<User> UpdateUser(int id, UpdateUserInputModel updateUserInputModel)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == updateUserInputModel.Id);

        if (user is null) throw new Exception("Usuário não encontrado.");

        user.UpdateUser(updateUserInputModel.Name, updateUserInputModel.Email);

        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    private bool ExistsByEmail(string email)
    {
        return _dbContext.Users.Any(d => d.Email == email);
    }
}
