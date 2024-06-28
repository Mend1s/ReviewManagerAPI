using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os usuários.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
    {
        _logger.LogInformation("[UsersController] Iniciando a obtenção de todos os usuários.");

        try
        {
            var users = await _userService.GetAllUsers();
            if (users == null || !users.Any())
            {
                _logger.LogWarning("[UsersController] Nenhum usuário encontrado.");
                return NotFound("Nenhum usuário encontrado.");
            }

            _logger.LogInformation("[UsersController] {Count} usuários encontrados.", users.Count());
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UsersController] Erro ao obter todos os usuários.");
            return BadRequest($"Erro ao obter todos os usuários: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">O ID do usuário a ser obtido.</param>
    /// <returns>O usuário solicitado.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserViewModel>> GetById(int id)
    {
        _logger.LogInformation("[UsersController] Iniciando a obtenção do usuário com ID: {Id}.", id);

        try
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                _logger.LogWarning("[UsersController] Usuário com ID: {Id} não encontrado.", id);
                return NotFound($"Usuário com ID: {id} não encontrado.");
            }

            _logger.LogInformation("[UsersController] Usuário com ID: {Id} obtido com sucesso.", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UsersController] Erro ao obter o usuário com ID: {Id}.", id);
            return BadRequest($"Erro ao obter o usuário com ID: {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="createUserInputModel">Os detalhes do usuário a ser criado.</param>
    /// <returns>O usuário criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserInputModel createUserInputModel)
    {
        _logger.LogInformation("[UsersController] Iniciando a criação de um novo usuário.");

        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[UsersController] ModelState inválida ao criar um novo usuário.");
                return BadRequest(ModelState);
            }

            var user = await _userService.CreateUser(createUserInputModel);
            _logger.LogInformation("[UsersController] Usuário criado com sucesso com ID: {Id}.", user.Id);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UsersController] Erro ao criar um novo usuário.");
            return BadRequest($"Erro ao criar um novo usuário: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    /// <param name="id">O ID do usuário a ser atualizado.</param>
    /// <param name="updateUserInputModel">Os novos detalhes do usuário.</param>
    /// <returns>O usuário atualizado.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserInputModel updateUserInputModel)
    {
        _logger.LogInformation("[UsersController] Iniciando a atualização do usuário com ID: {Id}.", id);

        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[UsersController] ModelState inválida ao atualizar o usuário com ID: {Id}.", id);
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUser(id, updateUserInputModel);
            if (updatedUser == null)
            {
                _logger.LogWarning("[UsersController] Usuário com ID: {Id} não encontrado para atualização.", id);
                return NotFound($"Usuário com ID: {id} não encontrado.");
            }

            _logger.LogInformation("[UsersController] Usuário com ID: {Id} atualizado com sucesso.", id);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UsersController] Erro ao atualizar o usuário com ID: {Id}.", id);
            return BadRequest($"Erro ao atualizar o usuário com ID: {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Deleta um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser deletado.</param>
    /// <returns>O usuário deletado.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation("[UsersController] Iniciando a exclusão do usuário com ID: {Id}.", id);

        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[UsersController] ModelState inválida ao excluir o usuário com ID: {Id}.", id);
                return BadRequest(ModelState);
            }

            var result = await _userService.DeleteUser(id);
            if (result == null)
            {
                _logger.LogWarning("[UsersController] Usuário com ID: {Id} não encontrado para exclusão.", id);
                return NotFound($"Usuário com ID: {id} não encontrado.");
            }

            _logger.LogInformation("[UsersController] Usuário com ID: {Id} excluído com sucesso.", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UsersController] Erro ao excluir o usuário com ID: {Id}.", id);
            return BadRequest($"Erro ao excluir o usuário com ID: {id}: {ex.Message}");
        }
    }
}
