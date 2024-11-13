using Wallets.Application.Interfaces;
using Wallets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wallets.Infrastructure;

class WalletsWriterDbContext : DbContext, IWalletsWritableDbContext
{
    public WalletsWriterDbContext(DbContextOptions<WalletsWriterDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
}
