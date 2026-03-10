using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateTournamentCommandValidator>();

        return services;
    }
}
