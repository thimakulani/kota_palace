using Kota_Palace_Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class EmployeeLink
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(AppUsers))]
        public string EmployeeId { get; set; }
        public AppUsers Employee { get; set; }
        [ForeignKey(nameof(Business))]
        public string BusinessId { get; set; }
        public Business Business { get; set; }
        public string Status { get; set; }
    }
}
