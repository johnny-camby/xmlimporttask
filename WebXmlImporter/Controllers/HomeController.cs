using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebXmlImporter.Models;
using XmlDataExtractManager.Interfaces;

namespace WebXmlImporter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IXmlDataExtractorService _xmlDataExtractorService;
        private readonly IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto> _customerBusinessService;

        public HomeController(ILogger<HomeController> logger,
            IBufferedFileUploadService bufferedFileUploadService,
            IXmlDataExtractorService xmlDataExtractorService,
            IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto> customerBusinessService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bufferedFileUploadService = bufferedFileUploadService ?? throw new ArgumentNullException(nameof(bufferedFileUploadService));
            _xmlDataExtractorService = xmlDataExtractorService ?? throw new ArgumentNullException(nameof(xmlDataExtractorService));
            _customerBusinessService = customerBusinessService ?? throw new ArgumentNullException(nameof(customerBusinessService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _customerBusinessService.GetAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var customer = await BuildCustomerViewModel(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerForCreateUpdateDto customerDto)
        {
            if (ModelState.IsValid)
            {
                await _customerBusinessService.AddAsync(customerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(new CustomerCreateViewModel());
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == default)
            {
                return NotFound();
            }

            var customer = await BuildCustomerViewModel(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, CustomerForCreateUpdateDto customerDto)
        {
            if (id == default)
            {
                return NotFound();
            }

            var customerToUpdate = await _customerBusinessService.GetAsync(id);

            if (customerToUpdate != null)
            {
                await _customerBusinessService.UpdateAsync(customerDto);
                try
                {
                    await _customerBusinessService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException  ex)
                {
                    Log.Error(ex, "Unable to save changes ");
                }
            }
            return View(customerToUpdate);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == default)
            {
                return NotFound();
            }

            var customer = await _customerBusinessService.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _customerBusinessService.GetAsync(id);
            await _customerBusinessService.DeleteAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadXmlAsync(IFormFile file)
        {
            try
            {
                var (isExisting, xmlfile) = await _bufferedFileUploadService.UploadFile(file);
                if (!string.IsNullOrEmpty(xmlfile))
                {
                    ViewBag.Message = "File Upload Successful";
                    await _xmlDataExtractorService.ProcessXmlAsync(xmlfile);
                }
                else
                {
                    Log.Information("Not proper xml file");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "File Upload Failed");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<CustomerDetailsViewModel> BuildCustomerViewModel(Guid id)
        {
            var customer = await _customerBusinessService.GetAsync(id);

            return new CustomerDetailsViewModel
            {
                Id = customer.Id,
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                Phone = customer.Phone,
                Fax = customer.Fax,
                FullAddressId = customer.FullAddressId,
                Address = customer.FullAddress.Address,
                City = customer.FullAddress.City,
                Country = customer.FullAddress.Country,
                PostalCode = customer.FullAddress.PostalCode,
                Region = customer.FullAddress.Region
            };
        }
    }
}
