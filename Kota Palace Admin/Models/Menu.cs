using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class Menu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public string Url { get; set; }
        public double Discount { get; set; }

        public bool Status { get; set; }
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public ICollection<Extras> Extras { get; set; } //
        public int IsDeleted { get; set; } = 0;
    }

    public class Extras
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MenuId { get; set; }
        public int IsDeleted { get; set; } = 0;
    }
}
