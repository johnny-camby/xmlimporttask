using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Repository.Entities
{
    public class FullAddress
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid FullAddressId { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public int PostalCode { get; set; }
		public string Country { get; set; }
	}
}
