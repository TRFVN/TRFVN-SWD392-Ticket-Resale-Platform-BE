using NuGet.Common;
using Ticket_Hub.DataAccess.IRepository;
using Ticket_Hub.DataAccess.Repository;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Services.Services;

namespace Ticket_Hub.API.Extension;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IFirebaseService, FirebaseService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ILocationService, LocationService>();
        return services;
    }
}