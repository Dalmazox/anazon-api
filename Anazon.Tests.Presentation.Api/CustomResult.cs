namespace Anazon.Tests.Presentation.Api
{
    public class CustomResult<T> where T : class
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
