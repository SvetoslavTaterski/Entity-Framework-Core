using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models;

public class Theatre
{
    public Theatre()
    {
        Tickets = new HashSet<Ticket>();
    }

    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = null!;

    [Required]
    public sbyte NumberOfHalls { get; set; }

    [Required] 
    public string Director { get; set; } = null!;

    public ICollection<Ticket> Tickets { get; set; }
}

