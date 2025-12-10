using SoccerTournaments.Teams;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTeamsModule(builder.Configuration);

var app = builder.Build();

app.MapTeamsEndpoints();

app.Run();
