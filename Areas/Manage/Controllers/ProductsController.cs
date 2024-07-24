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
                .Include(pt => pt.ProductTags).ThenInclude(pt => pt.Tag).ToListAsync();


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

            bool resultcategory = await _dbContext.ProductsCategories.AnyAsync(c => c.Id == productCreateVm.CategoryId);

            if (!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category yoxdur");
                return View(productCreateVm);
            }

            if (!productCreateVm.ImgFile.ContentType.Contains("image/")) return View(productCreateVm);

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
                Color = productCreateVm.Color,
                Size = productCreateVm.Size,
                Description=productCreateVm.Description,
                CategoryId = productCreateVm.CategoryId,
                ProductTags = new List<ProductTag>()
            };

            product.PhotoUrl = fileame;

            foreach (var tagId in productCreateVm.TagIds)
            {
                var tag = await _dbContext.Tags.FindAsync(tagId);

                if (tag == null)
                {
                    continue; 
                }

                ProductTag productTag = new()
                {
                    Product = product,
                    TagId = tagId,
                    Name = tag.Name
                };

                product.ProductTags.Add(productTag);
            }

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _dbContext.Products.Include(p => p.ProductTags)
                                                   .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();

            var viewModel = new ProductUpdateVm
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                OldPrice = product.OldPrice,
                Color = product.Color,
                Size = product.Size,
                CategoryId = product.CategoryId,
                TagIds = product.ProductTags.Select(pt => pt.TagId).ToList(),
                Description = product.Description
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVm productUpdateVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _dbContext.ProductsCategories.ToListAsync();
                ViewBag.Tags = await _dbContext.Tags.ToListAsync();
                return View(productUpdateVm);
            }

            var product = await _dbContext.Products.Include(p => p.ProductTags)
                                                   .FirstOrDefaultAsync(p => p.Id == productUpdateVm.Id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productUpdateVm.Name;
            product.Price = productUpdateVm.Price;
            product.OldPrice = productUpdateVm.OldPrice;
            product.Color = productUpdateVm.Color;
            product.Size = productUpdateVm.Size;
            product.CategoryId = productUpdateVm.CategoryId;
            product.Description = productUpdateVm.Description;

            // Fotoğraf güncelleme işlemi
            if (productUpdateVm.ImgFile != null && productUpdateVm.ImgFile.ContentType.Contains("image/"))
            {
                string path = _hostEnvironment.WebRootPath + @"/upload/products/";

                // Eski fotoğraf dosyasını sil
                if (!string.IsNullOrEmpty(product.PhotoUrl))
                {
                    string oldFilePath = Path.Combine(path, product.PhotoUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string fileame = Guid.NewGuid().ToString() + productUpdateVm.ImgFile.FileName;

                using (FileStream stream = new FileStream(Path.Combine(path, fileame), FileMode.Create))
                {
                    await productUpdateVm.ImgFile.CopyToAsync(stream);
                }

                product.PhotoUrl = fileame;
            }

           
            _dbContext.ProductTags.RemoveRange(product.ProductTags);

            foreach (var tagId in productUpdateVm.TagIds)
            {
                var tag = await _dbContext.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    product.ProductTags.Add(new ProductTag
                    {
                        ProductId = product.Id,
                        TagId = tagId,
                        Name = tag.Name
                    });
                }
            }

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

