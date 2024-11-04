namespace SignInClick.DTOS
{
    public class CreateDocumentRequestDTO
    {
        public string Filename { get; set; }     // Nome do arquivo
        public string ContentBase64 { get; set; } // Conteúdo do arquivo em Base64 (sem o prefixo)
        public string Metadata { get; set; }      // Metadados do documento (opcional)
    }

    public class CreateDocumentResponseDTO
    {
        public string Id { get; set; }              // ID do documento criado
        public string Filename { get; set; }         // Nome do arquivo
        public string Status { get; set; }           // Status do documento
        public string Metadata { get; set; }         // Metadados do documento (se retornados pela API)
        public DateTime CreatedAt { get; set; }      // Data de criação
        // Adicione mais propriedades conforme necessário
    }
}
