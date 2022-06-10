using AutoMapper;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using CommAPP.Models.ViewModels.Admin;
using CommAPP.Models.ViewModels.OrderRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.AutoMapper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Category,CategoryViewModel>().ForMember(vm => vm.Children, opts => opts.MapFrom(src => src.Children))
                                                   .ForMember(vm => vm.Parent, opts => opts.MapFrom(src => src.Parent))
                                                   .ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();

            //Cart
            CreateMap<CartItem, CartItemModel>().ReverseMap();
            CreateMap<Cart, CartModel>().ReverseMap();

            CreateMap<OrderItem, OrderItemModel>().ReverseMap();
            CreateMap<Order, OrderFullViewModel>().ReverseMap();

            CreateMap<Order, OrderViewModel>().ForMember(vm => vm.OrderId,opts => opts.MapFrom(src=>src.Id)).ReverseMap();

            CreateMap<Order, OrderModel>().ReverseMap();
            
           // CreateMap<Project, ProjectDTO>().ForMember(dto => dto.ProjectCategoryDTOs, opts => opts.MapFrom(src => src.ProjectCategories)).ForMember(dto => dto.ProjectImageDTOs, opts => opts.MapFrom(src => src.ProjectImages)).ReverseMap();
        }
    }
}
