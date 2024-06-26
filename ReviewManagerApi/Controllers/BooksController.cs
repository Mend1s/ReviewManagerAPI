using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Obtém todos os livros.
    /// </summary>
    /// <returns>Uma lista de livros.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<BookViewModel>>> GetAll()
    {
        try
        {
            return Ok(await _bookService.GetAllBooks());
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[GetAllBooks] : {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém um livro pelo ID.
    /// </summary>
    /// <param name="id">O ID do livro a ser obtido.</param>
    /// <returns>O livro solicitado.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookViewModel>> GetById(int id)
    {
        try
        {
            return Ok(await _bookService.GetBookById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[GetBookById] : {ex.Message}");
        }
    }

    /// <summary>
    /// Cria um novo livro.
    /// </summary>
    /// <param name="createBookInputModel">Os detalhes do livro a ser criado.</param>
    /// <returns>O doador criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromForm] CreateBookInputModel createBookInputModel)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _bookService.CreateBook(createBookInputModel);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);

        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[CreateBook] : {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza um livro existente.
    /// </summary>
    /// <param name="id">O ID do livro a ser atualizado.</param>
    /// <param name="updateBookModel">Os novos detalhes do livro.</param>
    /// <returns>O livro atualizado.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateBookInputModel updateBookModel)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _bookService.UpdateBook(id, updateBookModel));

        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[UpdateBook] : {ex.Message}");
        }
    }

    /// <summary>
    /// Deleta um livro pelo ID.
    /// </summary>
    /// <param name="id">ID do livro a ser deletado.</param>
    /// <returns>O livro deletado.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _bookService.DeleteBook(id));
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[DeleteBook] : {ex.Message}");
        }
    }

    /// <summary>
    /// Baixa a imagem de capa de um livro pelo ID.
    /// </summary>
    /// <remarks>
    /// Retorna a imagem de capa do livro como um arquivo de imagem (png/jpeg).
    /// </remarks>
    /// <param name="id">ID do livro.</param>
    /// <returns>O arquivo de imagem do livro.</returns>
    [HttpGet("download/{id:int}")]
    public async Task<IActionResult> DownloadPhotoBookAsync(int id)
    {
        try
        {
            var fileResult = await _bookService.DownloadPhotoBookAsync(id);
            return fileResult;
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[DownloadPhotoBookAsync] : {ex.Message}");
        }
    }
}
