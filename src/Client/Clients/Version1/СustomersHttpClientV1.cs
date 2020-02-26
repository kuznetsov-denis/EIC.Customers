using System.Threading.Tasks;

using PipServices3.Commons.Data;
using PipServices3.Rpc.Clients;

using EIC.Customers.Data.Version1;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersHttpClientV1 : CommandableHttpClient, ICustomersClientV1
    {
        public CustomersHttpClientV1()
            : base("v1/customers")
        { }

        public async Task<DataPage<CustomerV1>> GetCustomersAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return await CallCommandAsync<DataPage<CustomerV1>>(
                "get_customers",
                correlationId,
                new
                {
                    filter = filter,
                    paging = paging,
                    sort = sort
                }
            );
        }

        public async Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<CustomerV1>(
                "get_customer_by_id",
                correlationId,
                new
                {
                    customer_id = id
                }
            );
        }

        public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            return await CallCommandAsync<CustomerV1>(
                "create_customer",
                correlationId,
                new
                {
                    customer = customer
                }
            );
        }

        public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            return await CallCommandAsync<CustomerV1>(
                "update_customer",
                correlationId,
                new
                {
                    customer = customer
                }
            );
        }

        public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<CustomerV1>(
                "delete_customer_by_id",
                correlationId,
                new
                {
                    customer_id = id
                }
            );
        }

    }
}
