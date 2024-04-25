using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Evidence9.Models;

public partial class EvidenceCore9Context : DbContext
{
    public EvidenceCore9Context()
    {
    }

    public EvidenceCore9Context(DbContextOptions<EvidenceCore9Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EvidenceCore9;Integrated Security=True;MultipleActiveResultSets=true;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11FDC1ADB8");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JoinDate).HasColumnType("date");
            entity.Property(e => e.Picture).IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<EmployeeSkill>(entity =>
        {
            entity.HasKey(e => e.EmployeeSkillId).HasName("PK__Employee__1CC7FE6C143A91B6");

            entity.ToTable("EmployeeSkill");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSkills)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__EmployeeS__Emplo__286302EC");

            entity.HasOne(d => d.Skill).WithMany(p => p.EmployeeSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK__EmployeeS__Skill__29572725");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skill__DFA091874E3B259D");

            entity.ToTable("Skill");

            entity.Property(e => e.SkillName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
