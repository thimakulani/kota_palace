using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class UserInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string Url { get; set; }
        [ForeignKey(nameof(Models.Business))]
        public int BusinessId { get; set; }
        public virtual Business Business  { get; set; }
    }
}
