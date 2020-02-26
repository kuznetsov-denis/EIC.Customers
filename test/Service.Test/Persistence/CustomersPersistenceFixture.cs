using EIC.Customers.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Customers.Persistence
{
    public class CustomersPersistenceFixture
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
            MobilePhone = "079-2959-4308",
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
        private CustomerV1 CUSTOMER3 = new CustomerV1
        {
            Id = "3",
            FirstName = "Evelyn",
            MiddleName = "G.",
            LastName = "Johnson",
            Birthdate = new DateTime(1981, 12, 11),
            Gender = CustomerGenderV1.Female,
            Itin = "07812908983",
            MobilePhone = "079-2959-4308",
            Passport = "NN98712232"
        };

        private ICustomersPersistence _persistence;

        public CustomersPersistenceFixture(ICustomersPersistence persistence)
        {
            _persistence = persistence;
        }

        private async Task TestCreateCustomersAsync()
        {
            // Create the first customer
            var customer = await _persistence.CreateAsync(null, CUSTOMER1);

            AssertCustomers(CUSTOMER1, customer);

            // Create the second customer
            customer = await _persistence.CreateAsync(null, CUSTOMER2);

            AssertCustomers(CUSTOMER2, customer);

            // Create the third customer
            customer = await _persistence.CreateAsync(null, CUSTOMER3);

            AssertCustomers(CUSTOMER3, customer);
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create items
            await TestCreateCustomersAsync();

            // Get all customers
            var page = await _persistence.GetPageByFilterAsync(
                null,
                new FilterParams(),
                new PagingParams(),
                new SortParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            var customer1 = page.Data[0];

            // Update the customer
            customer1.FirstName = "ABC";

            var customer = await _persistence.UpdateAsync(null, customer1);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);
            Assert.Equal("ABC", customer.FirstName);

            // Delete the customer
            customer = await _persistence.DeleteByIdAsync(null, customer1.Id);

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);

            // Try to get deleted customer
            customer = await _persistence.GetByIdAsync(null, customer1.Id);

            Assert.Null(customer);
        }

        public async Task TestGetWithFiltersAsync()
        {
            // Create items
            await TestCreateCustomersAsync();

            // Filter by id
            var page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "id", "1"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Single(page.Data);

            // Filter by birthdate
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "birthdate", "1981-12-11"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Single(page.Data);

            // Filter by gender
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "gender", CustomerGenderV1.Female
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by mobile_phone
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "mobile_phone", "079-2959-4308"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Equal(2, page.Data.Count);
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
