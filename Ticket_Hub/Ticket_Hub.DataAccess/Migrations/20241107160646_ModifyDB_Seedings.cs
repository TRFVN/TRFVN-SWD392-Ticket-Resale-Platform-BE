using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticket_Hub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDB_Seedings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("47494aae-1f53-44c6-8517-55b977391773"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("64d38301-a84a-4cf3-890c-242e30910da8"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("aca790af-8d1f-4874-bf8b-31c4e50d87de"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "459ebc87-0fe5-46b8-bb6a-675e6d63343b", "AQAAAAIAAYagAAAAEJEbRq24gxd+ge0vEY5bUdk8MZCjauY825umJZnrWE77I09rhWE5jpdPbib/U7y46w==", "6cd8a564-dda1-4413-96c0-fc03a22b82b8" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AvatarUrl", "BirthDate", "Cccd", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "StaffId", 0, "123 Staff St", "https://example.com/avatarStaff.png", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456789126", "41abfb63-31a8-48e7-9ad4-fc964c0ae760", "Country", "staff1@gmail.com", true, "Staff_1 User", true, null, "STAFF1@GMAIL.COM", "STAFF1@GMAIL.COM", "AQAAAAIAAYagAAAAEBSW+HK4vqXoytSLovyyhZ+umIH+9ZOm8oSKZv9N5i/tPNx3yKj3Ha/lFGNCDm24gQ==", "0123456789", true, "8b9578b8-3fc6-4dc6-97af-68b2fc1a91d3", false, "staff1@gmail.com" },
                    { "StaffId2", 0, "456 Staff St", "https://example.com/avatarStaff2.png", new DateTime(1991, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456789124", "842447e7-6b95-4bdf-a5fb-3c2aed3beb16", "Country", "staff2@gmail.com", true, "Staff_2 User", true, null, "STAFF2@GMAIL.COM", "STAFF2@GMAIL.COM", "AQAAAAIAAYagAAAAEGrUjBR0gXHVFln6KifURTwSNguvylhWp4N0k4HIrniZBlfB/qPcKX+0Zbn/aAxZBg==", "0987654321", true, "9c9c08b5-53fd-483c-812d-6b3471253506", false, "staff2@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("172269c0-0ee7-4913-b778-8b1e92b1aa49"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("41d3bc64-3723-47dc-8e23-ee0d90560732"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("6df13d22-fdc9-4e19-820d-af6ad500f604"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "StaffId" },
                    { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "StaffId2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "StaffId" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-b4dd-480d-a660-e07a90855d5s", "StaffId2" });

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("172269c0-0ee7-4913-b778-8b1e92b1aa49"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("41d3bc64-3723-47dc-8e23-ee0d90560732"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("6df13d22-fdc9-4e19-820d-af6ad500f604"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6307e09c-e384-42a1-b53f-dfdbea3394ba", "AQAAAAIAAYagAAAAECV22eGEE+2rdPFOLr6qvGvnuios9sjpDF+40hW/mDWUzN2aW5X2/9KbT042o1rurg==", "16a57a62-fafc-4665-af29-51c184533cdc" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("47494aae-1f53-44c6-8517-55b977391773"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("64d38301-a84a-4cf3-890c-242e30910da8"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null },
                    { new Guid("aca790af-8d1f-4874-bf8b-31c4e50d87de"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null }
                });
        }
    }
}
