using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpiceProject.Data;
using SpiceProject.Models;
using SpiceProject.Models.ViewModels;
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

        [TempData]  // this is given by microsft and allows us to pass temp data which we use for a error message
        public string StatusMessage { get; set; }

        public object SubCategoryAndCategoryViewModel { get; private set; }

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


        //GET - CREATE
        public async Task<IActionResult> Create()   
        {
            
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()    // this is an instance of the SubCategoryAndCategoryViewModel found in our Models Folder and inside the ViewModels Folder
            {                                                                               // we get the lists and fields from this model
                CategoryList = await _db.Category.ToListAsync(),    // note the comma, also note list was called for this in the view model
                SubCategory = new Models.SubCategory(), // see viewmodel for method, when creating an instnce or a model that is inside a model, have to add Modles. This is because we are calling the subCategory inside of the SubCategoryAndCategoryViewModel 
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()    // this gives the subcategoryList ordered by the category
            };
            return View(model);  // we called the instance of the subCategoryAndCategoryViewModel model
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
                if (ModelState.IsValid)
                  {
                    var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId); // include method is eager loading, we then used the Where clause to soecify the name to a sub category
                    
                    if(doesSubCategoryExists.Count() > 0)   // if there is a sub category already inside of the category, this erorr will run
                    {
                    //Error, if a subcategory exists in a category, this error coed will run and the it will the page will be reloaded to enter again
                    //had to pass in tempData (field shown above) to execute and call called it Status Message

                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + " category. Please use another name.";
                
                } 
                    else //if there isnt a sub category in the category, it will add to the database and redirect to the database
                    {
                        _db.SubCategory.Add(model.SubCategory);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel() // if the modelState is not valid, we have to return back to the view and create a new view model. Because of this we have tor retrieve everything and find it using the code below
            {
                CategoryList = await _db.Category.ToArrayAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage   // remember this is given by microsft, called field at top of page. There is a partial for statusmessage given my microsoft also under identy/pages/account/_statusMessage
                };                                                                                                  // we copied the partial from identity and passed it into our shared folder so we can consume in the UI

                return View(modelVM);  // need to reload page with the same category.  if you try to create a sub category in a category and the subcategory already exists, it will go into the error if block above, THEN IT WILL get all the information for the category and bring you back to the viewmodel page to re-enter
            
        }

        [ActionName("GetSubCategory")]  // on the create page for the sub categories, this is how we populate the existing subcategories on the right, by passing a json object 
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from SubCategory in _db.SubCategory
                             where SubCategory.CategoryId == id
                             select SubCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id); // could of also used the where clause

            if (subCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()    // this is an instance of the SubCategoryAndCategoryViewModel found in our Models Folder and inside the ViewModels Folder
            {                                                                               // we get the lists and fields from this model
                CategoryList = await _db.Category.ToListAsync(),    // note the comma, also note list was called for this in the view model
                SubCategory = subCategory, // passed the subcategory that was found above 
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()    // this gives the subcategoryList ordered by the category
            };
            return View(model);  // we called the instance of the subCategoryAndCategoryViewModel model
        }


        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId); // include method is eager loading, we then used the Where clause to soecify the name to a sub category

                if (doesSubCategoryExists.Count() > 0)   // if there is a sub category already inside of the category, this erorr will run
                {
                    //Error, if a subcategory exists in a category, this error coed will run and the it will the page will be reloaded to enter again
                    //had to pass in tempData (field shown above) to execute and call called it Status Message

                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + " category. Please use another name.";

                }
                else //if there isnt a sub category in the category, it will add to the database and redirect to the database
                {
                    var subCatFromDb = await _db.SubCategory.FindAsync(model.SubCategory.Id); // we passed it from the edit page on line 19
                    subCatFromDb.Name = model.SubCategory.Name; // we updated a different way instead of used _db, this will update only cetain properties instead of all

                 
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel() // if the modelState is not valid, we have to return back to the view and create a new view model. Because of this we have tor retrieve everything and find it using the code below
            {
                CategoryList = await _db.Category.ToArrayAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage   // remember this is given by microsft, called field at top of page. There is a partial for statusmessage given my microsoft also under identy/pages/account/_statusMessage
            };                                                                                                  // we copied the partial from identity and passed it into our shared folder so we can consume in the UI
            return View(modelVM);  // need to reload page with the same category.  if you try to create a sub category in a category and the subcategory already exists, it will go into the error if block above, THEN IT WILL get all the information for the category and bring you back to the viewmodel page to re-enter

        }

        // GET DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if(subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        //GET DELETE 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                {
                    return NotFound();
                }
            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if(subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        //POST Delete 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);
            _db.SubCategory.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
