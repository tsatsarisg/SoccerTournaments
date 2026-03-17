using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Teams;
using SoccerTournaments.Teams.Api;
using SoccerTournaments.Tournaments;
using SoccerTournaments.Tournaments.Api;
using SoccerTournaments.Players;
using SoccerTournaments.Players.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddTeamsModule(builder.Configuration);
builder.Services.AddTournamentsModule(builder.Configuration);
builder.Services.AddPlayersModule(connectionString);

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var teamsDbContext = scope.ServiceProvider.GetRequiredService<TeamsDbContext>();
    teamsDbContext.Database.Migrate();

    var tournamentsDbContext = scope.ServiceProvider.GetRequiredService<TournamentsDbContext>();
    tournamentsDbContext.Database.Migrate();

    var playersDbContext = scope.ServiceProvider.GetRequiredService<PlayersDbContext>();
    playersDbContext.Database.Migrate();
}

app.MapTeamsEndpoints();
app.MapTournamentsEndpoints();
app.MapPlayersModule();

app.Run();
