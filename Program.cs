using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Star_Wars_DataBase.Areas.Identity;
using Star_Wars_DataBase.Data;
using Star_Wars_DataBase.Data.Weather;
using Star_Wars_DataBase.Data.SWAPI;
using Microsoft.Fast.Components.FluentUI;
using Star_Wars_DataBase.Data.DaData.Service;
using Star_Wars_DataBase.Data.PhoneBook;
using Star_Wars_DataBase.Shared.Containers;
using Vite.AspNetCore.Extensions;

#region Initial configuration

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString), optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString), lifetime: ServiceLifetime.Singleton);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddViteServices();

builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddFluentUIComponents(options => { options.HostingModel = BlazorHostingModel.Server; });
builder.Services.AddDataGridEntityFrameworkAdapter();

#endregion Initial configuration

#region Services

builder.Services
    .AddHttpClient()
    .AddHttpClient("SWAPI", c =>
        c.BaseAddress = new Uri(
            builder
                .Configuration
                .GetRequiredSection("SWAPI")
                .GetValue<string>("BaseUrl") ?? throw new InvalidOperationException()
        )
    );
    
builder.Services
    .AddSingleton<WeatherForecastService>()
    .AddSingleton<PhoneBookService>()
    .AddSingleton<SwapiService>()
    .AddSingleton<DaDataService>()
    .AddSingleton(builder.Configuration);

#endregion Services

#region Containers

builder.Services
    .AddScoped<PhoneBookContainer>()
    .AddScoped<CounterWithDifferentScopesContainer>();

#endregion Containers

#region Initialization

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();

app.UseStaticFiles();

if (Environment.GetEnvironmentVariable("WITH_DEV_SERVER") == "1")
{
    app.UseViteDevMiddleware();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();

#endregion