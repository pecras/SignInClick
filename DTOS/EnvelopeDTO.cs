namespace SignInClick.DTOS

{
public class CreateEnvelopeRequestDTOs
{
        public string Name { get; set; }= string.Empty;
        public string Locale { get; set; } = "pt-BR"; // Definir valor padrão
        public bool AutoClose { get; set; } = true;   // Definir valor padrão
       
        public bool BlockAfterRefusal { get; set; } = true; // Definir valor padrão

        public string DeadlineAt { get; set; } = string.Empty; // Formato ISO 8601 esperado
    }

    public class EnvelopeCreatedResponseDTO
    {
        public string EnvelopeId { get; set; } =string.Empty;
    }

    public class CreateEnvelopeData
    {
        public string Name { get; set; } = string.Empty;
    }


public class EnvelopeResponse
{
    public EnvelopeData Data { get; set; }
}

public class EnvelopeData
{
    public string Id { get; set; }
    public string Type { get; set; }
    public EnvelopeLinks Links { get; set; }
    public EnvelopeAttributes Attributes { get; set; }
    public EnvelopeRelationships Relationships { get; set; }
}

public class EnvelopeLinks
{
    public string Self { get; set; }
}

public class EnvelopeAttributes
{
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime DeadlineAt { get; set; }
    public string Locale { get; set; }
    public bool AutoClose { get; set; }
    public bool RubricEnabled { get; set; }
    public int RemindInterval { get; set; }
    public bool BlockAfterRefusal { get; set; }
    public object DefaultMessage { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}

public class EnvelopeRelationships
{
    public EnvelopeRelationship Folder { get; set; }
    public EnvelopeRelationship Documents { get; set; }
    public EnvelopeRelationship Signers { get; set; }
    public EnvelopeRelationship Requirements { get; set; }
}

public class EnvelopeRelationship
{
    public EnvelopeLinks Links { get; set; }
}



}

