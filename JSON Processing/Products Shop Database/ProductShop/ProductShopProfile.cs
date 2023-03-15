using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //User
            CreateMap<ImportUserDto, User>();

            //Product
            CreateMap<ImportedProductsDto, Product>();

            //Categories
            CreateMap<ImportedCategoriesDto, Category>();

            //CategoriesProducts
            CreateMap<ImportedCategoryProductsDto, CategoryProduct>();
        }
    }
}
