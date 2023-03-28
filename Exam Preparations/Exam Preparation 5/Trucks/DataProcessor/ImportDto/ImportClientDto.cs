﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Trucks.DataProcessor.ImportDto;

public class ImportClientDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(40)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;

    public int[] Trucks { get; set; }
}

