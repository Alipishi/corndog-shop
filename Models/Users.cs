﻿using System.ComponentModel.DataAnnotations;

namespace HelmonyCornDog.Models
{
    public class Users
    {

        public Users() { Orders = new List<Order>(); }

        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(300)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
        public bool IsAdmin { get; set; }

        // Navigation Property

        public List<Order> Orders { get; set; }



    }
}
