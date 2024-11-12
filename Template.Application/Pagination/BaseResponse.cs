namespace Template.Application.Pagination
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public string? Message { get; set; }

        public BaseResponse(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }



    }
}
