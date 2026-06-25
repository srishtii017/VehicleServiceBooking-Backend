namespace User_Management.Models
{
    // Base class
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    // Generic class inherits from base
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public ApiResponse(bool success, string message, T? data = default)
            : base(success, message)   // calls base constructor
        {
            Data = data;
        }
    }
}