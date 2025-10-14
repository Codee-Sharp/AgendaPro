using AgendaPro.Domain.Tags.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaPro.Infrastucture.Data.Context;

public class AgendaProDbContext : DbContext
{
    public AgendaProDbContext(DbContextOptions<AgendaProDbContext> options) : base(options)
    {
    }

    public DbSet<TagModel> Tags { get; set; }
}
