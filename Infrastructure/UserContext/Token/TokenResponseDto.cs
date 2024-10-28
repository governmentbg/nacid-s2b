namespace Infrastructure.Token
{
    public class TokenResponseDto
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Token_type { get; set; }
        public string ClientId { get; set; }
    }
}
