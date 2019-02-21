using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Sql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Reflection;
using DevexpressReport.Web.Areas.Identity.Data;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.BL.Concrete;
using DevexpressReport.Web.Data;
using DevexpressReport.Web.Devexpress;
using DevexpressReport.Web.Models.Auth;

namespace DevexpressReport.Web {
    public class Startup {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IFileProvider FileProvider { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReportDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("MS SQL Connection"), b => b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name))
                );

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("AuthConnectionString")));

            services.AddTransient<IReportBL, ReportBL>();
            services.AddTransient<IUserBL, UserBL>();

            services
                .AddMvc()
                .AddDefaultDashboardController((configurator, serviceProvider) =>
                {
                    configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));

                    //DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(FileProvider.GetFileInfo("Data/Dashboards").PhysicalPath);
                    //configurator.SetDashboardStorage(dashboardFileStorage);

                    DataBaseDashboardStorage dataBaseDashboardStorage = new DataBaseDashboardStorage(Configuration.GetConnectionString("MS SQL Connection"));
                    configurator.SetDashboardStorage(dataBaseDashboardStorage);

                });


            services.AddDevExpressControls(options => options.Resources = ResourcesType.ThirdParty | ResourcesType.DevExtreme);

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
            })
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultUI()
                   .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false; // Change to true
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false; // change to true
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 2; // change to 6

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

            });
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(5);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.AccessDeniedPath = "/Home";
                options.LoginPath = "/Identity/Account/Login";
            });

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseDevExpressControls();
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapDashboardRoute();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}