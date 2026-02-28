using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace ShopxBase.Domain.Entities;

/// <summary>
/// Lưu text chunks + Gemini embedding vectors cho RAG chatbot.
/// Dùng pgvector extension trên Supabase.
/// </summary>
public class ChatDocument : BaseEntity
{
    /// <summary>Tiêu đề / tên chunk (VD: tên sản phẩm, tên FAQ)</summary>
    [Required, MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Nội dung text đã chunk</summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>Nguồn gốc: "product", "faq", "policy", "category", "brand"</summary>
    [MaxLength(500)]
    public string? Source { get; set; }

    /// <summary>ID tham chiếu tới entity gốc (VD: Product.Id)</summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Embedding vector 768 chiều từ Gemini text-embedding-004.
    /// Lưu dưới dạng float[] — EF Core map sang pgvector column.
    /// </summary>
    [Column(TypeName = "vector(768)")]
    public Vector? Embedding { get; set; }
}
