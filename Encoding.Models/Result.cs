using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Encoding.Models
{
    public record Result<T>(T? Value, bool IsSuccess, Error? Error)
    {
        public static Result<T> Success(T value) => new(value, true, null);
        public static Result<T> Failure(Error error) => new(default, false, error);
    }
}
