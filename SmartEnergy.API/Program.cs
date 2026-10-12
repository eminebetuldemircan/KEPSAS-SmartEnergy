using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.Interfaces;
using SmartEnergy.Application.Services;
using SmartEnergy.Infrastructure.Persistence;
using SmartEnergy.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SmartEnergyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Address
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

// Meter
builder.Services.AddScoped<IMeterRepository, MeterRepository>();
builder.Services.AddScoped<IMeterService, MeterService>();

// Subscription
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

// Bill
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillService, BillService>();

// Consumption Record
builder.Services.AddScoped<IConsumptionRecordRepository, ConsumptionRecordRepository>();
builder.Services.AddScoped<IConsumptionRecordService, ConsumptionRecordService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Dashboard
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// Email & Notification Service
builder.Services.AddScoped<IEmailService, SmartEnergy.Infrastructure.Services.EmailService>();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SmartEnergyDbContext>();
        await DataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding Error: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevelopmentPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();