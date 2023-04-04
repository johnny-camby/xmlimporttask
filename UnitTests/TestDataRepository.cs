using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestDataRepository
    {
        [TestMethod]
        public void test_repository_mocking()
        {
            List<CustomerDto> customers = new List<CustomerDto>
            {
                new CustomerDto
                {
                   Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                   CustomerID = "TEST2023",
                   FullAddressId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                   FullAddress = new FullAddressDto
                   {
                       FullAddressId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7")
                   }
                }
            };

            Mock<IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>> mockCustomerBusinessService = new Mock<IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto>>();
            mockCustomerBusinessService.Setup(obj => obj.GetAsync()).Returns(Task.FromResult(customers));

            IBusinessMainService<CustomerDto, CustomerForCreateUpdateDto> customerBusinessService = mockCustomerBusinessService.Object;
            var customerList = customerBusinessService.GetAsync();
            Assert.IsTrue(customerList.Result == customers);
        }
    }
}
