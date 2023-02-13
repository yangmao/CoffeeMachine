using CoffeeMachine.Controllers;
using CoffeeMachine.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherService,WeatherService>();
builder.Services.AddTransient<IUtilsService, UtilsService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<CoffeeController>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();
app.MapControllers();


app.Run();
