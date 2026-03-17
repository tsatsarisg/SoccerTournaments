using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using SoccerTournaments.Players.Api;
using SoccerTournaments.Players.Features;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players;

public static class PlayersModule
{
    public static IServiceCollection AddPlayersModule(
        this IServiceCollection services,
        string connectionString)
    {
        // Database
        services.AddDbContext<PlayersDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Repository
        services.AddScoped<IPlayersRepository, PlayersRepository>();

        // Handlers
        services.AddScoped<CreatePlayerHandler>();
        services.AddScoped<GetPlayerByIdHandler>();
        services.AddScoped<GetAllPlayersHandler>();
        services.AddScoped<GetPlayersByTeamHandler>();
        services.AddScoped<AssignPlayerToTeamHandler>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreatePlayerCommandValidator>();

        return services;
    }

    public static IEndpointRouteBuilder MapPlayersModule(this IEndpointRouteBuilder app)
    {
        app.MapPlayersEndpoints();
        return app;
    }
}
