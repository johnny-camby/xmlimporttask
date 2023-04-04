using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Repository;
using Data.Repository.Entities;
using Data.Repository.Interfaces;
using Data.Repository.Repository;
using FluentAssertions;
using IntegrationTests.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WebXmlImporter.Configuration.Test;
using WebXmlImporter.Controllers;
using XmlDataExtractManager.Interfaces;
using XmlDataExtractManager.Services;
using Xunit;

namespace IntegrationTests
{
    public class HomeControllerTests : BaseClassFixture
    {
        private readonly IServiceProvider _serviceProvider;

        public HomeControllerTests(WebApplicationFactory<StartupTest> factory) : base(factory)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            RegisterServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task EveryoneHasAccessToHomepage()
        {
            Client.DefaultRequestHeaders.Clear();

            // Act
            var response = await Client.GetAsync("/home/index");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void GetIndex()
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<HomeController>>();
            var bufferedFileUploadService = _serviceProvider.GetRequiredService<IBufferedFileUploadService>();
            var xmlDataExtractorService = _serviceProvider.GetRequiredService<IXmlDataExtractorService>();
            var customerBusinessService = _serviceProvider.GetRequiredService<IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>>();

            var controller = new HomeController(logger, bufferedFileUploadService, xmlDataExtractorService, customerBusinessService);

            // Action
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
        }

        private void RegisterServices(ServiceCollection services)
        {
            services.AddDbContext<XmlImporterDbContext>(options => options.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=CustomerOrderDb;Integrated Security=True"));
            services.AddScoped<IBufferedFileUploadService, BufferedFileUploadService>();
            services.AddScoped<IXmlDataExtractorService, XmlDataExtractorService>();
            services.AddScoped<IXmlImporterRepository<Customer>, CustomerRepository>();
            services.AddScoped<IXmlImporterRepository<FullAddress>, FullAddressRepository>();
            services.AddScoped<IXmlImporterRepository<Order>, OrderRepository>();
            services.AddScoped<IXmlImporterRepository<ShipInfo>, ShipInfoRepository>();
            services.AddScoped<IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>, BusinessCustomerService>();
        }
    }
}
