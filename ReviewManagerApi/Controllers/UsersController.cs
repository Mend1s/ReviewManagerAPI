using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
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
        try
        {
            return Ok(await _userService.GetAllUsers());
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[GetAllUsers] : {ex.Message}");
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
        try
        {
            return Ok(await _userService.GetUserById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[GetUserById] : {ex.Message}");
        }
    }


    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="createUserInputModel">Os detalhes do usuário a ser criado.</param>
    /// <returns>O doador criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserInputModel createUserInputModel)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.CreateUser(createUserInputModel);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[CreateUser] : {ex.Message}");
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
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _userService.UpdateUser(id, updateUserInputModel));
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[UpdateUser] : {ex.Message}");
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
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _userService.DeleteUser(id));
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[DeleteUser] : {ex.Message}");
        }
    }
}
