namespace EventApi.Utility
{
    public static class ApiResponse
    {
        public static object SetResponse(ApiResponseStatus responseStatus, string message, object data)
        {
            return new { ResponseStatus = responseStatus, Message = message, Data = data };
        }
    }

    public enum ApiResponseStatus
    {
        Ok,

        Error,

        NotFound,

        NotValidate
    }
}