using FlyEase_ApiRest_.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Authentication;
using System.Security.Claims;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FlyEaseDataBaseContextAuthentication>(con => con.UseNpgsql(builder.Configuration.GetConnectionString("Fl0ServerConnection")));

builder.Services.AddControllers().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IAuthentication, AuthenticationService>();

var secretkey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes("FlyEaseWebApiTokenEncryptedKeyForAdmin");

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
        RoleClaimType = ClaimTypes.Role // Asegura que las reclamaciones de roles se manejen correctamente
    };
    config.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            // Verifica si el usuario es un administrador
            var isAdmin = context.Principal.IsInRole("Admin");

            if (isAdmin)
            {
                var expirationClaim = context.Principal.FindFirst("exp");
                if (expirationClaim != null)
                {
                    var newExpiration = DateTime.UtcNow.AddSeconds(3); // 3 minutos para caducar
                    context.Principal.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim("exp", newExpiration.ToString("yyyy-MM-ddTHH:mm:ssZ")) }));
                }
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("CommonUser"));
});
builder.Services.AddSignalR();
var hub = new WebSocketHub();


builder.Services.AddSingleton(hub);
var app = builder.Build();

app.UseRouting();
app.UseCors(CorsRules);

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebSocketHub>("/FlyEaseHub");
    endpoints.MapControllers();
});
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapControllers();
app.Run();