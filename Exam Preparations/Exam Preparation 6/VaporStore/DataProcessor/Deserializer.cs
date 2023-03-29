using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.ImportDto;
using VaporStore.Utilities;

namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;

    using Data;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGamesDto[] gamesDtos = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

            List<Game> games = new List<Game>();
            List<Genre> genres = new List<Genre>();
            List<Developer> developers = new List<Developer>();
            List<Tag> tags = new List<Tag>();

            foreach (var gameDto in gamesDtos)
            {
                bool isReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validReleaseDate);

                if (!IsValid(gameDto)
                    || !isReleaseDateValid
                    || !gameDto.Tags.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = validReleaseDate
                };

                Genre genre = genres.Find(g => g.Name == gameDto.Genre);

                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = gameDto.Genre,
                    };

                    genres.Add(genre);
                }

                game.Genre = genre;

                Developer developer = developers.Find(d => d.Name == gameDto.Developer);

                if (developer == null)
                {
                    developer = new Developer()
                    {
                        Name = gameDto.Developer
                    };

                    developers.Add(developer);
                }

                game.Developer = developer;

                foreach (var tagName in gameDto.Tags)
                {
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }

                    Tag tag = tags.Find(t => t.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = tagName,
                        };

                        tags.Add(tag);
                    }

                    game.GameTags.Add(new GameTag() { Game = game, Tag = tag });
                }

                games.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportUsersDto[] usersDtos = JsonConvert.DeserializeObject<ImportUsersDto[]>(jsonString);

            ICollection<User> validUsers = new HashSet<User>();

            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto)
                    || !userDto.Cards.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = Enum.Parse<CardType>(cardDto.Type),
                        UserId = user.Id
                    };

                    user.Cards.Add(card);
                }

                validUsers.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportPurchasesDto[] purchasesDtos =
                xmlHelper.Deserialize<ImportPurchasesDto[]>(xmlString, "Purchases");

            ICollection<Purchase> validPurchases = new HashSet<Purchase>();


            foreach (var purchaseDto in purchasesDtos)
            {
                Game game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);
                Card card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);
                bool isDateTimeValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDateTime);

                if (!IsValid(purchaseDto)
                    || !isDateTimeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Type = Enum.Parse<PurchaseType>(purchaseDto.Type),
                    ProductKey = purchaseDto.Key,
                    Card = card,
                    Date = validDateTime,
                    Game = game
                };

                validPurchases.Add(purchase);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, game.Name, purchase.Card.User.Username));
            }

            context.Purchases.AddRange(validPurchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}