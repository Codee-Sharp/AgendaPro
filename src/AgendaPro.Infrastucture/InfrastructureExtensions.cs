using AgendaPro.Domain.Tags.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using AgendaPro.Infrastucture.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaPro.Infrastucture;

/// <summary>
/// Classe de extensão para configurar os serviços de infraestrutura.
/// </summary>
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddConfigurationDatabase(configuration);
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddConfigurationDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("PostgreSqlConnection");

        services.AddDbContext<AgendaProDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITagRepository, TagRepository>();
        return services;
    }
}
