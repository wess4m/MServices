using AutoMapper;
using Mango.Services.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using ShoppingCartAPI;
using ShoppingCartAPI.Authentication;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Service;
using ShoppingCartAPI.Service.IService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(MappingConfig.RegisterMaps().CreateMapper());
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();

// For Aspire
builder.AddServiceDefaults();

//=============================================================================================================================
// --- API Client Configuration ---
// Configures HttpClients for external services with a centralized authentication handler.
// This handler intercepts outgoing requests to inject the Bearer token for authorization.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BackendApiAuthenticationHttpClientHandler>();
// Register Product API Client
builder.Services.AddHttpClient("Product", x => x.BaseAddress 
= new Uri(builder.Configuration["ServiceURLs:ProductAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
// Register Coupon API Client
builder.Services.AddHttpClient("Coupon", x => x.BaseAddress = 
new Uri(builder.Configuration["ServiceURLs:CouponAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
//==============================================================================================================================

builder.AddAuthentication();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapScalarApiReference(options => options
    .AddPreferredSecuritySchemes("Bearer")
    .AddHttpAuthentication("Bearer", auth =>
    {
        auth.Token = "";
    }));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Apply any pinding migration automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    if (db.Database.GetPendingMigrations().Count() > 0)
    {
        db.Database.Migrate();
    }
}

app.Run();

