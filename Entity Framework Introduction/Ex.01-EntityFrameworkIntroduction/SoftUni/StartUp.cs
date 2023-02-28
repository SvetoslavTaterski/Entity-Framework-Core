using SoftUni.Data;
using SoftUni.Models;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();

            Console.WriteLine(GetEmployeesInPeriod(dbContext));
        }

        //Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Select(e => new
                                   {
                                       e.EmployeeId,
                                       e.FirstName,
                                       e.LastName,
                                       e.MiddleName,
                                       e.JobTitle,
                                       e.Salary
                                   })
                                   .OrderBy(e => e.EmployeeId)
                                   .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Select(e => new
                                   {
                                       e.FirstName,
                                       e.Salary
                                   })
                                   .Where(e => e.Salary > 50000)
                                   .OrderBy(e => e.FirstName)
                                   .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Select(e => new
                                   {
                                       e.FirstName,
                                       e.Department.Name,
                                       e.Salary,
                                       e.LastName
                                   })
                                   .Where(e => e.Name == "Research and Development")
                                   .OrderBy(e => e.Salary)
                                   .ThenByDescending(e => e.FirstName)
                                   .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from Research and Development - ${employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            Address newAddress = new Address()
            {
                TownId = 4,
                AddressText = "Vitoshka 15"
            };

            Employee? employeeNakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employeeNakov!.Address = newAddress;

            context.SaveChanges();

            string[] employees = context.Employees
                                  .OrderByDescending(e => e.AddressId)
                                  .Take(10)
                                  .Select(e => e.Address!.AddressText)
                                  .ToArray();

            return string.Join(Environment.NewLine, employees);

        }

        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                                   .OrderByDescending(a => a.Employees.Count)
                                   .ThenBy(t => t.Town!.Name)
                                   .ThenBy(a => a.AddressText)
                                   .Take(10)
                                   .Select(a => new
                                   {
                                       a.AddressText,
                                       TownName = a.Town!.Name,
                                       EmployeeCount = a.Employees.Count
                                   })
                                   .ToArray();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147Info = context.Employees
                                         .Where(e => e.EmployeeId == 147)
                                         .Select(e => new
                                         {
                                             FirstName = e.FirstName,
                                             LastName = e.LastName,
                                             JobTitle = e.JobTitle,
                                             Projects = e.EmployeesProjects
                                                         .Select(p => new
                                                         {
                                                             ProjectName = p.Project!.Name
                                                         })
                                                         .OrderBy(p => p.ProjectName)
                                                         .ToArray()
                                         })
                                         .FirstOrDefault();

            sb.AppendLine($"{employee147Info!.FirstName} {employee147Info.LastName} - {employee147Info.JobTitle}");

            foreach (var project in employee147Info.Projects)
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                                     .Where(e => e.Employees.Count > 5)
                                     .OrderBy(e => e.Employees.Count)
                                     .ThenBy(d => d.Name)
                                     .Select(d => new
                                     {
                                         Name = d.Name,
                                         ManagerFirstName = d.Manager.FirstName,
                                         ManagerLastName = d.Manager.LastName,
                                         Employees = d.Employees
                                     })
                                     .ToArray();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                                  .OrderByDescending(p => p.StartDate)
                                  .Take(10)
                                  .OrderBy(p => p.Name)
                                  .Select(p => new
                                  {
                                      Name = p.Name,
                                      Description = p.Description,
                                      StartDate = p.StartDate,
                                  })
                                  .ToArray();

            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Where(e => e.Department.Name == "Engineering" ||
                                               e.Department.Name == "Tool Design" ||
                                               e.Department.Name == "Marketing" ||
                                               e.Department.Name == "Information Services")
                                   .ToArray();

            decimal salaryModifier = 1.12m;

            foreach (var emp in employees)
            {
                emp.Salary *= salaryModifier;
            }

            context.SaveChanges();

            foreach (var emp in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Where(e => e.FirstName.StartsWith("Sa"))
                                   .Select(e => new
                                   {
                                       FirstName = e.FirstName,
                                       LastName = e.LastName,
                                       JobTitle = e.JobTitle,
                                       Salary = e.Salary
                                   })
                                   .OrderBy(e => e.FirstName)
                                   .ThenBy(e => e.LastName)
                                   .ToArray();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {

            var projectToDelete = context.Projects.Find(2);

            var employeeProjects = context.EmployeesProjects
                                          .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjects);

            context.Projects.Remove(projectToDelete!);

            context.SaveChanges();

            var projectsToPrint = context.Projects
                                         .Take(10)
                                         .Select(p => p.Name)
                                         .ToArray();

            return string.Join(Environment.NewLine, projectsToPrint);

        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            Town? town = context.Towns
                                .Where(t => t.Name == "Seattle")
                                .FirstOrDefault();

            Address[] adresses = context.Addresses
                                  .Where(a => a.TownId == town!.TownId)
                                  .ToArray();

            Employee[] employees = context.Employees
                                          .Where(e => adresses.Contains(e.Address))
                                          .ToArray();

            foreach (var emp in employees)
            {
                emp.AddressId = null;
            }

            context.Addresses.RemoveRange(adresses);
            context.Towns.Remove(town!);
            context.SaveChanges();

            return $"{adresses.Count()} addresses in Seattle were deleted";

        }

        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                                   .Take(10)
                                   .Select(e => new
                                   {
                                       e.FirstName,
                                       e.LastName,
                                       ManagerFirstName = e.Manager!.FirstName,
                                       ManagerLastName = e.Manager!.LastName,
                                       Projects = e.EmployeesProjects
                                                   .Where(ep => ep.Project!.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                                                   .Select(ep => new
                                                   {
                                                       ProjectName = ep.Project!.Name,
                                                       StartDate = ep.Project!.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                                       EndDate = ep.Project!.EndDate != null
                                                                                     ? ep.Project!.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                                                                     : "not finished"
                                                   })
                                   })
                                   .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var project in emp.Projects)
                {
                    sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}