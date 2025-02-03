using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NewSchoolDb.Models;

namespace NewSchoolDb.Data;

public partial class NewSchoolDbContext : DbContext
{
    public NewSchoolDbContext()
    {
    }

    public NewSchoolDbContext(DbContextOptions<NewSchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseName> CourseNames { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = localhost;Database = NewSchoolDb;Trusted_Connection=True;Trust server certificate = true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927A0D446D87B");

            entity.ToTable("Class");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasColumnName("Staff_ID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Classes)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Class__Staff_ID__412EB0B6");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D7187AEFB5B79");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseNameId).HasColumnName("CourseName_ID");
            entity.Property(e => e.SubjectId).HasColumnName("Subject_ID");

            entity.HasOne(d => d.CourseName).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseNameId)
                .HasConstraintName("FK_Course_CourseName");

            entity.HasOne(d => d.Subject).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Course__Subject___3B75D760");
        });

        modelBuilder.Entity<CourseName>(entity =>
        {
            entity.HasKey(e => e.CourseNameId).HasName("PK__CourseNa__37FE4860DD0B0C54");

            entity.ToTable("CourseName");

            entity.Property(e => e.CourseNameId).HasColumnName("CourseNameID");
            entity.Property(e => e.CourseName1)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("CourseName");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD2711E2C5");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A37FFD318C4");

            entity.ToTable("Grade");

            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.Grade1)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Grade");
            entity.Property(e => e.StaffId).HasColumnName("Staff_ID");
            entity.Property(e => e.StudentId).HasColumnName("Student_ID");
            entity.Property(e => e.SubjectId).HasColumnName("Subject_ID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Grade__Staff_ID__49C3F6B7");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grade__Student_I__47DBAE45");

            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Grade__Subject_I__48CFD27E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A5C61C638");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(55)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7F0FF573C");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");

            entity.HasOne(d => d.Department).WithMany(p => p.Staff)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Staff__Departmen__5DCAEF64");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Staff__Role_ID__3E52440B");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79998E9BDB");

            entity.ToTable("Student");

            entity.HasIndex(e => e.SecurityNum, "UQ__Student__C8B9AB79B3154C7D").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.ClassId).HasColumnName("Class_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.SecurityNum)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Student__Class_I__44FF419A");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subject__AC1BA3881310B2B3");

            entity.ToTable("Subject");

            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
