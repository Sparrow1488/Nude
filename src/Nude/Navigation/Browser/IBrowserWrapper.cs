using AngleSharp.Dom;
using Nude.Navigation.Abstractions;

namespace Nude.Navigation.Browser;

public interface IBrowserWrapper : IWebNavigator
{
    Task<IDocument> GetDocumentAsync(string url, string waitSelector);
    Task<string> GetTextAsync(string url, string waitSelector);
}