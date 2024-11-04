namespace SignInClick.Model
{
    public class WebhookResponse
    {
        public string Event { get; set; } // Nome do evento (e.g. signer.completed)
      
        public WebhookData Data { get; set; } // Dados do evento
    }

    public class WebhookData
    {
        public DocumentInfo Document { get; set; } // Informações do documento
        public SignerInfo Signer { get; set; } // Informações do signatário (se aplicável)
    }

    public class DocumentInfo
    {
        public string Key { get; set; } // Chave do documento
        public string Status { get; set; } // Status do documento (e.g. completed, pending)
        public DateTime CompletedAt { get; set; } // Data de finalização do documento
    }

    public class SignerInfo
    {
        public string Name { get; set; } // Nome do signatário
        public string Email { get; set; } // E-mail do signatário
        public string Action { get; set; } // Ação do signatário (e.g. sign)
        public DateTime SignedAt { get; set; } // Data em que o signatário assinou
    }


    public class CreateWebhookRequest
    {
        public string Endpoint { get; set; }  // URL para receber o webhook
        public string Status { get; set; }    // Status (e.g., active)
        public List<string> Events { get; set; } // Lista de eventos (e.g., "sign", "update_auto_close")

    }

    public class WebhookEvent
    {
        public string Name { get; set; } // Nome do evento (e.g., "add_signer")
        public WebhookEventData Data { get; set; } // Dados do evento
        public DateTime OccurredAt { get; set; } // Data em que o evento ocorreu
    }

    public class WebhookEventData
    {
        public UserInfo User { get; set; }
        public AccountInfo Account { get; set; }
        public List<SignerInfo> Signers { get; set; }
    }

    public class UserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class AccountInfo
    {
        public string Key { get; set; }
    }

    public class SignerInfoEvent 
    {
        public string Key { get; set; }
        public string RequestSignatureKey { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SignAs { get; set; }
        public List<string> Auths { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberHash { get; set; }
    }

    public class WebhookPayload
    {
        public WebhookEvent Event { get; set; }
        public DocumentInfo Document { get; set; }
        public List<SignerInfo> Signers { get; set; }
        public List<WebhookEvent> Events { get; set; }
    }

}
