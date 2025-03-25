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

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}

