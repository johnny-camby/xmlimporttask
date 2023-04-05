using Data.Repository.Entities;
using Data.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using XmlDataExtractManager.Helpers;
using XmlDataExtractManager.Interfaces;

namespace XmlDataExtractManager.Services
{
    public class XmlDataExtractorService : IXmlDataExtractorService
    {
        private readonly IXmlImporterRepository<Customer> _customerRepository;
        private readonly IXmlImporterRepository<FullAddress> _fullAddressRepository;
        private readonly IXmlImporterRepository<Order> _orderRepository;
        private readonly IXmlImporterRepository<ShipInfo> _shipInfoRepository;

        public XmlDataExtractorService(IXmlImporterRepository<Customer> customerRepository,
            IXmlImporterRepository<FullAddress> fullAddressRepository,
            IXmlImporterRepository<Order> orderRepository,
            IXmlImporterRepository<ShipInfo> shipInfoRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _fullAddressRepository = fullAddressRepository ?? throw new ArgumentNullException(nameof(fullAddressRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _shipInfoRepository = shipInfoRepository ?? throw new ArgumentNullException(nameof(shipInfoRepository));
        }

        public async Task ProcessXmlAsync(string xmlfile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlfile);

            XElement xelement = XElement.Parse(xmlDoc.OuterXml);

            await ExtractCustomersAsync(xelement);
            await ExtractOrdersAsync(xelement);
        }

        private async Task ExtractCustomersAsync(XElement xelement)
        {
            var customerElements = xelement.Descendants("Customer");

            foreach (var customer in customerElements)
            {
                var cust = new Customer
                {
                    CustomerID = customer.CreateNavigator().SelectSingleNode("@CustomerID")?.Value,
                    CompanyName = customer.XPathSelectElement("CompanyName")?.Value,
                    ContactName = customer.XPathSelectElement("ContactName")?.Value,
                    ContactTitle = customer.XPathSelectElement("ContactTitle")?.Value,
                    Phone = customer.XPathSelectElement("Phone")?.Value,
                    Fax = customer.XPathSelectElement("Fax")?.Value,

                    FullAddress = new FullAddress
                    {
                        Address = customer.XPathSelectElement("FullAddress/Address")?.Value,
                        City = customer.XPathSelectElement("FullAddress/City")?.Value,
                        Region = customer.XPathSelectElement("FullAddress/Region")?.Value,
                        PostalCode = (int)(customer.XPathSelectElement("FullAddress/PostalCode")?.Value.ToNullableInt()),
                        Country = customer.XPathSelectElement("FullAddress/Country")?.Value,
                    }
                };
                await _fullAddressRepository.AddAsync(cust.FullAddress);
                await _customerRepository.AddAsync(cust);
            }
        }

        private async Task ExtractOrdersAsync(XElement xelement)
        {
            var orderElements = xelement.Descendants("Order");

            foreach (var order in orderElements)
            {
                var ord = new Order
                {
                    CustomerID = order.XPathSelectElement("CustomerID")?.Value,
                    EmployeeID = (int)(order.XPathSelectElement("EmployeeID")?.Value.ToNullableInt()),
                    OrderDate = (DateTime)(order.XPathSelectElement("OrderDate")?.Value.ToNullableDateTime()),
                    RequiredDate = (DateTime)(order.XPathSelectElement("RequiredDate")?.Value.ToNullableDateTime()),
                    ShipInfo = new ShipInfo
                    {
                        ShippedDate = (DateTime)(order.CreateNavigator().SelectSingleNode("//ShipInfo/@ShippedDate")?.Value.ToNullableDateTime()),
                        Freight = (double)(order.XPathSelectElement("ShipInfo/Freight")?.Value.ToNullableDecimal()),
                        ShipAddress = order.XPathSelectElement("ShipInfo/ShipAddress")?.Value,
                        ShipCity = order.XPathSelectElement("ShipInfo/ShipCity")?.Value,
                        ShipCountry = order.XPathSelectElement("ShipInfo/ShipCountry")?.Value,
                        ShipName = order.XPathSelectElement("ShipInfo/ShipName")?.Value,
                        ShipPostalCode = (int)(order.XPathSelectElement("ShipInfo/ShipPostalCode")?.Value.ToNullableInt()),
                        ShipRegion = order.XPathSelectElement("ShipInfo/ShipRegion")?.Value,
                        ShipVia = (int)(order.XPathSelectElement("ShipInfo/ShipVia")?.Value.ToNullableInt()),
                    }
                };
                await _shipInfoRepository.AddAsync(ord.ShipInfo);
                await _orderRepository.AddAsync(ord);
            }
        }
    }
}
