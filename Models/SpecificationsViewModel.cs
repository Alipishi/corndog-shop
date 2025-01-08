using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelmonyCornDog.Models
{
    public class SpecificationsViewModel
    {
        
        public int Id { get; set; }
        public string weight { get; set; }
        public string Combinations { get; set; }
        public string cookTime { get; set; }
        public string Otherexplanations { get; set; }

        //Navigation Property
        [ForeignKey("Id")]
        public Product Product { get; set; }




    }
}
