namespace Nude.API.Infrastructure.Exceptions.Client;

public class TicketLimitExceededException : BadRequestException
{
    public TicketLimitExceededException(string? message) : base(message) { }
}