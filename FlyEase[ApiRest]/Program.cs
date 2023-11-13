using FlyEase_ApiRest_.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlyEase_ApiRest_.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using FlyEase_ApiRest_.Hub;

var builder = WebApplication.CreateBuilder(args);
var CorsRules = "Reglas";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: CorsRules, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<FlyEaseDataBaseContextAuthentication>(con => con.UseNpgsql(builder.Configuration.GetConnectionString("Fl0ServerConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSingleton<DbListener>();
builder.Services.AddScoped<IAuthentication, AuthenticationTokenService>();

var keyBytes = Encoding.UTF8.GetBytes("FlyEaseWebApiTokenEncryptedKeyString");

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("CommonUser"));
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<WebSocketHub>();

var app = builder.Build();

var dbListener = app.Services.GetRequiredService<DbListener>();
dbListener.IniciarEscuchaCambiosEnVuelos();

app.UseRouting();
app.UseCors(CorsRules);
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebSocketHub>("/FlyEaseHub");
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
