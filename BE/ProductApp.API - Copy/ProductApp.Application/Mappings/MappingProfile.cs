using AutoMapper;
using ProductApp.Application.DTOs.Category;
using ProductApp.Application.DTOs.Order;
using ProductApp.Application.DTOs.Product;
using ProductApp.Domain.Entities;

namespace ProductApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductViewDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Category mappings
        CreateMap<Category, CategoryViewDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();

        // Order mappings
        CreateMap<Order, OrderViewDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));
        CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        CreateMap<OrderUpdateDto, Order>();

        // OrderItem mappings
        CreateMap<OrderItem, OrderItemViewDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
        CreateMap<OrderItemCreateDto, OrderItem>();
    }
}