using FlyEase_ApiRest_;
using FlyEase_ApiRest_.Authentication;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Hub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlyEaseWebApi Swagger UI", Version = "v1" });
    c.EnableAnnotations(); // Habilita las anotaciones de Swashbuckle
    c.MapType<string>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("Texto")
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorizacion JWT usando el esquema Bearer, Ingresa un token valido.\n si todavia no tienes uno, puedes solicitarlo con los metodos:" +
        " \n --Administradores/Authentication" +
        " \n --ApplicationTokens/GenerateAppsToken",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

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
app.UseWhen(context => context.Request.Path.StartsWithSegments("/FlyEaseWebApiSwaggerUI"), appBuilder =>
{
    appBuilder.Use(async (context, next) =>
    {
        // Verificamos si la solicitud tiene la cabecera Authorization y comienza con "Basic "
        string authHeader = context.Request.Headers["Authorization"];
        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            // Evitar el almacenamiento de credenciales en cookies
            context.Response.Headers["Cache-Control"] = "no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
        }

        await next();
    });

    appBuilder.UseBasicAuth("SwaggerUI", "Username", "Password");
});

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
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "FlyEaseWebApiSwaggerUI";

    // Habilitar la autenticación JWT en Swagger UI
    c.OAuthClientId("Swagger");
    c.OAuthClientSecret(string.Empty);
    c.OAuthRealm(string.Empty);
    c.OAuthAppName("Swagger");
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});
app.Map("/FlyEaseWebApiSwaggerUI", swaggerApp =>
{
    swaggerApp.UseStaticFiles(); // Si es necesario para servir archivos estáticos
    swaggerApp.Use(async (context, next) =>
    {
        if (context.Request.Path == "/" || context.Request.Path == "")
        {
            // Sirve la página HTML deseada en la raíz de la aplicación
            context.Request.Path = "/FlyEaseWebApiSwaggerUI/index.html";
        }

        await next();
    });
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/swagger" || context.Request.Path == "//" || context.Request.Path == "/s" || context.Request.Path == "/S")
    {
        context.Response.Redirect("/FlyEaseWebApiSwaggerUI/index.html");
        return;
    }

    await next();
});
app.UseHttpsRedirection();

app.MapControllers();
app.Run();