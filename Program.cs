using DinkToPdf.Contracts;
using DinkToPdf;
using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.CategoryInterface;
using Inventory_PLM.Interfaces.ProductInterface;
using Inventory_PLM.Interfaces.PurhaseOrderInterfae;
using Inventory_PLM.Interfaces.SubCategoryInterface;
using Inventory_PLM.Interfaces.UnitOfMeasureInterface;
using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Services.CategoryService;
using Inventory_PLM.Services.ProductService;
using Inventory_PLM.Services.PurchaseOrderService;
using Inventory_PLM.Services.SubCategoryService;
using Inventory_PLM.Services.UnitOfMeasureService;
using Inventory_PLM.Services.UserService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Inventory_PLM.Services;
using Inventory_PLM.Interfaces;
//using Inventory_PLM.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Configure authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//    options.LoginPath = "/Account/Index";
//    options.LogoutPath = "/Account/Logout";
//})
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});

// Add CSP middleware to set Content-Security-Policy header
//builder.Services.AddAntiforgery(options =>
//{
//    options.HeaderName = "X-CSRF-TOKEN";
//});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Index";
            options.LogoutPath = "/Account/Logout";
        });

//Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//PDF
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<PdfService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IUnitOfMeasureService, UnitOfMeasureService>();
//builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddSingleton<IRazorViewEngine, RazorViewEngine>();
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider => new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add CSP middleware here
//app.Use(async (context, next) =>
//{
//    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; " +
//        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
//        "style-src 'self' 'unsafe-inline'; " +
//        "img-src 'self' data:; " +
//        "connect-src 'self' http://localhost:21863 https://localhost:44335 http://localhost:5042 https://localhost:7169;";
//    await next();
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapHub<ChatHub>("/chathub"); // Maps the ChatHub with a endpoint of /chathub.



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
