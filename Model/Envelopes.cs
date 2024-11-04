namespace SignInClick.Model
{
    // Models/EnvelopeModel.cs
    public class EnvelopeModel
    {
        public string Name { get; set; }
        public string Locale { get; set; }
        public bool AutoClose { get; set; }
        public int RemindInterval { get; set; }
        public bool BlockAfterRefusal { get; set; }
        public DateTime DeadlineAt { get; set; }
    }

    // Models/EnvelopeResponseModel.cs
    public class EnvelopeResponseModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }


}
