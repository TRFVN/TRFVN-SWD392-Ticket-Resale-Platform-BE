using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticket_Hub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDB_TicketImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "TicketQuantity",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e6d0f898-9bbd-4a24-9e7d-1b57a430cb28", "AQAAAAIAAYagAAAAED7lxg97/mL3+/NJ/fieKxWFOSE/g6mS565YElEW7Fy6/UZXqOXTXfGfr4HqeAvW9g==", "d6116735-247c-4959-982d-44b678262882" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("887cc423-ddef-462f-9aa7-7399214d805d"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={UserId}&token={Token}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("b2857b98-91f6-4166-92fd-13fe8322c719"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("c2cfa27a-86a7-4aed-a838-54d7abd90b16"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("887cc423-ddef-462f-9aa7-7399214d805d"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("b2857b98-91f6-4166-92fd-13fe8322c719"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c2cfa27a-86a7-4aed-a838-54d7abd90b16"));

            migrationBuilder.AddColumn<int>(
                name: "TicketQuantity",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
