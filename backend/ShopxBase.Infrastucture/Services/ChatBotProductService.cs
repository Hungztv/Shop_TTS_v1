using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Infrastructure.Services;

/// <summary>
/// Service chuyên phục vụ ChatBot: tìm kiếm sản phẩm thông minh & gợi ý recommendation
/// </summary>
public interface IChatBotProductService
{
    /// <summary>
    /// Tìm sản phẩm theo nhiều keyword, hỗ trợ match Name, Description, Brand, Category
    /// </summary>
    Task<List<ChatProductInfo>> SearchProductsAsync(string query, int maxResults = 5);

    /// <summary>
    /// Gợi ý sản phẩm tương tự (cùng category/brand)
    /// </summary>
    Task<List<ChatProductInfo>> GetSimilarProductsAsync(int productId, int maxResults = 5);

    /// <summary>
    /// Gợi ý sản phẩm bán chạy theo category
    /// </summary>
    Task<List<ChatProductInfo>> GetTrendingProductsAsync(int? categoryId = null, int maxResults = 5);

    /// <summary>
    /// Gợi ý sản phẩm theo khoảng giá
    /// </summary>
    Task<List<ChatProductInfo>> GetProductsByPriceRangeAsync(decimal? minPrice, decimal? maxPrice, string? category = null, int maxResults = 5);

    /// <summary>
    /// Lấy danh sách categories hiện có
    /// </summary>
    Task<List<string>> GetAvailableCategoriesAsync();
}

/// <summary>
/// DTO nhẹ chứa thông tin sản phẩm cho ChatBot context
/// </summary>
public class ChatProductInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? ShopName { get; set; }
    public decimal AverageScore { get; set; }
    public int RatingCount { get; set; }
    public int SoldOut { get; set; }
    public bool IsInStock { get; set; }
    public string? ShortDescription { get; set; }
}

public class ChatBotProductService : IChatBotProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public ChatBotProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _productRepository = unitOfWork.ProductRepository;
    }

    public async Task<List<ChatProductInfo>> SearchProductsAsync(string query, int maxResults = 5)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<ChatProductInfo>();

        // Tách keyword từ query
        var keywords = query.ToLower()
            .Split(new[] { ' ', ',', '.', '!', '?', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(k => k.Length >= 2)
            .Distinct()
            .ToList();

        if (!keywords.Any())
            return new List<ChatProductInfo>();

        // Dùng IProductRepository.SearchAsync cho keyword chính, 
        // sau đó bổ sung tìm theo Brand/Category
        var allResults = new List<Product>();

        // Tìm theo từng keyword
        foreach (var keyword in keywords.Take(5)) // Giới hạn 5 keyword
        {
            var results = await _productRepository.SearchAsync(keyword);
            allResults.AddRange(results);
        }

        // Tìm thêm theo Brand name
        var brandProducts = await _unitOfWork.Brands
            .FindAsync(b => !b.IsDeleted && keywords.Any(k => b.Name.ToLower().Contains(k)));
        var brandIds = brandProducts.Select(b => b.Id).ToList();

        if (brandIds.Any())
        {
            foreach (var brandId in brandIds.Take(3))
            {
                var bp = await _productRepository.GetByBrandAsync(brandId);
                allResults.AddRange(bp);
            }
        }

        // Tìm thêm theo Category name
        var catProducts = await _unitOfWork.Categories
            .FindAsync(c => !c.IsDeleted && keywords.Any(k => c.Name.ToLower().Contains(k)));
        var catIds = catProducts.Select(c => c.Id).ToList();

        if (catIds.Any())
        {
            foreach (var catId in catIds.Take(3))
            {
                var cp = await _productRepository.GetByCategoryAsync(catId);
                allResults.AddRange(cp);
            }
        }

        // Deduplicate + ranking: ưu tiên match nhiều keyword, bán chạy, rating cao
        var ranked = allResults
            .Where(p => !p.IsDeleted && p.Quantity > 0)
            .GroupBy(p => p.Id)
            .Select(g => new
            {
                Product = g.First(),
                MatchCount = g.Count() // Số lần xuất hiện = số keyword match
            })
            .OrderByDescending(x => x.MatchCount)
            .ThenByDescending(x => x.Product.SoldOut)
            .ThenByDescending(x => x.Product.AverageScore)
            .Take(maxResults)
            .ToList();

        // Load Brand/Category names
        var productIds = ranked.Select(r => r.Product.Id).ToList();
        var productsWithDetails = new List<ChatProductInfo>();

        foreach (var item in ranked)
        {
            var p = item.Product;
            var detail = await _productRepository.GetWithDetailsAsync(p.Id);
            productsWithDetails.Add(MapToInfo(detail ?? p));
        }

        return productsWithDetails;
    }

    public async Task<List<ChatProductInfo>> GetSimilarProductsAsync(int productId, int maxResults = 5)
    {
        var product = await _productRepository.GetWithDetailsAsync(productId);
        if (product == null)
            return new List<ChatProductInfo>();

        // Lấy sản phẩm cùng category + brand, bỏ chính nó
        var sameCategoryProducts = (await _productRepository.GetByCategoryAsync(product.CategoryId))
            .Where(p => p.Id != productId && p.Quantity > 0)
            .ToList();

        var sameBrandProducts = (await _productRepository.GetByBrandAsync(product.BrandId))
            .Where(p => p.Id != productId && p.Quantity > 0 && !sameCategoryProducts.Any(sc => sc.Id == p.Id))
            .ToList();

        // Kết hợp: ưu tiên cùng category trước
        var combined = sameCategoryProducts
            .Concat(sameBrandProducts)
            .OrderByDescending(p => p.SoldOut)
            .ThenByDescending(p => p.AverageScore)
            .Take(maxResults)
            .ToList();

        var results = new List<ChatProductInfo>();
        foreach (var p in combined)
        {
            var detail = await _productRepository.GetWithDetailsAsync(p.Id);
            results.Add(MapToInfo(detail ?? p));
        }

        return results;
    }

    public async Task<List<ChatProductInfo>> GetTrendingProductsAsync(int? categoryId = null, int maxResults = 5)
    {
        IEnumerable<Product> products;

        if (categoryId.HasValue)
        {
            products = (await _productRepository.GetByCategoryAsync(categoryId.Value))
                .Where(p => p.Quantity > 0)
                .OrderByDescending(p => p.SoldOut)
                .Take(maxResults);
        }
        else
        {
            products = await _productRepository.GetBestSellingAsync(maxResults);
        }

        var results = new List<ChatProductInfo>();
        foreach (var p in products)
        {
            var detail = await _productRepository.GetWithDetailsAsync(p.Id);
            results.Add(MapToInfo(detail ?? p));
        }

        return results;
    }

    public async Task<List<ChatProductInfo>> GetProductsByPriceRangeAsync(
        decimal? minPrice, decimal? maxPrice, string? category = null, int maxResults = 5)
    {
        // Build predicate
        var (items, _) = await _productRepository.GetFilteredAsync(
            p => (!minPrice.HasValue || p.Price >= minPrice.Value)
                 && (!maxPrice.HasValue || p.Price <= maxPrice.Value)
                 && p.Quantity > 0,
            1,
            50 // lấy nhiều hơn để filter category nếu cần
        );

        var filtered = items.ToList();

        // Filter theo category name nếu có
        if (!string.IsNullOrEmpty(category))
        {
            var cat = (await _unitOfWork.Categories
                .FindAsync(c => !c.IsDeleted && c.Name.ToLower().Contains(category.ToLower())))
                .FirstOrDefault();

            if (cat != null)
            {
                filtered = filtered.Where(p => p.CategoryId == cat.Id).ToList();
            }
        }

        var ranked = filtered
            .OrderByDescending(p => p.SoldOut)
            .ThenByDescending(p => p.AverageScore)
            .Take(maxResults)
            .ToList();

        var results = new List<ChatProductInfo>();
        foreach (var p in ranked)
        {
            var detail = await _productRepository.GetWithDetailsAsync(p.Id);
            results.Add(MapToInfo(detail ?? p));
        }

        return results;
    }

    public async Task<List<string>> GetAvailableCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories
            .FindAsync(c => !c.IsDeleted);

        return categories.Select(c => c.Name).OrderBy(n => n).ToList();
    }

    private static ChatProductInfo MapToInfo(Product p)
    {
        return new ChatProductInfo
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            Price = p.Price,
            Image = p.Image ?? "",
            BrandName = p.Brand?.Name ?? "",
            CategoryName = p.Category?.Name ?? "",
            ShopName = p.Shop?.Name,
            AverageScore = p.AverageScore,
            RatingCount = p.RatingCount,
            SoldOut = p.SoldOut,
            IsInStock = p.Quantity > 0,
            ShortDescription = p.Description?.Length > 100 ? p.Description[..100] + "..." : p.Description
        };
    }
}
