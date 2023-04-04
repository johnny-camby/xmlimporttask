using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Repository.Entities
{
    public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string CustomerID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime RequiredDate { get; set; }
		public Guid ShipInfoId { get; set; }
		public ShipInfo ShipInfo { get; set; }
	}
}
