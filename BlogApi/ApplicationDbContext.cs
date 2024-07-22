using ArticlesAPI.Entities;
using BlogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApi;

public class ApplicationDbContext : IdentityDbContext<User, Role, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Person>()
            .HasOne(p => p.User)
            .WithOne(u => u.Person)
            .HasForeignKey<Person>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Rating>()
            .HasKey(r => new { r.ArticleId, r.PersonId });

        builder.Entity<Rating>()
            .HasOne(r => r.Person)
            .WithMany(p => p.Ratings)
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Rating>()
            .HasOne(r => r.Article)
            .WithMany(p => p.Ratings)
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ArticleCategory>()
            .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

        builder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Article)
            .WithMany(a => a.ArticleCategories)
            .HasForeignKey(ac => ac.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Category)
            .WithMany(c => c.ArticleCategories)
            .HasForeignKey(ac => ac.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }

    public async Task SeedData(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new Role()
            {
                Name = "Admin",
            });
        }

        var adminEmail = "admin@admin.com";
        var passwordHasher = new PasswordHasher<User>();
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User()
            {
                UserName = "Admin01",
                Email = adminEmail,
                PasswordHash = passwordHasher.HashPassword(null, "Abcd1234!"),
            };

            await userManager.CreateAsync(adminUser);
            await userManager.AddToRoleAsync(adminUser, "Admin");

            await this.SaveChangesAsync();
        }
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<Rating> Ratings { get; set; }
}
