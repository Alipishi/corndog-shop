﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelmonyCornDog.Models
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
        
        


        // Navigation Property

        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }





    }
}
