using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpiceProject.Data;
using SpiceProject.Models;
using SpiceProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //       private readonly ILogger<HomeController> _logger;

        //     public HomeController(ILogger<HomeController> logger)
        //   {
        //     _logger = logger;
        //        }

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;

        }



        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVm = new IndexViewModel()
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.IsActive == true).ToListAsync()  // so if the isactive field is true, it will then only display 
            };
            return View(IndexVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
