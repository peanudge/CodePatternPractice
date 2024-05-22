
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebDatabaseProject.EAV;

namespace WebDatabaseProject;

public class BlogContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; } = null!;

    public DbSet<BlogConfig> BlogConfigs { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;User Id=sa;Password=MyPass@word;TrustServerCertificate=true");
        }
        // optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>()
            .HasMany(b => b.Configs)
            .WithOne()
            .HasForeignKey(v => v.BlogId);


        modelBuilder.Entity<BlogConfig>()
            .HasIndex(v => new { v.BlogId, v.Key })
            .IsUnique();
    }
}

public class BlogContextFactory : IDesignTimeDbContextFactory<BlogContext>
{
    public BlogContext CreateDbContext(string[] args)
    {
        return new BlogContext();
    }
}
