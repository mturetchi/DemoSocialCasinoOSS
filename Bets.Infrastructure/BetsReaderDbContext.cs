using Bets.Application.Interfaces;
using Bets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bets.Infrastructure;

class BetsReaderDbContext : DbContext, IBetsReadableDbContext
{
    public BetsReaderDbContext(DbContextOptions<BetsReaderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bet> Bets { get; set; }
}
