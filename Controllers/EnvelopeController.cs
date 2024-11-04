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
    public class EnvelopesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EnvelopesController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnvelopes()
        {    

              var UrlClick=_configuration["Clicksign:URl"]; 

            var url = $"{UrlClick}/api/v3/envelopes";
            var accessToken = _configuration["Clicksign:AccessToken"];

            // Criar a requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("Authorization", accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Enviar a requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verificar se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = "Erro ao buscar envelopes", details = error });
            }
        }

        // Método para buscar um envelope específico pelo ID
        [HttpGet("{envelopeId}")]
        public async Task<IActionResult> GetEnvelopeById(string envelopeId)
        {

           var UrlClick=_configuration["Clicksign:URl"]; 

            var url = $"{UrlClick}/api/v3/envelopes/{envelopeId}";
            var accessToken = _configuration["Clicksign:AccessToken"];

            // Criar a requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("Authorization", accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Enviar a requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verificar se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = "Erro ao buscar o envelope", details = error });
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateEnvelope([FromBody] CreateEnvelopeRequestDTOs requestDto)
        {
            
             var UrlClick=_configuration["Clicksign:URl"]; 
            var url = $"{UrlClick}/api/v3/envelopes";

            // Montar o corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "envelopes",
                    attributes = new
                    {
                        name = requestDto.Name,
                        locale = requestDto.Locale ?? "pt-BR",
                        auto_close = requestDto.AutoClose,
                        block_after_refusal = requestDto.BlockAfterRefusal,
                        deadline_at = requestDto.DeadlineAt // Garantir que está em formato ISO 8601
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
                return Ok(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = "Erro ao criar o envelope", details = error });
            }
        }
    }
}
