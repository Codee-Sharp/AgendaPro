using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Tags.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaPro.Infrastucture.Data.Context;

public class AgendaProDbContext : DbContext
{
    public AgendaProDbContext(DbContextOptions<AgendaProDbContext> options) : base(options)
    {
    }

    public DbSet<TagModel> Tags { get; set; }
    public DbSet<ServiceModel> Services { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendaProDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
