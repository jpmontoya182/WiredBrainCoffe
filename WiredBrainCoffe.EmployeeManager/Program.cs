
using Microsoft.EntityFrameworkCore;
using WiredBrainCoffe.EmployeeManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContextFactory<EmployeeManagerDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagerDB")));

var app = builder.Build();

// Don't do this in production, just useful for development
await EnsureDatabaseIsMigrated(app.Services);


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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


async Task EnsureDatabaseIsMigrated(IServiceProvider services)
{
    // get dependencies only valid inside in this scope
    using var scope = services.CreateScope();
    using var context = scope.ServiceProvider.GetService<EmployeeManagerDbContext>();
    if (context is not null)
    {
        await context.Database.MigrateAsync();
    }
}