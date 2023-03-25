using AngleSharp.Dom;

namespace Nude.Navigation.Abstractions;

public interface IWebNavigator : ICookieStorable, IDisposable
{
    Task<IDocument> GetDocumentAsync(string url);
    Task<string> GetTextAsync(string url);
    Task<(string? html, int status)> GetTextWithStatusAsync(string url);
}