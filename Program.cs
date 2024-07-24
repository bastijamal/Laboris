using Laboris.Abstractions.MailService;
using Laboris.DAL;
using Laboris.Middlewares;
using Laboris.Models;
using Laboris.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//seccion
builder.Services.AddSession();



builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IEmailService,EmailService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//Odenish
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


///Email Sender
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService,MailService>();


var app = builder.Build();


//seccion
app.UseSession();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();


//Odenish

StripeConfiguration.ApiKey = builder.Configuration["Stripe:Secretkey"];

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

