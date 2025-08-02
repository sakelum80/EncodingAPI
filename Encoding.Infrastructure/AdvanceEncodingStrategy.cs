using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoding.Core;

namespace Encoding.Infrastructure
{
    public class AdvanceEncodingStrategy : IEncodingStrategy
    {
        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var span = input.AsSpan();
            var result = new StringBuilder();
            int count = 1;

            for (int i = 1; i < span.Length; i++)
            {
                if (span[i] == span[i - 1])
                    count++;
                else
                {
                    result.Append(span[i - 1]).Append(count);
                    count = 1;
                }
            }

            result.Append(span[^1]).Append(count);
            return result.ToString();
        }
    }
}
