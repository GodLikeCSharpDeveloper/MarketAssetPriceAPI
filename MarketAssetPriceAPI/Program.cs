using MarketAssetPriceAPI.Data.Models.ApiProviderModels.ConnectionModels;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using MarketAssetPriceAPI.Data.Services.DbService;
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
builder.Services.AddScoped<IInstrumentControllerService, InstrumentControllerService>();
builder.Services.AddScoped<IBarsControllerService, BarsControllerService>();
builder.Services.AddSingleton<IWebSocketClientControllerService, WebSocketClientControllerService>();
builder.Services.AddScoped<IInstrumentRepository, InstrumentRepository>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddSingleton<IClientWebSocket, ClientWebSocketWrapper>();
builder.Services.AddScoped<IInstrumentProviderRepository, InstrumentProviderRepository>();
builder.Services.AddScoped<IInstrumentProviderService, InstrumentProviderService>();
builder.Services.AddSingleton<ITokenControllerService, TokenControllerService>();
builder.Services.AddScoped<IExchangeRepository, ExchangeRepository>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();
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
