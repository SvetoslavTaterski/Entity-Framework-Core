using System.Xml.Serialization;
using Footballers.Common;

namespace Footballers.DataProcessor.ExportDto;

[XmlType("Coach")]
public class ExportCoachesDto
{
    [XmlAttribute("FootballersCount")]
    public int FootballersCount { get; set; }

    [XmlElement("CoachName")]
    public string CoachName { get; set; } = null!;

    [XmlArray("Footballers")]
    public ExportFootballersDto[] Footballers { get; set; }
}

[XmlType("Footballer")]
public class ExportFootballersDto
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Position")]
    public string Position { get; set; }
}

