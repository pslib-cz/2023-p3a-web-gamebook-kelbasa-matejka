using Game.Helpers;
using Game.Models;
using Game.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSingleton<ISessionService, SessionService>();

//Registrace databaze
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);


// Samostatne servisy hry
builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<EffectService>();
builder.Services.AddSingleton<PlayerService>();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
app.UsePathBase("/EscapeTheFortress");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Run();
