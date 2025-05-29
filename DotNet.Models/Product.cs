using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(20, ErrorMessage = "Brand name cannot exceed 20 characters")]
        public string Brand { get; set; } = "Unknown";

        [Range(1, 5, ErrorMessage = "Rating must be between 1 to 5 only")]
        public decimal Rating { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Ratings must be a positive value")]
        public int RatingNos { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ValidateNever]
        [DisplayName("Product Image File")]
        public string? ImageUrl { get; set; }
    }
}