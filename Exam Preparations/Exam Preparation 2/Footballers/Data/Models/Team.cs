﻿using System.ComponentModel.DataAnnotations;
using Footballers.Common;

namespace Footballers.Data.Models;

public class Team
{
    public Team()
    {
        TeamsFootballers = new HashSet<TeamFootballer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TeamNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
    public string Nationality { get; set; } = null!;

    [Required]
    public int Trophies { get; set; }

    public ICollection<TeamFootballer> TeamsFootballers { get; set; }
}

