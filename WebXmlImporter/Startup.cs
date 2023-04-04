using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Repository;
using Data.Repository.Entities;
using Data.Repository.Interfaces;
using Data.Repository.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XmlDataExtractManager.Interfaces;
using XmlDataExtractManager.Services;

namespace WebXmlImporter
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
            RegisterDbContext(services);
            services.AddScoped<IBufferedFileUploadService, BufferedFileUploadService>();
            services.AddScoped<IXmlDataExtractorService, XmlDataExtractorService>();
            services.AddScoped<IXmlImporterRepository<Customer>, CustomerRepository>();
            services.AddScoped<IXmlImporterRepository<FullAddress>, FullAddressRepository>();
            services.AddScoped<IXmlImporterRepository<Order>, OrderRepository>();
            services.AddScoped<IXmlImporterRepository<ShipInfo>, ShipInfoRepository>();
            services.AddScoped<IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>, BusinessCustomerService>();
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public virtual void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<XmlImporterDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:CustomerOrderDbConn"], m => m.MigrationsAssembly("WebXmlImporter"))
             .EnableSensitiveDataLogging()
             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }
    }
}
