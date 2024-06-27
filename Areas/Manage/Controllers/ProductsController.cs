using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboris.Areas.Manage.ViewModels.Product;
using Laboris.DAL;
using Laboris.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laboris.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class ProductsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(AppDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            List<Products> products = await _dbContext.Products.Include(p => p.Category)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag).ToListAsync();

            return View(products);
        }

    


        public async Task<IActionResult> Create()
        {

            ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVm productCreateVm)
        {

            ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();

            if (!ModelState.IsValid) return View(productCreateVm);

            bool resultcategory = await _dbContext.ProductsCategories.AnyAsync(c =>c.Id == productCreateVm.CategoryId);

            if (!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category yoxdur");
                return View();
            }


            ///
            if (!productCreateVm.ImgFile.ContentType.Contains("image/")) return View();

            string path = _hostEnvironment.WebRootPath + @"/upload/products/";

            string fileame = Guid.NewGuid().ToString() + productCreateVm.ImgFile.FileName;

            using (FileStream stream = new FileStream(Path.Combine(path, fileame), FileMode.Create))
            {
                await productCreateVm.ImgFile.CopyToAsync(stream);
            }

            Products product = new()
            {
                Name = productCreateVm.Name,
                OldPrice = productCreateVm.OldPrice,
                Percent = productCreateVm.Percent,
                Price = productCreateVm.Price,
                Color=productCreateVm.Color,
                Size=productCreateVm.Size,
                CategoryId=productCreateVm.CategoryId

            };

            product.PhotoUrl = fileame;

            ///

            foreach (var tagId in productCreateVm.TagIds)
            {
                ProductTag productTag = new()
                {
                    Product = product,
                    TagId = tagId
                };
                product.ProductTags.Add(productTag);
            }


            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Update(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            //
            ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();
            //

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductUpdateVm
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Color = product.Color,
                Size = product.Size,
                Percent = product.Percent,
                CategoryId = product.CategoryId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVm ProductUpdateVm)
        {
            //
            ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();

            //

            if (!ModelState.IsValid)
            {
                return View(ProductUpdateVm);
            }

            var product = await _dbContext.Products.FindAsync(ProductUpdateVm.Id);
            if (product == null)
            {
                return NotFound();
            }

            if (ProductUpdateVm.ImgFile != null)
            {
                
                string path = Path.Combine(_hostEnvironment.WebRootPath, "upload/products");
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductUpdateVm.ImgFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await ProductUpdateVm.ImgFile.CopyToAsync(fileStream);
                }
                product.PhotoUrl = fileName;
            }

            product.Name = ProductUpdateVm.Name;
            product.Price = ProductUpdateVm.Price;
            product.OldPrice = ProductUpdateVm.OldPrice;
            product.Size= ProductUpdateVm.Size;
            product.Color = ProductUpdateVm.Color;
            product.CategoryId = ProductUpdateVm.CategoryId;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }




        public IActionResult Delete(int id)
        {
            var products = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            _dbContext.Products.Remove(products);
            _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

