namespace EventApi.Utility
{
    public enum ResponseStatus
    {
        Ok,

        Error,

        NotFound,

        NotValidate
    }

    public class Message
    {
        public string Description { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }

    public class Response<T>
        where T : class
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public int TotalCount { get; set; }
    }
}