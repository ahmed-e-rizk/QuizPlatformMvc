using Microsoft.AspNetCore.Mvc;
using QuizPlatform.DTO.Quiz;
using QuizPlatform.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuizPlatform.Controllers
{
    public class QuizController : Controller
    {

        private static QuizDto? _cachedQuizDto;
        private static MemoryStream? image;

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
        public async Task<IActionResult> Create([FromForm] QuizDto inDto)
        {
            var fileStream = inDto.Image.OpenReadStream();
            image = new MemoryStream();
            
                await fileStream.CopyToAsync(image);
                image.Seek(0, SeekOrigin.Begin);


            _cachedQuizDto = inDto;
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

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(_cachedQuizDto.Id.ToString()), "Id");
            content.Add(new StringContent(_cachedQuizDto.Name), "Name");
            content.Add(new StringContent(_cachedQuizDto.Description), "Description");
         //   content.Add(new StreamContent(image), "Image", _cachedQuizDto.Image.FileName);
            content.Add(new StringContent(_cachedQuizDto.Date.Value.ToString("o")), "Date");
            var questionsJson = System.Text.Json.JsonSerializer.Serialize(_cachedQuizDto.Questions);
            content.Add(new StringContent(questionsJson, System.Text.Encoding.UTF8, "application/json"), "Question");


         

            var fileContent = new StreamContent(image);
           fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_cachedQuizDto.Image.ContentType);

          //  content.Add(fileContent, "Image", _cachedQuizDto.Image.FileName);

            try
                {

                 

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_cachedQuizDto.Image.ContentType);
                fileContent.Headers.ContentLength = _cachedQuizDto.Image.Length;

                    content.Add(fileContent, "Image", _cachedQuizDto.Image.FileName);
            }
            catch (Exception ex)
                {

                    throw new Exception();
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
