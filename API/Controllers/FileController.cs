using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// Converts an uploaded file to a Base64-encoded string. Supports images and videos with appropriate data URI prefix.
        /// </summary>
        [HttpPost("convert-to-base64")]
        public IActionResult ConvertToBase64([FromForm] ConvertToBase64Request request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            using var memoryStream = new MemoryStream();
            request.File.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(fileBytes);

            var contentType = request.File.ContentType;
            if (!string.IsNullOrEmpty(contentType) &&
                (contentType.StartsWith("image/") || contentType.StartsWith("video/"))
                )
            {
                base64String = $"data:{contentType};base64,{base64String}";
            }

            return Ok(new ConvertToBase64Response
            {
                Base64String = base64String
            });
        }
    }
}
