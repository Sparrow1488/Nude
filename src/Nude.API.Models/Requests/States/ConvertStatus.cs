namespace Nude.API.Models.Requests.States;

public enum ConvertStatus
{
    WaitToReceive = 0x00,
    InProcess = 0x10,
    Success = 0x20,
    Failed = 0x30
}