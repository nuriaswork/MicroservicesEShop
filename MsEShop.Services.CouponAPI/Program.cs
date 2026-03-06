using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MsEShop.Services.CouponAPI.Data;
using MsEShop.Services.CouponAPI.Models;
using MsEShop.Services.CouponAPI.Models.Dto;
using System.Text;

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

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer <token>`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
    });
});

//AddAuthentication
var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience
    };
});

//AddAuthorization
builder.Services.AddAuthorization();

//Build the app:

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

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