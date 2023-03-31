using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;
using SoftJail.Utilities;

namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportDeparmentDto[] departmentsDtos =
                JsonConvert.DeserializeObject<ImportDeparmentDto[]>(jsonString);

            ICollection<Department> validDepartments = new HashSet<Department>();

            foreach (var departmentDto in departmentsDtos)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDto.Name,
                };

                ICollection<Cell> validCells = new List<Cell>();

                foreach (var cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        validCells.Clear();
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };

                    validCells.Add(cell);
                }

                department.Cells = validCells;

                if (department.Cells.Any())
                {
                    validDepartments.Add(department);
                    sb.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
                }
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonersDto[] prisonersDtos =
                JsonConvert.DeserializeObject<ImportPrisonersDto[]>(jsonString);

            ICollection<Prisoner> validPrisoners = new HashSet<Prisoner>();

            foreach (var prisonerDto in prisonersDtos)
            {
                bool isIncarcerationDateValid = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validIncarcerationDate);

                if (!IsValid(prisonerDto)
                    || !isIncarcerationDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    ReleaseDate = prisonerDto.ReleaseDate == null
                        ? null
                        : DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None),
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                ICollection<Mail> validMails = new List<Mail>();

                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        validMails.Clear();
                        continue;
                    }

                    Mail mail = new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };

                    validMails.Add(mail);
                }

                prisoner.Mails = validMails;
                validPrisoners.Add(prisoner);
                sb.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ImportOfficersDto[] officersDtos =
                xmlHelper.Deserialize<ImportOfficersDto[]>(xmlString, "Officers");

            ICollection<Officer> validOfficers = new HashSet<Officer>();

            foreach (var officerDto in officersDtos)
            {
                bool isPositionValid = Enum.TryParse<Position>(officerDto.Position, out Position validPosition);
                bool isWeaponValid = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon validWeapon);

                if (!IsValid(officerDto)
                    || !isPositionValid
                    || !isWeaponValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = validPosition,
                    Weapon = validWeapon,
                    DepartmentId = officerDto.DepartmentId,
                };

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    OfficerPrisoner prisoner = new OfficerPrisoner()
                    {
                        PrisonerId = prisonerDto.Id,
                        OfficerId = officer.Id
                    };

                    officer.OfficerPrisoners.Add(prisoner);
                }

                validOfficers.Add(officer);
                sb.AppendLine(string.Format(SuccessfullyImportedOfficer,officer.FullName,officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}