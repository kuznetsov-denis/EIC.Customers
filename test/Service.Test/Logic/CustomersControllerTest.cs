using EIC.Customers.Data.Version1;
using EIC.Customers.Persistence;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Customers.Logic
{
    public class CustomersControllerTest: IDisposable
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

        private CustomersController _controller;
        private CustomersMemoryPersistence _persistence;

        public CustomersControllerTest()
        {
            _persistence = new CustomersMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _controller = new CustomersController();

            var references = References.FromTuples(
                new Descriptor("eic-customers", "persistence", "memory", "*", "1.0"), _persistence,
                new Descriptor("eic-customers", "controller", "default", "*", "1.0"), _controller
            );

            _controller.SetReferences(references);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first customer
            var customer = await _controller.CreateCustomerAsync(null, CUSTOMER1);

            AssertCustomers(CUSTOMER1, customer);

            // Create the second customer
            customer = await _controller.CreateCustomerAsync(null, CUSTOMER2);

            AssertCustomers(CUSTOMER2, customer);

            // Get all customers
            var page = await _controller.GetCustomersAsync(
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

            customer = await _controller.UpdateCustomerAsync(null, customer1);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);
            Assert.Equal("ABC", customer.FirstName);

            // Delete the customer
            customer = await _controller.DeleteCustomerByIdAsync(null, customer1.Id);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);

            // Try to get deleted customer
            customer = await _controller.GetCustomerByIdAsync(null, customer1.Id);

            Assert.Null(customer);
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
