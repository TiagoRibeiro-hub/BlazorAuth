using BlazorAuth.Server.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities.Entities;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity();

builder.Services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
{
    options.IdentityResources["openid"].UserClaims.Add("role");
    options.ApiResources.Single().UserClaims.Add("role");
});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

builder.Services.AddPolicies();

builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RegisterRoles();

app.Run();
