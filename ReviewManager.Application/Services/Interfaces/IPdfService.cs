using Microsoft.AspNetCore.Http;
using ReviewManager.Core.Entities;

namespace ReviewManager.Application.Services.Interfaces;

public interface IPdfService
{
    IResult GerarRelatorioProdutos(IEnumerable<Book> books);
}
