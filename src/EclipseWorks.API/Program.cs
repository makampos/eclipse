using System.Text.Json.Serialization;
using EclipseWorks.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.BaseRegister(builder.Configuration);

var app = builder.Build();

DependencyInjection.MigrateDatabase(app.Services);


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }