using FlyEase_ApiRest_.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FlyEaseDataBaseContextPrueba>(con => con.UseNpgsql(builder.Configuration.GetConnectionString("Fl0ServerConnection")));

builder.Services.AddControllers().AddJsonOptions(c =>
{
    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var CorsRules = "Reglas";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: CorsRules, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddSignalR();
var hub = new WebSocketHub();


builder.Services.AddSingleton(hub);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebSocketHub>("/FlyEaseHub");
    endpoints.MapControllers();
});
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseCors(CorsRules);

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();