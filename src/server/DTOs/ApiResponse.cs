namespace server.DTOs
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T> { Status = "Success", Message = message, Data = data };
        }

        public static ApiResponse<T> Error(string message = "Error")
        {
            return new ApiResponse<T> { Status = "Error", Message = message };
        }
    }

    public class ApiResponse
    {
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = string.Empty;

        public static ApiResponse Success(string message = "Success")
        {
            return new ApiResponse { Status = "Success", Message = message };
        }

        public static ApiResponse Error(string message = "Error")
        {
            return new ApiResponse { Status = "Error", Message = message };
        }
    }
}
