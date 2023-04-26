using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Internal;

public class JsonConverterForgotException : ApiException
{
    public JsonConverterForgotException(string converterType) : base(
        $"Not all methods are configured in {converterType}")
    {
    }
}