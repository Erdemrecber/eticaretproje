using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eticaret.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }  

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        
        public decimal TotalPrice { get; set; }

        
        public List<OrderItem> OrderItems { get; set; }
    }
}

