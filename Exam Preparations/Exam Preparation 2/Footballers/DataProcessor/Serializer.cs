using System.Globalization;
using Footballers.DataProcessor.ExportDto;
using Footballers.Utilities;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(c => new ExportCoachesDto()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers
                        .Select(f => new ExportFootballersDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(f => f.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return xmlHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(p => p.Footballer.ContractStartDate >= date)
                        .OrderByDescending(f => f.Footballer.ContractEndDate)
                        .ThenBy(f => f.Footballer.Name)
                        .ToArray()
                        .Select(p => new
                        {
                            FootballerName = p.Footballer.Name,
                            ContractStartDate = p.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = p.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = p.Footballer.BestSkillType.ToString(),
                            PositionType = p.Footballer.PositionType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(f => f.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
