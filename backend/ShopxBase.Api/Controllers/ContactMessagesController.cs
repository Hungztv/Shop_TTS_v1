using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.ContactMessage;
using ShopxBase.Application.Features.ContactMessages.Commands.CreateContactMessage;
using ShopxBase.Application.Features.ContactMessages.Commands.DeleteContactMessage;
using ShopxBase.Application.Features.ContactMessages.Commands.MarkAsRead;
using ShopxBase.Application.Features.ContactMessages.Commands.ReplyToContactMessage;
using ShopxBase.Application.Features.ContactMessages.Queries.GetAllContactMessages;
using ShopxBase.Application.Features.ContactMessages.Queries.GetContactMessageById;
using ShopxBase.Application.Features.ContactMessages.Queries.GetUnreadCount;

namespace ShopxBase.Api.Controllers;

[Route("api/contact-messages")]
public class ContactMessagesController : BaseApiController
{
    /// <summary>
    /// Submit a contact message (Public)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateContactMessageCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Gửi tin nhắn liên hệ thành công");
    }

    /// <summary>
    /// Get all contact messages (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isRead = null)
    {
        var query = new GetAllContactMessagesQuery
        {
            Page = page,
            PageSize = pageSize,
            IsRead = isRead
        };
        var result = await Mediator.Send(query);
        return Success(result, "Lấy danh sách tin nhắn liên hệ thành công");
    }

    /// <summary>
    /// Get contact message by ID (Admin only)
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetContactMessageByIdQuery(id));
        if (result == null)
            return Error("Không tìm thấy tin nhắn", 404);

        return Success(result, "Lấy thông tin tin nhắn thành công");
    }

    /// <summary>
    /// Get unread messages count (Admin only)
    /// </summary>
    [HttpGet("unread-count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var result = await Mediator.Send(new GetUnreadCountQuery());
        return Success(new { count = result }, "Lấy số tin nhắn chưa đọc thành công");
    }

    /// <summary>
    /// Mark message as read (Admin only)
    /// </summary>
    [HttpPut("{id:int}/read")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await Mediator.Send(new MarkAsReadCommand(id));
        if (!result)
            return Error("Không tìm thấy tin nhắn", 404);

        return Success(result, "Đánh dấu đã đọc thành công");
    }

    /// <summary>
    /// Reply to contact message (Admin only)
    /// </summary>
    [HttpPost("{id:int}/reply")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reply(int id, [FromBody] ReplyToContactMessageDto dto)
    {
        var command = new ReplyToContactMessageCommand
        {
            Id = id,
            ReplyMessage = dto.ReplyMessage
        };
        var result = await Mediator.Send(command);
        if (result == null)
            return Error("Không tìm thấy tin nhắn", 404);

        return Success(result, "Phản hồi tin nhắn thành công");
    }

    /// <summary>
    /// Delete contact message (Admin only)
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteContactMessageCommand(id));
        if (!result)
            return Error("Không tìm thấy tin nhắn", 404);

        return Success(result, "Xóa tin nhắn thành công");
    }
}
