using Microsoft.EntityFrameworkCore;
using ShoelessJoeAPI.Core.Interfaces;
using ShoelessJoeAPI.DataAccess.DataModels;
using ShoelessJoeAPI.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShoelessJoeContext>(options => options.UseMySql(SecretConfig.ConnectionString, new MySqlServerVersion(SecretConfig.Version)));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IManufacterService, ManufacterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
