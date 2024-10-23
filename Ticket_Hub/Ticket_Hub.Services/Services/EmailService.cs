﻿using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class EmailService : IEmailService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public EmailService(IConfiguration configuration, IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SendEmailToClientAsync(string toEmail, string subject, string body)
    {
        // Lấy thông tin cấu hình email từ file appsettings.json
        try
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromPassword = _configuration["EmailSettings:FromPassword"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

            // Tạo đối tượng MailMessage
            var message = new MailMessage(fromEmail, toEmail, subject, body);
            message.IsBodyHtml = true;

            // Tạo đối tượng SmtpClient và gửi email
            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> SendVerifyEmail(string toMail, string userId, string token)
    {
        // Tạo confirmationLink với domain của frontend
        string frontendDomain = "http://localhost:5173";
        string confirmationLink = $"{frontendDomain}/verifyemail?token={token}&userId={userId}";

        // Gọi SendEmailFromTemplate và truyền các tham số userId và token
        return await SendEmailFromTemplate(toMail, "SendVerifyEmail", userId, confirmationLink);
    }

    public async Task<bool> SendEmailResetAsync(string toEmail, string subject, ApplicationUser user,
        string currentDate, string resetLink,
        string operatingSystem, string browser, string ip, string region, string city, string country)
    {
        // Lấy thông tin cấu hình email từ file appsettings.json
        try
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromPassword = _configuration["EmailSettings:FromPassword"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

            var body = $@"
<table style=""width: 720px; margin: 0 auto;"">
    <tr>
        <td align=""left""><img src=""https://demo.stripocdn.email/content/guids/videoImgGuid/images/group_48_CGo.png"" alt=""Cursus Logo"" style=""display: block"" height=""37"" title=""Logo"" /></td>
        <td align=""center""><h2 style=""font-size: 16px"">{currentDate}</h2></td>
        <td align=""right"">
           <div>   
                <a target=""_blank"" href=""#""><img title=""Facebook"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-black/facebook-logo-black.png"" alt=""Facebook"" width=""24"" height=""24"" /></a>           
                <a target=""_blank"" href=""#""><img title=""Twitter"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-black/twitter-logo-black.png"" alt=""Twitter"" width=""24"" height=""24"" /></a>           
                <a target=""_blank"" href=""#""><img title=""Instagram"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-black/instagram-logo-black.png"" alt=""Instagram"" width=""24"" height=""24"" /></a>           
                <a target=""_blank"" href=""#""><img title=""Youtube"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-black/youtube-logo-black.png"" alt=""Youtube"" width=""24"" height=""24"" /></a>
           </div>
        </td>
    </tr>
</table>

<div style=""width: 720px; margin: 0 auto;"">
    <div style=""text-align: -webkit-center;"">
        <img src=""https://demo.stripocdn.email/content/guids/videoImgGuid/images/5184834_EjS.png"" alt=""Reset Image"" style=""display: block;"" title=""Reset Image"" width=""560"">
    </div>
    <div>
        <h2 style=""line-height: 120%;"">Forgot Your Password ?</h2>
        <p style=""font-size: 16px; line-height: 120%;"">Hi <strong>{user.FullName}</strong>,<br><br>We’ve received a request to reset the password for the Cursus account associated with <strong>{user.UserName}</strong>. No changes have been made to your account yet. This password reset is only valid for the next 24 hours.<br><br>You can reset your password by clicking the link button below:</p>
        <div style=""text-align: center; margin-bottom: 40px; margin-top: 40px;"">
            <button style=""background: #26c662; border-radius: 5px; border: none; padding: 10px 20px;"">
                <a href=""{resetLink}"" target=""_blank"" style=""color: rgb(255, 255, 255); background: rgb(38, 198, 98); border-radius: 5px; text-decoration: none;"">RESET YOUR PASSWORD</a>
            </button>
        </div>
    </div>
    <div>
        <p style=""font-size: 16px; line-height: 120%;"">For security, this request was received from a <span style=""color: blue; font-weight: bold;"">{operatingSystem}</span> device using <span style=""color: blue; font-weight: bold;"">{browser}</span> have IP address is <span style=""color: blue; font-weight: bold;"">{ip}</span> at location <span style=""color: blue; font-weight: bold;"">{region}</span>, <span style=""color: blue; font-weight: bold;"">{city}</span>, <span style=""color: blue; font-weight: bold;"">{country}</span>. If you did not request a password reset, please ignore this email or contact support if you have questions.<br><br></p><p style=""font-size: 16px; line-height: 120%;"">Thanks,</p><p style=""font-size: 16px; line-height: 120%;"">The Cursus Team</p></td>
    </div>
    <div style=""background-color: #f6f6f6;"">
        <div style=""padding-top: 10px; "">
            <h2 style=""padding-left: 20px;"" ;><strong>Questions? We have people</strong></h2>
        </div>
        <div style=""text-align: center; padding-top: 20px;"">

<table style=""width: 720px; margin: 0 auto; padding-bottom: 10px;"">
    <tr>
        <td align=""center"">
           <div style=""background-color: white; width: 273px; border-radius: 10px; text-align: center; padding-top: 1px; padding-bottom: 1px;"">
              <h2 style=""color: #666666;""><strong>Call</strong></h2>
              <a target=""_blank"" href=""#""><img src=""https://tlr.stripocdn.email/content/guids/CABINET_2af5bc24a97b758207855506115773ae/images/73661620310209153.png"" alt=""Phone""  width=""20""></a>
              <p style=""margin-top: 5px;""><a target=""_blank"" style=""color: #666666; text-decoration: none;"" href=""tel:"">(+84) 0329 - 258 - 953&nbsp;</a></p>
           </div>
        </td>
        <td align=""center"">
           <div style=""background-color: white; width: 273px; border-radius: 10px; text-align: center; padding-top: 1px; padding-bottom: 1px;"">
              <h2 style=""color: #666666;""><strong>Reply</strong></h2>
              <a target=""_blank"" href=""#""><img src=""https://tlr.stripocdn.email/content/guids/CABINET_2af5bc24a97b758207855506115773ae/images/16961620310208834.png"" alt=""Email""  width=""20""></a>
              <p style=""margin-top: 5px;""><a target=""_blank"" href=""mailto:cursusservicetts@email.com"" style=""color: #666666; text-decoration: none;"">cursusservicetts@gmail.com</a></p>
           </div>
        </td>
    </tr>
</table>
 
            <p style=""padding-top: 10px; padding-bottom: 20px;"">Monday - Friday, 8 am - 6 pm est</p>
        </div>                       
    </div>    
    <div style=""background-color: #26c662; "">
        <div style=""text-align: center; padding-top: 30px;"">
            <a target=""_blank"" href=""mailto:cursusservicetts@email.com"" style=""color: #ffffff; text-decoration: none; padding-right: 20px;"">About us</a>
            <a target=""_blank"" href=""#"" style=""color: #ffffff; text-decoration: none; padding-right: 20px;"">Blog</a>
            <a target=""_blank"" href=""#"" style=""color: #ffffff; text-decoration: none; padding-right: 20px;"">Career</a>
            <a target=""_blank"" href=""#"" style=""color: #ffffff; text-decoration: none;"">News</a>
        </div>
        <div style=""text-align: center; padding-top: 20px;"">            
            <a style=""padding: 20px;"" target=""_blank"" href=""#""><img title=""Facebook"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-white/facebook-logo-white.png"" alt=""Facebook"" width=""24"" height=""24""></a>
            <a style=""padding: 20px;"" target=""_blank"" href=""#""><img title=""Twitter"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-white/twitter-logo-white.png"" alt=""Twitter"" width=""24"" height=""24""></a>
            <a style=""padding: 20px;"" target=""_blank"" href=""#""><img title=""Instagram"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-white/instagram-logo-white.png"" alt=""Instagram"" width=""24"" height=""24""></a>
            <a style=""padding: 20px; align-self: center;"" target=""_blank"" href=""#""><img title=""Youtube"" src=""https://tlr.stripocdn.email/content/assets/img/social-icons/logo-white/youtube-logo-white.png"" alt=""Youtube"" width=""24"" height=""24""></a>          
        </div>
        <div>
            <p style=""font-size: 12px; text-align: center; color: #ffffff;padding: 10px 50px 30px 50px"">You are receiving this email because you have visited our site or asked us about the regular newsletter. Make sure our messages get to your Inbox (and not your bulk or junk folders).<br>
            <a target=""_blank"" style=""font-size: 12px; color: #ffffff;"" href=""#"">Privacy police</a> | <a target=""_blank"" style=""font-size: 12px; color: #ffffff;"">Unsubscribe</a></p>
        </div>
    </div>  
</div>";

            // Tạo đối tượng MailMessage
            var message = new MailMessage(fromEmail, toEmail, subject, body);
            message.IsBodyHtml = true;

            // Tạo đối tượng SmtpClient và gửi email
            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private async Task<bool> SendEmailFromTemplate(string toMail, string templateName, string userId, string token)
    {
        var template = await _unitOfWork.EmailTemplateRepository.GetAsync(t => t.TemplateName == templateName);
        if (template == null)
        {
            throw new Exception("Email template not found");
        }

        string callToAction = template.CallToAction
            .Replace("{{UserId}}", Uri.EscapeDataString(userId))
            .Replace("{{Token}}", Uri.EscapeDataString(token));
        string subject = template.SubjectLine;
        string body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <title>{subject}</title>
    <!--[if mso]>
    <noscript>
        <xml>
            <o:OfficeDocumentSettings>
                <o:PixelsPerInch>96</o:PixelsPerInch>
            </o:OfficeDocumentSettings>
        </xml>
    </noscript>
    <![endif]-->
    <style>
        /* Reset CSS */
        body, table, td, p, a, li, blockquote {{
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
            margin: 0;
            padding: 0;
        }}
        
        /* Base Styles */
        body {{
            margin: 0 !important;
            padding: 0 !important;
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333333;
            width: 100% !important;
            -webkit-font-smoothing: antialiased;
        }}

        /* Main Container */
        .email-container {{
            max-width: 600px !important;
            margin: 0 auto;
            padding: 20px;
        }}

        /* Card Container */
        .card {{
            background: #ffffff;
            border-radius: 16px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            margin: 20px 0;
            overflow: hidden;
        }}

        /* Header Styles */
        .header {{
            background: linear-gradient(135deg, #f97316 0%, #fb923c 100%);
            padding: 40px 20px;
            text-align: center;
        }}

        .header img {{
            width: 180px;
            height: auto;
            margin-bottom: 20px;
        }}

        .header h1 {{
            color: #ffffff;
            font-size: 28px;
            font-weight: 600;
            margin: 0;
            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}

        /* Content Styles */
        .content {{
            padding: 40px 30px;
            background: #ffffff;
        }}

        .welcome-text {{
            font-size: 18px;
            color: #1f2937;
            margin-bottom: 25px;
            line-height: 1.6;
        }}

        /* Button Styles */
        .button-container {{
            text-align: center;
            margin: 35px 0;
        }}

        .button {{
            background: #f97316;
            color: #ffffff !important;
            padding: 16px 32px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: 600;
            font-size: 16px;
            display: inline-block;
            box-shadow: 0 4px 6px rgba(249, 115, 22, 0.2);
            transition: all 0.3s ease;
        }}

        .button:hover {{
            background: #fb923c;
            box-shadow: 0 6px 8px rgba(249, 115, 22, 0.3);
        }}

        /* Information Box */
        .info-box {{
            background: #f8fafc;
            border-radius: 8px;
            padding: 20px;
            margin: 25px 0;
            border-left: 4px solid #f97316;
        }}

        .info-box p {{
            margin: 0;
            color: #64748b;
            font-size: 14px;
        }}

        /* Footer Styles */
        .footer {{
            background: #f8fafc;
            padding: 30px;
            text-align: center;
            border-top: 1px solid #e2e8f0;
        }}

        .social-links {{
            margin-bottom: 20px;
        }}

        .social-link {{
            display: inline-block;
            margin: 0 10px;
        }}

        .footer-text {{
            color: #64748b;
            font-size: 14px;
            margin: 10px 0;
        }}

        .footer-links {{
            margin-top: 15px;
        }}

        .footer-link {{
            color: #f97316;
            text-decoration: none;
            margin: 0 10px;
            font-size: 14px;
        }}

        /* Responsive Design */
        @media screen and (max-width: 600px) {{
            .email-container {{
                width: 100% !important;
                padding: 10px !important;
            }}

            .content {{
                padding: 30px 20px !important;
            }}

            .header h1 {{
                font-size: 24px !important;
            }}

            .button {{
                padding: 14px 28px !important;
                font-size: 15px !important;
            }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='card'>
            <!-- Header -->
            <div class='header'>
                <img src='https://yourwebsite.com/logo.png' alt='Ticket Hub Logo' />
                <h1>Verify Your Email Address</h1>
            </div>

            <!-- Content -->
            <div class='content'>
                <p class='welcome-text'>
                    Welcome to Ticket Hub! We're excited to have you on board. To get started, please verify your email address by clicking the button below.
                </p>

                <!-- Call to Action Button -->
                <div class='button-container'>
                    {callToAction}
                </div>

                <!-- Information Box -->
                <div class='info-box'>
                    <p>🔒 This verification link will expire in 24 hours for security reasons.</p>
                    <p>If you didn't create a Ticket Hub account, you can safely ignore this email.</p>
                </div>
            </div>

            <!-- Footer -->
            <div class='footer'>
                <div class='social-links'>
                    <a href='#' class='social-link'><img src='https://yourwebsite.com/facebook-icon.png' alt='Facebook' width='32' height='32'></a>
                    <a href='#' class='social-link'><img src='https://yourwebsite.com/twitter-icon.png' alt='Twitter' width='32' height='32'></a>
                    <a href='#' class='social-link'><img src='https://yourwebsite.com/instagram-icon.png' alt='Instagram' width='32' height='32'></a>
                </div>
                
                <p class='footer-text'>
                    This email was sent to you because you signed up for a Ticket Hub account.
                    <br>
                    © 2024 Ticket Hub. All rights reserved.
                </p>

                <div class='footer-links'>
                    <a href='#' class='footer-link'>Privacy Policy</a>
                    <a href='#' class='footer-link'>Terms of Service</a>
                    <a href='#' class='footer-link'>Contact Support</a>
                </div>

                <p class='footer-text' style='margin-top: 20px;'>
                    Ticket Hub, Inc.<br>
                    123 Ticket Street, Suite 100<br>
                    San Francisco, CA 94105
                </p>
            </div>
        </div>
    </div>
</body>
</html>";
        return await SendEmailToClientAsync(toMail, subject, body);
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        // Lấy thông tin cấu hình email từ file appsettings.json
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var fromPassword = _configuration["EmailSettings:FromPassword"];
        var smtpHost = _configuration["EmailSettings:SmtpHost"];
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

        // Tạo đối tượng MailMessage
        var message = new MailMessage(fromEmail, toEmail, subject, body);
        message.IsBodyHtml = true;

        // Tạo đối tượng SmtpClient và gửi email
        using var smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(fromEmail, fromPassword),
            EnableSsl = true
        };
        await smtpClient.SendMailAsync(message);
        return true;
    }
}