using Microsoft.EntityFrameworkCore;
using MsEShop.Services.EmailAPI.Data;
using MsEShop.Services.EmailAPI.Messaging;
using MsEShop.Services.EmailAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EF:
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection")!);
});

//we cannot use AddDbContext with independency injection in a Singleton instance, so we have to add a DbContextOptionsBuilder in Program.cs
var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection")!);
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

builder.Services.AddControllers();
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

app.MapControllers();

ApplyPendingDBMigrations();

IAzureServiceBusConsumer azureServiceBusConsumer = ((IApplicationBuilder)app).ApplicationServices.GetService<IAzureServiceBusConsumer>()!;
IHostApplicationLifetime hostApplicationLife = ((IApplicationBuilder)app).ApplicationServices.GetService<IHostApplicationLifetime>()!;
hostApplicationLife.ApplicationStarted.Register(OnStart);
hostApplicationLife.ApplicationStopping.Register(OnStop);

app.Run();

void ApplyPendingDBMigrations()
{
    using var scope = app.Services.CreateScope();
    using var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (_db == null || _db.Database == null) return;
    if (_db.Database.GetPendingMigrations().Any()) _db.Database.Migrate();
}

void OnStart()
{
    azureServiceBusConsumer.Start();
}
void OnStop()
{
    azureServiceBusConsumer.Stop();
}
