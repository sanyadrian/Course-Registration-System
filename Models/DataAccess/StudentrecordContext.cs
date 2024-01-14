using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab6.DataAccess;

public partial class StudentrecordContext : DbContext
{
    public StudentrecordContext()
    {
    }

    public StudentrecordContext(DbContextOptions<StudentrecordContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicRecord> AcademicRecords { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicRecord>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseCode }).HasName("PRIMARY");

            entity.ToTable("AcademicRecord");

            entity.HasIndex(e => e.CourseCode, "FK_AcademicRecord_Course");

            entity.Property(e => e.StudentId).HasMaxLength(16);
            entity.Property(e => e.CourseCode).HasMaxLength(16);

            entity.HasOne(d => d.CourseCodeNavigation).WithMany(p => p.AcademicRecords)
                .HasForeignKey(d => d.CourseCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AcademicRecord_Course");

            entity.HasOne(d => d.Student).WithMany(p => p.AcademicRecords)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AcademicRecord_Student");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Course");

            entity.Property(e => e.Code).HasMaxLength(16);
            entity.Property(e => e.Description).HasMaxLength(2048);
            entity.Property(e => e.FeeBase).HasPrecision(6);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasMany(d => d.StudentStudentNums).WithMany(p => p.CourseCourses)
                .UsingEntity<Dictionary<string, object>>(
                    "Registration",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentStudentNum")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Registration_ToStudent"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseCourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Registration_ToCourse"),
                    j =>
                    {
                        j.HasKey("CourseCourseId", "StudentStudentNum").HasName("PRIMARY");
                        j.ToTable("Registration");
                        j.HasIndex(new[] { "StudentStudentNum" }, "FK_Registration_ToStudent");
                        j.IndexerProperty<string>("CourseCourseId")
                            .HasMaxLength(16)
                            .HasColumnName("Course_CourseID");
                        j.IndexerProperty<string>("StudentStudentNum")
                            .HasMaxLength(16)
                            .HasColumnName("Student_StudentNum");
                    });
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasMany(d => d.Roles).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ToRole"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ToEmployee"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "RoleId").HasName("PRIMARY");
                        j.ToTable("Employee_Role");
                        j.HasIndex(new[] { "RoleId" }, "FK_ToRole");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("Employee_Id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("Role_Id");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Role1)
                .HasMaxLength(100)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Student");

            entity.Property(e => e.Id).HasMaxLength(16);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
