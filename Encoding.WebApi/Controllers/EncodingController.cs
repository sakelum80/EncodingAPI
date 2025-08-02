using Encoding.Core;
using Encoding.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Encoding.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/encoding")]
    [ApiVersion("1.0")]
    public class EncodingController : ControllerBase
    {
        private readonly IEncoderService _encoder;
        private readonly ILogger<EncodingController> _logger;

        public EncodingController(
            IEncoderService encoder,
            ILogger<EncodingController> logger)
        {
            _encoder = encoder;
            _logger = logger;
        }

        [HttpPost("rle")]
        [Authorize(Roles = "Encoder")]
        [ProducesResponseType(typeof(EncodingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult RunLengthEncode([FromBody] EncodingRequest request)
        {
            _logger.LogInformation("Encoding request received");
         
            if (request == null || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new
                {
                    Error = "Input text cannot be empty",
                    Status = 400
                });
            }

          
            var result = _encoder.Encode(request.Text);

      
            if (result == null)
            {
                return StatusCode(500, new
                {
                    Error = "Encoding service error",
                    Status = 500
                });
            }

          
            if (result.IsSuccess)
            {
                return Ok(new EncodingResponse(result.Value!));
            }
            else
            {
                return BadRequest(new
                {
                    Error = result.Error,
                    Status = 400
                });
            }
        }
    }
}
