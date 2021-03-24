using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NRDCL.Data;
using NRDCL.IService.User;
using NRDCL.Models;
using NRDCL.Models.Acc;
using NRDCL.Repository;
using NRDCL.Service.User;

namespace NRDCL
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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NRDCL_DB_Context>();
            services.ConfigureApplicationCookie(config =>
            {
                config.AccessDeniedPath = new PathString("/AccessDenied/AccessDenied403");
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<NRDCL_DB_Context>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DataAccessPostgreSqlProvider")));
            services.AddMvc();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.AddScoped<IUserService, UserService>();
            
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Login}/{action=Login}/{id?}");
                    pattern: "{controller=Account}/{action=Signin}/{id?}");
            });
        }
    }
}
