using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YourNamespace.Controllers;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Validators;
using YourNamespace.Services;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockProductService.Object);
    }

    [Fact]
    public async Task AddProduct_ValidProduct_ReturnsCreatedResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
        _mockProductService.Setup(s => s.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(product);

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(product, createdResult.Value);
    }

    [Fact]
    public async Task AddProduct_InvalidProduct_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.AddProduct(new Product());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditProduct_ValidId_ReturnsNoContent()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Updated Product", Price = 150 };
        _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                           .ReturnsAsync(true);

        // Act
        var result = await _controller.EditProduct(1, product);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task EditProduct_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                           .ReturnsAsync(false);

        // Act
        var result = await _controller.EditProduct(99, new Product());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ValidId_ReturnsNoContent()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteProductAsync(It.IsAny<int>()))
                           .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteProductAsync(It.IsAny<int>()))
                           .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteProduct(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProduct_ValidId_ReturnsOkResultWithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
        _mockProductService.Setup(s => s.GetProductByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetProduct_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProduct(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SearchProducts_ValidQuery_ReturnsOkResultWithProducts()
    {
        // Arrange
        var products = new[] { new Product { Id = 1, Name = "Product 1" }, new Product { Id = 2, Name = "Product 2" } };
        _mockProductService.Setup(s => s.SearchProductsAsync(It.IsAny<string>()))
                           .ReturnsAsync(products);

        // Act
        var result = await _controller.SearchProducts("Product");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task SearchProducts_InvalidQuery_ReturnsEmptyList()
    {
        // Arrange
        _mockProductService.Setup(s => s.SearchProductsAsync(It.IsAny<string>()))
                           .ReturnsAsync(new Product[0]);

        // Act
        var result = await _controller.SearchProducts("NonExistingProduct");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Empty((Product[])okResult.Value);
    }
}
