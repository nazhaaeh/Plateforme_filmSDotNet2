using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moviesplatform.Areas.Identity.Data;
using Moviesplatform.Models;
using Plateforme_Filmes.Models;
using System.Reflection.Emit;

namespace Moviesplatform.Data;

public class MoviesplatformDBContext : IdentityDbContext<MoviesplatformUser>
{
    public MoviesplatformDBContext(DbContextOptions<MoviesplatformDBContext> options)
        : base(options)
    {
    }
    public DbSet<Film> Films { get; set; }

    public DbSet<Series> Series { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Watchlist> WatchLists { get; set; }
    public DbSet<WachListItem> WatchlistItems { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Film>()
              .HasOne(m => m.Category)
              .WithMany(c => c.Movies)
              .HasForeignKey(m => m.CategoryId);

        // Relation Many-to-One Series -> Category
        builder.Entity<Series>()
    .HasOne(s => s.Category)
    .WithMany(c => c.Series)
    .HasForeignKey(s => s.CategoryId);


        builder.Entity<WachListItem>()
             .HasOne(wli => wli.Watchlist)
             .WithMany(w => w.WatchlistItems)
             .HasForeignKey(wli => wli.WatchlistId);

        // Relation entre WatchlistItem et Film
        builder.Entity<WachListItem>()
            .HasOne(wli => wli.Film)
            .WithMany()
            .HasForeignKey(wli => wli.FilmId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relation entre WatchlistItem et Series
        builder.Entity<WachListItem>()
            .HasOne(wli => wli.Series)
            .WithMany()
            .HasForeignKey(wli => wli.SeriesId)
            .OnDelete(DeleteBehavior.SetNull);


    }
}
