﻿namespace P01_StudentSystem.Data.Models;

using Microsoft.EntityFrameworkCore;

using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

public class Student
{
    public Student()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Homeworks = new HashSet<Homework>();
    }

    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.StudentMaxNameLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.StudentMaxPhonenumberLength)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; }

    public ICollection<StudentCourse> StudentsCourses { get; set; }

    public ICollection<Homework> Homeworks { get; set; }

}