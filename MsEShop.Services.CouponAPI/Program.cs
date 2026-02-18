using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MsEShop.Services.CouponAPI.Data;
using MsEShop.Services.CouponAPI.Models;
using MsEShop.Services.CouponAPI.Models.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EF:
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection"));
});

//Automapper:
IMapper mapper = new MapperConfiguration(config =>
{
    config.CreateMap<Coupon, CouponDto>();
    config.CreateMap<CouponDto, Coupon>();
}
).CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //for use Automapper with dependency injection


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


//Build the app:

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

app.Run();


void ApplyPendingDBMigrations()
{
    using var scope = app.Services.CreateScope();
    using var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (_db == null || _db.Database == null) return;
    if (_db.Database.GetPendingMigrations().Any()) _db.Database.Migrate();
}