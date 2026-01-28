using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopxBase.Application.DTOs.Upload;
using ShopxBase.Application.Interfaces;
using ShopxBase.Application.Settings;

namespace ShopxBase.Infrastructure.Services;

public class SupabaseStorageService : ISupabaseStorageService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseSettings _settings;
    private readonly ILogger<SupabaseStorageService> _logger;
    
    private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp", "image/svg+xml"
    };
    
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg"
    };
    
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    public SupabaseStorageService(
        HttpClient httpClient,
        IOptions<SupabaseSettings> settings,
        ILogger<SupabaseStorageService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri($"{_settings.Url}/storage/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", _settings.AnonKey);
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.ServiceRoleKey);
    }

    public async Task<UploadResult> UploadFileAsync(
        string bucketName, 
        string filePath, 
        Stream fileStream,
        string? folder = null)
    {
        try
        {
            var fullPath = string.IsNullOrEmpty(folder) 
                ? filePath 
                : $"{folder.TrimEnd('/')}/{filePath}";

            using var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(filePath));

            var response = await _httpClient.PostAsync(
                $"object/{bucketName}/{fullPath}", 
                content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var publicUrl = GetPublicUrl(fullPath, bucketName);
                _logger.LogInformation("File uploaded successfully: {Url}", publicUrl);
                
                return new UploadResult(
                    Success: true,
                    Url: publicUrl,
                    FilePath: fullPath,
                    FileName: Path.GetFileName(filePath),
                    FileSize: fileStream.Length,
                    Error: null
                );
            }

            _logger.LogWarning("Upload failed: {Response}", responseContent);
            return new UploadResult(false, null, null, null, null, responseContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to Supabase Storage");
            return new UploadResult(false, null, null, null, null, ex.Message);
        }
    }

    public async Task<UploadResult> UploadAsync(IFormFile file, string bucket, string? folder = null)
    {
        // Validate file
        var validationError = ValidateFile(file);
        if (validationError != null)
        {
            return new UploadResult(false, null, null, null, null, validationError);
        }

        // Generate unique filename
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";

        await using var stream = file.OpenReadStream();
        return await UploadFileAsync(bucket, uniqueFileName, stream, folder);
    }

    public async Task<bool> DeleteFileAsync(string filePath, string bucket)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"object/{bucket}/{filePath}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("File deleted successfully: {Bucket}/{FilePath}", bucket, filePath);
                return true;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Delete failed: {Response}", responseContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from Supabase Storage");
            return false;
        }
    }

    public string GetPublicUrl(string filePath, string bucket)
    {
        return $"{_settings.Url}/storage/v1/object/public/{bucket}/{filePath}";
    }

    public async Task<List<UploadResult>> UploadMultipleAsync(
        IEnumerable<IFormFile> files, 
        string bucket, 
        string? folder = null)
    {
        var results = new List<UploadResult>();
        
        foreach (var file in files)
        {
            var result = await UploadAsync(file, bucket, folder);
            results.Add(result);
        }
        
        return results;
    }

    private string? ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return "File không được để trống";
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return $"File vượt quá kích thước cho phép (tối đa {MaxFileSizeBytes / 1024 / 1024}MB)";
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            return $"Định dạng file không được hỗ trợ. Chỉ chấp nhận: {string.Join(", ", AllowedExtensions)}";
        }

        if (!string.IsNullOrEmpty(file.ContentType) && !AllowedImageTypes.Contains(file.ContentType))
        {
            return "Loại file không được hỗ trợ";
        }

        return null;
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}
