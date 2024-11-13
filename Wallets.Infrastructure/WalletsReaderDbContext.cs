using Microsoft.EntityFrameworkCore;
using Wallets.Application.Interfaces;
using Wallets.Core.Entities;

namespace Wallets.Infrastructure;

class WalletsReaderDbContext : DbContext, IWalletsReadableDbContext
{
    public WalletsReaderDbContext(DbContextOptions<WalletsReaderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
}
