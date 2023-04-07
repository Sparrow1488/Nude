namespace Nude.API.Models.Tickets.States;

public enum ReceiveStatus
{
    WaitToProcess = 0x00,
    Success = 0x10,
    Failed = 0x20
}