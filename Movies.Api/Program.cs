using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddDatabaseService(config["database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
var DbInitializer = app.Services.GetRequiredService<DbInitializer>();
await DbInitializer.InitializeAsync();
app.MapControllers();
app.Run();

