using AngleSharp.Dom;

namespace Nude.Navigation;

public interface IBrowserWrapper : IDisposable
{
    Task<IDocument> GetDocumentAsync(string url);
    Task<IDocument> GetDocumentAsync(string url, string waitSelector);
    Task<string> GetTextAsync(string url);
    Task<string> GetTextAsync(string url, string waitSelector);
}