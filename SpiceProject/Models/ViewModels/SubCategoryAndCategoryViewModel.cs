using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {

        public IEnumerable<Category> CategoryList { get; set; } // added this because we needed a list of Categories, Note in the create View page, the Category is a list
        public SubCategory SubCategory { get; set; }    // needed a subcategory. Note in the Create View page, the subcategory is typed and create it
        public List<string> SubCategoryList { get; set; }  // on the right side of the subcategories create view page. we needed a list for the subCategory on the right side of the page to display the subcategoies for a category 
        public string StatusMessage { get; set; }   // this was needed for the statusMessage that is shown if a subCategory has already been created in a Category
    }
}
