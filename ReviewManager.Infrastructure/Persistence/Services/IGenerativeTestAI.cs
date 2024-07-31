namespace ReviewManager.Infrastructure.Persistence.Services;

public interface IGenerativeTestAI
{
    Task<string> GenerateContentAsync();
}
