namespace SignInClick.Model;
 public class NotificationRequest
{
    public string AccessToken { get; set; } = string.Empty; // Token de acesso da Clicksign
    public string RequestSignatureKey { get; set; } = string.Empty; // Chave da solicitação de assinatura
    public string Message { get; set; } = string.Empty; // Mensagem personalizada
    public string Url { get; set; } = string.Empty; // URL opcional
}
