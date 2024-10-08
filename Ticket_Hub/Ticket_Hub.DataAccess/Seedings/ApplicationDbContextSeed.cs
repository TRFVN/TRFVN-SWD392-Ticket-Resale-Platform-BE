using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.DataAccess.Seedings;

public class ApplicationDbContextSeed
{
    public static void SeedEmailTemplate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailTemplate>().HasData(
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "WelcomeEmail",
                SenderName = "Ticket Hub",
                SenderEmail = "tickethub4@gmail.com",
                Category = "Welcome",
                SubjectLine = "Welcome to Ticket Hub!",
                PreHeaderText = "Thank you for signing up!",
                PersonalizationTags = "{FirstName}, {LastName}",
                BodyContent =
                    "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.",
                FooterContent = "<p>Contact us at tickethub4@gmail.com</p>",
                CallToAction = "<a href=\"{{VerificationLink}}\">Verify Your Email</a>",
                Language = "English",
                RecipientType = "Member",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "ForgotPasswordEmail",
                SenderName = "Ticket Hub",
                SenderEmail = "tickethub4@gmail.com",
                Category = "Security",
                SubjectLine = "Reset Your Password",
                PreHeaderText = "Reset your password to regain access",
                PersonalizationTags = "[UserFullName], [ResetPasswordLink]",
                BodyContent =
                    "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.",
                FooterContent = "If you did not request a password reset, please ignore this email.",
                CallToAction =
                    $"https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "SendVerifyEmail",
                SenderName = "Ticket Hub",
                SenderEmail = "tickethub4@gmail.com",
                Category = "Verify",
                SubjectLine = "Ticket Hub Verify Email",
                PreHeaderText = "User Account Verified!",
                PersonalizationTags = "{FirstName}, {LinkLogin}",
                BodyContent =
                    "<p>Thank you for registering your Ticket Hub account. Click here to go back the page</p>",
                FooterContent = "<p>Contact us at tickethub4@gmail.com</p>",
                CallToAction = "<a href=\"{{Login}}\">Login now</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            }
        );
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedAdminAccount(ModelBuilder modelBuilder)
    {
        var userRoleId = "8fa7c7bb-b4dc-480d-a660-e07a90855d5u";
        var staffRoleId = "8fa7c7bb-b4dd-480d-a660-e07a90855d5s";
        var adminRoleId = "8fa7c7bb-daa5-a660-bf02-82301a5eb32a"; // Add admin role

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = userRoleId,
                ConcurrencyStamp = StaticUserRoles.Member,
                Name = StaticUserRoles.Member,
                NormalizedName = StaticUserRoles.Member,
            },
            new IdentityRole
            {
                Id = staffRoleId,
                ConcurrencyStamp = StaticUserRoles.Staff,
                Name = StaticUserRoles.Staff,
                NormalizedName = StaticUserRoles.Staff,
            },
            new IdentityRole
            {
                Id = adminRoleId,
                ConcurrencyStamp = StaticUserRoles.Admin,
                Name = StaticUserRoles.Admin,
                NormalizedName = StaticUserRoles.Admin,
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Seeding admin user
        var adminUserId = "BestZedAndYasuo";
        var hasher = new PasswordHasher<ApplicationUser>();
        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            FullName = "Admin User",
            BirthDate = new DateTime(1990, 1, 1), // Set appropriate value
            AvatarUrl = "https://example.com/avatar.png", // Set appropriate value
            Country = "Country", // Set appropriate value
            Address = "123 Admin St",
            Cccd = "123456789123",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "Admin123!"),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // Assigning the admin role to the admin user
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminUserId
        });
    }
}