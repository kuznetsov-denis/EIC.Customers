using System.Threading.Tasks;
using PipServices3.Commons.Data;
using EIC.Customers.Data.Version1;

namespace EIC.Customers.Logic
{
    public interface ICustomersController
    {
        Task<DataPage<CustomerV1>> GetCustomersAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort);
        Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id);
        Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer);
        Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer);
        Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id);
    }
}
