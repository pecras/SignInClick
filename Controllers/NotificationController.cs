using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SignInClick.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignerNotificationsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
              private readonly string _clickSignUrl;
 

        public SignerNotificationsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
_clickSignUrl = configuration["Clicksign:URl"] ?? string.Empty;

        }

        [HttpPost("{envelopeId}/signers/{signerId}/notifications")]
        public async Task<IActionResult> SendSignerNotification(string envelopeId, string signerId, [FromBody] NotificationRequestSigners request)
        {
            var url = $"https://sandbox.clicksign.com/api/v3/envelopes/{envelopeId}/signers/{signerId}/notifications";

            // Corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "notifications",
                    attributes = new
                    {
                        message = request.Message 
                    }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");

            // Recupera o token de acesso do configuration
            var accessToken = _configuration["Clicksign:AccessToken"];

            // Criação da requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Cabeçalhos
            requestMessage.Headers.Add("Authorization", accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Envio da requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verifica se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = "Erro ao enviar notificação para o signatário", details = error });
            }
        }
    }

    // DTO para a requisição
    public class NotificationRequestSigners
    {
        public string Message { get; set; } = "Assinatura Enviada!";
    }
}
