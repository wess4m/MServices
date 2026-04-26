var builder = DistributedApplication.CreateBuilder(args);

var authAPI = builder.AddProject<Projects.AuthAPI>("AuthAPI");
var couponAPI = builder.AddProject<Projects.CouponAPI>("CouponAPI");
var productAPI = builder.AddProject<Projects.ProductAPI>("ProductAPI");
var shoppingcartAPI = builder.AddProject<Projects.ShoppingCartAPI>("ShoppingCartAPI");

builder.AddProject<Projects.Store>("store")
    .WithReference(authAPI)
    .WithReference(couponAPI)
    .WithReference(productAPI)
    .WithReference(shoppingcartAPI);


builder.Build().Run();



