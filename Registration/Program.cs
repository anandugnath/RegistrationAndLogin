using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<CoreDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcCoreMovieContext")));
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});
var a = builder.Configuration.GetConnectionString("MvcCoreMovieContext");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    // Check if session is expired
    var userId = context.Session.GetInt32("UserID");
    if (userId == null)
    {
        // Session is expired, log out the user
        // You can redirect to a logout action or perform any other logout logic here
        context.Response.Redirect("/logout");
        return;
    }

    // Session is active, continue with the request pipeline
    await next();
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
