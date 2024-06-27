using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboris.DAL;
using Laboris.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laboris.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class ProductsCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.ProductsCategories.ToListAsync();

            return View(categories);
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductsCategory productsCategory)
        {

            if (!ModelState.IsValid) return View(productsCategory);


            await _context.ProductsCategories.AddAsync(productsCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }



        public IActionResult Update(int id)
        {
            var productsCategory = _context.ProductsCategories.FirstOrDefault(s => s.Id == id);
            if (productsCategory == null)
            {
                return View();
            }

            return View(productsCategory);
        }


        [HttpPost]
        public IActionResult Update(ProductsCategory newproductscategory)
        {
            var oldproductscategory = _context.ProductsCategories.FirstOrDefault(x => x.Id == newproductscategory.Id);
            if (oldproductscategory == null)
            {
                return View();
            }

            oldproductscategory.Name = newproductscategory.Name;


            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var productsCategory = await _context.ProductsCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (productsCategory == null)
            {
                return NotFound();
            }

            _context.ProductsCategories.Remove(productsCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

