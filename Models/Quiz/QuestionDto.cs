namespace QuizPlatform.DTO.Quiz
{
    public class QuestionDto
    {



        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int AnswerType { get; set; }
        public AnswerDto? Answer { get; set; }
        public List<OptionsDto>? Options { get; set; }
    }
    public class CreateQuestionDto
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public int QuizId { get; set; }

        public int AnswerType { get; set; }
        public AnswerDto? Answer { get; set; }
        public List<OptionsDto>? Options { get; set; }
    }

}
