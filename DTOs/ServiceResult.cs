namespace EventManagementApi.DTOs;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public ErrorType? ErrorType { get; set; }
}

public enum ErrorType
{
    NotFound,
    ValidationError,
    BusinessRuleViolation,
    Conflict,
    DatabaseError
} 