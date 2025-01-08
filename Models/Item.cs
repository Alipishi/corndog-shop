namespace HelmonyCornDog.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }

        //Navigation Property
        public Product Product { get; set; }


    }
}
