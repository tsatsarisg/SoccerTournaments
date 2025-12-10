using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SoccerTournaments.Teams;

public static class TeamsModule
{
    public static IServiceCollection AddTeamsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITeamsService, TeamsService>();
        return services;
    }
}