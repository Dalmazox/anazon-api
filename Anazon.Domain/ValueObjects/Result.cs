namespace Anazon.Domain.ValueObjects
{
    public class Result
    {
        public Result(object data, string message, int statusCode)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
