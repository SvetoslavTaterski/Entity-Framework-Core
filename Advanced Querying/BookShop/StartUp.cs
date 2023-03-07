using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;

namespace BookShop;

using Data;
using Initializer;

public class StartUp
{
    public static void Main()
    {
        using var db = new BookShopContext();
        DbInitializer.ResetDatabase(db);

        RemoveBooks(db);
    }

    //Problem 2
    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

        string[] books = context.Books
            .Where(b => b.AgeRestriction == ageRestriction)
            .Select(b => b.Title)
            .OrderBy(b => b)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    //Problem 3
    public static string GetGoldenBooks(BookShopContext context)
    {
        var books = context.Books
            .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    //Problem 4
    public static string GetBooksByPrice(BookShopContext context)
    {
        var books = context.Books
            .Where(b => b.Price > 40)
            .Select(b => new
            {
                Title = b.Title,
                Price = b.Price
            })
            .OrderByDescending(b => b.Price)
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var book in books)
        {
            sb.AppendLine($"{book.Title} - ${book.Price:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 5
    public static string GetBooksNotReleasedIn(BookShopContext context, int year)
    {
        var books = context.Books
            .Where(b => b.ReleaseDate!.Value.Year != year)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    //Problem 6
    public static string GetBooksByCategory(BookShopContext context, string input)
    {
        List<string> categories = input
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.ToLower())
            .ToList();

        var books = context.Books
            .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);

    }

    //Problem 7
    public static string GetBooksReleasedBefore(BookShopContext context, string date)
    {
        DateTime givenDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

        var books = context.Books
            .Where(b => b.ReleaseDate < givenDate)
            .OrderByDescending(b => b.ReleaseDate)
            .Select(b => new
            {
                Title = b.Title,
                EditionType = b.EditionType,
                Price = b.Price
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var book in books)
        {
            sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 8
    public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
    {
        var authors = context.Authors
            .Where(a => a.FirstName.EndsWith(input))
            .OrderBy(a => a.FirstName)
            .ThenBy(a => a.LastName)
            .Select(a => new
            {
                FullName = a.FirstName + " " + a.LastName,
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var author in authors)
        {
            sb.AppendLine(author.FullName);
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 9
    public static string GetBookTitlesContaining(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(b => b.Title.ToLower().Contains(input.ToLower()))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    //Problem 10
    public static string GetBooksByAuthor(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
            .OrderBy(b => b.BookId)
            .Select(b => new
            {
                BookTitle = b.Title,
                AuthorName = b.Author.FirstName + " " + b.Author.LastName
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var book in books)
        {
            sb.AppendLine($"{book.BookTitle} ({book.AuthorName})");
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 11
    public static int CountBooks(BookShopContext context, int lengthCheck)
    {
        return context.Books.Where(b => b.Title.Length > lengthCheck).Count();
    }

    //Problem 12
    public static string CountCopiesByAuthor(BookShopContext context)
    {
        var authors = context.Authors
            .Select(a => new
            {
                BookCopiesCount = a.Books.Sum(b => b.Copies),
                AuthorName = a.FirstName + " " + a.LastName
            })
            .OrderByDescending(a => a.BookCopiesCount)
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var author in authors)
        {
            sb.AppendLine($"{author.AuthorName} - {author.BookCopiesCount}");
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 13
    public static string GetTotalProfitByCategory(BookShopContext context)
    {
        var categories = context.Categories
            .Select(c => new
            {
                CategoryName = c.Name,
                TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
            })
            .OrderByDescending(c => c.TotalProfit)
            .ThenBy(c => c.CategoryName)
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var category in categories)
        {
            sb.AppendLine($"{category.CategoryName} ${category.TotalProfit:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 14
    public static string GetMostRecentBooks(BookShopContext context)
    {
        var categories = context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new
            {
                CategoryName = c.Name,
                Books = c.CategoryBooks
                    .OrderByDescending(bc => bc.Book.ReleaseDate)
                    .Select(bc => new
                    {
                        BookTitle = bc.Book.Title,
                        BookReleaseYear = bc.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var category in categories)
        {
            sb.AppendLine($"--{category.CategoryName}");

            foreach (var book in category.Books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.BookReleaseYear})");
            }
        }

        return sb.ToString().TrimEnd();
    }

    //Problem 15
    public static void IncreasePrices(BookShopContext context)
    {
        var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

        foreach (var book in books)
        {
            book.Price += 5;
        }

        context.SaveChanges();
    }

    //Problem 16
    public static int RemoveBooks(BookShopContext context)
    {
        var books = context.Books.Where(b => b.Copies < 4200);

        int count = books.Count();

        context.RemoveRange(books);

        context.SaveChanges();

        return count;
    }
}




