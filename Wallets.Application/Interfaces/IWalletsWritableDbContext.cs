using Wallets.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Wallets.Application.Interfaces;

public interface IWalletsWritableDbContext
{
    DbSet<Wallet> Wallets { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
