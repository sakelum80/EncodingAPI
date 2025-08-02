using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoding.Application;
using Encoding.Core;
using Encoding.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Encoding.Tests
{
    public class EncoderServiceTests
    {
        [Fact]
        public void Encode_NullInput_ReturnsFailure()
        {
            var mockStrategy = new Mock<IEncodingStrategy>();
            var service = new EncoderService(mockStrategy.Object, Mock.Of<ILogger<EncoderService>>());

            var result = service.Encode(null);

            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NullInput, result.Error);
        }


    }
}
