using Wallets.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wallets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WalletsWriterDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WriterConnection")));

        services.AddDbContext<WalletsReaderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ReaderConnection")));

        services.AddScoped<IWalletsWritableDbContext>(provider => provider.GetRequiredService<WalletsWriterDbContext>());

        services.AddScoped<IWalletsReadableDbContext>(provider => provider.GetRequiredService<WalletsReaderDbContext>());

        return services;
    }
}
