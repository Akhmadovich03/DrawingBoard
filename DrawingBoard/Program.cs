using DrawingBoard.Hubs;
using Microsoft.EntityFrameworkCore;
using DrawingBoard.Data;

namespace DrawingBoard;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration.GetConnectionString("AzureSignalRConnectionString"));
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<BoardDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Board}/{action=Index}/{id?}");

        app.MapHub<BoardHub>("/boardHub");

        app.Run();
    }
}
