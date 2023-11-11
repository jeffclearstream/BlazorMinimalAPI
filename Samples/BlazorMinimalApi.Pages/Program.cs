using BlazorMinimalApis.Data;
using BlazorMinimalApis.Endpoints.Pages;
using BlazorMinimalApis.Lib.Session;
using BlazorMinimalApis.Pages.Auth;
using BlazorMinimalApis.Pages.Lib;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

using Spark.Library.Auth;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

builder.Services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();

builder.Services.AddAuthorization(builder.Configuration, new string[] { CustomRoles.Admin, CustomRoles.User });
builder.Services.AddAuthentication<IAuthValidator>(builder.Configuration);

builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAntiforgery();
builder.Services.AddTransient<SessionManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnectionString"]);
//});
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnectionString"]);
});

builder.Services.AddAntiforgery();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<IAuthValidator, AuthValidator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DatasetPageController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAntiforgery();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.RegisterRoutes();

app.Run();