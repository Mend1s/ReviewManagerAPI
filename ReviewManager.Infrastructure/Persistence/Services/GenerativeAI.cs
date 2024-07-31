using Mscc.GenerativeAI;
using System.Text.RegularExpressions;

namespace ReviewManager.Infrastructure.Persistence.Services;

public class GenerativeAI : IGenerativeTestAI
{
    private GenerativeModel _model;
    public GenerativeAI()
    {
        var apiKey = "AIzaSyBcB5tE7ORiT1la48jdtZHl4ACgXsL0SU0";
        var genai = new GoogleAI(apiKey);
        _model = genai.GenerativeModel();
    }

    public async Task<string> GenerateContentAsync()
    {
        var prompt = @"retorne um json de um livro real com os dados preenchidos, somente o json, não quero mais nada além disso:
            public string Title { get; set; }
            public string Description { get; set; }
            public string ISBN { get; set; }
            public string Author { get; set; }
            public string Publisher { get; set; }
            public BookGenre Genre { get; set; }
            public int YearOfPublication { get; set; }
            public int NumberOfPages { get; set; }";

        var response = await _model.GenerateContent(prompt);

        return ExtractJsonFromResponse(response.Text);
    }

    private string ExtractJsonFromResponse(string response)
    {
        var regex = new Regex(@"(\{.*?\})", RegexOptions.Singleline);
        var match = regex.Match(response);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return response;
    }
}
