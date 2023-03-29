using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models;

public class Game
{
    public Game()
    {
        Purchases = new HashSet<Purchase>();
        GameTags = new HashSet<GameTag>();
    }

    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [ForeignKey(nameof(Developer))]
    [Required]
    public int DeveloperId { get; set; }

    [Required]
    public Developer Developer { get; set; } = null!;

    [ForeignKey(nameof(Genre))]
    [Required]
    public int GenreId { get; set; }

    [Required]
    public Genre Genre { get; set; } = null!;

    public ICollection<Purchase> Purchases { get; set; }

    public ICollection<GameTag> GameTags { get; set; }
}

