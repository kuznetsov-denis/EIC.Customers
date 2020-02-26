using EIC.Customers.Data.Version1;
using EIC.Customers.Logic;
using EIC.Customers.Persistence;
using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Customers.Services.Version1
{
    public class CustomersHttpServiceV1Test
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

        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", "3000"
        );

        private CustomersMemoryPersistence _persistence;
        private CustomersController _controller;
        private CustomersHttpServiceV1 _service;

        public CustomersHttpServiceV1Test()
        {
            _persistence = new CustomersMemoryPersistence();
            _controller = new CustomersController();
            _service = new CustomersHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-customers", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-customers", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-customers", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            //_service.OpenAsync(null).Wait();
            // Todo: This is defect! Open shall not block the tread
            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(5000); // Just let service a sec to be initialized
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first customer
            var customer = await Invoke<CustomerV1>("create_customer", new { customer = CUSTOMER1 });

            AssertCustomers(CUSTOMER1, customer);

            // Create the second customer
            customer = await Invoke<CustomerV1>("create_customer", new { customer = CUSTOMER2 });

            AssertCustomers(CUSTOMER2, customer);

            // Get all customers
            var page = await Invoke<DataPage<CustomerV1>>(
                "get_customers",
                new
                {
                    filter = new FilterParams(),
                    paging = new PagingParams()
                }
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var customer1 = page.Data[0];

            // Update the customer
            customer1.FirstName = "ABC";

            customer = await Invoke<CustomerV1>("update_customer", new { customer = customer1 });

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);
            Assert.Equal("ABC", customer.FirstName);

            // Delete the customer
            customer = await Invoke<CustomerV1>("delete_customer_by_id", new { customer_id = customer1.Id });

            Assert.NotNull(customer);
            Assert.Equal(customer1.Id, customer.Id);

            // Try to get deleted customer
            customer = await Invoke<CustomerV1>("get_customer_by_id", new { customer_id = customer1.Id });

            Assert.Null(customer);
        }

        private static async Task<T> Invoke<T>(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestValue = JsonConverter.ToJson(request);
                using (var content = new StringContent(requestValue, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("http://localhost:3000/v1/customers/" + route, content);
                    var responseValue = response.Content.ReadAsStringAsync().Result;
                    return JsonConverter.FromJson<T>(responseValue);
                }
            }
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
