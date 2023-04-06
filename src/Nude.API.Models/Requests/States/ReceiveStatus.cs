namespace Nude.API.Models.Requests.States;

public enum ReceiveStatus
{
    WaitToProcess = 0x00,
    Success = 0x10,
    Failed = 0x20
}