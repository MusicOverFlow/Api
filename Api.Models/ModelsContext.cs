global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;

using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class ModelsContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Commentary> Commentaries { get; set; }
    public DbSet<Group> Groups { get; set; }

    public ModelsContext(DbContextOptions<ModelsContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasMany(a => a.OwnedPosts).WithOne(p => p.Owner)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Account>()
            .HasMany(a => a.OwnedCommentaries).WithOne(p => p.Owner)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.Groups).WithMany(g => g.Members);

        modelBuilder.Entity<Group>()
            .HasMany(g => g.Members).WithMany(a => a.Groups);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Commentaries).WithOne(c => c.Post)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Likes).WithMany(a => a.LikedPosts);

        modelBuilder.Entity<Commentary>()
            .HasMany(p => p.Likes).WithMany(a => a.LikedCommentaries);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.Owner);

        modelBuilder.Entity<Post>().Navigation(p => p.Owner).AutoInclude();
        modelBuilder.Entity<Post>().Navigation(p => p.Group).AutoInclude();
        modelBuilder.Entity<Post>().Navigation(p => p.Commentaries).AutoInclude();
        modelBuilder.Entity<Post>().Navigation(p => p.Likes).AutoInclude();

        modelBuilder.Entity<Commentary>().Navigation(c => c.Owner).AutoInclude();
        modelBuilder.Entity<Commentary>().Navigation(c => c.Likes).AutoInclude();
        
        modelBuilder.Entity<Group>().Navigation(g => g.Owner).AutoInclude();
        modelBuilder.Entity<Group>().Navigation(g => g.Members).AutoInclude();
    }
}
