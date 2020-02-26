using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PipServices3.Commons.Data;
using PipServices3.Data.Persistence;
using EIC.Customers.Data.Version1;
using MongoDB.Driver;

namespace EIC.Customers.Persistence
{
    public class CustomersMemoryPersistence : IdentifiableMemoryPersistence<CustomerV1, string>, ICustomersPersistence
    {
        public CustomersMemoryPersistence()
        {
            _maxPageSize = 1000;
        }

		private List<Func<CustomerV1, bool>> ComposeFilter(FilterParams filterParams)
		{
			filterParams = filterParams ?? new FilterParams();

			var builder = Builders<CustomerV1>.Filter;
			var filter = builder.Empty;

			var id = filterParams.GetAsNullableString("id");
			var lastName = filterParams.GetAsNullableString("last_name");
			var firstName = filterParams.GetAsNullableString("first_name");
			var middleName = filterParams.GetAsNullableString("middle_name");
			var gender = filterParams.GetAsNullableString("gender");
			var birthdate = filterParams.GetAsNullableString("birthdate");
			var itin = filterParams.GetAsNullableString("itin");
			var passport = filterParams.GetAsNullableString("passport");
			var mobile_phone = filterParams.GetAsNullableString("mobile_phone");

			return new List<Func<CustomerV1, bool>>
			{ 
				(item) => 
				{
					if (!string.IsNullOrWhiteSpace(id) && item.Id != id) return false;
					if (!string.IsNullOrWhiteSpace(lastName) && item.LastName != lastName)  return false;
					if (!string.IsNullOrWhiteSpace(firstName) && item.FirstName != firstName)  return false;
					if (!string.IsNullOrWhiteSpace(middleName) && item.MiddleName != middleName)  return false;
					if (!string.IsNullOrWhiteSpace(gender) && item.Gender != gender)  return false;

					if (!string.IsNullOrWhiteSpace(birthdate) &&
						DateTime.TryParse(birthdate, out DateTime dt_birthdate) &&
						item.Birthdate != dt_birthdate)
						return false;

					if (!string.IsNullOrWhiteSpace(itin) && item.Itin != itin) return false;
					if (!string.IsNullOrWhiteSpace(passport) && item.Passport != passport) return false;
					if (!string.IsNullOrWhiteSpace(mobile_phone) && item.MobilePhone != mobile_phone) return false;

					return true;
				}
			};
		}

		public Task<DataPage<CustomerV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

		public Task<CustomerV1> GetByIdAsync(string correlationId, string id)
		{
			return base.GetOneByIdAsync(correlationId, id);
		}
	}
}
