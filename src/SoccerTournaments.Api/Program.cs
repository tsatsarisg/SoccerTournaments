var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTournamentsModule(builder.Configuration);
builder.Services.AddTeamsModule(builder.Configuration);

var app = builder.Build();

app.MapTournamentsEndpoints();
app.MapTeamsEndpoints();

app.Run();
