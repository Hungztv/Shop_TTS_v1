using Microsoft.AspNetCore.Http;  
namespace ShopxBase.Application.DTOs.Upload;
public record UploadResult(
    bool Success,
    string? Url,
    string? FilePath,
    string? FileName,
    long? FileSize,
    string? Error
);
public record UploadRequest(
    IFormFile File,
    string Bucket,
    string? Folder = null
);