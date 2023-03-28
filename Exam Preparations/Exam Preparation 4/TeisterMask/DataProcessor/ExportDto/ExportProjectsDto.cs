using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Project")]
public class ExportProjectsDto
{
    [XmlAttribute("TasksCount")]
    public int TaskCount { get; set; }

    [XmlElement("ProjectName")]
    public string ProjectName { get; set; }

    [XmlElement("HasEndDate")]
    public string HasEndDate { get; set; }

    [XmlArray("Tasks")]
    public ExportTasksDto[] Tasks { get; set; }
}

[XmlType("Task")]
public class ExportTasksDto
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Label")]
    public string Label { get; set; }
}

