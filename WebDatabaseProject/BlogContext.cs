
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace WebDatabaseProject;

public class BlogContext : DbContext
{
    public DbSet<BlogBase> Blogs { get; set; } = null!;
    public DbSet<ShopBlog> ShopBlogs { get; set; } = null!;
    public DbSet<TechBlog> TechBlogs { get; set; } = null!;


    public DbSet<BlogConfig> BlogConfigs { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;User Id=sa;Password=MyPass@word;TrustServerCertificate=true");
        }
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BlogBase>()
            .HasMany(b => b.Configs)
            .WithOne()
            .HasForeignKey(v => v.BlogId);


        modelBuilder.Entity<BlogBase>()
            .HasMany(b => b.ChildBlogs)
            .WithOne(ch => ch.ParentBlog)
            .HasForeignKey(ch => ch.ParentBlogId)
            .OnDelete(DeleteBehavior.ClientCascade);

        // INFO: if you want to use Table Per Table, uncomment this.
        modelBuilder.Entity<BlogBase>().ToTable("Blogs");
        modelBuilder.Entity<ShopBlog>().ToTable("ShopBlogs");
        modelBuilder.Entity<TechBlog>().ToTable("TechBlogs");

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
