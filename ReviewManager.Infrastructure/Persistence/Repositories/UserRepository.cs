using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ReviewDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        ReviewDbContext dbContext,
        ILogger<UserRepository> logger) 
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<User> CreateAsync(User user)
    {
        _logger.LogInformation("[UserRepository] Criando um novo usuário.");

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("[UserRepository] Usuário criado com sucesso com ID: {Id}.", user.Id);
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        _logger.LogInformation("[UserRepository] Excluindo usuário com ID: {Id}.", user.Id);

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("[UserRepository] Usuário com ID: {Id} excluído com sucesso.", user.Id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        _logger.LogInformation("[UserRepository] Obtendo todos os usuários.");

        var users = await _dbContext.Users.ToListAsync();

        _logger.LogInformation("[UserRepository] {Count} usuários encontrados.", users.Count());
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        _logger.LogInformation("[UserRepository] Obtendo usuário com ID: {Id}.", id);

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

        _logger.LogInformation("[UserRepository] Usuário com ID: {Id} obtido com sucesso.", id);
        return user;
    }

    public async Task<User> GetByIdWithReviewAsync(int id)
    {
        _logger.LogInformation("[UserRepository] Obtendo usuário com ID: {Id} e suas avaliações.", id);

        var user = await _dbContext.Users
            .Include(r => r.Reviews)
            .SingleOrDefaultAsync(u => u.Id == id);

        _logger.LogInformation("[UserRepository] Usuário com ID: {Id} obtido com suas avaliações.", id);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _logger.LogInformation("[UserRepository] Atualizando usuário com ID: {Id}.", user.Id);

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("[UserRepository] Usuário com ID: {Id} atualizado com sucesso.", user.Id);
    }

    public bool ValidatorExistsByEmail(string email)
    {
        _logger.LogInformation("[UserRepository] Verificando se o e-mail {Email} já existe.", email);

        var exists = _dbContext.Users.Any(d => d.Email == email);

        _logger.LogInformation("[UserRepository] Verificação de e-mail concluída: {Exists}.", exists);
        return exists;
    }
}
