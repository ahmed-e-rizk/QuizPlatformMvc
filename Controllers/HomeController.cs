using Microsoft.AspNetCore.Mvc;
using QuizPlatform.Models;
using QuizPlatform.Response;
using System.Diagnostics;
using QuizPlatform.DTO.Quiz;

namespace QuizPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Quiz");
        }
        

        public async Task<IActionResult> Index()
        {
            var result = await _httpClient.GetAsync("Quiz/GetAllQuiz");
            if (result.IsSuccessStatusCode)
            {
                var resp = await result.Content.ReadFromJsonAsync<Response<List<QuizResultDto>>>();
                if (!resp.IsSuccess)
                {
                    ViewData["Error"] = resp.Errors;
                    return View();
                }
                return View(resp.Data);

            }
            else
            {
                return View();
            }

            return Redirect("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
