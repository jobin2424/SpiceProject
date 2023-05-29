using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Category Name")] // with this, the label will change to Category Name when using the asp-for tag helper
        [Required]
        public String Name { get; set; }




    }
}
