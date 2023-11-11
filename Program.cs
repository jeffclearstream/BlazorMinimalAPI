using BlazorMinimalApis.Data;
using BlazorMinimalApis.Endpoints;
using BlazorMinimalApis.Lib;
using BlazorMinimalApis.Lib.Session;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Spark.Library.Auth;
using BlazorMinimalApis.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultChallengeScheme = "Cookies";
//    options.DefaultSignInScheme = "Cookies";
//    options.DefaultAuthenticateScheme = "Cookies";
//}).AddCookie(options =>
//{
//    options.SlidingExpiration = false;
//    options.LoginPath = "/login";
//    options.LogoutPath = "/logout";
//    //options.AccessDeniedPath = config2.GetValue("Spark:Auth:AccessDeniedPath", "/access-denied");
//    options.Cookie.Name = ".myapp.spark.cookie";
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
//    options.Cookie.SameSite = SameSiteMode.Lax;
//    options.Events = new CookieAuthenticationEvents
//    {
//        OnValidatePrincipal = (CookieValidatePrincipalContext context) => context.HttpContext.RequestServices.GetRequiredService<IAuthValidator>().ValidateAsync(context)
//    };
//});
builder.Services.AddRazorComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAntiforgery();
builder.Services.AddTransient<SessionManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnectionString"]);
});

//builder.Services.AddDbContextFactory<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnectionString"]);
//});

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<IAuthValidator, AuthValidator>();
builder.Services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();
app.MapPageEndpoints();
app.MapApiEndpoints();

app.Run();