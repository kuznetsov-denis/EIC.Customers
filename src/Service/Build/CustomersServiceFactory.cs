using PipServices3.Commons.Refer;
using PipServices3.Components.Build;

using EIC.Customers.Logic;
using EIC.Customers.Persistence;
using EIC.Customers.Services.Version1;

namespace EIC.Customers.Build
{
    public class CustomersServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("eic-customers", "factory", "service", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("eic-customers", "persistence", "memory", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("eic-customers", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("eic-customers", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("eic-customers", "service", "http", "*", "1.0");


        public CustomersServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(CustomersMemoryPersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(CustomersMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(CustomersController));
            RegisterAsType(HttpServiceDescriptor, typeof(CustomersHttpServiceV1));
        }
    }
}
