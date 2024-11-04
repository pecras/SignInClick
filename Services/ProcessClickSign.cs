using System.Net.Http.Headers;
using System.Text;
using SignInClick.DTOS;
using System.Text.Json;


namespace SignInClick.Services
{
    public class ProcessClickSign
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

         private readonly string _clickSignUrl;


        public ProcessClickSign(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _accessToken = configuration["Clicksign:AccessToken"] ?? string.Empty;
            _clickSignUrl = configuration["Clicksign:URl"] ?? string.Empty;
        }

        public async Task<string> CreateEnvelopeAsync( string name)
  {
    var url = $"{_clickSignUrl}/api/v3/envelopes";

    var requestBody = new
    {
        data = new
        {
            type = "envelopes",
            attributes = new
            {
                name = name,
                // Outros atributos podem ser adicionados aqui
            }
        }
    };

    var json = JsonSerializer.Serialize(requestBody);
    var content = new StringContent(json, Encoding.UTF8)
    {
        Headers = { ContentType = new MediaTypeHeaderValue("application/vnd.api+json") }
    };

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
    {
        Content = content
    };

    requestMessage.Headers.Add("Authorization", _accessToken);
    requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

    var response = await _httpClient.SendAsync(requestMessage);
    var result = await response.Content.ReadAsStringAsync();
   
    if (response.IsSuccessStatusCode)
    {
     using (JsonDocument doc = JsonDocument.Parse(result))
      {
        var root = doc.RootElement;
        if (root.TryGetProperty("data", out var dataElement) && 
            dataElement.TryGetProperty("id", out var idElement))
        {
            return idElement.GetString() ?? string.Empty; // Retorna o ID como string
        }
    }

          return "ID do envelope não encontrado."; 
        
    }
    else
    {
        return $"Erro ao criar o envelope: {result}";
    }
}
 
        public async Task<string> ProcessDocument(string envelopeId, IFormFile file )
        {
            if (file == null || file.Length == 0)
            {
                return "Nenhum arquivo foi enviado.";
            }

            var url = $"{_clickSignUrl}/api/v3/envelopes/{envelopeId}/documents";

            // Ler e converter o arquivo para Base64
            string contentBase64;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                contentBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            // Montar o corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "documents",
                    attributes = new
                    {
                        filename = file.FileName,
                        content_base64 = $"data:application/pdf;base64,{contentBase64}",
                    }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
           var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");

            

            // Criar a requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            requestMessage.Headers.Add("Authorization", _accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Enviar a requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verificar se a resposta foi bem-sucedida
            var result = await response.Content.ReadAsStringAsync();
             Console.WriteLine($"{result}");
            return response.IsSuccessStatusCode ? result : $"Erro ao criar o documento: {result}";
        }

        public async Task<string> CreateSigner(string envelopeId, string signer, string emailSigner)
        {
            var url = $"{_clickSignUrl}/api/v3/envelopes/{envelopeId}/signers";

            // Montar o corpo da requisição
            var requestBody = new
            {
                data = new
                {
                    type = "signers",
                    attributes = new
                    {
                        
                      name = signer,
                      email = emailSigner 
                       
                    }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");

             content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");

            // Criar a requisição HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            requestMessage.Headers.Add("Authorization", _accessToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            // Enviar a requisição
            var response = await _httpClient.SendAsync(requestMessage);

            // Verificar se a resposta foi bem-sucedida
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{result}");
            return response.IsSuccessStatusCode ? result : $"Erro ao criar o signatário: {result}";
        }

       }


    }


    public class CreateEnvelopeData
    {
        public string Name { get; set; } = string.Empty;
    }





   public class EnvelopeResponse
    {
        public EnvelopeData Data { get; set; } = new EnvelopeData();
    }

    public class EnvelopeData
    {
        public string Id { get; set; } = string.Empty;
    }



public class CreateProcessSigner
    {
       public string Name { get; set; }
      
        public string Documentation { get; set; }
      
        
    }

    public class CommunicateEventsDTO
    {
        public string DocumentSigned { get; set; }
        public string SignatureRequest { get; set; }
        public string SignatureReminder { get; set; }
    }

