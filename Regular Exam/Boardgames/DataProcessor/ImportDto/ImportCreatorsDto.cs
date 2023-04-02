using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto;

[XmlType("Creator")]
public class ImportCreatorsDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(7)]
    [XmlElement("FirstName")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(7)]
    [XmlElement("LastName")]
    public string LastName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public ImportBoardGamesDto[] BoardGames { get; set; }
}

[XmlType("Boardgame")]
public class ImportBoardGamesDto
{
    [Required]
    [XmlElement("Name")]
    [MinLength(10)]
    [MaxLength(20)]
    public string Name { get; set; } = null!;

    [Required]
    [XmlElement("Rating")]
    [Range(1,10.00)]
    public double Rating { get; set; }

    [Required]
    [XmlElement("YearPublished")]
    [Range(2018,2023)]
    public int YearPublished { get; set; }

    [Required]
    [XmlElement("CategoryType")]
    public string CategoryType { get; set; } = null!;

    [Required]
    [XmlElement("Mechanics")]
    public string Mechanics { get; set; } = null!;
}

