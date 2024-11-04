using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;



namespace SignInClick.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DocumentsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }



        [HttpPost("{envelopeId}")]
        public async Task<IActionResult> CreateDocument(string envelopeId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }
            var UrlClick=_configuration["Clicksign:URl"]; 
           
            var url = $"{UrlClick}/v3/envelopes/{envelopeId}/documents";

            // Ler e converter o arquivo para Base64
            string contentBase64;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var contentBytes = memoryStream.ToArray();
                contentBase64 = Convert.ToBase64String(contentBytes);
            }

            // Montar o corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "documents",
                    attributes = new
                    {
                        filename = file.FileName, // Nome do arquivo
                        content_base64 = $"data:application/pdf;base64,{contentBase64}", // Formato correto do Base64
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
                return StatusCode((int)response.StatusCode, new { error = "Erro ao criar o documento", details = error });
            }
        }
    }
}
