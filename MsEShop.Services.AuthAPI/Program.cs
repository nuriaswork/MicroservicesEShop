using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MsEShop.Services.AuthAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EF and DotNetIdentity
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


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

app.UseAuthentication(); //must be added before UseAuthorization()

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