using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticket_Hub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDB_TicketNegotiations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("03498463-610f-4320-923e-8e63b48d6b4c"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("61d05f0b-312d-440a-80d1-ef19c0a6d2ca"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("d8ec3f16-5f55-4c1f-9be3-81a05a174f35"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "338b67fb-b988-4963-b562-21bae6119c91", "AQAAAAIAAYagAAAAEGrx5PsNFZ51AA6ew8RU/ejERUGt4o+rvk3fGCqE3U0+yV1837I/NL0V3QgZvGNhwg==", "d732e6a7-a90b-442e-b9de-853e1d96bb83" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd71a0b2-e582-45d2-aa4c-5e9f783355b5", "AQAAAAIAAYagAAAAEOO3LSBUFlsj/EYa3idyUPoQeEHhSxALAPo5mInk16LvsSlk7MkyMa9yDOqSMFEtfQ==", "171b4ca0-f4bd-4456-a40d-e6e673d822ed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d06b414-663a-4d0a-bcf0-671ab314f1d3", "AQAAAAIAAYagAAAAEHE6R1uHcBVFaa0Qq0bSx3c1Ktzo9W68UHPuJPygxsylpyKL8VrBeIzyM5kPWd27zg==", "c2706585-15c8-443a-ae8d-68f09c6170f8" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("5b9b656d-516f-4a5c-8ff6-37e8def961c0"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("626335c5-b677-491e-b22d-0b498dcf822c"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null },
                    { new Guid("84caf337-3349-4cdd-8690-dd21e8cde810"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Negotiations_MessageId",
                table: "Negotiations",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Negotiations_Messages_MessageId",
                table: "Negotiations",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negotiations_Messages_MessageId",
                table: "Negotiations");

            migrationBuilder.DropIndex(
                name: "IX_Negotiations_MessageId",
                table: "Negotiations");

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("5b9b656d-516f-4a5c-8ff6-37e8def961c0"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("626335c5-b677-491e-b22d-0b498dcf822c"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("84caf337-3349-4cdd-8690-dd21e8cde810"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41ebc4d6-c48b-4627-ad52-0276ceeadc7a", "AQAAAAIAAYagAAAAEHrnzNRSfW2G9kZLb07EDLtqBrI+u3O1EuVXGsXOB0poC9rWS1K4wiCcn3Wgf3bTQw==", "01787fcf-2256-4e1b-9bca-eb85d0b487ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1cca6ac-95a2-4f97-b34c-c717d100086d", "AQAAAAIAAYagAAAAECg127nxW+xusLAoP8cRLfdNVHsF7w6E3eFNANaGSRXb+3//zal7DB5Po5mqRf8g9A==", "b16b9fed-231f-4ac5-8489-d8c8a2f74e42" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "StaffId2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e118387c-ff5b-4d28-9f20-036673b88d5e", "AQAAAAIAAYagAAAAEDUrEyqs1luKuPRYL9n08akoUwNzFJMTU8TYm6ZbjoQbc6MfhL43oJ/G2U6iudHWDg==", "c47dc3f2-61e3-4826-8b11-a8d71c87a50d" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("03498463-610f-4320-923e-8e63b48d6b4c"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("61d05f0b-312d-440a-80d1-ef19c0a6d2ca"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null },
                    { new Guid("d8ec3f16-5f55-4c1f-9be3-81a05a174f35"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null }
                });
        }
    }
}
