using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUsersDto, User>();

            CreateMap<ImportProductsDto, Product>();
                
            CreateMap<ImportCategoriesDto, Category>();

            CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
