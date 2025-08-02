using Encoding.Models;

namespace Encoding.Core
{
    public interface IEncoderService
    {
        Result<string> Encode(string input);
    }
}
