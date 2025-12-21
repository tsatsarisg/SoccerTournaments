using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Teams;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTeamsModule(builder.Configuration);

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TeamsDbContext>();
    dbContext.Database.Migrate();
}

app.MapTeamsEndpoints();

app.Run();
