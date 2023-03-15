using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

        string result = GetUsersWithProducts(context);

        Console.WriteLine(result);
    }

    public static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }

    //Problem 1
    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

        ICollection<User> validUsers = new HashSet<User>();

        foreach (var userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            validUsers.Add(user);
        }

        context.Users.AddRange(validUsers);
        context.SaveChanges();

        return $"Successfully imported {validUsers.Count}";
    }

    //Problem 2
    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportedProductsDto[] importedProducts = JsonConvert.DeserializeObject<ImportedProductsDto[]>(inputJson);

        Product[] products = mapper.Map<Product[]>(importedProducts);

        context.Products.AddRange(products);
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    //Problem 3
    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var importedCategories = JsonConvert.DeserializeObject<ImportedCategoriesDto[]>(inputJson);

        ICollection<Category> validCategories = new HashSet<Category>();

        foreach (var category in importedCategories)
        {
            if (category.Name == null)
            {
                continue;
            }

            Category validCategory = mapper.Map<Category>(category);
            validCategories.Add(validCategory);
        }

        context.Categories.AddRange(validCategories);
        context.SaveChanges();

        return $"Successfully imported {validCategories.Count}";
    }

    //Problem 4
    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var importedCategoryProducts = JsonConvert.DeserializeObject<ImportedCategoryProductsDto[]>(inputJson);

        ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();

        foreach (var categoryProduct in importedCategoryProducts)
        {
            CategoryProduct validCategoryProduct = mapper.Map<CategoryProduct>(categoryProduct);

            validCategoryProducts.Add(validCategoryProduct);
        }

        context.CategoriesProducts.AddRange(validCategoryProducts);
        context.SaveChanges();

        return $"Successfully imported {validCategoryProducts.Count}";
    }

    //Problem 5
    public static string GetProductsInRange(ProductShopContext context)
    {
        var products = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new
            {
                name = p.Name,
                price = p.Price,
                seller = p.Seller.FirstName + " " + p.Seller.LastName,
            })
            .ToArray();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }

    //Problem 6
    public static string GetSoldProducts(ProductShopContext context)
    {
        var users = context.Users
            .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold
                    .Where(p => p.BuyerId != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                    .ToArray()
            })
            .ToArray();

        return JsonConvert.SerializeObject(users, Formatting.Indented);
    }

    //Problem 7
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categories = context.Categories
            .OrderByDescending(c => c.CategoriesProducts.Count)
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts.Count,
                averagePrice = c.CategoriesProducts.Average(p => p.Product.Price).ToString("f2"),
                totalRevenue = c.CategoriesProducts.Sum(p => p.Product.Price).ToString("f2")
            })
            .ToArray();

        return JsonConvert.SerializeObject(categories, Formatting.Indented);
    }

    //Problem 8
    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var users = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                    products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                            .ToArray()
                }
            })
            .OrderByDescending(u => u.soldProducts.count)
            .ToArray();

        var userWrapperDto = new
        {
            usersCount = users.Length,
            users = users
        };

        return JsonConvert.SerializeObject(userWrapperDto,
            Formatting.Indented,
            new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
    }
}
