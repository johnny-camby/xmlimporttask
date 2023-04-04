using Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebXmlImporter.Configuration.Test
{
    public class StartupTest : Startup
    {
        public StartupTest(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        { }

        public override void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<XmlImporterDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:CustomerOrderDbConn"], m => m.MigrationsAssembly("WebXmlImporter")));
        }
    }
}
