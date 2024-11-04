
using SignInClick.Model;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace SignInClick.Services
{
    public class ClicksignService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        public ClicksignService(HttpClient httpClient, ILogger<ClicksignService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
    
            _configuration = configuration;
        }

        // Método para autenticação (token obtido da configuração)
        private void SetAuthenticationHeader()
        {
            var apiKey = _configuration["Clicksign:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
              
                throw new Exception("API Key não configurada.");
            }

            

            // Mascara a API Key nos logs (por questões de segurança)
            string maskedApiKey = apiKey.Substring(0, 4) + new string('*', apiKey.Length - 4);
           

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authentication", apiKey);
        }

       
        public async Task<string> CreateWebhookAsync(string endpoint, string status, List<string> events)
        {
            try
            {
                SetAuthenticationHeader();

                var UrlClick=_configuration["Clicksign:URl"]; 
                var url = $"{UrlClick}/api/v2/webhooks";
                var accessToken = _configuration["Clicksign:AccessToken"];
                var HMACSHA256Secret =_configuration["Clicksign:HMACSHA256Secret"];
                var webhookRequest = new
                {
                    endpoint = endpoint,
                    status = status,
                    events = events
                };

                var fullUrl = $"{url}?access_token={accessToken}";
                var response = await _httpClient.PostAsJsonAsync(fullUrl, webhookRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content; // Retorna a resposta do webhook
                }

                var responseContent = await response.Content.ReadAsStringAsync();


         
                throw new Exception("Erro ao cadastrar o webhook.");
            }
            catch (Exception ex)
            {


             
                throw;
            }
        }

        // Método para disparar o webhook
        public async Task SendWebhookAsync(WebhookPayload payload, string webhookUrl, string hmacSignature)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                };

                // Adicionando os headers necessários
                request.Headers.Add("Event", "upload");
                request.Headers.Add("Content-Hmac", $"sha256={hmacSignature}");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
               
                    throw new Exception("Falha ao disparar o webhook.");
                }
            }
            catch (Exception ex)
            {
           
                throw;
            }
        }
    }

    // Models
   

   
}
