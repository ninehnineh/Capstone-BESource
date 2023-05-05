using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/upload-image")]
    [ApiController]
    public class ZUploadImageController : ControllerBase
    {
        private readonly HttpClient _client;

        public ZUploadImageController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "886d0b92410e625");
        }
        [HttpPost]
        public async Task<IActionResult> UploadImagess(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            var response = await _client.PostAsync("https://api.imgur.com/3/image", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurResponse>(responseContent);

            var item1 = new Dictionary<string, string>
            {
                { "link", result.Data.Link}
            };
            string json = JsonSerializer.Serialize(item1);
            return Ok(json);
        }
    }
    public class ImgurData
    {
        public string Link { get; set; }
    }
    public class ImgurResponse
    {
        public ImgurData Data { get; set; }
    }
}
