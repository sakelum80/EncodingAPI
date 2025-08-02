using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Encoding.Core;
using Encoding.Models;
using Encoding.WebApi.Controllers;
using Encoding.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Encoding.Tests.Controllers
{
    public class EncodingControllerTests
    {
        private readonly HttpClient _client;
        private readonly Mock<IEncoderService> _mockEncoder = new();
        private readonly Mock<ILogger<EncodingController>> _mockLogger = new();
        private EncodingController _controller;

        public EncodingControllerTests()
        {
            _controller = new EncodingController(
            _mockEncoder.Object,
            _mockLogger.Object);
        }
        [Fact]
        public void RunLengthEncode_WhenAuthenticated_ReturnsEncodedString()
        {

            // Arrange
            var mockEncoder = new Mock<IEncoderService>();
            mockEncoder.Setup(x => x.Encode(It.IsAny<string>()))
                      .Returns(Result<string>.Success("a3b2c1"));

            var controller = new EncodingController(
                mockEncoder.Object,
                Mock.Of<ILogger<EncodingController>>());

            // Mock authenticated user with "Encoder" rolek
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "admin@gmail.com"),
                new Claim(ClaimTypes.Role, "Encoder") // Required for [Authorize(Roles="Encoder")]
            }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = controller.RunLengthEncode(new EncodingRequest("aaabbc"));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("a3b2c1", ((EncodingResponse)okResult.Value).Encoded);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RunLengthEncode_InvalidInput_ReturnsBadRequest(string input)
        {
            // Arrange
            var authContext = CreateAuthenticatedContext("Encoder");
            _controller.ControllerContext = authContext;

            // Act
            var result =  _controller.RunLengthEncode(new EncodingRequest(input));

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequest.Value);
        }

        private static ControllerContext CreateAuthenticatedContext(string role)
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, "test@example.com"),
                    new Claim(ClaimTypes.Role, role)
                }, "TestAuth"))
                }
            };
        }
    }
}
