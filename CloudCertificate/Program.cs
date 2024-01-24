using AzureSQL;
using CloudCertificate.Configs;
using CloudCertificate.Services;
using CosmosDB;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection("CosmosDbSettings"));
builder.Services.Configure<SQLSettings>(builder.Configuration.GetSection("SQLSettings"));
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<ICosmosDbService, CosmosDbService>();
builder.Services.AddScoped<ISqlDbService, SqlDbService>();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry();
// The following configures DependencyTrackingTelemetryModule.
// Similarly, any other default modules can be configured.
builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
{
    module.EnableW3CHeadersInjection = true;
});

// The following removes all default counters from EventCounterCollectionModule, and adds a single one.
builder.Services.ConfigureTelemetryModule<EventCounterCollectionModule>((module, o) =>
{
    module.Counters.Add(new EventCounterCollectionRequest("System.Runtime", "gen-0-size"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(builder => builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
});
app.UseStaticFiles();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
