using Nude.Helpers.Abstractions;
using Nude.Parsers.Abstractions;

namespace Nude.Parsers.HentaiChan;

public interface IHentaiChanParser : IMangaParser
{
    IHentaiChanHelper Helper { get; }
}