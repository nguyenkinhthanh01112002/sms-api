namespace smsCoffee.WebAPI.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
