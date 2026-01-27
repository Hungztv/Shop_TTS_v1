using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Cart;
using ShopxBase.Application.Features.Cart.Commands.AddToCart;
using ShopxBase.Application.Features.Cart.Commands.UpdateCartQuantity;
using ShopxBase.Application.Features.Cart.Commands.RemoveFromCart;
using ShopxBase.Application.Features.Cart.Commands.ClearCart;
using ShopxBase.Application.Features.Cart.Queries.GetUserCart;
using ShopxBase.Application.Features.Cart.Queries.GetCartCount;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Cart API Controller - CQRS Pattern
/// Manages shopping cart operations for authenticated users
/// </summary>
[Authorize]
public class CartController : BaseApiController
{
    /// <summary>
    /// Gets the current user ID from JWT claims
    /// </summary>
    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Không thể xác định người dùng");
    }

    /// <summary>
    /// Lấy giỏ hàng của người dùng hiện tại
    /// </summary>
    /// <returns>CartSummaryDto chứa danh sách sản phẩm và tổng giá</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserCart()
    {
        var userId = GetUserId();
        var result = await Mediator.Send(new GetUserCartQuery(userId));
        return Success(result);
    }

    /// <summary>
    /// Lấy số lượng sản phẩm trong giỏ hàng
    /// </summary>
    /// <returns>Số lượng sản phẩm unique trong giỏ hàng</returns>
    [HttpGet("count")]
    public async Task<IActionResult> GetCartCount()
    {
        var userId = GetUserId();
        var result = await Mediator.Send(new GetCartCountQuery(userId));
        return Success(result);
    }

    /// <summary>
    /// Thêm sản phẩm vào giỏ hàng
    /// </summary>
    /// <param name="dto">Thông tin sản phẩm và số lượng</param>
    /// <returns>CartDto của sản phẩm vừa thêm</returns>
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        var userId = GetUserId();
        var command = new AddToCartCommand
        {
            UserId = userId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        var result = await Mediator.Send(command);
        return Success(result, "Đã thêm vào giỏ hàng");
    }

    /// <summary>
    /// Cập nhật số lượng sản phẩm trong giỏ hàng
    /// </summary>
    /// <param name="cartId">ID của cart item</param>
    /// <param name="dto">Số lượng mới</param>
    /// <returns>CartDto sau khi cập nhật</returns>
    [HttpPut("{cartId}")]
    public async Task<IActionResult> UpdateQuantity(int cartId, [FromBody] UpdateCartQuantityDto dto)
    {
        var userId = GetUserId();
        var command = new UpdateCartQuantityCommand
        {
            CartId = cartId,
            UserId = userId,
            Quantity = dto.Quantity
        };

        var result = await Mediator.Send(command);
        return Success(result, "Đã cập nhật giỏ hàng");
    }

    /// <summary>
    /// Xóa một sản phẩm khỏi giỏ hàng
    /// </summary>
    /// <param name="cartId">ID của cart item cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    [HttpDelete("{cartId}")]
    public async Task<IActionResult> RemoveFromCart(int cartId)
    {
        var userId = GetUserId();
        var command = new RemoveFromCartCommand
        {
            CartId = cartId,
            UserId = userId
        };

        await Mediator.Send(command);
        return Success(true, "Đã xóa khỏi giỏ hàng");
    }

    /// <summary>
    /// Xóa toàn bộ giỏ hàng của người dùng
    /// </summary>
    /// <returns>True nếu xóa thành công</returns>
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();
        var command = new ClearCartCommand
        {
            UserId = userId
        };

        await Mediator.Send(command);
        return Success(true, "Đã xóa toàn bộ giỏ hàng");
    }
}
