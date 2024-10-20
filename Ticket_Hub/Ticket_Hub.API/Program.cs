using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ticket_Hub.API.Extension;
using Ticket_Hub.API.Middleware;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.Mappings;
using Ticket_Hub.Services.Services;
using Ticket_Hub.Models.DTO.Hubs;

namespace Ticket_Hub.API
{
    public class Program
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
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
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


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://localhost:5173")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // Apply database migrations
            //ApplyMigration(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

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