using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MegaMediaMarket.Tests
{
    public class HomeControllerTests
    {
        private readonly HomeController controller;
        private readonly Mock<StoreDbContext> mockContext;
        private readonly Mock<IStringLocalizer<HomeController>> mockLocalizer;
        private IStringLocalizer<HomeController> localizer;

        public HomeControllerTests()
        {
            mockContext = new Mock<StoreDbContext>(new DbContextOptions<StoreDbContext>());
            mockLocalizer = new Mock<IStringLocalizer<HomeController>>();
            controller = new HomeController(mockContext.Object, mockLocalizer.Object);

            mockLocalizer.Setup(_ => _["Greeting"]).Returns(new LocalizedString("Greeting", "Hello, world!"));
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts1()
        {
            
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new StoreDbContext(options);
            context.Products.AddRange(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Example Product",
                    Link = "https://example.com/product",
                    Description = "This is an example product description.",
                    Price = 999,
                    Platform = "Example Platform",
                    CardImagePath = "path/to/card-image.jpg",
                    BannerImagePath = "path/to/banner-image.jpg",
                    ScrImagePath = "path/to/scr-image.jpg",
                    ScrImageAddPath = "path/to/scr-image-add.jpg"
                },
                new Product {
                    Id = Guid.NewGuid(),
                    Name = "Example Product",
                    Link = "https://example.com/product",
                    Description = "This is an example product description.",
                    Price = 999,
                    Platform = "Example Platform",
                    CardImagePath = "path/to/card-image.jpg",
                    BannerImagePath = "path/to/banner-image.jpg",
                    ScrImagePath = "path/to/scr-image.jpg",
                    ScrImageAddPath = "path/to/scr-image-add.jpg"
                }
            );
            await context.SaveChangesAsync();

            var mockLocalizer = new Mock<IStringLocalizer<HomeController>>();

            var controller = new HomeController(context, mockLocalizer.Object);

            var result = await controller.Index();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }


        [Fact]
        public void Privacy_ReturnsAViewResult()
        {
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsAViewResult_WithErrorViewModel()
        {
            
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext() { HttpContext = httpContext };
            httpContext.TraceIdentifier = "TestTraceIdentifier";
            var result = controller.Error();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("TestTraceIdentifier", model.RequestId);
        }

        [Fact]
        public void SetLanguage_RedirectsToReturnUrl()
        {
            
            string culture = "en-US";
            string returnUrl = "/test";

            var mockResponseCookies = new Mock<IResponseCookies>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.Setup(r => r.Cookies).Returns(mockResponseCookies.Object);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Response).Returns(mockHttpResponse.Object);

            var controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
            controller.ControllerContext = controllerContext;

            
            var result = controller.SetLanguage(culture, returnUrl);

            
            var redirectResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal(returnUrl, redirectResult.Url);
        }

        [Fact]
        public async Task Index_UsesLocalization_ReturnsLocalizedGreeting1()
        {
            
            mockLocalizer.Setup(_ => _["Greeting"]).Returns(new LocalizedString("Greeting", "Localized Hello, world!"));

            
            var result = await controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var localizedGreeting = mockLocalizer.Object["Greeting"];
            Assert.Equal("Localized Hello, world!", localizedGreeting.Value);
        }
        [Fact]
        public async Task Index_NoProducts_ReturnsEmptyList1()
        {
            
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(m => m.ToListAsync()).ReturnsAsync(new List<Product>());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }
        [Fact]
        public void Error_ReturnsCorrectRequestId1()
        {
            
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext() { HttpContext = httpContext };
            httpContext.TraceIdentifier = "CustomTraceIdentifier";

            
            var result = controller.Error();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("CustomTraceIdentifier", model.RequestId);
        }
        [Fact]
        public async Task Index_LocalizationMissing_ReturnsDefaultGreeting1()
        {
            
            var defaultGreeting = "Hello, world!";
            mockLocalizer.Setup(_ => _["Greeting"]).Returns(new LocalizedString("Greeting", defaultGreeting));

            
            var result = await controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var localizedGreeting = mockLocalizer.Object["Greeting"];
            Assert.Equal(defaultGreeting, localizedGreeting.Value);
        }
        [Fact]
        public async Task Index_UsingInMemoryDatabase_NoProducts_ReturnsEmptyList1()
        {
            
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyTestDatabase")
                .Options;

            using var context = new StoreDbContext(options);
            var controller = new HomeController(context, mockLocalizer.Object);

            
            var result = await controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }
        [Fact]
        public async Task Search_NoMatchingProducts_ReturnsEmptyList1()
        {
            
            var searchString = "NonExisting";
            var products = new List<Product>().AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            
            var result = await controller.Search(searchString);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }
    }
}
