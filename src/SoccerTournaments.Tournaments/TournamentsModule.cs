using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerTournaments.Tournaments.Features;

namespace SoccerTournaments.Tournaments;

public static class TournamentsModule
{
    public static IServiceCollection AddTournamentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<TournamentsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ITournamentsRepository, TournamentsRepository>();

        // Handlers
        services.AddScoped<CreateTournamentHandler>();
        services.AddScoped<GetTournamentByIdHandler>();
        services.AddScoped<GetAllTournamentsHandler>();
        services.AddScoped<AddTeamToTournamentHandler>();
        services.AddScoped<GetTournamentTeamsHandler>();
        services.AddScoped<RecordMatchResultHandler>();
        services.AddScoped<GetTournamentStandingsHandler>();
        services.AddScoped<ScheduleMatchHandler>();
        services.AddScoped<GetTournamentMatchesHandler>();
        services.AddScoped<UpdateMatchStatusHandler>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateTournamentCommandValidator>();

        return services;
    }
}
