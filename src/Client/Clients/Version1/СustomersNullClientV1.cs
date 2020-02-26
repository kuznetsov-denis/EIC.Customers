using System.Threading.Tasks;
using PipServices3.Commons.Data;
using EIC.Customers.Data.Version1;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersNullClientV1 : ICustomersClientV1
    {
        public async Task<DataPage<CustomerV1>> GetCustomersAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return await Task.FromResult(new DataPage<CustomerV1>());
        }

        public async Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new CustomerV1());
        }

        public async Task<CustomerV1> GetCustomerByUdiAsync(string correlationId, string udi)
        {
            return await Task.FromResult(new CustomerV1());
        }

        public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            return await Task.FromResult(new CustomerV1());
        }

        public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            return await Task.FromResult(new CustomerV1());
        }

        public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new CustomerV1());
        }
    }
}
