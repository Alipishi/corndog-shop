namespace HelmonyCornDog.Models
{
    public class categoryToProduct
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        //Navigation Property

        public Category Category { get; set; }
        public Product Product { get; set; }

    }
}
