using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboris.Areas.Manage.ViewModels.Blog;
using Laboris.DAL;
using Laboris.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Laboris.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(AppDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
           
            List<Blog> blog = await _dbContext.Blogs.Include(p => p.Category).ToListAsync();
            return View(blog);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _dbContext.BlogCategories.ToListAsync();
            return View();
        }




        [HttpPost]

        public async Task<IActionResult> Create(CreateBlogVm createBlogVm)
        {
            ViewBag.Categories = await _dbContext.BlogCategories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool resultCategory = await _dbContext.BlogCategories.AnyAsync(c => c.Id == createBlogVm.CategoryId);

            if (!resultCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir Category yoxdur");
                return View();
            }

            Blog blog = new Blog()
            {
                Header=createBlogVm.Header,
                Description=createBlogVm.Description,
                Admin = createBlogVm.Admin,
                Date = createBlogVm.Date,
                CategoryId=createBlogVm.CategoryId,
          

            };


            //elave

            if (!createBlogVm.ImgFile.ContentType.Contains("image/")) return View(blog);

            string path = _hostEnvironment.WebRootPath + @"/upload/blogs/";

            string fileame = Guid.NewGuid().ToString() + createBlogVm.ImgFile.FileName;

            using (FileStream stream = new FileStream(Path.Combine(path, fileame), FileMode.Create))
            {
                await createBlogVm.ImgFile.CopyToAsync(stream);
            }

            blog.PhotoUrl = fileame;

            //



            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _dbContext.BlogCategories.ToListAsync();

            var blogs = _dbContext.Blogs.Where(p => p.Id == id).FirstOrDefault();

            if (blogs is null)
            {
                return View("Error");
            }

            UpdateBlogVm updateBlog = new UpdateBlogVm()
            {
                Id=id,
                Header = blogs.Header,
                Description=blogs.Description,
                Admin=blogs.Admin,
                Date=blogs.Date,
                CategoryId=blogs.CategoryId
            };


            return View(blogs);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogVm updateBlogVm)
        {
            ViewBag.Categories = await _dbContext.BlogCategories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(updateBlogVm);
            }

            var existingBlog = await _dbContext.Blogs.FirstOrDefaultAsync(p => p.Id == updateBlogVm.Id);

            if (existingBlog == null)
            {
                return View("Error");
            }

            var resultCategory = await _dbContext.BlogCategories.AnyAsync(c => c.Id == updateBlogVm.CategoryId);

            if (!resultCategory)
            {
                ModelState.AddModelError("CategoryId", "Bu kategori ID'si mevcut değil.");
                return View(updateBlogVm);
            }

            existingBlog.Admin = updateBlogVm.Admin;
            existingBlog.Date = updateBlogVm.Date;
            existingBlog.Description = updateBlogVm.Description;
            existingBlog.CategoryId = updateBlogVm.CategoryId;



            //elave


            if (updateBlogVm.ImgFile != null)
            {
                string path = _hostEnvironment.WebRootPath + @"/upload/blogs/";
                FileInfo fileInfo = new FileInfo(path + existingBlog.PhotoUrl);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                string filename = Guid.NewGuid() + updateBlogVm.ImgFile.FileName;
                using (FileStream stream = new FileStream(path + filename, FileMode.Create))
                {
                    updateBlogVm.ImgFile.CopyTo(stream);
                }

                existingBlog.PhotoUrl = filename;
            }

            //

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)
        {
            var blogs = _dbContext.Blogs.FirstOrDefault(p => p.Id == id);

            if(blogs is null)
            {
                return View("Error");
            }

            _dbContext.Blogs.Remove(blogs);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}

