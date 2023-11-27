using Microsoft.EntityFrameworkCore;
using PortaleCorsi.Context;
using PortaleCorsi.Repositories;

var builder = WebApplication.CreateBuilder(args);
   builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("app_connection_string"), a => a.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICorsoRepository, CorsoRepository>();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
