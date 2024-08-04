

namespace QuizPlatform.Response
{
    public interface IResponse<T>
    {
        public bool IsSuccess { get; set; }
        public List<TErrorField> Errors { get; set; }
        public T Data { get; set; }
       

    }
}
