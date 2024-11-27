﻿using Microsoft.EntityFrameworkCore;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Configurations;
using Task = EclipseWorks.Domain.Models.Task;

namespace EclipseWorks.Infrastructure.Implementations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<Project>());
        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<Task>());

        // Configure Project entity
        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Project>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Project>()
            .Property(p => p.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for tasks when project is deleted if not soft deleted

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        // Configure Task entity
        modelBuilder.Entity<Task>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Task>()
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Task>()
            .Property(t => t.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Task>()
            .Property(t => t.PriorityLevel)
            .HasConversion<string>()
            .IsRequired();

        modelBuilder.Entity<Task>()
            .Property(t => t.IsCompleted)
            .HasDefaultValue(false)
            .IsRequired();

        modelBuilder.Entity<Task>()
            .HasMany(t => t.Histories)
            .WithOne()
            .HasForeignKey(h => h.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for histories when task is deleted

        // Configure many-to-many relationship between User and Project
        modelBuilder.Entity<ProjectUser>()
            .HasKey(pu => new { pu.UserId, pu.ProjectId });

        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany(u => u.ProjectUsers)
            .HasForeignKey(pu => pu.UserId);

        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId);

        // Configure many-to-many relationship between User and Task
        modelBuilder.Entity<TaskUser>()
            .HasKey(tu => new { tu.UserId, tu.TaskId });

        modelBuilder.Entity<TaskUser>()
            .HasOne(tu => tu.User)
            .WithMany(u => u.TaskUsers)
            .HasForeignKey(tu => tu.UserId);

        modelBuilder.Entity<TaskUser>()
            .HasOne(tu => tu.Task)
            .WithMany(t => t.TaskUsers)
            .HasForeignKey(tu => tu.TaskId);
    }
}