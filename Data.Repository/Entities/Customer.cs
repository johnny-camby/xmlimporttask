using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Repository.Entities
{
    public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string CustomerID { get; set; }
		public string CompanyName { get; set; }
		public string ContactName { get; set; }
		public string ContactTitle { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public Guid FullAddressId { get; set; }
		public FullAddress FullAddress { get; set; }
	}
}
