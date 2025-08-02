using System.ComponentModel.DataAnnotations;

namespace Encoding.WebApi.Models
{
    public record EncodingRequest(
     [Required][MinLength(1)][MaxLength(10000)] string Text);
}
