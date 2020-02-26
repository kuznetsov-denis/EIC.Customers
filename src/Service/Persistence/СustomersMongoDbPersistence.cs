using System;
using System.Threading.Tasks;

using PipServices3.Commons.Data;
using PipServices3.MongoDb.Persistence;

using EIC.Customers.Data.Version1;
using EIC.Customers.Persistence.MongoDb;
using MongoDB.Driver;
using PipServices3.Commons.Data.Mapper;

namespace EIC.Customers.Persistence
{
	public class CustomersMongoDbPersistence : IdentifiableMongoDbPersistence<CustomerMongoDbSchema, string>, ICustomersPersistence
	{
		public CustomersMongoDbPersistence()
			: base("customers")
		{
		}

		private new FilterDefinition<CustomerMongoDbSchema> ComposeFilter(FilterParams filterParams)
		{
			filterParams = filterParams ?? new FilterParams();

			var builder = Builders<CustomerMongoDbSchema>.Filter;
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

			if (!string.IsNullOrWhiteSpace(id)) filter &= builder.Eq(b => b.Id, id);
			if (!string.IsNullOrWhiteSpace(lastName)) filter &= builder.Eq(b => b.LastName, lastName);
			if (!string.IsNullOrWhiteSpace(firstName)) filter &= builder.Eq(b => b.FirstName, firstName);
			if (!string.IsNullOrWhiteSpace(middleName)) filter &= builder.Eq(b => b.MiddleName, middleName);

			if (!string.IsNullOrWhiteSpace(birthdate) && DateTime.TryParse(birthdate, out DateTime dt_birthdate)) 
				filter &= builder.Eq(b => b.Birthdate, dt_birthdate);

			if (!string.IsNullOrWhiteSpace(itin)) filter &= builder.Eq(b => b.Itin, itin);
			if (!string.IsNullOrWhiteSpace(passport)) filter &= builder.Eq(b => b.Passport, passport);
			if (!string.IsNullOrWhiteSpace(mobile_phone)) filter &= builder.Eq(b => b.MobilePhone, mobile_phone);

			//if (!string.IsNullOrWhiteSpace(lastName)) filter &= builder.Regex(b => b.LastName, new BsonRegularExpression(lastName, "i"));

			return filter;
		}

		public async Task<DataPage<CustomerV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
		{
			var result = await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
			var data = result.Data.ConvertAll(ToPublic);

			return new DataPage<CustomerV1>
			{
			    Data = data,
			    Total = result.Total
			};
		}

		public async Task<CustomerV1> GetByIdAsync(string correlationId, string id)
		{
			var result = await GetOneByIdAsync(correlationId, id);

			return ToPublic(result);
		}
		
		public async Task<CustomerV1> CreateAsync(string correlationId, CustomerV1 customer)
		{
			customer.Id = customer.Id ?? IdGenerator.NextLong();
			customer.Gender = customer.Gender ?? CustomerGenderV1.Unknown;

			var result = await CreateAsync(correlationId, FromPublic(customer));

			return ToPublic(result);
		}

		public async Task<CustomerV1> UpdateAsync(string correlationId, CustomerV1 customer)
		{
			customer.Gender = customer.Gender ?? CustomerGenderV1.Unknown;

			var result = await UpdateAsync(correlationId, FromPublic(customer));

			return ToPublic(result);
		}

		public new async Task<CustomerV1> DeleteByIdAsync(string correlationId, string id)
		{
			var result = await base.DeleteByIdAsync(correlationId, id);

			return ToPublic(result);
		}

		private static CustomerV1 ToPublic(CustomerMongoDbSchema value)
		{
			return value == null ? null : ObjectMapper.MapTo<CustomerV1>(value);
		}

		private static CustomerMongoDbSchema FromPublic(CustomerV1 value)
		{
			return ObjectMapper.MapTo<CustomerMongoDbSchema>(value);
		}
	}
}
