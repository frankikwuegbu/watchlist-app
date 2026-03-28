using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interface;

public interface IApplicationDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Watchlist> Watchlist { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}