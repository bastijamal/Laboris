using Laboris.DAL;
using Laboris.Middlewares;
using Laboris.Models;
using Laboris.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//seccion
builder.Services.AddSession();



builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<LayoutService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();


//seccion
app.UseSession();



app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

