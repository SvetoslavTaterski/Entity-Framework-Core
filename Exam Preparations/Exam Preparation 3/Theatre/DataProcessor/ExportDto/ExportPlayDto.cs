using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ExportDto;

[XmlType("Play")]
public class ExportPlayDto
{
    [XmlAttribute("Title")]
    public string Title { get; set; } = null!;

    [XmlAttribute("Duration")]
    public string Duration { get; set; }

    [XmlAttribute("Rating")]
    public string Rating { get; set; }

    [XmlAttribute("Genre")]
    public string Genre { get; set; }

    [XmlArray("Actors")]
    public ExportActorDto[] Actors { get; set; }
}

[XmlType("Actor")]
public class ExportActorDto
{
    [XmlAttribute("FullName")]
    public string FullName { get; set; } = null!;

    [XmlAttribute("MainCharacter")]
    public string MainCharacter { get; set; } = null!;
}

