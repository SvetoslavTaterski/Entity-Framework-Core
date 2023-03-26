using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models;

public class Play
{
    public Play()
    {
        Tickets = new HashSet<Ticket>();
        Casts = new HashSet<Cast>();
    }

    [Key]
    public int Id { get; set; }

    [Required] 
    public string Title { get; set; } = null!;

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public float Rating { get; set; }

    [Required]
    public Genre Genre { get; set; }

    [Required] 
    public string Description { get; set; } = null!;

    [Required] 
    public string Screenwriter { get; set; } = null!;

    public ICollection<Cast> Casts { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}

