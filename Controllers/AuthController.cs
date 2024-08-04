using Microsoft.AspNetCore.Mvc;
using QuizPlatform.Models.Auth;
using QuizPlatform.Response;

namespace QuizPlatform.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Quiz");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromForm] UserInputModel input)
        {
            var x = JsonContent.Create(input);
            var result = await _httpClient.PostAsync("Auth/Register", x);
            if (result.IsSuccessStatusCode)
            {
                var resp = await result.Content.ReadFromJsonAsync<Response<bool>>();
                if (!resp.IsSuccess)
                {
                    ViewData["Error"] = resp.Errors;
                    return View();
                }
            }
            else
            {
                return View();
            }

            return Redirect("Login");
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm] LoginDto inputDto)
        {
            var x = JsonContent.Create(inputDto);

            var result = await _httpClient.PostAsync("Auth/Login", x);
            if (result.IsSuccessStatusCode)
            {
                var resp = await result.Content.ReadFromJsonAsync<Response<TokenResultDto>>();
                if (!resp.IsSuccess)
                {
                    ViewData["Error"] = resp.Errors;
                    return View();
                }

                Response.Cookies.Append("AccessToken", resp.Data.Token, new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true, 
                    Expires = DateTime.UtcNow.AddMinutes(25) 
                });
                Response.Cookies.Append("RefreshToken", resp.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true, 
                    Expires = DateTime.UtcNow.AddDays(30) 
                });
                return Redirect("/Home/Index");
            }
            return View();

        }


    }
}
