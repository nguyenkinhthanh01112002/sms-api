using smsCoffee.WebAPI.DTOs.Common;

namespace smsCoffee.WebAPI.Services.CommonService
{
    public static class ResponseFactory
    {
        public static ApiResponse<T> CreateResponse<T>(
            bool status,
            string path,
            string message,
            int statusCode,
            T? data = default)
        {
            return new ApiResponse<T>
            {
                Status = status,
                Path = path,
                Message = message,
                StatusCode = statusCode,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }
        public static ApiResponse<T> Success<T>(string path, T data, string message = "success", int statusCode = 200)
        {
            return CreateResponse(true, path, message, statusCode, data);
        }

        public static ApiResponse<T> Error<T>(string path, string message, int statusCode = 400)
        {
            return CreateResponse<T>(false, path, message, statusCode);
        }
    }
}
