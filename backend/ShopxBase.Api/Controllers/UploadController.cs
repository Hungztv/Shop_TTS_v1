using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Upload;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ISupabaseStorageService _storageService;
    private readonly ILogger<UploadController> _logger;

    public UploadController(
        ISupabaseStorageService storageService,
        ILogger<UploadController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("product")]
    [Authorize]
    public async Task<IActionResult> UploadProductImage(IFormFile file)
    {
        var result = await _storageService.UploadAsync(file, "products");
        return HandleUploadResult(result);
    }

    [HttpPost("category")]
    [Authorize]
    public async Task<IActionResult> UploadCategoryImage(IFormFile file)
    {
        var result = await _storageService.UploadAsync(file, "categories");
        return HandleUploadResult(result);
    }

    [HttpPost("brand")]
    [Authorize]
    public async Task<IActionResult> UploadBrandImage(IFormFile file)
    {
        var result = await _storageService.UploadAsync(file, "brands");
        return HandleUploadResult(result);
    }

    [HttpPost("avatar")]
    [Authorize]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var result = await _storageService.UploadAsync(file, "avatars");
        return HandleUploadResult(result);
    }

    [HttpPost("multiple")]
    [Authorize]
    public async Task<IActionResult> UploadMultiple(
        [FromForm] List<IFormFile> files,
        [FromQuery] string bucket = "products",
        [FromQuery] string? folder = null)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest(new { success = false, message = "Không có file nào được gửi" });
        }

        if (files.Count > 10)
        {
            return BadRequest(new { success = false, message = "Chỉ được upload tối đa 10 file một lần" });
        }

        var results = await _storageService.UploadMultipleAsync(files, bucket, folder);
        
        var successCount = results.Count(r => r.Success);
        var failCount = results.Count(r => !r.Success);

        return Ok(new
        {
            success = failCount == 0,
            message = $"Upload thành công {successCount}/{results.Count} file",
            data = results.Where(r => r.Success).Select(r => new { r.Url, r.FileName }),
            errors = results.Where(r => !r.Success).Select(r => r.Error)
        });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteFile(
        [FromQuery] string filePath,
        [FromQuery] string bucket)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(bucket))
        {
            return BadRequest(new { success = false, message = "FilePath và Bucket là bắt buộc" });
        }

        var success = await _storageService.DeleteFileAsync(filePath, bucket);
        
        if (success)
        {
            return Ok(new { success = true, message = "Xóa file thành công" });
        }

        return BadRequest(new { success = false, message = "Không thể xóa file" });
    }

    [HttpGet("url")]
    public IActionResult GetPublicUrl(
        [FromQuery] string filePath,
        [FromQuery] string bucket)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(bucket))
        {
            return BadRequest(new { success = false, message = "FilePath và Bucket là bắt buộc" });
        }

        var url = _storageService.GetPublicUrl(filePath, bucket);
        return Ok(new { success = true, url });
    }

    private IActionResult HandleUploadResult(UploadResult result)
    {
        if (result.Success)
        {
            return Ok(new
            {
                success = true,
                message = "Upload thành công",
                data = new
                {
                    url = result.Url,
                    filePath = result.FilePath,
                    fileName = result.FileName,
                    fileSize = result.FileSize
                }
            });
        }

        return BadRequest(new
        {
            success = false,
            message = result.Error ?? "Upload thất bại"
        });
    }
}
