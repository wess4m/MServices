using Microsoft.AspNetCore.Authentication.Cookies;
using Store.Service;
using Store.Service.IService;
using Store.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];


builder.Services.AddHttpClient<IProductService, ProductService>(client => {
    client.BaseAddress = new(SD.ProductAPIBase);
});
builder.Services.AddHttpClient<ICouponService, CouponService>(client => {
    client.BaseAddress = new(SD.CouponAPIBase);
});
builder.Services.AddHttpClient<IAuthService, AuthService>(client => {
    client.BaseAddress = new(SD.AuthAPIBase);
});
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>(client => {
    client.BaseAddress = new(SD.ShoppingCartAPIBase);
});


//builder.Services.AddScoped<ICouponService, CouponService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/auth/login/";
        options.AccessDeniedPath = "/auth/accessdenied";
    }
    );

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
