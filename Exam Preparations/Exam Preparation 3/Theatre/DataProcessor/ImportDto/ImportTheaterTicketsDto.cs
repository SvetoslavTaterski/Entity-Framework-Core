using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto;

public class ImportTheaterTicketsDto
{
    [MinLength(4)]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Range(1,10)]
    public sbyte NumberOfHalls { get; set; }

    [MaxLength(30)]
    [MinLength(4)]
    public string Director { get; set; } = null!;

    public ImportTicketsDto[] Tickets { get; set; }
}

public class ImportTicketsDto
{
    [Range(1.00,100.00)]
    public decimal Price { get; set; }

    [Range(1,10)]
    public sbyte RowNumber { get; set; }

    public int PlayId { get; set; }
}
