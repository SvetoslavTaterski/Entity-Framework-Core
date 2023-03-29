using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto;

[XmlType("User")]
public class ExportUsersDto
{
    [XmlAttribute("username")]
    public string UserName { get; set; }

    [XmlArray("Purchases")]
    public ExportPurchaseDto[] Purchase { get; set; }

    [XmlElement("TotalSpent")]
    public decimal TotalSpent { get; set; }
}

[XmlType("Purchase")]
public class ExportPurchaseDto
{
    [XmlElement("Card")]
    public string Card { get; set; }

    [XmlElement("Cvc")]
    public string Cvc { get; set; }

    [XmlElement("Date")]
    public string Date { get; set; }

    [XmlElement("Game")]
    public ExportGameDto Game { get; set; }

}

[XmlType("Game")]
public class ExportGameDto
{
    [XmlAttribute("title")]
    public string Title { get; set; }

    [XmlElement("Genre")]
    public string Genre { get; set; }

    [XmlElement("Price")]
    public decimal Price { get; set; }
}

