using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Common;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
	public StudentSystemContext()
	{

	}

	public StudentSystemContext(DbContextOptions options)
		: base(options)
	{

	}

	public DbSet<Course> Courses { get; set; } = null!;

	public DbSet<Homework> Homeworks { get; set; } = null!;

	public DbSet<Resource> Resources { get; set; } = null!;

	public DbSet<Student> Students { get; set; } = null!;

	public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
		}

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<StudentCourse>()
			.HasKey(x => new
			{
				x.StudentId,
				x.CourseId
			});

        modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentsCourses)
                .WithOne(s => s.Student)
                .HasForeignKey(s => s.StudentId);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Homeworks)
            .WithOne(s => s.Student)
            .HasForeignKey(s => s.StudentId);

        modelBuilder.Entity<Course>()
            .HasMany(c => c.StudentsCourses)
            .WithOne(c => c.Course)
            .HasForeignKey(c => c.CourseId);

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Resources)
            .WithOne(c => c.Course)
            .HasForeignKey(c => c.CourseId);

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Homeworks)
            .WithOne(c => c.Course)
            .HasForeignKey(c => c.CourseId);
    }
}