using Bets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bets.Application.Interfaces;

public interface IBetsReadableDbContext
{
    DbSet<Bet> Bets { get; set; }
}
