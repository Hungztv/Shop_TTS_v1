using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Pgvector;
using ShopxBase.Domain.Entities;
using ShopxBase.Infrastructure.Data;

namespace ShopxBase.Infrastructure.Services;

// ═══════════════════════════════════════════════════════════
//  Interface
// ═══════════════════════════════════════════════════════════

public interface IVectorSearchService
{
    /// <summary>
    /// Tìm top-k chunks gần nhất với query embedding bằng cosine similarity.
    /// Dùng raw SQL + pgvector operator &lt;=&gt;.
    /// </summary>
    Task<List<VectorSearchResult>> SearchAsync(float[] queryEmbedding, int topK = 3, double threshold = 0.4);

    /// <summary>
    /// Index 1 document mới (tạo embedding + lưu vào DB).
    /// </summary>
    Task<ChatDocument?> IndexDocumentAsync(string title, string content, string? source, int? sourceId, float[] embedding);

    /// <summary>
    /// Bulk index documents cho seeding data.
    /// </summary>
    Task<int> BulkIndexAsync(IEnumerable<DocumentInput> documents, IEmbeddingService embeddingService);
}

/// <summary>
/// Kết quả tìm kiếm vector
/// </summary>
public class VectorSearchResult
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Source { get; set; }
    public int? SourceId { get; set; }
    public double Similarity { get; set; }
}

/// <summary>
/// Input cho bulk indexing
/// </summary>
public class DocumentInput
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Source { get; set; }
    public int? SourceId { get; set; }
}

// ═══════════════════════════════════════════════════════════
//  Implementation — Raw SQL + pgvector
// ═══════════════════════════════════════════════════════════

public class VectorSearchService : IVectorSearchService
{
    private readonly ShopxBaseDbContext _dbContext;
    private readonly ILogger<VectorSearchService> _logger;

    public VectorSearchService(
        ShopxBaseDbContext dbContext,
        ILogger<VectorSearchService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<VectorSearchResult>> SearchAsync(
        float[] queryEmbedding, int topK = 3, double threshold = 0.4)
    {
        try
        {
            // Dùng raw SQL vì EF Core chưa hỗ trợ pgvector operators natively
            var embeddingStr = "[" + string.Join(",", queryEmbedding) + "]";

            var sql = @"
                SELECT 
                    ""Id"", 
                    ""Title"", 
                    ""Content"", 
                    ""Source"", 
                    ""SourceId"",
                    1 - (""Embedding"" <=> @embedding::vector) AS ""Similarity""
                FROM ""ChatDocuments""
                WHERE ""IsDeleted"" = FALSE
                  AND 1 - (""Embedding"" <=> @embedding::vector) > @threshold
                ORDER BY ""Embedding"" <=> @embedding::vector
                LIMIT @topK";

            var results = new List<VectorSearchResult>();

            await using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@embedding", embeddingStr);
            cmd.Parameters.AddWithValue("@threshold", threshold);
            cmd.Parameters.AddWithValue("@topK", topK);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new VectorSearchResult
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    Source = reader.IsDBNull(3) ? null : reader.GetString(3),
                    SourceId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Similarity = reader.GetDouble(5)
                });
            }

            _logger.LogInformation("Vector search returned {Count} results (threshold: {Threshold})",
                results.Count, threshold);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Vector search failed");
            return new List<VectorSearchResult>();
        }
    }

    public async Task<ChatDocument?> IndexDocumentAsync(
        string title, string content, string? source, int? sourceId, float[] embedding)
    {
        try
        {
            var doc = new ChatDocument
            {
                Title = title,
                Content = content,
                Source = source,
                SourceId = sourceId,
                Embedding = new Vector(embedding),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.ChatDocuments.Add(doc);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Indexed document: {Title} (Source: {Source}, Id: {SourceId})",
                title, source, sourceId);

            return doc;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to index document: {Title}", title);
            return null;
        }
    }

    public async Task<int> BulkIndexAsync(
        IEnumerable<DocumentInput> documents, IEmbeddingService embeddingService)
    {
        var docList = documents.ToList();
        if (!docList.Any()) return 0;

        var indexed = 0;

        // Batch embed (max 100 per batch)
        var batches = docList.Chunk(100);

        foreach (var batch in batches)
        {
            var texts = batch.Select(d => $"{d.Title}\n{d.Content}").ToList();
            var embeddings = await embeddingService.EmbedBatchAsync(texts);

            for (int i = 0; i < Math.Min(batch.Length, embeddings.Count); i++)
            {
                var doc = batch[i];
                var embedding = embeddings[i];

                if (embedding.Length == 0) continue;

                var entity = new ChatDocument
                {
                    Title = doc.Title,
                    Content = doc.Content,
                    Source = doc.Source,
                    SourceId = doc.SourceId,
                    Embedding = new Vector(embedding),
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.ChatDocuments.Add(entity);
                indexed++;
            }
        }

        if (indexed > 0)
            await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Bulk indexed {Count}/{Total} documents", indexed, docList.Count);
        return indexed;
    }
}
