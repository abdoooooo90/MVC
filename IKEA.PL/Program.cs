using Castle.Core.Smtp;
using IKEA.BLL.Common.Services;
using IKEA.BLL.Common.Services.EmailSettings;
using IKEA.BLL.Services;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Models.Identity;
using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using IKEA.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace IKEA.PL

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Configure Services
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
            builder.Services.AddTransient<IAttachmentService, AttachmentService>();
			builder.Services.AddScoped<IEmailSettings,EmailSettings>();
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>((option =>
            {
                option.Password.RequiredLength = 5;
                option.Password.RequireNonAlphanumeric = true; //@#*
                option.Password.RequireUppercase = true;    
                option.Password.RequireLowercase = true;
                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.MaxFailedAccessAttempts = 5;

            }))
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(option =>
            option.LoginPath = "/Account/SignIn");
            #endregion


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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
