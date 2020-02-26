using EIC.Customers.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersClientV1Fixture
    {
        private CustomerV1 CUSTOMER1 = new CustomerV1
        {
            Id = "1",
            FirstName = "Dustin",
            MiddleName = "J.",
            LastName = "Groom",
            Birthdate = new DateTime(1980, 4, 8),
            Gender = CustomerGenderV1.Male,
            Itin = "098190283",
            MobilePhone = "+7809309424749",
            Passport = "TN 671232"
        };
        private CustomerV1 CUSTOMER2 = new CustomerV1
        {
            Id = "2",
            FirstName = "Pamela",
            MiddleName = "A.",
            LastName = "Woods",
            Birthdate = new DateTime(1941, 12, 9),
            Gender = CustomerGenderV1.Female,
            Itin = "909089012",
            MobilePhone = "077-7871-7461",
            Passport = "BK671232"
        };

        private ICustomersClientV1 _client;

        public CustomersClientV1Fixture(ICustomersClientV1 client)
        {
            _client = client;
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create the first customer
            var customer = await _client.CreateCustomerAsync(null, CUSTOMER1);

            AssertCustomers(CUSTOMER1, customer);

            // Create the second customer
            customer = await _client.CreateCustomerAsync(null, CUSTOMER2);

            AssertCustomers(CUSTOMER2, customer);

            // Get all customers
            var page = await _client.GetCustomersAsync(
                null,
                new FilterParams(),
                new PagingParams(),
                new SortParams()
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var customer1 = page.Data[0];

            // Update the customer
            customer1.FirstName = "ABC";

            customer = await _client.UpdateCustomerAsync(null, customer1);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);
            Assert.Equal("ABC", customer.FirstName);

            // Delete the customer
            customer = await _client.DeleteCustomerByIdAsync(null, customer1.Id);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);

            // Try to get deleted customer
            customer = await _client.GetCustomerByIdAsync(null, customer1.Id);

            Assert.Null(customer);

            // Clean up for the second test
            await _client.DeleteCustomerByIdAsync(null, CUSTOMER2.Id);
        }

        private static void AssertCustomers(CustomerV1 etalon, CustomerV1 customer)
        {
            Assert.NotNull(customer);
            Assert.Equal(etalon.FirstName, customer.FirstName);
            Assert.Equal(etalon.LastName, customer.LastName);
            Assert.Equal(etalon.MiddleName, customer.MiddleName);
            Assert.Equal(etalon.MobilePhone, customer.MobilePhone);
            Assert.Equal(etalon.Gender, customer.Gender);
            Assert.Equal(etalon.Itin, customer.Itin);
            Assert.Equal(etalon.Passport, customer.Passport);
            Assert.Equal(etalon.Birthdate, customer.Birthdate);
        }
    }
}
