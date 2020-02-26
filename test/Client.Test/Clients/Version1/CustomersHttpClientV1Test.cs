using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using EIC.Customers.Persistence;
using EIC.Customers.Logic;
using EIC.Customers.Services.Version1;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersHttpClientV1Test
    {
        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 8080
        );

        private CustomersMemoryPersistence _persistence;
        private CustomersController _controller;
        private CustomersHttpClientV1 _client;
        private CustomersHttpServiceV1 _service;
        private CustomersClientV1Fixture _fixture;

        public CustomersHttpClientV1Test()
        {
            _persistence = new CustomersMemoryPersistence();
            _controller = new CustomersController();
            _client = new CustomersHttpClientV1();
            _service = new CustomersHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-customers", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-customers", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-customers", "client", "http", "default", "1.0"), _client,
                new Descriptor("eic-customers", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            _client.Configure(HttpConfig);
            _client.SetReferences(references);

            _fixture = new CustomersClientV1Fixture(_client);

            _service.OpenAsync(null).Wait();
            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }
    }
}
