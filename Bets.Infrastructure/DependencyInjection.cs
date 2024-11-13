using Bets.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BetsWriterDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WriterConnection")));

        services.AddDbContext<BetsReaderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ReaderConnection")));

        services.AddScoped<IBetsWritableDbContext>(provider => provider.GetRequiredService<BetsWriterDbContext>());

        services.AddScoped<IBetsReadableDbContext>(provider => provider.GetRequiredService<BetsReaderDbContext>());

        return services;
    }
}
