using Kota_Palace_Admin.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class Business
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("TRADING NAME")]
        public string Name { get; set; }
        [DisplayName("DESCRIPTION")]
        public string Description { get; set; }
        [DisplayName("CONTACT NUMBER")]
        public string PhoneNumber { get; set; }
        public string ImgUrl { get; set; }
        [ForeignKey(nameof(AppUsers))]
        public string OwnerId { get; set; }
        [DisplayName("APPLICATION STATUS")]
        public string Status { get; set; }
        public string Online { get; set; }
        public virtual AppUsers Owner { get; set; }
        public virtual Address Address { get; set; }
    }
}
