namespace EdgeProject.APIs.Errors
{
    public class ApiExceptionResponse:ApiErrorResponse
    {

        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode,string? Message = null, string? Details= null):base(StatusCode,Message)
        {
            this.Details = Details;
        }

    }
}
