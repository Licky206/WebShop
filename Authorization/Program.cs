using Authorization.Controllers;
using Authorization.Extensions;
using Authorization.Models;
using Authorization.Repositories;
using Authorization.Services;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();



builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);
  

                


builder.Services.AddScoped<IProizvodRepository, ProizvodRepository>();
builder.Services.AddScoped<IProizvodService, ProizvodService>();
builder.Services.AddScoped<IRacunService, RacunService>();
builder.Services.AddScoped<IRacunRepository, RacunRepository>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditRacunStatus", policy =>
        policy.RequireClaim("CanEditRacunStatus", "true"));
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Proveri da li je ovo URL tvog frontend-a
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

//dodavanje middlewera authentikacije
var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .AddIdentityAuthMiddlewares();

app.UseHttpsRedirection();

app.MapControllers();
app.MapGroup("/api")
   .MapIdentityApi<AppUser>();
app.
    MapGroup("/api")
    .MapIdentityUserEndpoints()
    .MapAcountEndpoints();
 

app.Run();


