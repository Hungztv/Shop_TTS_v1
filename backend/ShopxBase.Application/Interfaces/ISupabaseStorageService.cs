using Microsoft.AspNetCore.Http;  //  IFormFile
using ShopxBase.Application.DTOs.Upload;  //  UploadResult

namespace ShopxBase.Application.Interfaces;

public interface ISupabaseStorageService
{
    Task<UploadResult> UploadFileAsync(string bucketName, string filePath, Stream fileStream, string? folder = null);
    Task<UploadResult> UploadAsync(IFormFile file, string bucket, string? folder = null);
    Task<bool> DeleteFileAsync(string filePath, string bucket);
    string GetPublicUrl(string filePath, string bucket);
    Task<List<UploadResult>> UploadMultipleAsync(IEnumerable<IFormFile> files, string bucket, string? folder = null);
}