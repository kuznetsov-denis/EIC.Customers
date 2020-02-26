using PipServices3.Commons.Commands;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using EIC.Customers.Data.Version1;
using EIC.Customers.Persistence;
using EIC.Customers.Logic;

namespace EIC.Customers.Logic
{
	public class CustomersController : ICustomersController, IConfigurable, IReferenceable, ICommandable
	{
		private ICustomersPersistence _persistence;
		private CustomersCommandSet _commandSet;

		public CustomersController()
		{ }

		public void Configure(ConfigParams config)
		{ }

		public void SetReferences(IReferences references)
		{
			_persistence = references.GetOneRequired<ICustomersPersistence>(
				new Descriptor("eic-customers", "persistence", "*", "*", "1.0")
			);
		}

		public CommandSet GetCommandSet()
		{
			if (_commandSet == null) 
				_commandSet = new CustomersCommandSet(this);

			return _commandSet;
		}

		public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
		{
			return await _persistence.CreateAsync(correlationId, customer);
		}

		public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
		{
			return await _persistence.UpdateAsync(correlationId, customer);
		}

		public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
		{
			return await _persistence.DeleteByIdAsync(correlationId, id);
		}

		public async Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id)
		{
			return await _persistence.GetByIdAsync(correlationId, id);
		}

		public async Task<DataPage<CustomerV1>> GetCustomersAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
		{
			return await _persistence.GetPageByFilterAsync(correlationId, filter, paging, sort);
		}
	}
}
