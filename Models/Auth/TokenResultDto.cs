namespace QuizPlatform.Models.Auth
{
    public class TokenResultDto
    {
        public string? Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
