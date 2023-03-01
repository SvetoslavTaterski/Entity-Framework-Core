namespace P01_StudentSystem.Data.Models;

using System.ComponentModel.DataAnnotations;

using P01_StudentSystem.Data.Common;

public class Course
{
    public Course()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Resources = new HashSet<Resource>();
        Homeworks = new HashSet<Homework>();
    }

    [Key]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.CourseMaxNameLength)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public decimal Price { get; set; }

    public ICollection<StudentCourse> StudentsCourses { get; set; }

    public ICollection<Resource> Resources { get; set; }

    public ICollection<Homework> Homeworks { get; set; }

}
