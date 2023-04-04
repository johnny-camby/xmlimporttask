using System;

namespace BusinessLogic.Dtos
{
    public class FullAddressDto
	{
		public Guid FullAddressId { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public int PostalCode { get; set; }
		public string Country { get; set; }
	}
}
