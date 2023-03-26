using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Play")]
public class ImportPlaysDto
{
    [MaxLength(ValidationConstants.TitleMaxLength)]
    [MinLength(ValidationConstants.TitleMinLength)]
    [XmlElement("Title")]
    public string Title { get; set; } = null!;

    [XmlElement("Duration")]
    public string Duration { get; set; }

    [Range(0.0,10.0)]
    [XmlElement("Raiting")]
    public float Rating { get; set; }

    [XmlElement("Genre")]
    public string Genre { get; set; }

    [MaxLength(ValidationConstants.DescriptionMaxLength)]
    [XmlElement("Description")]
    public string Description { get; set; } = null!;

    [MinLength(ValidationConstants.ScreenwriterMinLength)]
    [MaxLength(ValidationConstants.ScreenwriterMaxLength)]
    [XmlElement("Screenwriter")]
    public string Screenwriter { get; set; } = null!;
}

