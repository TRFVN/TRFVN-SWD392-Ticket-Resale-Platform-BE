﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ticket_Hub.API.Extension;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.Mappings;
using Ticket_Hub.Models.DTO.Hubs;
using Net.payOS;

namespace Ticket_Hub.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient();

            // Configure DbContext with SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(StaticConnectionString.SqldbDefaultConnectionAzure));
            });

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // Register services life cycle
            builder.Services.RegisterServices();

            // Register Firebase Service
            builder.Services.AddFirebaseService();



            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure authentication and authorization
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSecret = builder.Configuration["JWT:Secret"];
                if (string.IsNullOrEmpty(jwtSecret))
                {
                    throw new InvalidOperationException("JWT:Secret is required and cannot be null or empty.");
                }
                
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });

            builder.Services.AddAuthorization();

            // Thêm dịch vụ SignalR
            builder.Services.AddSignalR();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter your token with this format: \"Bearer YOUR_TOKEN\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });


            // Configure CORS
            var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder
                        .WithOrigins(corsOrigins ?? Array.Empty<string>())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });


            // Đăng ký dịch vụ PayOS với cấu hình từ appsettings
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                
                // Retrieve configuration values and check for null
                string clientId = configuration["PayOS:ClientId"] 
                                  ?? throw new InvalidOperationException("Client ID is required for PayOS.");
                string apiKey = configuration["PayOS:ApiKey"] 
                                ?? throw new InvalidOperationException("API Key is required for PayOS.");
                string checksumKey = configuration["PayOS:ChecksumKey"] 
                                     ?? throw new InvalidOperationException("Checksum Key is required for PayOS.");

                return new PayOS(clientId, apiKey, checksumKey);
            });


            var app = builder.Build();

            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // Apply database migrations
            ApplyMigration(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // Middleware để xử lý CORS
            // app.Use(async (context, next) =>
            // {
            //     var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            //     var logger = loggerFactory.CreateLogger("CORSMiddleware");
            //
            //     logger.LogInformation($"Request from origin: {context.Request.Headers["Origin"]}");
            //     logger.LogInformation($"Request method: {context.Request.Method}");
            //     logger.LogInformation($"Request path: {context.Request.Path}");
            //
            //     if (context.Request.Method == "OPTIONS")
            //     {
            //         context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
            //         context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
            //         context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            //         context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //         context.Response.StatusCode = 200;
            //         await context.Response.CompleteAsync();
            //     }
            //     else
            //     {
            //         await next();
            //     }
            //
            //     if (context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            //     {
            //         logger.LogInformation($"Response Access-Control-Allow-Origin: {context.Response.Headers["Access-Control-Allow-Origin"]}");
            //     }
            // });

            // Đặt UseCors ngay sau middleware này
            app.UseCors("AllowSpecificOrigin");


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // Thêm cấu hình SignalR endpoint
            app.MapHub<NotificationHub>("/notificationHub");

            app.MapControllers();

            app.Run();
        }

        //auto update database
        private static void ApplyMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}