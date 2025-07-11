using Movies.Application;
using Movies.Application.Database;
using RestApi.Mapping;

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

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();
var DbInitializer = app.Services.GetRequiredService<DbInitializer>();
await DbInitializer.InitializeAsync();
app.Run();

