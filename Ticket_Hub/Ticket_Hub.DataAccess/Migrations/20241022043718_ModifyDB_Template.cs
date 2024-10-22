using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticket_Hub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDB_Template : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("121839ef-31cb-4311-9da1-a1e56dfc3f71"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("51c58c51-58a7-47ad-93c3-5cebeca6af00"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("e2918395-6396-442f-bcdc-a475ab6d254b"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ee7554c-2774-48d1-8832-141caa4fb7b8", "AQAAAAIAAYagAAAAEGTx8gx26/54lNZ0pPwLBSIhp3OdvnjGQMF+yX352nCmt1XsPUMADzuRq8TdHd0ShA==", "b454a122-8d0c-426d-a05f-a7831a2ae7a0" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("177b607a-68a9-4a1f-87ce-12034654b9d0"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={UserId}&token={Token}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("8803389c-725e-4c0d-b2f0-9f1f12398436"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("f5f626cd-604b-4ab7-aa44-136aaf73d27c"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("177b607a-68a9-4a1f-87ce-12034654b9d0"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("8803389c-725e-4c0d-b2f0-9f1f12398436"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("f5f626cd-604b-4ab7-aa44-136aaf73d27c"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd3b75e2-e3c6-489d-8709-8b5dc1900378", "AQAAAAIAAYagAAAAEC76AREKKF7YpTAQdSxyw1BmCe+hsWDPmEUGJy+q24UCnlD3BBLEmYSnJs2R1r90lw==", "7dcef17b-cee3-4031-9e4b-76883d6dcdb2" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("121839ef-31cb-4311-9da1-a1e56dfc3f71"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null },
                    { new Guid("51c58c51-58a7-47ad-93c3-5cebeca6af00"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("e2918395-6396-442f-bcdc-a475ab6d254b"), "<p>Thank you for registering your Ticket Hub account. Click here to go back the page</p>", "<a href=\"{{Login}}\">Login now</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null }
                });
        }
    }
}
