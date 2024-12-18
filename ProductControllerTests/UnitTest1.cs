using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductServiceAPI.Controllers;
using Xunit;
using CourseProject.Controllers;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Validators;
using CourseProject.BLL.Services;


public class ProductControllerTests
{
    private readonly Mock<TransportService> _mockTransportService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockTransportService = new Mock<TransportService>();
        _controller = new ProductController(_mockTransportService.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkWithTransports()
    {
        // Arrange
        var transports = new List<Transport> { new Transport { Id = 1, Name = "Transport 1" } };
        _mockTransportService.Setup(s => s.Get()).Returns(transports);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(transports, okResult.Value);
    }

    [Fact]
    public void GetById_ValidId_ReturnsOkWithTransport()
    {
        // Arrange
        var transport = new Transport { Id = 1, Name = "Transport 1" };
        _mockTransportService.Setup(s => s.Get(1)).Returns(transport);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(transport, okResult.Value);
    }

    [Fact]
    public void GetById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockTransportService.Setup(s => s.Get(99)).Throws(new InvalidOperationException("Not found"));

        // Act
        var result = _controller.GetById(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public void GetByName_ValidName_ReturnsOkWithTransport()
    {
        // Arrange
        var transport = new Transport { Id = 1, Name = "Transport 1" };
        _mockTransportService.Setup(s => s.GetTransportName("Transport 1")).Returns(transport);

        // Act
        var result = _controller.GetByName("Transport 1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(transport, okResult.Value);
    }

    [Fact]
    public void GetByName_InvalidName_ReturnsNotFound()
    {
        // Arrange
        _mockTransportService.Setup(s => s.GetTransportName("Invalid"))
                              .Throws(new InvalidOperationException("Not found"));

        // Act
        var result = _controller.GetByName("Invalid");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public void Create_ValidTransport_ReturnsCreatedAtAction()
    {
        // Arrange
        var transport = new Transport { Id = 1, Name = "New Transport" };

        // Act
        var result = _controller.Create(transport);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(transport, createdResult.Value);
    }

    [Fact]
    public void Create_InvalidTransport_ReturnsBadRequest()
    {
        // Arrange
        _mockTransportService.Setup(s => s.CreateTransport(It.IsAny<Transport>()))
                              .Throws(new InvalidOperationException("Invalid transport"));

        // Act
        var result = _controller.Create(new Transport());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid transport", badRequestResult.Value);
    }

    [Fact]
    public void Update_ValidTransport_ReturnsNoContent()
    {
        // Arrange
        var transport = new Transport { Id = 1, Name = "Updated Transport" };

        // Act
        var result = _controller.Update(1, transport);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_InvalidTransport_ReturnsBadRequest()
    {
        // Arrange
        var transport = new Transport { Id = 2, Name = "Mismatch Transport" };

        // Act
        var result = _controller.Update(1, transport);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID in route and body do not match.", badRequestResult.Value);
    }

    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        _mockTransportService.Setup(s => s.DeleteTransport(1)).Verifiable();

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockTransportService.Setup(s => s.DeleteTransport(99))
                              .Throws(new InvalidOperationException("Not found"));

        // Act
        var result = _controller.Delete(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }
}
