namespace EdgeProject.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            this.StatusCode = statusCode;
            this.Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request, You have made!!",
                401 => "Authorized, you are not",
                404 => "Resourses are not found",
                500 => "Server Error",
                _ => null
            };
        }
    }
}
