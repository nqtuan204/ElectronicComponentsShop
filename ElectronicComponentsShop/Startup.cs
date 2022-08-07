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
using ElectronicComponentsShop.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ElectronicComponentsShop.Services.Jwt;
using ElectronicComponentsShop.Services.Cart;
using ElectronicComponentsShop.Services.Order;
using ElectronicComponentsShop.Services.Email;
using ElectronicComponentsShop.Services.Stat;

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
            string connectionString = $"User ID=postgres;Password=123;Host=localhost;Port=5432;Database=ElectronicComponentsShop;Pooling=true;Trust Server Certificate=True;";
            if (!IsDevelopment)
            {
                string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(connectionUrl);
                string db = databaseUri.LocalPath.TrimStart('/');
                string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
                connectionString = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
            }

            services.AddDbContext<ECSDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStatService, StatService>();
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"])),
                };
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("OnlyUser", policyConfig =>
                {
                    policyConfig.RequireClaim("Role", "Customer");
                });
                config.AddPolicy("OnlyAdmin", policyConfig =>
                {
                    policyConfig.RequireClaim("Role", "Admin");
                });
            });
            services.AddMemoryCache();
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

            app.Use((context, next) =>
            {
                string token = "";
                if (context.Request.Cookies.TryGetValue("token", out token))
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                return next();
            });

            app.UseAuthentication();

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == 401)
                {
                    response.Cookies.Delete("token");
                    response.Redirect("/User/Login");
                }
                if (response.StatusCode == 403)
                {
                    response.Redirect("/");
                }
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "confirm reset password",
                    pattern: "/User/ConfirmResetPassword/{token}",
                    defaults: new { controller = "User", action = "ConfirmResetPassword" });
                endpoints.MapControllerRoute(
                    name: "product details",
                    pattern: "/product/{id}.{slug}",
                    defaults: new { controller = "Product", action = "Details" });
                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "/Checkout",
                    defaults: new { controller = "Order", action = "Checkout" });
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
