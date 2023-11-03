using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoadAppWEB.Data;
using RoadAppWEB.Models;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RoadAppWEBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RoadAppWEBContext") ?? throw new InvalidOperationException("Connection string 'RoadAppWEBContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});
builder.Services.AddDbContext<RoadAppWEBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RoadAppWEBContext")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Map}/{action=BaiduMap}/{id?}");

app.Run();
