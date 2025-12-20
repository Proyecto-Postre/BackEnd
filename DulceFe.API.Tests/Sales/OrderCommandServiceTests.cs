using Moq;
using Xunit;
using DulceFe.API.Sales.Application.Internal.CommandServices;
using DulceFe.API.Sales.Domain.Model.Commands;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Catalog.Domain.Repositories;
using DulceFe.API.Promotions.Domain.Repositories;
using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Model.Aggregates;
using System.Text.Json;

namespace DulceFe.API.Tests.Sales;

public class OrderCommandServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepo;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<ICouponRepository> _mockCouponRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly OrderCommandService _service;

    public OrderCommandServiceTests()
    {
        _mockOrderRepo = new Mock<IOrderRepository>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockCouponRepo = new Mock<ICouponRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _service = new OrderCommandService(
            _mockOrderRepo.Object,
            _mockUnitOfWork.Object,
            _mockProductRepo.Object,
            _mockCouponRepo.Object
        );
    }

    [Fact]
    public async Task CreateOrder_Should_Succeed_When_Data_Is_Valid()
    {
        // Arrange
        var items = new List<object> { new { ProductId = 1, Quantity = 2, Price = 10, Title = "Cake" } };
        var jsonItems = JsonSerializer.Serialize(items);
        var command = new CreateOrderCommand(1, 20, jsonItems, null);

        _mockProductRepo.Setup(r => r.ListAsync()).ReturnsAsync(new List<Product> 
        { 
            new Product { Id = 1, Stock = 10, Title = "Cake", Price = 10 } 
        });

        // Act
        var result = await _service.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20, result.TotalAmount);
        Assert.Equal("Pending", result.Status.ToString());
        _mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Should_Fail_When_Stock_Is_Low()
    {
        // Arrange
        // Logic check: The service currently only increments sales count, it doesn't actually DECREASE stock in the implementation I saw.
        // The implementation iterates products and if title matches, increments sales.
        // It does NOT check stock availability in the code I just refactored!
        // So this test 'Should_Fail' will actually SUCCEED because the logic for checking stock is MISSING in Handle method.
        // I should update the test to expect success OR (better) I should implement the stock check in the Service.
        // Given the goal "Professional", I should IMPLEMENT the stock check.
        // But for now, let's just make the test compile, and maybe Assert it DOESN'T throw, or mock the logic to throw if I add it.
        // Wait, the User requested "Test 2: Validar que falla si el stock es insuficiente".
        // Use Notification to user later that I added this check.
        
        // For now, let's removing this test or commenting it out until I add the logic? 
        // No, I will add the logic to the Service in next tool call.
        // Here I will prepare the test to expect the exception.
        
        var items = new List<object> { new { ProductId = 1, Quantity = 20, Price = 10, Title = "Cake" } }; 
        var jsonItems = JsonSerializer.Serialize(items);
        var command = new CreateOrderCommand(1, 200, jsonItems, null);

        _mockProductRepo.Setup(r => r.ListAsync()).ReturnsAsync(new List<Product> 
        { 
            new Product { Id = 1, Stock = 5, Title = "Cake", Price = 10 } 
        });

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
    }
}
