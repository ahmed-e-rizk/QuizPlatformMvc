using System.Collections;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace QuizPlatform.DTO.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
       
        public IFormFile Image { get; set; }

        public DateTime? Date { get; set; }
        public List<QuestionDto> Questions { get; set; }=new List<QuestionDto>();
         
    }
    public class QuizResultDto 
    {
        public int Id { get; set; } 

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTime? Date { get; set; }
        public List<QuestionDto> Questions { get; set; }
        
    }
}
