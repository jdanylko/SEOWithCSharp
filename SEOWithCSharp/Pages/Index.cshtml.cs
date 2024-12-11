using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEOWithCSharp.Middleware;

namespace SEOWithCSharp.Pages;

//[MinifyHtmlPageFilter]
public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    private string[] _stopWords =
    [
        "a", "an", "on", "of", "or", "as", "i", "in", "is", "to",
        "the", "and", "for", "with", "not", "by", "was", "it"
    ];

    [BindProperty] public string TextToAnalyze { get; set; } = string.Empty;

    public List<KeyValuePair<string, int>> KeywordResults { get; set; } = null!;
    public int WordCount { get; set; } = 0;

    public void OnGet() { }

    public void OnPost()
    {
        var words = TextToAnalyze
            .Split([' ', '.', ','], StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.ToLowerInvariant())
            .Where(e => !_stopWords.Contains(e))
            .ToList();

        WordCount = words.Count;

        KeywordResults = words
            .CountBy(e => e)
            .OrderByDescending(e => e.Value)
            .Where(e=> e.Value >= 2)
            .ToList();
    }

}