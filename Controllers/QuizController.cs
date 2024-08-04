using Microsoft.AspNetCore.Mvc;
using QuizPlatform.DTO.Quiz;

namespace QuizPlatform.Controllers
{
    public class QuizController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]QuizDto inDto)
        {
            var c = new QuizDto();
   
   

            return View();
        }
    }
}
