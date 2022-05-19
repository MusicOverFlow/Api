using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class ModelsContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Commentary> Commentaries { get; set; }

    public ModelsContext(DbContextOptions<ModelsContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasMany(a => a.Posts).WithOne(p => p.Account)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.Commentaries).WithOne(p => p.Account)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Commentaries).WithOne(c => c.Post)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
