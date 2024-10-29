using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticket_Hub.DataAccess.Seedings;
using Ticket_Hub.Models;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.DataAccess.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<MemberRating> MemberRatings { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Transactions> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<RefreshTokens> RefreshTokens { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Privacy> Privacies { get; set; }
    public DbSet<TermOfUse> TermOfUses { get; set; }
    public DbSet<AppLogo> AppLogos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        ApplicationDbContextSeed.SeedAdminAccount(modelBuilder);

        //Seed Email Template
        ApplicationDbContextSeed.SeedEmailTemplate(modelBuilder);
    }
}