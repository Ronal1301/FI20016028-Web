using BooksApp.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BooksApp.EFData;

public class AppDbContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TitleTag> TitlesTags => Set<TitleTag>();

    public string DbPath { get; }

    public AppDbContext()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
        var dataDir = Path.Combine(projectRoot, "data");
        Directory.CreateDirectory(dataDir);
        DbPath = Path.Combine(dataDir, "books.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TitleTag>().ToTable("TitlesTags");

        modelBuilder.Entity<Title>(entity =>
        {
            entity.Property(p => p.TitleId).HasColumnOrder(0);
            entity.Property(p => p.AuthorId).HasColumnOrder(1);
            entity.Property(p => p.TitleName).HasColumnOrder(2);
        });

        modelBuilder.Entity<Title>()
            .HasOne(t => t.Author)
            .WithMany(a => a.Titles)
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TitleTag>()
            .HasOne(tt => tt.Title)
            .WithMany(t => t.TitlesTags)
            .HasForeignKey(tt => tt.TitleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TitleTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TitlesTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
