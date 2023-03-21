using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("Users")]
public class ExportUsersAndProductsDto
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("users")]
    public ExportUserDto[] Users { get; set; }
}

[XmlType("User")]
public class ExportUserDto
{
    [XmlElement("firstName")]
    public string FirstName { get; set; }

    [XmlElement("lastName")]
    public string LastName { get; set; }

    [XmlElement("age")]
    public int? Age { get; set; }

    [XmlElement("SoldProducts")]
    public SoldProductsCount SoldProductsCount { get; set; }
}

[XmlType("SoldProducts")]
public class SoldProductsCount
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("products")]
    public ProductsSold[] Products { get; set; }
}

[XmlType("Product")]
public class ProductsSold
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("price")]
    public decimal Price { get; set; }
}