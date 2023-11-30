using Moq;
using Microsoft.Extensions.Logging;
using RollDiceWebAPISimulation.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RollDiceWebAPISimulation.Data;
using RollDiceWebAPISimulation.Models;
using RollDiceWebAPISimulation.Services;

namespace RollDiceSimulationTests
{
    public class RollDiceControllerTests
    {
        private readonly Mock<IDiceRollerService> mockService;
        private readonly Mock<ILogger<RollDiceController>> mockLogger;
        private readonly DbContextOptions<RollDiceDbContext> dbContextOptions;

        public RollDiceControllerTests()
        {
            mockService = new Mock<IDiceRollerService>();
            mockLogger = new Mock<ILogger<RollDiceController>>();
            dbContextOptions = new DbContextOptionsBuilder<RollDiceDbContext>().UseInMemoryDatabase(databaseName: "RollDiceDb").Options;
        }

        private RollDiceController CreateController()
        {
            var dbContext = new RollDiceDbContext(dbContextOptions);
            return new RollDiceController(mockService.Object, dbContext, mockLogger.Object);
        }

        [Fact]
        public async Task ShowRolls_ReturnsOkResult_WithDiceRolls()
        {
            // Arrange
            var controller = CreateController();
            using (var dbContext = new RollDiceDbContext(dbContextOptions))
            {
                dbContext.DiceRolls?.Add(new DiceRoll(2)); // Add test data
                dbContext.SaveChanges();
            }

            // Act
            var result = await controller.ShowRolls();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<DiceRoll>>(okResult.Value);
        }

        [Fact]
        public async Task Roll_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = CreateController();
            var settings = new DiceSettings { FactorDie1 = 2, FactorDie2 = 2, FavoredFaceDie1 = 3, FavoredFaceDie2 = 4, NumberOfRolls = 3};

            mockService.Setup(s => s.RollDiceAsync(settings)).ReturnsAsync(new List<DiceRoll>()); // Mock service response

            // Act
            var result = await controller.Roll(settings);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
