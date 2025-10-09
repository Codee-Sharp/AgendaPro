namespace AgendaPro.Api.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string>? Errors { get; set; }
        public string TraceId { get; set; }

        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
            TraceId = Guid.NewGuid().ToString();
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
            TraceId = Guid.NewGuid().ToString();
        }
    }


}
