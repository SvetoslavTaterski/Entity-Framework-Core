using System.ComponentModel.DataAnnotations;
using Footballers.Common;

namespace Footballers.Data.Models;

public class Coach
{
    public Coach()
    {
        Footballers = new HashSet<Footballer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.CoachNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required] 
    public string Nationality { get; set; } = null!;

    public ICollection<Footballer> Footballers { get; set; }
}

