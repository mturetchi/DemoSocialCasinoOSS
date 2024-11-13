using Bets.Application.Interfaces;
using Bets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bets.Infrastructure;

class BetsWriterDbContext : DbContext, IBetsWritableDbContext
{
    public BetsWriterDbContext(DbContextOptions<BetsWriterDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bet> Bets { get; set; }
}
