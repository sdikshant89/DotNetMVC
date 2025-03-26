using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet.Models
{
	public class Product
	{
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive value, greater than 1$")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
        public string Description { get; set; }
    }
}