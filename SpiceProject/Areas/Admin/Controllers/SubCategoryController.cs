using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpiceProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }


        // GET INDEX 
        public async Task<IActionResult> Index()
        {
            // with subcategories we want to display the subcategory name, thats why we used Eager loading
            // Eager loading means that when we load the SUBCATEGORY, it will pre-fill the category inside aswell
            //if you go to subCategory controller, we referenced the foreign key to the category model 
            var subCategories = await _db.SubCategory.Include(s => s.Category).ToListAsync();
            return View(subCategories);

        }
    }
}
