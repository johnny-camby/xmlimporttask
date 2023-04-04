using Bogus;
using Data.Repository.Entities;
using System;

namespace IntegrationTests
{
    public static class MockData
    {
        public static Faker<Customer> GetCustomerFaker(Guid id)
        {
            var fakerFullAddress = new Faker<FullAddress>()
                .RuleFor(a => a.FullAddressId, f => Guid.NewGuid())
                .RuleFor(a => a.City, "Berlin")
               .RuleFor(a => a.Address, "Europaplatz 1")
               .RuleFor(a => a.Country, "Germany")
               .RuleFor(a => a.PostalCode, 10557)
               .RuleFor(a => a.Region, "Berlin-Brandenburg");

            var fakerCustomer = new Faker<Customer>()
                .RuleFor(c => c.Id, id)
                .RuleFor(c => c.CompanyName, "Xplicit GmbH")
                .RuleFor(c => c.ContactName, "Jane Doe")
                .RuleFor(c => c.ContactTitle, "Miss")
                .RuleFor(c => c.Phone, "0176-463737228")
                .RuleFor(c => c.Fax, "463737228")
                .RuleFor(c => c.CustomerID, f => $"{f.Random.String2(3).ToUpper()}{f.Random.Number(1, 5)}")
                .RuleFor(c => c.FullAddress, () => fakerFullAddress)
                .RuleFor(c => c.FullAddressId, (f, o) => o.FullAddress.FullAddressId);
            return fakerCustomer;
        }

        public static Faker<Customer> GetCustomerFaker(Guid id, Guid fullAddressId)
        {
            var fakerFullAddress = new Faker<FullAddress>()
               .RuleFor(a => a.FullAddressId, fullAddressId)
               .RuleFor(a => a.City, "Frankfurt")
               .RuleFor(a => a.Address, "Ferdinand-Happ-Str. 0")
               .RuleFor(a => a.Country, "Germany")
               .RuleFor(a => a.PostalCode, 60314)
               .RuleFor(a => a.Region, "Hessen");

            var fakerCustomer = new Faker<Customer>()
                .RuleFor(c => c.Id, id)
                .RuleFor(c => c.CompanyName, "Xplicit GmbH")
                .RuleFor(c => c.ContactName, "Jane Doe")
                .RuleFor(c => c.ContactTitle, "Miss")
                .RuleFor(c => c.Phone, "0176-463737228")
                .RuleFor(c => c.Fax, "463737228")
                .RuleFor(c => c.CustomerID, f => $"{f.Random.String2(3).ToUpper()}{f.Random.Number(1, 5)}")
                .RuleFor(c => c.FullAddress, () => fakerFullAddress)
                .RuleFor(c => c.FullAddressId, (f, o) => o.FullAddress.FullAddressId);
            return fakerCustomer;
        }

        public static Customer GenerateRandomCustomer(Guid id)
        {
            var customer = GetCustomerFaker(id).Generate();
            return customer;
        }

        public static Customer GenerateRandomFullAddress(Guid id, Guid fullAddressId)
        {
            var customer = GetCustomerFaker(id, fullAddressId).Generate();
            return customer;
        }
    }
}

