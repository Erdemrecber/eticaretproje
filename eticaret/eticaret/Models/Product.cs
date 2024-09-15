
namespace eticaret.Models
{
    using System.ComponentModel.DataAnnotations;
    
    

    namespace eticaret.Models
    {
        public class Product
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Required]
            public int Stock { get; set; } 

            public string Description { get; set; }

            public string ImageUrl { get; set; }

            public bool IsFeatured { get; set; } 
        }
    }





   
}



