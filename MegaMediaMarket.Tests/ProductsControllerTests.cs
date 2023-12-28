using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace MegaMediaMarket.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController controller;
        private readonly Mock<StoreDbContext> mockContext;
        private readonly Mock<ShoppingCartService> mockShoppingCartService;
        private readonly Mock<IWebHostEnvironment> mockWebHostEnvironment;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly StoreDbContext context;
        private readonly ShoppingCartService shoppingCartService;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly Mock<ISession> mockSession;


        public ProductsControllerTests()
        {
            controller = new ProductsController(mockContext.Object, mockWebHostEnvironment.Object, mockShoppingCartService.Object);
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            context = new StoreDbContext(options);

            var productId = Guid.Parse("0fd738c0-1403-4908-9440-225216f9080c");
            context.Products.Add(new Product
            {
                Id = productId,
                Name = "Example Product",
                Link = "https://example.com/product",
                Description = "This is an example product description.",
                Price = 999,
                Platform = "Example Platform",
                CardImagePath = "path/to/card-image.jpg",
                BannerImagePath = "path/to/banner-image.jpg",
                ScrImagePath = "path/to/scr-image.jpg",
                ScrImageAddPath = "path/to/scr-image-add.jpg"
            });
            context.SaveChanges();

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockSession = new Mock<ISession>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext.Session).Returns(mockSession.Object);
            shoppingCartService = new ShoppingCartService(mockHttpContextAccessor.Object);

            controller = new ProductsController(context, _appEnvironment, shoppingCartService);
        }

        [Fact]
        public void AddToCart_AddsItemAndRedirectsToIndex()
        {
            
            var productId = Guid.Parse("0fd738c0-1403-4908-9440-225216f9080c");

            
            var result = controller.AddToCart(productId, 1);

            
            mockSession.Verify(s => s.SetString(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Search_WithNonEmptyString_ReturnsFilteredProducts()
        {
            
            var searchString = "Test";
            var products = new List<Product>
    {
        new Product { Name = "Test Product 1" },
        new Product { Name = "Unrelated" }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.Search(searchString);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Add_ReturnsViewResult()
        {
            
            var result = controller.Add();

            
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Delete_RemovesProductAndRedirectsToIndex()
        {
            
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            mockContext.Setup(c => c.Products.FindAsync(productId)).ReturnsAsync(product);

            
            var result = await controller.Delete(new UpdateProductViewModel { Id = productId });

            
            mockContext.Verify(c => c.Products.Remove(product), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task ViewAll_ReturnsViewResultWithAllProducts()
        {
            
            var products = new List<Product> { new Product(), new Product() }.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.ViewAll();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count()); // Assuming there are 2 products
        }

        [Fact]
        public async Task UpdateGet_ExistingProduct_ReturnsViewWithModel()
        {
            
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                   .ReturnsAsync(product);
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.Update(productId);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UpdateProductViewModel>(viewResult.Model);
            Assert.Equal(productId, model.Id);
            Assert.Equal("Test Product", model.Name);
        }


        [Fact]
        public async Task UpdateGet_NonExistingProduct_ReturnsNotFound()
        {
            
            var productId = Guid.NewGuid();
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                   .ReturnsAsync((Product)null);
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.Update(productId);

            
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Details_ExistingProduct_ReturnsViewWithModel()
        {
            
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            mockContext.Setup(c => c.Products.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                       .ReturnsAsync(product);

            
            var result = await controller.Details(productId);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Product>(viewResult.Model);
            Assert.Equal(productId, model.Id);
            Assert.Equal("Test Product", model.Name);
        }


        [Fact]
        public async Task Details_NonExistingProduct_ReturnsNotFound()
        {
            
            var productId = Guid.NewGuid();
            mockContext.Setup(c => c.Products.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                       .ReturnsAsync((Product)null);

            var result = await controller.Details(productId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
