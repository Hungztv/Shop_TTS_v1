using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;
using System.Globalization;
using System.Text;

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

        var normalizedQuery = NormalizeText(query);

        // Tách keyword từ query (loại stop words)
        var keywords = normalizedQuery
            .Split(new[] { ' ', ',', '.', '!', '?', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(k => k.Length >= 2 && !SearchStopWords.Contains(k))
            .Distinct()
            .ToList();

        if (!keywords.Any())
            return new List<ChatProductInfo>();

        // Detect search intent (phone/laptop/accessory...)
        var searchType = DetectSearchType(normalizedQuery, keywords);

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

        // Deduplicate + relevance scoring theo intent
        var ranked = allResults
            .Where(p => !p.IsDeleted && p.Quantity > 0)
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .ToList();

        var scored = new List<(Product Product, double Score)>();

        foreach (var p in ranked)
        {
            var detail = await _productRepository.GetWithDetailsAsync(p.Id);
            if (detail == null) continue;

            var score = CalculateRelevanceScore(detail, keywords, searchType);
            if (score > 0)
            {
                scored.Add((detail, score));
            }
        }

        var best = scored
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Product.SoldOut)
            .ThenByDescending(x => x.Product.AverageScore)
            .Take(maxResults)
            .ToList();

        var productsWithDetails = new List<ChatProductInfo>();

        foreach (var item in best)
        {
            productsWithDetails.Add(MapToInfo(item.Product));
        }

        // Fallback: nếu query phone/laptop mà không ra kết quả scoring,
        // lấy sản phẩm theo category phù hợp để tránh trả lời text-only.
        if (!productsWithDetails.Any() && (searchType == SearchType.Phone || searchType == SearchType.Laptop))
        {
            var fallbackProducts = await GetCategoryFallbackProductsAsync(searchType, maxResults);
            foreach (var fallback in fallbackProducts)
            {
                var detail = await _productRepository.GetWithDetailsAsync(fallback.Id);
                if (detail != null)
                {
                    productsWithDetails.Add(MapToInfo(detail));
                }
            }
        }

        return productsWithDetails;
    }

    private async Task<List<Product>> GetCategoryFallbackProductsAsync(SearchType searchType, int maxResults)
    {
        var terms = searchType == SearchType.Phone ? PhoneTerms : LaptopTerms;

        var allCategories = await _unitOfWork.Categories
            .FindAsync(c => !c.IsDeleted);

        var categories = allCategories
            .Where(c =>
            {
                var normalizedName = NormalizeText(c.Name);
                return terms.Any(t => normalizedName.Contains(t) || t.Contains(normalizedName));
            })
            .ToList();

        var categoryIds = categories.Select(c => c.Id).Distinct().Take(5).ToList();
        if (!categoryIds.Any())
            return new List<Product>();

        var candidates = new List<Product>();
        foreach (var categoryId in categoryIds)
        {
            var items = await _productRepository.GetByCategoryAsync(categoryId);
            candidates.AddRange(items.Where(p => !p.IsDeleted && p.Quantity > 0));
        }

        return candidates
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .OrderByDescending(p => p.SoldOut)
            .ThenByDescending(p => p.AverageScore)
            .Take(maxResults)
            .ToList();
    }

    private static double CalculateRelevanceScore(Product product, List<string> keywords, SearchType searchType)
    {
        var name = NormalizeText(product.Name);
        var desc = NormalizeText(product.Description ?? "");
        var category = NormalizeText(product.Category?.Name ?? "");
        var brand = NormalizeText(product.Brand?.Name ?? "");

        var score = 0.0;

        // Intent-based boosting/filtering
        if (searchType == SearchType.Phone)
        {
            if (ContainsAny(name, PhoneTerms) || ContainsAny(category, PhoneTerms))
                score += 60;

            if (ContainsAny(name, AccessoryTerms) || ContainsAny(category, AccessoryTerms))
                score -= 80;
        }
        else if (searchType == SearchType.Laptop)
        {
            if (ContainsAny(name, LaptopTerms) || ContainsAny(category, LaptopTerms))
                score += 60;

            if (ContainsAny(name, AccessoryTerms) || ContainsAny(category, AccessoryTerms))
                score -= 50;
        }
        else if (searchType == SearchType.Accessory)
        {
            if (ContainsAny(name, AccessoryTerms) || ContainsAny(category, AccessoryTerms))
                score += 50;
        }

        // Keyword match scoring
        foreach (var keyword in keywords)
        {
            if (name.Contains(keyword)) score += 20;
            if (brand.Contains(keyword)) score += 12;
            if (category.Contains(keyword)) score += 10;
            if (desc.Contains(keyword)) score += 4;
        }

        // Quality signals
        score += Math.Min(12, product.SoldOut / 20.0);
        score += (double)product.AverageScore * 2;

        // Final hard filter for phone/laptop to avoid noise
        if (searchType == SearchType.Phone)
        {
            var isPhoneLike = ContainsAny(name, PhoneTerms) || ContainsAny(category, PhoneTerms);
            if (!isPhoneLike) score -= 100;
        }

        if (searchType == SearchType.Laptop)
        {
            var isLaptopLike = ContainsAny(name, LaptopTerms) || ContainsAny(category, LaptopTerms);
            if (!isLaptopLike) score -= 100;
        }

        return score;
    }

    private static SearchType DetectSearchType(string normalizedQuery, List<string> keywords)
    {
        if (keywords.Any(k => PhoneTerms.Any(t => k.Contains(t) || t.Contains(k))) || ContainsAny(normalizedQuery, PhoneTerms))
            return SearchType.Phone;

        if (keywords.Any(k => LaptopTerms.Any(t => k.Contains(t) || t.Contains(k))) || ContainsAny(normalizedQuery, LaptopTerms))
            return SearchType.Laptop;

        if (keywords.Any(k => AccessoryTerms.Any(t => k.Contains(t) || t.Contains(k))) || ContainsAny(normalizedQuery, AccessoryTerms))
            return SearchType.Accessory;

        return SearchType.General;
    }

    private static bool ContainsAny(string text, IReadOnlyCollection<string> terms)
        => terms.Any(text.Contains);

    private static string NormalizeText(string value)
    {
        var input = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC).Replace('đ', 'd');
    }

    private enum SearchType
    {
        General,
        Phone,
        Laptop,
        Accessory
    }

    private static readonly HashSet<string> SearchStopWords = new()
    {
        "toi", "cho", "muon", "can", "tim", "goi", "y", "gioi", "thieu", "hay",
        "ban", "co", "khong", "nao", "gi", "duoc", "xin", "vui", "long", "oi",
        "nhe", "di", "thu", "xem", "mot", "vai", "nhung", "cac", "cua", "voi",
        "va", "hoac", "trong", "ngoai", "dang", "se", "da", "roi", "lai", "nua", "them"
    };

    private static readonly string[] PhoneTerms =
    {
        "dien thoai", "phone", "smartphone", "iphone", "samsung", "xiaomi", "oppo", "vivo", "realme", "pixel", "galaxy"
    };

    private static readonly string[] LaptopTerms =
    {
        "laptop", "macbook", "notebook", "ultrabook", "thinkpad", "vivobook"
    };

    private static readonly string[] AccessoryTerms =
    {
        "tai nghe", "earbuds", "headphone", "sac", "adapter", "cu sac", "cap", "loa", "speaker", "pin du phong", "power bank"
    };

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
