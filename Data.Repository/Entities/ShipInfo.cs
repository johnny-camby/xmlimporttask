using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Repository.Entities
{
    public class ShipInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ShipInfoId { get; set; }
		public int ShipVia { get; set; }
		public double Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipCity { get; set; }
		public string ShipRegion { get; set; }
		public int ShipPostalCode { get; set; }
		public string ShipCountry { get; set; }
		public DateTime ShippedDate { get; set; }
	}
}
