using EIC.Customers.Data.Version1;
using PipServices3.Commons.Data;
using System.Threading.Tasks;

namespace EIC.Customers.Persistence
{
	public interface ICustomersPersistence
	{
		Task<DataPage<CustomerV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort);
		Task<CustomerV1> GetByIdAsync(string correlationId, string id);
		Task<CustomerV1> CreateAsync(string correlationId, CustomerV1 customer);
		Task<CustomerV1> UpdateAsync(string correlationId, CustomerV1 customer);
		Task<CustomerV1> DeleteByIdAsync(string correlationId, string id);
	}
}
