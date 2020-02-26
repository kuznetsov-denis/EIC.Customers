using System.Collections.Generic;
using PipServices3.Commons.Data;
using PipServices3.Commons.Commands;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;
using EIC.Customers.Data.Version1;

namespace EIC.Customers.Logic
{
	public class CustomersCommandSet : CommandSet
	{
        private ICustomersController _controller;

        public CustomersCommandSet(ICustomersController controller)
        {
            _controller = controller;

            AddCommand(MakeGetCustomersCommand());
            AddCommand(MakeGetCustomerByIdCustomersCommand());
            AddCommand(MakeCreateCustomerCommand());
            AddCommand(MakeUpdateCustomerCommand());
            AddCommand(MakeDeleteCustomerByIdCommand());
        }

        private ICommand MakeGetCustomersCommand()
        {
            return new Command(
                "get_customers",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema())
                    .WithOptionalProperty("sort", new SortParamsSchema()),
                async (correlationId, parameters) =>
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    var paging = PagingParams.FromValue(parameters.Get("paging"));
                    var sort = SortParams.FromValue(parameters.Get("sort"));
                    return await _controller.GetCustomersAsync(correlationId, filter, paging, sort);
                });
        }

        private ICommand MakeGetCustomerByIdCustomersCommand()
        {
            return new Command(
                "get_customer_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("customer_id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("customer_id");
                    return await _controller.GetCustomerByIdAsync(correlationId, id);
                });
        }

        private ICommand MakeCreateCustomerCommand()
        {
            return new Command(
                "create_customer",
                new ObjectSchema()
                    .WithRequiredProperty("customer", new CustomerV1Schema()),
                async (correlationId, parameters) =>
                {
                    var customer = ConvertToCustomer(parameters.GetAsObject("customer"));
                    return await _controller.CreateCustomerAsync(correlationId, customer);
                });
        }

        private ICommand MakeUpdateCustomerCommand()
        {
            return new Command(
               "update_customer",
               new ObjectSchema()
                    .WithRequiredProperty("customer", new CustomerV1Schema()),
               async (correlationId, parameters) =>
               {
                   var customer = ConvertToCustomer(parameters.GetAsObject("customer"));
                   return await _controller.UpdateCustomerAsync(correlationId, customer);
               });
        }

        private ICommand MakeDeleteCustomerByIdCommand()
        {
            return new Command(
               "delete_customer_by_id",
               new ObjectSchema()
                   .WithRequiredProperty("customer_id", TypeCode.String),
               async (correlationId, parameters) =>
               {
                   var id = parameters.GetAsString("customer_id");
                   return await _controller.DeleteCustomerByIdAsync(correlationId, id);
               });
        }

        private CustomerV1 ConvertToCustomer(object value)
        {
            return JsonConverter.FromJson<CustomerV1>(JsonConverter.ToJson(value));
        }
    }
}
