using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SalePoint.Primitives.Interfaces;
using SalePoint.Repository;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(option =>
//    {
//        option.LoginPath = "/Login/index";
//        option.ExpireTimeSpan= TimeSpan.FromMinutes(30);
//        option.AccessDeniedPath= "/Home/Privacy";
//    });

builder.Services.AddAuthentication(
    option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
   AddJwtBearer(o =>
   {
       var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
       o.SaveToken = true;
       o.TokenValidationParameters = new TokenValidationParameters
       {
           ValidIssuer = builder.Configuration["JWT:Issuer"],
           ValidAudience = builder.Configuration["JWT:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true
       };
   });

builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddSingleton<ICatalogRepository, CatalogRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ICashRegisterRepository, CashRegisterRepository>();
builder.Services.AddSingleton<ISaleRepository, SaleRepository>();
builder.Services.AddSingleton<ISellRepository, SellRepository>();

builder.Services.AddHttpClient("SalePoinApi", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:SalePoinApiUrl"]);
});

builder.Services.AddHttpClient("SalePoinAuthApi", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ServicesUrl:SalePointAuthApiUrl"]);
});

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
app.UseCookiePolicy();
app.UseRouting();

app.UseSession();
app.Use(async (context, next) =>
{
    string? token = context.Session.GetString("TokenAuth");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
