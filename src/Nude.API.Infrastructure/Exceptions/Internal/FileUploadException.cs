using Nude.API.Infrastructure.Exceptions.Base;

namespace Nude.API.Infrastructure.Exceptions.Internal;

public class FileUploadException : ApiInternalException
{
    public FileUploadException()
    {
    }

    public FileUploadException(string? message) : base(message)
    {
    }
}