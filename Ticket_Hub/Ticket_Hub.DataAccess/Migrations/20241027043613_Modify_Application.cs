using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticket_Hub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Modify_Application : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("37917612-8f3f-494d-906a-275e1dbf661f"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("562029a8-430f-43f6-90e6-342afcc28710"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("5b0fb601-40fd-487f-9c22-fba37f8997ff"));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ba0f379-22dd-4e24-8d4b-eb07cb04c82f", "AQAAAAIAAYagAAAAENefJdH7LIgg1LIDaOa1bg1gZwbvHCf0vZlvERg4hsuJnj/mEKDqfvIL0iMrEuWD/Q==", "ceb7faa1-b171-4758-b563-4b3e8852161d" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("57a2c8d8-f3f1-42ac-b884-7998ed95eaf2"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null },
                    { new Guid("749bdc0f-ea77-4ab3-9b4d-fc836e674db2"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("b94cc6ef-4269-41fa-b22a-2245af4dc25a"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("57a2c8d8-f3f1-42ac-b884-7998ed95eaf2"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("749bdc0f-ea77-4ab3-9b4d-fc836e674db2"));

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: new Guid("b94cc6ef-4269-41fa-b22a-2245af4dc25a"));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BestZedAndYasuo",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17bf58aa-5725-4a20-85cc-0871ac5cc41f", "AQAAAAIAAYagAAAAECA2Eyb/b6i1hhYHwAFPmyop5QDNXmcLTillwTA+SPN+mWQaP4guC4kwW6egs/mGOg==", "cc85c067-caeb-4604-a742-7e16f9e0f87d" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "BodyContent", "CallToAction", "Category", "CreatedBy", "CreatedTime", "FooterContent", "Language", "PersonalizationTags", "PreHeaderText", "RecipientType", "SenderEmail", "SenderName", "Status", "SubjectLine", "TemplateName", "UpdatedBy", "UpdatedTime" },
                values: new object[,]
                {
                    { new Guid("37917612-8f3f-494d-906a-275e1dbf661f"), "<p>Thank you for registering your Ticket Hub account. Click here to verify your email.</p>", "<a href=\"https://localhost:5173/verifyemail?userId={{UserId}}&token={{Token}}\" class='button'>Verify Email</a>", "Verify", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LinkLogin}", "User Account Verified!", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Ticket Hub Verify Email", "SendVerifyEmail", null, null },
                    { new Guid("562029a8-430f-43f6-90e6-342afcc28710"), "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.", "https://cursuslms.xyz/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)", "Security", null, null, "If you did not request a password reset, please ignore this email.", "English", "[UserFullName], [ResetPasswordLink]", "Reset your password to regain access", "Customer", "tickethub4@gmail.com", "Ticket Hub", 1, "Reset Your Password", "ForgotPasswordEmail", null, null },
                    { new Guid("5b0fb601-40fd-487f-9c22-fba37f8997ff"), "Dear [UserFullName],<br><br>Welcome to Ticket Hub!  We are thrilled to have you as part of our community dedicated to providing the best ticket-buying and reselling experience.", "<a href=\"{{VerificationLink}}\">Verify Your Email</a>", "Welcome", null, null, "<p>Contact us at tickethub4@gmail.com</p>", "English", "{FirstName}, {LastName}", "Thank you for signing up!", "Member", "tickethub4@gmail.com", "Ticket Hub", 1, "Welcome to Ticket Hub!", "WelcomeEmail", null, null }
                });
        }
    }
}
