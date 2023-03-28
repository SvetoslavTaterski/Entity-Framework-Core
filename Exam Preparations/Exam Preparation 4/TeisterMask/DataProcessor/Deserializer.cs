// ReSharper disable InconsistentNaming

using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;
using TeisterMask.Utilities;

namespace TeisterMask.DataProcessor
{
    using TeisterMask.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportProjectsDto[] projectsDtos = xmlHelper.Deserialize<ImportProjectsDto[]>(xmlString, "Projects");

            ICollection<Project> validProjects = new HashSet<Project>();

            foreach (var projectDto in projectsDtos)
            {

                bool isProjectOpenDateValid =
                    DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validProjectOpenDate);
                bool isProjectDueDateValid =
                    DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedProjectDueDate);

                if (!IsValid(projectDto)
                    || !isProjectOpenDateValid
                    || (!string.IsNullOrWhiteSpace(projectDto.DueDate) && !isProjectDueDateValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? validProjectDueDate = string.IsNullOrWhiteSpace(projectDto.DueDate)
                    ? null
                    : parsedProjectDueDate;

                Project project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = validProjectOpenDate,
                    DueDate = validProjectDueDate,
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    bool isTaskOpenDateValid =
                        DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskOpenDate);
                    bool isTaskDueDateValid =
                        DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskDueDate);

                    if (!IsValid(taskDto)
                        || validTaskOpenDate < validProjectOpenDate
                        || validTaskDueDate > validProjectDueDate
                        || !isTaskOpenDateValid
                        || !isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = validTaskOpenDate,
                        DueDate = validTaskDueDate,
                        ExecutionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType),
                        LabelType = Enum.Parse<LabelType>(taskDto.LabelType),
                        ProjectId = project.Id
                    };

                    project.Tasks.Add(task);
                }

                validProjects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportEmployeesDto[] employeesDtos =
                JsonConvert.DeserializeObject<ImportEmployeesDto[]>(jsonString);

            ICollection<Employee> validEmployees = new HashSet<Employee>();

            List<int> validTaskIds = context.Tasks.Select(t => t.Id).ToList();

            foreach (var employeeDto in employeesDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,
                };

                foreach (var taskId in employeeDto.Tasks.Distinct())
                {
                    if (!validTaskIds.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask task = new EmployeeTask()
                    {
                        EmployeeId = employee.Id,
                        TaskId = taskId,
                    };

                    employee.EmployeesTasks.Add(task);
                }

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee,employee.Username,employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(validEmployees);
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