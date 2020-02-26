using MongoDB.Bson.Serialization.Attributes;
using PipServices3.Commons.Data;
using System;

namespace EIC.Customers.Persistence.MongoDb
{
	[BsonIgnoreExtraElements]
	[BsonNoId]
	public class CustomerMongoDbSchema : IStringIdentifiable
	{
		[BsonElement("id")]
		public string Id { get; set; }

		[BsonElement("last_name")]
		public string LastName { get; set; }

		[BsonElement("first_name")]
		public string FirstName { get; set; }

		[BsonElement("middle_name")]
		public string MiddleName { get; set; }

		[BsonElement("gender")]
		public string Gender { get; set; }

		[BsonElement("birthdate")]
		public DateTime Birthdate { get; set; }

		[BsonElement("itin")]
		public string Itin { get; set; }

		[BsonElement("passport")]
		public string Passport { get; set; }

		[BsonElement("mobile_phone")]
		public string MobilePhone { get; set; }
	}
}
