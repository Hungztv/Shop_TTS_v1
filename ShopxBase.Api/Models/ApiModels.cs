namespace ShopxBase.Api.Models;

/// <summary>
/// API Response wrapper
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public ApiResponse(T data, string message = "Success")
    {
        Success = true;
        Data = data;
        Message = message;
    }

    public ApiResponse(string message, List<string> errors)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }
}

/// <summary>
/// Pagination request model
/// </summary>
public class PaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Pagination response model
/// </summary>
public class PaginationResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
