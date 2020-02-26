using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using Xunit;

namespace EIC.Customers.Persistence
{
    public class CustomersMemoryPersistenceTest : IDisposable
    {
        public CustomersMemoryPersistence _persistence;
        public CustomersPersistenceFixture _fixture;

        public CustomersMemoryPersistenceTest()
        {
            _persistence = new CustomersMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _fixture = new CustomersPersistenceFixture(_persistence);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();    
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestGetWithFiltersAsync()
        {
            await _fixture.TestGetWithFiltersAsync();
        }

    }
}
