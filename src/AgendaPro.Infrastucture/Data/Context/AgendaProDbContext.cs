using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Clients.Models;
using Microsoft.EntityFrameworkCore;


namespace AgendaPro.Infrastucture.Data.Context;

public class AgendaProDbContext : DbContext
{
    public AgendaProDbContext(DbContextOptions<AgendaProDbContext> options) : base(options)
    {
    }

    public DbSet<TagModel> Tags { get; set; }
    public DbSet<ServiceModel> Services { get; set; }
    public DbSet<ClientModel> Clients { get; set; }
}
