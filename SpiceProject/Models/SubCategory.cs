using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
    [Display(Name = "SubCategory Name")]
       [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]  // this says that the categoryId is the foreignkey and it references the Category Model or table 
        public virtual Category Category { get; set; }
    }
}
