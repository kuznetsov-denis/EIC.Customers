using PipServices3.Container;
using PipServices3.Rpc.Build;

using EIC.Customers.Build;

namespace EIC.Customers.Container
{
	public class CustomersProcess : ProcessContainer
	{
		public CustomersProcess()
			: base("customers", "Customers microservice")
		{
			_factories.Add(new DefaultRpcFactory());
			_factories.Add(new CustomersServiceFactory());
		}
	}
}
