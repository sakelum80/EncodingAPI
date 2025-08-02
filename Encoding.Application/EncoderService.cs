using Encoding.Core;
using Encoding.Models;
using Microsoft.Extensions.Logging;

namespace Encoding.Application
{
    public class EncoderService : IEncoderService
    {
        private readonly IEncodingStrategy _strategy;
        private readonly ILogger<EncoderService> _logger;

        public EncoderService(
            IEncodingStrategy strategy,
            ILogger<EncoderService> logger)
        {
            _strategy = strategy;
            _logger = logger;
        }

        public Result<string> Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                _logger.LogWarning("Null or empty input received");
                return Result<string>.Failure(Error.NullInput);
            }

            try
            {
                var result = _strategy.Encode(input);
                _logger.LogDebug("Encoded {Length} characters", input.Length);
                return Result<string>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encoding failed for input: {Input}", input);
                return Result<string>.Failure(Error.ProcessingError);
            }
        }
    }
}
