using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using EIC.Customers.Persistence;
using EIC.Customers.Logic;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersDirectClientV1Test
    {
        private CustomersMemoryPersistence _persistence;
        private CustomersController _controller;
        private CustomersDirectClientV1 _client;
        private CustomersClientV1Fixture _fixture;

        public CustomersDirectClientV1Test()
        {
            _persistence = new CustomersMemoryPersistence();
            _controller = new CustomersController();
            _client = new CustomersDirectClientV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-customers", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-customers", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-customers", "client", "direct", "default", "1.0"), _client
            );

            _controller.SetReferences(references);

            _client.SetReferences(references);

            _fixture = new CustomersClientV1Fixture(_client);

            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }


    }
}
