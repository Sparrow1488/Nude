namespace Nude.API.Models.Tickets.States;

public enum FormattingStatus
{
    /// <summary>
    /// Пошел процесс форматирования
    /// </summary>
    Started = 0x00,
    /// <summary>
    /// Форматирование прошло успешно
    /// </summary>
    Success = 0x10,
    /// <summary>
    /// Произошел полный пиздец на стороне апи
    /// </summary>
    FailedInternalException = 0x20,
    /// <summary>
    /// Контента слишком много, мы не можем его обработать
    /// </summary>
    FailedTooLong = 0x20,
}