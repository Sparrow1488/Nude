using Nude.API.Contracts.Errors.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nude.Bot.Tg.models.Api
{
    public class ApiResult<T>
    {
        public T? Result { get; set; }
        public bool IsSuccess => ErrorResponse == null;
        public string? Message { get; set; }
        public ErrorResponse? ErrorResponse { get; set; }
    }
}
