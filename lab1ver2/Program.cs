using FluentValidation;
using FluentValidation.AspNetCore;
using InvertedSoftware.PLogger.Core;
using lab1ver2;
using lab1ver2.Data;
using lab1ver2.Models;
using lab1ver2.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) => 
            factory.Create(typeof(SharedResource));
    });

builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk"),
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IValidator<Contact>, ContactValidator>();

var settings = new PLoggerSettings(builder.Configuration);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddPLogger(settings);
});

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connection);
});

var app = builder.Build();

app.UseRequestLocalization();

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

app.UseAuthorization();

app.Use((context, next) =>
{
    var request = context.Request;
    var ipAddress = context.Connection.RemoteIpAddress;
    var requestTime = DateTime.Now;

    var logMessage = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString} [:] " +
                     $"T:{requestTime}, " +
                     $"IP:{ipAddress}";

    context.RequestServices.GetRequiredService<ILogger<Program>>().LogInformation(logMessage);

    return next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
