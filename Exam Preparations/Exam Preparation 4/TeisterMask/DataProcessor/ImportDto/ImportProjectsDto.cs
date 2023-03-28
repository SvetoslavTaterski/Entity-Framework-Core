using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Project")]
public class ImportProjectsDto
{
    [MinLength(2)]
    [MaxLength(40)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("OpenDate")]
    public string OpenDate { get; set; }

    [XmlElement("DueDate")]
    public string? DueDate { get; set; }

    [XmlArray("Tasks")]
    public ImportTasksDto[] Tasks { get; set; }
}

[XmlType("Task")]
public class ImportTasksDto
{
    [MinLength(2)]
    [MaxLength(40)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("OpenDate")]
    public string OpenDate { get; set; }

    [XmlElement("DueDate")]
    public string DueDate { get; set; }

    [XmlElement("ExecutionType")]
    public string ExecutionType { get; set; }

    [XmlElement("LabelType")]
    public string LabelType { get; set; }
}

