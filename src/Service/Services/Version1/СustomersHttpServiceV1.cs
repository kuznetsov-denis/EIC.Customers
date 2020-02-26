using PipServices3.Commons.Refer;
using PipServices3.Rpc.Services;

namespace EIC.Customers.Services.Version1
{
	public class CustomersHttpServiceV1 : CommandableHttpService
	{
		public CustomersHttpServiceV1()
			: base("v1/customers")
		{
			_dependencyResolver.Put("controller", new Descriptor("eic-customers", "controller", "default", "*", "1.0"));
		}
	}
}
