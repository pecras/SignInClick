using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignInClick.Model;
using SignInClick.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SignInClick.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookCallbackController : ControllerBase
    {
        private readonly ClicksignService _clicksignService;
        private readonly ILogger<WebhookCallbackController> _logger;

        public WebhookCallbackController(ClicksignService clicksignService, ILogger<WebhookCallbackController> logger)
        {
            _clicksignService = clicksignService;
            _logger = logger;
        }

        // Método para criar um webhook
        [HttpPost("webhook/create")]
        public async Task<IActionResult> CreateWebhook([FromBody] CreateWebhookRequest request)
        {
            // Verifica se a entrada é válida
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Requisição inválida para criação de webhook: {@Request}", request);
                return BadRequest(ModelState);
            }

            try
            {
                // Criação do webhook usando o serviço
                var result = await _clicksignService.CreateWebhookAsync(
                    request.Endpoint,
                    request.Status,
                    request.Events
                );

                _logger.LogInformation("Webhook criado com sucesso. Endpoint: {Endpoint}", request.Endpoint);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // Log do erro para diagnóstico
                _logger.LogError(ex, "Erro ao criar webhook para o endpoint: {Endpoint}", request.Endpoint);
                return StatusCode(500, new { error = "Erro interno ao criar o webhook.", details = ex.Message });
            }
        }
    }

    // Modelo de requisição para criar o webhook
    public class CreateWebhookRequest
    {
        [Required]
        [Url(ErrorMessage = "O campo Endpoint deve ser uma URL válida.")]
        public string Endpoint { get; set; }

        [Required]
        [RegularExpression("^(active|inactive)$", ErrorMessage = "Status deve ser 'active' ou 'inactive'.")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Deve haver ao menos um evento.")]
        public List<string> Events { get; set; }
    }
}
