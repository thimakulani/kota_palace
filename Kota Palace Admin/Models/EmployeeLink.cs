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
        public virtual AppUsers Employee { get; set; }
        [ForeignKey(nameof(Models.Business))]
        public int BusinessId { get; set; }
        public virtual Business Business { get; set; }
        public string Status { get; set; }
    }
}
