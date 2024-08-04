using Microsoft.AspNetCore.Mvc;
using QuizPlatform.DTO.Quiz;
using QuizPlatform.Response;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuizPlatform.Controllers
{
    public class QuizController : Controller
    {

        private static QuizDto? _cachedQuizDto;
        private readonly HttpClient _httpClient;
        public QuizController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Quiz");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] QuizDto inDto)
        {

            _cachedQuizDto= inDto;
            return Redirect("/Quiz/CreateQuestions");
        }


        [HttpGet]
        public IActionResult CreateQuestions()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestions([FromBody] List<QuestionDto> inDto)
        {

            _cachedQuizDto.Questions= inDto;

            using var content = new MultipartFormDataContent();

            // Serialize the QuizDto properties
            content.Add(new StringContent(_cachedQuizDto.Id.ToString()), "Id");
            content.Add(new StringContent(_cachedQuizDto.Name), "Name");
            if (_cachedQuizDto.Description != null)
            {
                content.Add(new StringContent(_cachedQuizDto.Description), "Description");
            }
            if (_cachedQuizDto.Date.HasValue)
            {
                content.Add(new StringContent(_cachedQuizDto.Date.Value.ToString("o")), "Date");
            }

            if (_cachedQuizDto.Image != null)
            {
                try
                {

                    using var memoryStream = new MemoryStream();
                    await _cachedQuizDto.Image.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_cachedQuizDto.Image.ContentType);

                    content.Add(fileContent, "Image", _cachedQuizDto.Image.FileName);

                                   
                }
                catch (Exception ex)
                {
                                       return StatusCode(500, "Error processing file.");
                }
            }

            if (_cachedQuizDto.Questions != null && _cachedQuizDto.Questions.Count > 0)
            {
                var questionsJson = System.Text.Json.JsonSerializer.Serialize(_cachedQuizDto.Questions);
                content.Add(new StringContent(questionsJson, System.Text.Encoding.UTF8, "application/json"), "Questions");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["AccessToken"]);

            var response = await _httpClient.PostAsync("Quiz/CreateQuiz", content);
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadFromJsonAsync<Response<bool>>();
                if (!resp.IsSuccess)
                {
                    ViewData["Error"] = resp.Errors;
                    return View();
                }
                return Redirect("/Quiz/Create");

            }
            else
            {
                return View();
            }

        }
    }
}
