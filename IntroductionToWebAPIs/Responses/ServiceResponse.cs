namespace IntroductionToWebAPIs.Responses
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ServiceResponse Ok(string message = "Success")
        {
            return new ServiceResponse
            {
                Success = true,
                Message = message
            };
        }

        public static ServiceResponse Fail(string message = "Failure")
        {
            return new ServiceResponse
            {
                Success = false,
                Message = message
            };
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T data, string message = "Success")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static new ServiceResponse<T> Fail(string message = "Failure")
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
