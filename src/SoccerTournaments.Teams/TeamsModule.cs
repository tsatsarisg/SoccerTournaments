using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SoccerTournaments.Teams;

public static class TeamsModule
{
    public static IServiceCollection AddTeamsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<TeamsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ITeamsRepository, TeamsRepository>();

        // Handlers
        services.AddScoped<CreateTeamHandler>();
        services.AddScoped<GetTeamByIdHandler>();
        services.AddScoped<GetAllTeamsHandler>();
        services.AddScoped<GetTeamByNameHandler>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateTeamCommandValidator>();

        return services;
    }
}