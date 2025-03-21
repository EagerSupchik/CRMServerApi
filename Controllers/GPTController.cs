using CRMServerApi.Data;
using CRMServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using System.Net.Http;
using System.Text.Json;
using System.Text;
namespace CRMServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public GPTController(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        // GET: api/gpt/{content}
        [HttpGet("{content}")]
        public async Task<IActionResult> AskGPT(string content)
        {
            var apiUrl = "https://openrouter.ai/api/v1/chat/completions";

            var apiKey = _configuration["GPTapi:API-KEY"];

            var requestBody = new
            {
                model = "google/gemini-2.0-flash-lite-preview-02-05:free",
                messages = new[]
                {
                    new { role = "user", content = content }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = httpContent
            };


            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, error);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
