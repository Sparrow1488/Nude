namespace Nude.API.Models.Tickets.States;

public enum FormattingStatus
{
    WaitToProcess = 0x00,
    InProcess = 0x10,
    Success = 0x20,
    Failed = 0x30
}