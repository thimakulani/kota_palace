using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models 
{
    public class AppUsers : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserType { get; set; }
        public string Url { get; set; }
    }
}
