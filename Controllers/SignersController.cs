using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SignInClick.DTOS;

namespace SignInClick.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignersController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _clickSignUrl;

        public SignersController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _clickSignUrl = configuration["Clicksign:URl"] ?? string.Empty;
        }

        [HttpPost("{envelopeId}")]
        public async Task<IActionResult> CreateSigner(string envelopeId, [FromBody] CreateSignerRequestDTO requestDto)
        {

            
            var url = $"https://{_clickSignUrl}/api/v3/envelopes/{envelopeId}/signers";

            // Montar o corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "signers",
                    attributes = new
                    {
                        name = requestDto.Name,
                
                        email = requestDto.Email,
                       
                        documentation = requestDto.Documentation,
                
                        group = requestDto.Group,
                        communicate_events = new
                        {
                            document_signed = requestDto.CommunicateEvents.DocumentSigned,
                            signature_request = requestDto.CommunicateEvents.SignatureRequest,
                            signature_reminder = requestDto.CommunicateEvents.SignatureReminder
                        }
                    }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");

            // Recuperar o token de autenticação do configuration
            var accessToken = _configuration["Clicksign:AccessToken"];

            // Criar a requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Definir cabeçalhos
            requestMessage.Headers.Add("Authorization", accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Enviar a requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verificar se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return CreatedAtAction(nameof(CreateSigner), new { envelopeId = envelopeId }, result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = "Erro ao criar o signatário", details = error });
            }
        }
    }
}
