using AutoMapper;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Domain.Entities;

namespace FoodOnline.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //account map
            CreateMap<ApplicationUser, AccountPostDTO>().ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();
            CreateMap<ApplicationUser, UserGetDTO>()
            .ForMember(dest => dest.IsActive, opt =>
                opt.MapFrom(src => src.IsActive ? "Active" : "InActive"))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name ?? string.Empty)).ReverseMap();
            CreateMap<ApplicationUser, CustomerGetDTO>().ReverseMap();


            //Brand map
            CreateMap<Branch, BranchGetDTO>()
            .ForMember(dest => dest.Address,
                       opt => opt.MapFrom(src => $"{src.Detail} {src.Ward}, {src.District}, {src.City}"));

            CreateMap<Branch, BranchPostDTO>().ReverseMap();
            CreateMap<Branch, BranchDropDownDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => $" {src.Name} - {src.Detail} {src.Ward}, {src.District}, {src.City}"));
            CreateMap<Branch, BranchDetailGetDTO>().ReverseMap();


            //Category map
            CreateMap<Category, CategoryGetDTO>().ForMember(dest => dest.IsActive, opt =>
                opt.MapFrom(src => src.IsActive ? "Active" : "InActive"));
            CreateMap<Category, CategoryPostDTO>().ReverseMap();
            CreateMap<Category, CategoryDropDownDTO>().ReverseMap();


            //Product map
            CreateMap<Product, ProductGetDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name ?? string.Empty)).ReverseMap();
            CreateMap<Product, ProductPostDTO>().ReverseMap();
            CreateMap<Product, ProductCartDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name ?? string.Empty));


            //Product detail map
            CreateMap<ProductDetail, ProductDetailGetDTO>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name ?? string.Empty))
                                                           .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name ?? string.Empty))
                                                           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name ?? string.Empty));
            CreateMap<ProductDetail, ProductDetailPostDTO>().ReverseMap();
            CreateMap<ProductDetail, ProductRelatedGetDTO>().ReverseMap();


            //Address map
            CreateMap<UserAddress, UserAddressPostDTO>().ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Street)).ReverseMap();
            CreateMap<UserAddress, UserAddressGetDTO>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Detail + ", " + src.Street + ", " + src.District + ", " + src.City));
            CreateMap<UserAddress, UserAddressDetailGetDTO>().ReverseMap();

            //Cart map
            CreateMap<Cart, CartGetDTO>().ReverseMap();
            CreateMap<Cart, CartPostDTO>().ReverseMap();

            //Order status map
            CreateMap<OrderStatus, OrderStatusDropDownDTO>().ReverseMap();

            //Order map
            CreateMap<Order, OrderGetDTO>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => $" {src.Branch.Name} - {src.Branch.Detail} {src.Branch.Ward}, {src.Branch.District}, {src.Branch.City}"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus.Name))
                .ForMember(dest => dest.UserOrder, opt => opt.MapFrom(src => src.Customer.UserName));

            CreateMap<Order, OrderPostDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();

            //Order detail mapp
            CreateMap<OrderDetail, OrderDetailGetDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.ImageURL));
            CreateMap<OrderDetail, OrderDetailPostDTO>().ReverseMap();
        }
    }
}
