using Wallets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wallets.Application.Interfaces;

public interface IWalletsReadableDbContext
{
    DbSet<Wallet> Wallets { get; set; }
}
