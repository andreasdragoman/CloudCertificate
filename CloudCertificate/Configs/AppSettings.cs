namespace CloudCertificate.Configs
{
    public class AppSettings
    {
        public AzureFunctions AzureFunctions { get; set; }
    }

    public class AzureFunctions
    {
        public string BaseURL { get; set; }
        public string GetPersons { get; set; }
    }
}
