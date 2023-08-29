using Kota_Palace_Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Models
{
    public class Cart
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(AppUsers))]
        public string Customer_Id { get; set; }
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }

        //cartiems
        public int Quantity { get; set; } //
        public string ItemName { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }

        //cart extras
        public string Extras { get; set; }


    }

    //public class CartItem
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }
    //    public  int Quantity { get; set; } //
    //    public string ItemName { get; set; }
    //    public int CartId { set; get; }//product id
    //    public string Note { get; set; }
    //    public decimal Price { get; set; }
    //    public virtual ICollection<CartItemsExtras> Extras { get; set; }
    //}

    //public class CartItemsExtras
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { set; get; }
    //    public string Title { get; set; }
    //    [ForeignKey("CartItem")]
    //    public int CartItemId { get; set; }
    //    public virtual CartItem CartItem { get; set; }
    //}
}
