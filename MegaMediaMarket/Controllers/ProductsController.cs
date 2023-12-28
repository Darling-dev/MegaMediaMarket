using SiteASPCOm.Data;
using SiteASPCOm.Models;
using SiteASPCOm.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteASPCOm.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace SiteASPCOm.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        private readonly IWebHostEnvironment _appEnvironment;

        private readonly StoreDbContext storeDbContext;

        public ProductsController(StoreDbContext storeDbContext, IWebHostEnvironment appEnvironment, ShoppingCartService shoppingCartService)
        {
            this.storeDbContext = storeDbContext;
            _appEnvironment = appEnvironment;
            _shoppingCartService = shoppingCartService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(Guid productId, int quantity)
        {
            var product = storeDbContext.Products.Find(productId);
            if (product != null)
            {
                _shoppingCartService.AddItem(product, quantity);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCartService.GetItems();
            return View(items);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid productId)
        {
            _shoppingCartService.RemoveItem(productId);
            return RedirectToAction("ShoppingCart");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await storeDbContext.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var membersQuery = from mem in storeDbContext.Products select mem;

            if (!String.IsNullOrEmpty(searchString))
            {
                membersQuery = membersQuery.Where(m => m.Name.Contains(searchString));
            }

            var memberList = await membersQuery.ToListAsync();
            return View(memberList);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel addProductRequest)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = addProductRequest.Name,
                Link = addProductRequest.Link,
                Description = addProductRequest.Description,
                Price = addProductRequest.Price,
                Platform = addProductRequest.Platform,
            };
            await storeDbContext.Products.AddAsync(product);
            await storeDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AddProductViewModel addProductRequest)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = addProductRequest.Name,
                    Link = addProductRequest.Link,
                    Description = addProductRequest.Description,
                    Price = addProductRequest.Price,
                    Platform = addProductRequest.Platform,
                };

                product.CardImagePath = await SaveFile(addProductRequest.CardImage, "sitefiles", "Categories");
                product.BannerImagePath = await SaveFile(addProductRequest.BannerImage, "sitefiles", "Categories");
                product.ScrImagePath = await SaveFile(addProductRequest.ScrImage, "sitefiles", "Categories");
                product.ScrImageAddPath = await SaveFile(addProductRequest.ScrImageAdd, "sitefiles", "Categories");

                await storeDbContext.Products.AddAsync(product);
                await storeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        private async Task<string> SaveFile(IFormFile file, string folder1, string folder2)
        {
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, folder1, folder2);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return uniqueFileName;
            }

            return null;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var products = await storeDbContext.Products.ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var product = await storeDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                var viewModel = new UpdateProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Link = product.Link,
                    Description = product.Description,
                    Price = product.Price,
                    Platform = product.Platform,
                    CardImagePath = product.CardImagePath,
                    BannerImagePath = product.BannerImagePath,
                    ScrImagePath = product.ScrImagePath,
                    ScrImageAddPath = product.ScrImageAddPath
                };

                return View("View", viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var product = await storeDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new UpdateProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Link = product.Link,
                Description = product.Description,
                Price = product.Price,
                Platform = product.Platform,
                // Assuming you have paths or URLs for images and videos
                CardImagePath = product.CardImagePath,
                BannerImagePath = product.BannerImagePath,
                ScrImagePath = product.ScrImagePath,
                ScrImageAddPath = product.ScrImageAddPath
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await storeDbContext.Products.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (product != null)
                {
                    // Update product details
                    product.Name = model.Name;
                    product.Link = model.Link;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.Platform = model.Platform;
                    // ... other properties ...
                    product.CardImagePath = model.CardImagePath;
                    product.BannerImagePath = model.BannerImagePath;
                    product.ScrImagePath = model.ScrImagePath;
                    product.ScrImageAddPath = model.ScrImageAddPath;

                    // Handle new file uploads
                    if (model.NewCardImage != null)
                    {
                        product.CardImagePath = await SaveFile(model.NewCardImage, "sitefiles", "Categories");
                    }
                    if (model.NewBannerImage != null)
                    {
                        product.BannerImagePath = await SaveFile(model.NewBannerImage, "sitefiles", "Categories");
                    }
                    if (model.NewScrImage != null)
                    {
                        product.ScrImagePath = await SaveFile(model.NewScrImage, "sitefiles", "Categories");
                    }
                    if (model.NewScrImageAdd != null)
                    {
                        product.ScrImageAddPath = await SaveFile(model.NewScrImageAdd, "sitefiles", "Categories");
                    }
                    // Repeat for other images

                    storeDbContext.Update(product);
                    await storeDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProductViewModel model)
        {
            var product = await storeDbContext.Products.FindAsync(model.Id);
            if (product != null)
            {
                storeDbContext.Products.Remove(product);
                await storeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || storeDbContext.Products == null)
            {
                return NotFound();
            }

            var topic = await storeDbContext.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }
    }
}