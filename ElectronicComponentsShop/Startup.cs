using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ElectronicComponentsShop.Database;
using ElectronicComponentsShop.Services.Product;
using ElectronicComponentsShop.Services.Category;

namespace ElectronicComponentsShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            bool IsDevelopment = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development");
            Action<DbContextOptionsBuilder> optionsAction = options => options.UseNpgsql(Configuration.GetConnectionString("PgSqlConnection"));
            if (!IsDevelopment)
            {
                string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(connectionUrl);
                string db = databaseUri.LocalPath.TrimStart('/');
                string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
                string connectionString = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
                optionsAction = options => options.UseNpgsql(connectionString);
            }
            services.AddDbContext<ECSDbContext>(optionsAction);
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "product list",
                    pattern: "/product/list",
                    defaults: new { controller = "Product", action = "List" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
