using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Categories.Commands.DeleteCategoryPermanently;

public class DeleteCategoryPermanentlyCommandHandler : IRequestHandler<DeleteCategoryPermanentlyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryPermanentlyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCategoryPermanentlyCommand request, CancellationToken cancellationToken)
    {
        // Check if category exists (kể cả đã soft delete)
        var category = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (category == null)
            throw new CategoryNotFoundException($"Danh mục với Id {request.Id} không tồn tại");

        // Check if category has products (bao gồm cả products đã xóa)
        var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.CategoryId == request.Id);
        if (hasProducts)
            throw new DomainException("Không thể xóa vĩnh viễn danh mục đang có sản phẩm");

        // Hard delete - xóa vĩnh viễn khỏi database
        await _unitOfWork.Categories.DeletePermanentlyAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
