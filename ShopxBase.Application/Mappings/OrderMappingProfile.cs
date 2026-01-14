using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        // Order Entity -> OrderDto (Read)
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.StatusText,
                       opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PaymentMethodText,
                       opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.PaymentStatusText,
                       opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.OrderDetails,
                       opt => opt.MapFrom(src => src.OrderDetails));

        // OrderDetail Entity -> OrderDetailDto
        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.ProductName,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.ProductImage,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : null))
            .ForMember(dest => dest.Total,
                       opt => opt.MapFrom(src => src.Price * src.Quantity));

        // CreateOrderDto -> Order Entity (Create)
        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderCode, opt => opt.Ignore())
            .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
            .ForMember(dest => dest.Total, opt => opt.Ignore())
            .ForMember(dest => dest.ShippingCost, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Coupon, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

        // CreateOrderDetailDto -> OrderDetail Entity
        CreateMap<CreateOrderDetailDto, OrderDetail>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());
    }
}