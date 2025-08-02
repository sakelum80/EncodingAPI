using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoding.Models
{
    public record Error(string Code, string Message)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullInput = new("ENC.400", "Input cannot be null");
        public static readonly Error ProcessingError = new("ENC.500", "Processing error occurred");
    }
}
