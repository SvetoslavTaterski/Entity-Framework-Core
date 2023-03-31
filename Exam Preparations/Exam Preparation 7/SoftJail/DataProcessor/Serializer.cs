using System.Globalization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;
using SoftJail.Utilities;

namespace SoftJail.DataProcessor
{
    using Data;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .ToArray()
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .Select(o => new
                        {
                            OfficerName = o.Officer.FullName,
                            Department = o.Officer.Department.Name,
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers.Select(o => Math.Round(o.Officer.Salary, 2)).Sum()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            XmlHelper xmlHelper = new XmlHelper();

            List<string> splittedPrisonersNames = prisonersNames.Split(",").ToList();

            var prisoners = context.Prisoners
                .Where(p => splittedPrisonersNames.Contains(p.FullName))
                .ToArray()
                .Select(p => new ExportPrisonersDto()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    Message = p.Mails
                        .Select(m => new ExportMessageDto()
                        {
                            Description = string.Join("",m.Description.Reverse())
                        })
                        .ToArray(),
                })
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            return xmlHelper.Serialize(prisoners, "Prisoners");
        }
    }
}