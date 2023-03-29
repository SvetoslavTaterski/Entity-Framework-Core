using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto;

[XmlType("Purchase")]
public class ImportPurchasesDto
{
    [XmlAttribute("title")]
    [Required]
    public string Title { get; set; } = null!;

    [XmlElement("Type")]
    [Required]
    public string Type { get; set; } = null!;

    [XmlElement("Key")]
    [Required]
    [RegularExpression(@"^[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}$")]
    public string Key { get; set; } = null!;

    [XmlElement("Card")] 
    [Required] 
    public string Card { get; set; } = null!;

    [XmlElement("Date")] 
    [Required] 
    public string Date { get; set; } = null!;
}

