using System.Globalization;
using System.Text;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Footballers.Utilities;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCoachesDto[] coachesDtos = xmlHelper.Deserialize<ImportCoachesDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>();

            foreach (var coachDto in coachesDtos)
            {
                if (!IsValid(coachDto) || string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    DateTime startDate;
                    bool isStartDateValid = DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

                    DateTime endDate;
                    bool isEndDateValid = DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

                    if (!IsValid(footballerDto) || startDate > endDate || !isEndDateValid || !isStartDateValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = Enum.Parse<BestSkillType>(footballerDto.BestSkillType),
                        PositionType = Enum.Parse<PositionType>(footballerDto.PositionType),
                        Coach = coach
                    };

                    coach.Footballers.Add(footballer);
                }
                
                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach,coach.Name,coach.Footballers.Count));
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamsDto[] teamsDtos = JsonConvert.DeserializeObject<ImportTeamsDto[]>(jsonString);

            ICollection<Team> validTeams = new HashSet<Team>();

            foreach (var teamDto in teamsDtos)
            {
                if (!IsValid(teamDto) || teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                };

                foreach (var footballerId in teamDto.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(footballerId);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        Footballer = footballer,
                        Team = team
                    });
                }

                validTeams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(validTeams);
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
