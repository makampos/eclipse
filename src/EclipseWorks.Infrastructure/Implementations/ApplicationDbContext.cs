using Microsoft.EntityFrameworkCore;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Configurations;

namespace EclipseWorks.Infrastructure.Implementations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<SampleModel> Deliverers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<SampleModel>());
    }
}