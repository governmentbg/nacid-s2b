namespace Integrations.EAuth.Dtos
{
    public class SamlRequestDto
    {
        public string PostUrl { get; set; }
        public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }
    }
}
