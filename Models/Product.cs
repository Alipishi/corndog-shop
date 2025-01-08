using System.ComponentModel.DataAnnotations.Schema;

namespace HelmonyCornDog.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public string ImagePath { get; set; }

        //Navigation Property
        public Item Item { get; set; }
        public ICollection<categoryToProduct> CategoryToProducts { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public SpecificationsViewModel Specifications { get; set; }







    }
}
