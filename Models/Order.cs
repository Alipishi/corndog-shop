using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelmonyCornDog.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CreatDate { get; set; }
        public bool IsFinally { get; set; }

        // Navigation Property

        [ForeignKey("UserId")]
        public Users Users { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }



    }
}
