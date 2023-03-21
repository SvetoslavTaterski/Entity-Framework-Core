using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        //string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");

        //string result = ImportCategoryProducts(context, inputXml);

        string result = GetUsersWithProducts(context);

        Console.WriteLine(result);
    }

    public static string ImportUsers(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateAutoMapper();

        XmlHelper xmlHelper = new XmlHelper();
        ImportUsersDto[] usersDtos = xmlHelper.Deserialize<ImportUsersDto[]>(inputXml, "Users");

        ICollection<User> validUsers = new HashSet<User>();

        foreach (ImportUsersDto userDto in usersDtos)
        {
            User user = mapper.Map<User>(userDto);
            validUsers.Add(user);
        }

        context.AddRange(validUsers);
        context.SaveChanges();

        return $"Successfully imported {validUsers.Count}";
    }

    public static string ImportProducts(ProductShopContext context, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();
        ImportProductsDto[] productsDtos = xmlHelper.Deserialize<ImportProductsDto[]>(inputXml, "Products");

        Product[] products = productsDtos
            .Select(p => new Product()
            {
                Name = p.Name,
                Price = p.Price,
                BuyerId = p.BuyerId == 0 ? null : p.BuyerId,
                SellerId = p.SellerId
            })
            .ToArray();

        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Count()}";
    }

    public static string ImportCategories(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportCategoriesDto[] categoriesDtos = xmlHelper.Deserialize<ImportCategoriesDto[]>(inputXml, "Categories");

        ICollection<Category> categories = new HashSet<Category>();

        foreach (var categoryDto in categoriesDtos)
        {
            if (categoryDto.Name == null)
            {
                continue;
            }

            Category category = mapper.Map<Category>(categoryDto);
            categories.Add(category);
        }

        context.Categories.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportCategoryProductDto[] categoryProductDtos =
            xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

        ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

        foreach (var categoryProductDto in categoryProductDtos)
        {
            if (categoryProductDto.ProductId == null || categoryProductDto.CategoryId == null)
            {
                continue;
            }

            CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(categoryProductDto);
            categoryProducts.Add(categoryProduct);
        }

        context.CategoryProducts.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count}";
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var products = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .Select(p => new ExportProductsDto()
            {
                Name = p.Name,
                Price = p.Price,
                Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
            })
            .ToArray();

        return xmlHelper.Serialize<ExportProductsDto[]>(products, "Products");
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var users = context.Users
            .Where(u => u.ProductsSold.Any())
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(5)
            .Select(u => new ExportSoldProductsDto()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Products = u.ProductsSold
                    .Select(p => new ExportProduct()
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
            })
            .ToArray();

        return xmlHelper.Serialize<ExportSoldProductsDto>(users, "Users");
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var categories = context.Categories
            .Select(c => new ExportCategoriesByProductsCountDto()
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count(),
                AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.TotalRevenue)
            .ToArray();

        return xmlHelper.Serialize<ExportCategoriesByProductsCountDto[]>(categories, "Categories");
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var users = context.Users
            .Where(u => u.ProductsSold.Any())
            .OrderByDescending(u => u.ProductsSold.Count)
            .Select(u => new ExportUserDto()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                SoldProductsCount = new SoldProductsCount()
                {
                    Count = u.ProductsSold.Count,
                    Products = u.ProductsSold.Select(p => new ProductsSold()
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                }
            })
            .Take(10)
            .ToArray();

        ExportUsersAndProductsDto userCountDto = new ExportUsersAndProductsDto()
        {
            Count = context.Users.Count(u => u.ProductsSold.Any()),
            Users = users
        };

        return xmlHelper.Serialize(userCountDto, "Users");
    }

    private static IMapper CreateAutoMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
}
