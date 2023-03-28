using System.Globalization;
using Newtonsoft.Json;
using TeisterMask.DataProcessor.ExportDto;
using TeisterMask.Utilities;

namespace TeisterMask.DataProcessor
{
    using Data;
    using Microsoft.VisualBasic;
    using TeisterMask.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectsDto()
                {
                    TaskCount = p.Tasks.Count,
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks
                        .Select(t => new ExportTasksDto()
                        {
                            Name = t.Name,
                            Label = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                })
                .OrderByDescending(p => p.Tasks.Length)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            return xmlHelper.Serialize(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .OrderByDescending(et => et.Task.DueDate)
                        .ThenBy(et => et.Task.Name)
                        .Select(et => new
                        {
                            TaskName = et.Task.Name,
                            OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = et.Task.LabelType.ToString(),
                            ExecutionType = et.Task.ExecutionType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }
    }
}