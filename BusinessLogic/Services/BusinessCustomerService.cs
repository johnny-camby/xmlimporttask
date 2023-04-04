using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Data.Repository.Entities;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class BusinessCustomerService : IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>
    {
        private readonly IXmlImporterRepository<Customer> _customerRespository;
        public BusinessCustomerService(IXmlImporterRepository<Customer> customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public async Task AddAsync(CustomerForCreateUpdateDto entity)
        {
            var customer = new Customer
            {
                CompanyName = entity.CompanyName,
                ContactName = entity.ContactName,
                ContactTitle = entity.ContactTitle,
                CustomerID = entity.CustomerID,
                Fax = entity.Fax,
                Phone = entity.Phone,
                FullAddress = new FullAddress
                {
                    Address = entity.Address,
                    City = entity.City,
                    Country = entity.Country,
                    PostalCode = entity.PostalCode,
                    Region = entity.Region
                }
            };
            await _customerRespository.AddAsync(customer);
        }

        public async Task DeleteAsync(CustomerDto entity)
        {
            var customer = new Customer
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                ContactName = entity.ContactName,
                ContactTitle = entity.ContactTitle,
                CustomerID = entity.CustomerID,
                Fax = entity.Fax,
                FullAddressId = entity.FullAddressId,
                Phone = entity.Phone,
                FullAddress = new FullAddress
                {
                    FullAddressId = entity.FullAddress.FullAddressId,
                    Address = entity.FullAddress.Address,
                    City = entity.FullAddress.City,
                    Country = entity.FullAddress.Country,
                    PostalCode = entity.FullAddress.PostalCode,
                    Region = entity.FullAddress.Region
                }
            };
            await _customerRespository.DeleteAsync(customer);
        }

        public async Task<List<CustomerDto>> GetAsync()
        {
            var customers = await _customerRespository.GetAsync();
            var data = customers.Select(customer => new CustomerDto
            {
                Id = customer.Id,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                CustomerID = customer.CustomerID,
                Fax = customer.Fax,
                FullAddressId = customer.FullAddressId,
                Phone = customer.Phone,
                FullAddress = new FullAddressDto
                {
                    FullAddressId = customer.FullAddress.FullAddressId,
                    Address = customer.FullAddress.Address,
                    City = customer.FullAddress.City,
                    Country = customer.FullAddress.Country,
                    PostalCode = customer.FullAddress.PostalCode,
                    Region = customer.FullAddress.Region
                }
            });
            return data.ToList();

        }

        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerRespository.GetAsync(id);

            if (customer != null)
            {
                return new CustomerDto
                {
                    Id = customer.Id,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    CustomerID = customer.CustomerID,
                    Fax = customer.Fax,
                    FullAddressId = customer.FullAddressId,
                    Phone = customer.Phone,
                    FullAddress = new FullAddressDto
                    {
                        FullAddressId = customer.FullAddress.FullAddressId,
                        Address = customer.FullAddress.Address,
                        City = customer.FullAddress.City,
                        Country = customer.FullAddress.Country,
                        PostalCode = customer.FullAddress.PostalCode,
                        Region = customer.FullAddress.Region
                    }
                };
            }
            else
            {
                return null;
            }

        }

        public async Task SaveAsync()
        {
            await _customerRespository.SaveAsync();
        }

        public async Task UpdateAsync(CustomerForCreateUpdateDto entity)
        {
            var customer = new Customer
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                ContactName = entity.ContactName,
                ContactTitle = entity.ContactTitle,
                CustomerID = entity.CustomerID,
                Fax = entity.Fax,
                Phone = entity.Phone,
                FullAddressId = entity.FullAddressId,
                FullAddress = new FullAddress
                {
                    FullAddressId = entity.FullAddressId,
                    Address = entity.Address,
                    City = entity.City,
                    Country = entity.Country,
                    PostalCode = entity.PostalCode,
                    Region = entity.Region
                }
            };
            await _customerRespository.UpdateAsync(customer);
        }
    }
}
