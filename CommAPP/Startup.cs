using Comm.Business.Abstract;
using Comm.Business.Concrete;
using Comm.DataAccess;
using Comm.DataAccess.Abstract;
using Comm.DataAccess.Concrete;
using Comm.DataAccess.Concrete.EF;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using CommAPP.AutoMapper;
using CommAPP.Models.ViewModels.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP
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
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<CommerceContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("connection"));
            });
            //services.AddDbContext<CommerceContext>();

            services.AddSession();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CommerceContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters="";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);



            });

            


            services.AddScoped<IProductRepository, EfProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, EfCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentRepository, EfCommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICartRepository, EfCartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderRepository, EfOrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEmailSender, EmailSender>(options =>
            new EmailSender(Configuration["EmailSender:Host"],
                            Configuration.GetValue<int>("EmailSender:Port"),
                            Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                            Configuration["EmailSender:UserName"],
                            Configuration["EmailSender:Password"])
           );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                SeedDb.Seed();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "product",
                    pattern: "{controller}/{productname}",
                    defaults: new { controller = "Shop", action = "Details" }

                    );


                endpoints.MapControllerRoute(
                        name: "products",
                        pattern: "products/{category?}",
                        defaults: new { controller = "Shop", action = "GetProductsByCategory" }
                    );


                endpoints.MapControllerRoute(
                    name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });

             SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
        }
    }
}
