using Datings.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Datings.Api.Data;

public sealed class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public DataContext (DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<UserBalance> UserBalances { get; set; } = null!;
    public DbSet<BalanceHistory> BalanceHistories { get; set; } = null!;
    public DbSet<CategoryTag> CategoryTags { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<AnswerComment> AnswerComments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<CategoryTag>()
            .HasMany(e => e.Questions)
            .WithMany(e => e.CategoryTags);
    }
}