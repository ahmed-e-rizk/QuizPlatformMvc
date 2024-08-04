

namespace QuizPlatform.Response
{
    public class Response<T> : IResponse<T>
    {
        public bool IsSuccess { get; set; }
        public List<TErrorField> Errors { get; set; } = new List<TErrorField>();
        public T? Data { get; set; }
    }


    public class TErrorField
    {
        public string FieldName { get; set; } = string.Empty;
        public string Code { get; set; }
        public string Message { get; set; }
    }

    
}
