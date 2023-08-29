using Kota_Palace_Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
