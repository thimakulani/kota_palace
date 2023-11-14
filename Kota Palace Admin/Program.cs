using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Hubs;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("conn_string")
        ));


// Add services to the container.
builder
    .Services
           .AddIdentity<AppUsers, IdentityRole>(options =>
           {
               options.Password.RequireDigit = false;
               options.Password.RequiredLength = 5;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
               options.Password.RequireLowercase = false;
           })
           .AddEntityFrameworkStores<AppDBContext>()
           .AddDefaultTokenProviders();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddMailKit(optionBuilder =>
{
    optionBuilder.UseMailKit(new MailKitOptions()
    {

        Server = "smtp.gmail.com",
        Port = 25,
        SenderName = "Kota Palace",
        SenderEmail = "sigauquetk@gmail.com",
        Account = "sigauquetk@gmail.com",
        Password = "nlguroepjshntcdo",
        Security = true

    });
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.WithOrigins(
                            "http://localhost:4200"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowAngularOrigins");
using (var h = app.Services.CreateScope())
{
    //InitializeRoles(h.ServiceProvider);
    var manager = h.ServiceProvider.GetRequiredService<UserManager<AppUsers>>();
    var roleManager = h.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Customer", "Employee", "Owner" };
    foreach (var roleName in roles)
    {
        // Check if the role already exists
        var roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
    AppUsers user = new AppUsers()
    {
        UserName = "admin@admin.com",
        Email = "admin@admin.com",
        Firstname = "Admin",
        Lastname = "Admin",
        PhoneNumber = "0713934923",
    };

    var _user = await manager.FindByEmailAsync(user.Email);
   if(_user == null)
    {
        var results = await manager.CreateAsync(user, "123456789");
        if (results.Succeeded)
        {
            _user = await manager.FindByEmailAsync(user.Email);
            await manager.AddToRoleAsync(_user, "Admin");
        }
    }
}


// Configure the HTTP request pipeline.
//await InitializeRoles(app.Services);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHub<OrderHub>("/OrderHub");
//app.MapHub<TrackOrderHub>("/TrackOrderHub");
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");

app.Run();

