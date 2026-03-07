using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Teams;
using SoccerTournaments.Teams.Api;
using SoccerTournaments.Tournaments;
using SoccerTournaments.Tournaments.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTeamsModule(builder.Configuration);
builder.Services.AddTournamentsModule(builder.Configuration);

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var teamsDbContext = scope.ServiceProvider.GetRequiredService<TeamsDbContext>();
    teamsDbContext.Database.Migrate();

    var tournamentsDbContext = scope.ServiceProvider.GetRequiredService<TournamentsDbContext>();
    tournamentsDbContext.Database.Migrate();
}

app.MapTeamsEndpoints();
app.MapTournamentsEndpoints();

app.Run();
