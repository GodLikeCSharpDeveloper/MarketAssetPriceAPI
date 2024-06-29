using MarketAssetPriceAPI.Data.Models.ConnectionModels;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.Configure<FintachartCredentials>(builder.Configuration.GetSection("FintachartCredentials"));
builder.Services.AddSingleton<TokenResponseStore>();
builder.Services.AddScoped<InstrumentControllerService>();
builder.Services.AddScoped<BarsControllerService>();
builder.Services.AddSingleton<WebSocketClientControllerService>();
builder.Services.AddScoped<InstrumentRepository>();
builder.Services.AddSingleton<TokenControllerService>();
builder.Services.AddDbContext<MarketDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
