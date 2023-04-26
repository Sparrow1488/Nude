using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;

namespace Nude.Creators;

public interface IParserCreator
{
    Task<INudeParser> CreateNudeMoonAsync(
        string login, 
        string password, 
        bool storeCredentials = false
    );
    
    Task<IHentaiChanParser> CreateHentaiChanAsync(
        string login, 
        string password, 
        bool storeCredentials = false
    );
}