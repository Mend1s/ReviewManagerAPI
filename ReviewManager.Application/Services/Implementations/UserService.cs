using Microsoft.Extensions.Logging;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User> CreateUser(CreateUserInputModel createUserInputModel)
    {
        _logger.LogInformation("[UserService] Criando um novo usuário.");

        var user = new User(createUserInputModel.Name, createUserInputModel.Email);

        if (ExistsByEmail(createUserInputModel.Email))
        {
            var message = "Esse e-mail já está cadastrado!";
            _logger.LogWarning("[UserService] {Message}", message);
            throw new Exception(message);
        }

        await _userRepository.CreateAsync(user);

        _logger.LogInformation("[UserService] Usuário criado com sucesso com ID: {Id}.", user.Id);
        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        _logger.LogInformation("[UserService] Iniciando a exclusão do usuário com ID: {Id}.", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            var message = "Usuário não encontrado.";
            _logger.LogWarning("[UserService] {Message}", message);
            throw new Exception(message);
        }

        await _userRepository.DeleteAsync(user);

        _logger.LogInformation("[UserService] Usuário com ID: {Id} excluído com sucesso.", id);
        return true;
    }

    public async Task<List<UserViewModel>> GetAllUsers()
    {
        _logger.LogInformation("[UserService] Obtendo todos os usuários.");

        var users = await _userRepository.GetAllAsync();
        if (users == null || !users.Any())
        {
            var message = "Usuários não encontrados.";
            _logger.LogWarning("[UserService] {Message}", message);
            throw new Exception(message);
        }

        var userViewModel = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        }).ToList();

        _logger.LogInformation("[UserService] {Count} usuários encontrados.", users.Count());
        return userViewModel;
    }

    public async Task<UserViewModel> GetUserById(int id)
    {
        _logger.LogInformation("[UserService] Obtendo o usuário com ID: {Id}.", id);

        var user = await _userRepository.GetByIdWithReviewAsync(id);
        if (user == null)
        {
            var message = "Usuário não encontrado.";
            _logger.LogWarning("[UserService] {Message}", message);
            throw new Exception(message);
        }

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

        _logger.LogInformation("[UserService] Usuário com ID: {Id} obtido com sucesso.", id);
        return userViewModel;
    }

    public async Task<User> UpdateUser(int id, UpdateUserInputModel updateUserInputModel)
    {
        _logger.LogInformation("[UserService] Atualizando o usuário com ID: {Id}.", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            var message = "Usuário não encontrado.";
            _logger.LogWarning("[UserService] {Message}", message);
            throw new Exception(message);
        }

        user.UpdateUser(updateUserInputModel.Name, updateUserInputModel.Email);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("[UserService] Usuário com ID: {Id} atualizado com sucesso.", id);
        return user;
    }

    private bool ExistsByEmail(string email)
    {
        _logger.LogInformation("[UserService] Verificando se o e-mail {Email} já existe.", email);
        var exists = _userRepository.ValidatorExistsByEmail(email);
        _logger.LogInformation("[UserService] Verificação de e-mail concluída: {Exists}.", exists);
        return exists;
    }
}
