namespace BackendApi.DTOs.Common
{
    public class ErrorResponse
    {
        public string? Message { get; set; }
        public string? Code { get; set; }
        public string? CorrelationId { get; set; }
        public object? Details { get; set; }
    }
}
