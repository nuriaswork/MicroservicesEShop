using Microsoft.AspNetCore.Authentication.Cookies;
using MsEShop.Web.Constants;
using MsEShop.Web.Service;
using MsEShop.Web.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
//we can set options:
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
//    options=>
//    {
//        options.ExpireTimeSpan = TimeSpan.FromHours(18);
//        options.LoginPath = "/Auth/Login";
//        options.AccessDeniedPath = "/Auth/AccessDenied";
//    }
//);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();

builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITokenProvider, TokenProviderService>();
builder.Services.AddScoped<IJwtTokenLoader, JwtTokenLoader>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

ApisUri.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
ApisUri.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
ApisUri.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
ApisUri.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
