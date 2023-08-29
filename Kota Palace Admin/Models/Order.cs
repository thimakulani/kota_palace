using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class Order
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(AppUsers))]
        public string Customer_Id { get; set; }
        public string Status { get; set; }
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        //public string OrderDate { get; set; }
        public DateTime OrderDateUtc { get; set; }
        public string Option { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DriverId { get; set; }

    }
}
