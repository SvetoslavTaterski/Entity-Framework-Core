using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Boardgames.Data.Models.Enums;

namespace Boardgames.Data.Models;

public class Boardgame
{
    public Boardgame()
    {
        BoardgamesSellers = new HashSet<BoardgameSeller>();
    }

    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = null!;

    [Required]
    public double Rating { get; set; }

    [Required]
    public int YearPublished { get; set; }

    [Required]
    public CategoryType CategoryType { get; set; }

    [Required] 
    public string Mechanics { get; set; } = null!;

    [ForeignKey(nameof(Creator))]
    [Required]
    public int CreatorId { get; set; }

    public Creator Creator { get; set; }

    public ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
}

