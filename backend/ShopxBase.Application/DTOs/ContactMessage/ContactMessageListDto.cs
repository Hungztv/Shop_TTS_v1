namespace ShopxBase.Application.DTOs.ContactMessage;

public class ContactMessageListDto
{
    public List<ContactMessageDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
