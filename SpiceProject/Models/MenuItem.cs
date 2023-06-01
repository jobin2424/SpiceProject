using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        public String SpiceLevel { get; set; }
        // an enum is a collection thats static, could of also just made a list 
        public enum ESpicy { NA = 0, NotSpicy = 1, Spicy = 2, VerySpicy = 3 }

        public string Image { get; set; }   // this is to add the image

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } // this to connect categoryId

        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; } // this to connect subcategoryId

        [Range(1, int.MaxValue, ErrorMessage ="Price should be greater than ${1}")]
        public double Price { get; set; }
    }
}
