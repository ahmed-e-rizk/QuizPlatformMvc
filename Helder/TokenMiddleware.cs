namespace QuizPlatform.Helder
{
    using Microsoft.AspNetCore.Http;
    using QuizPlatform.Models.Auth;
    using QuizPlatform.Response;
    using System.Threading.Tasks;

    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;

        public TokenMiddleware(RequestDelegate next, HttpClient httpClient)
        {
            _next = next;
            _httpClient = httpClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (accessToken == null && refreshToken != null)
            {
                // Refresh the token
                var x = JsonContent.Create(new TokenResultDto {RefreshToken = refreshToken});

                var response = await _httpClient.PostAsync("/RefreshToken", x);
               var res= await response.Content.ReadFromJsonAsync<Response<TokenResultDto>>();

                // Update the cookie with the new access token
                context.Response.Cookies.Append("AccessToken", res.Data.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(25)
                });
                context.Response.Cookies.Append("RefreshToken", res.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(30)
                });
            }

            await _next(context);
        }
    }
}
