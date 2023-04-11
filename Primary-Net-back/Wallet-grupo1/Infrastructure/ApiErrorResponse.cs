namespace Wallet_grupo1.Infrastructure;

public class ApiErrorResponse
{
    public int Status { get; set; }
    public List<ResponseError>? Errors { get; set; }

    public class ResponseError
    {
        public string? Error { get; set; }
    }
}   