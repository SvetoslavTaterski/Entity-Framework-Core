using Newtonsoft.Json;
using VaporStore.DataProcessor.ExportDto;
using VaporStore.Utilities;

namespace VaporStore.DataProcessor
{
    using Data;
    using System.Globalization;
    using VaporStore.Data.Models;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var games = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .ToArray()
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(g => g.Purchases.Any())
                        .Select(ga => new
                        {
                            Id = ga.Id,
                            Title = ga.Name,
                            Developer = ga.Developer.Name,
                            Tags = string.Join(", ",ga.GameTags.Select(gt => gt.Tag.Name)),
                            Players = ga.Purchases.Count
                        })
                        .OrderByDescending(p => p.Players)
                        .ThenBy(ga => ga.Id)
                        .ToArray(),
                    TotalPlayers = g.Games.Sum(p => p.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            return JsonConvert.SerializeObject(games, Formatting.Indented);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var users = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
                .Select(u => new ExportUsersDto()
                {
                    UserName = u.Username,
                    TotalSpent = u.Cards
                        .Sum(c => c.Purchases.Where(p => p.Type.ToString() == purchaseType).Sum(p => p.Game.Price)),
                    Purchase = u.Cards
                        .SelectMany(p => p.Purchases
                        .Where(p => p.Type.ToString() == purchaseType)
                        .Select(p => new ExportPurchaseDto()
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto()
                            {
                                Genre = p.Game.Genre.Name,
                                Title = p.Game.Name,
                                Price = p.Game.Price,
                            }
                        }))
                        .OrderBy(p => p.Date)
                        .ToArray()
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();

            return xmlHelper.Serialize(users, "Users");
        }
    }
}