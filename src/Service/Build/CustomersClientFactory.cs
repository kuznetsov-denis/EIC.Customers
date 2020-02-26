using PipServices3.Commons.Refer;
using PipServices3.Components.Build;

using EIC.Customers.Clients.Version1;

namespace EIC.Customers.Build
{
    public class CustomersClientFactory : Factory
    {
        public static Descriptor NullClientDescriptor = new Descriptor("eic-customers", "client", "null", "*", "1.0");
        public static Descriptor DirectClientDescriptor = new Descriptor("eic-customers", "client", "direct", "*", "1.0");
        public static Descriptor HttpClientDescriptor = new Descriptor("eic-customers", "client", "http", "*", "1.0");

        public CustomersClientFactory()
        {
            RegisterAsType(CustomersClientFactory.NullClientDescriptor, typeof(CustomersNullClientV1));
            RegisterAsType(CustomersClientFactory.DirectClientDescriptor, typeof(CustomersDirectClientV1));
            RegisterAsType(CustomersClientFactory.HttpClientDescriptor, typeof(CustomersHttpClientV1));
        }
    }
}
