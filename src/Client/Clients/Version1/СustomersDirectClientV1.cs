using System.Threading.Tasks;

using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using PipServices3.Rpc.Clients;

using EIC.Customers.Clients.Version1;
using EIC.Customers.Data.Version1;
using EIC.Customers.Logic;

namespace EIC.Customers.Clients.Version1
{
    public class CustomersDirectClientV1 : DirectClient<ICustomersController>, ICustomersClientV1
    {
        public CustomersDirectClientV1() : base()
        {
            _dependencyResolver.Put("controller", new Descriptor("eic-customers", "controller", "*", "*", "1.0"));
        }

        public async Task<DataPage<CustomerV1>> GetCustomersAsync(
            string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            using (Instrument(correlationId, "customers.get_customers"))
            {
                return await _controller.GetCustomersAsync(correlationId, filter, paging, sort);
            }
        }

        public async Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "customers.get_customer_by_id"))
            {
                return await _controller.GetCustomerByIdAsync(correlationId, id);
            }
        }

        public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            using (Instrument(correlationId, "customers.create_customer"))
            {
                return await _controller.CreateCustomerAsync(correlationId, customer);
            }
        }

        public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            using (Instrument(correlationId, "customers.update_customer"))
            {
                return await _controller.UpdateCustomerAsync(correlationId, customer);
            }
        }

        public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "customers.delete_customer_by_id"))
            {
                return await _controller.DeleteCustomerByIdAsync(correlationId, id);
            }
        }
    }
}
