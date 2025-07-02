using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Services.Interfaces;
using TrainComponentManagement.PL.Controllers;

namespace TrainComponentManagement.Tests.PresentationLayer
{
    public class ComponentControllerTests
    {
        private readonly Mock<IComponentService> _serviceMock = new();
        private readonly ComponentController _controller;
        private const string TestKey = "test‐key";

        public ComponentControllerTests()
        {
            _controller = new ComponentController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var inDto = TestDataGenerator.NewCreateDto();
            var outDto = TestDataGenerator.NewComponentDto();

            _serviceMock
               .Setup(s => s.CreateAsync(inDto, TestKey))
               .ReturnsAsync(outDto);

            // Act
            var actionResult = await _controller.Create(inDto, TestKey);

            // Assert
            var created = actionResult.Result
                .Should().BeOfType<CreatedAtActionResult>().Which;

            created.ActionName.Should().Be(nameof(_controller.Get));

            var createdValue = created.Value as ComponentDto;
            createdValue.Should().NotBeNull();
            createdValue!.Id.Should().Be(outDto.Id);

            _serviceMock.Verify(s => s.CreateAsync(inDto, TestKey), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithList()
        {
            // Arrange
            var list = Enumerable.Range(1, 5)
                                 .Select(_ => TestDataGenerator.NewComponentDto())
                                 .ToList();
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Result
                  .Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(list);

            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_WhenFound_ShouldReturnOk_WithDto()
        {
            // Arrange
            var dto = TestDataGenerator.NewComponentDto();
            _serviceMock.Setup(s => s.GetAsync(dto.Id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Get(dto.Id);

            // Assert
            result.Result
                  .Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(dto);

            _serviceMock.Verify(s => s.GetAsync(dto.Id), Times.Once);
        }

        [Fact]
        public async Task Update_WhenExists_ShouldReturnNoContent()
        {
            // Arrange
            var id = TestDataGenerator.NewComponentDto().Id;
            var dto = TestDataGenerator.NewCreateDto();

            _serviceMock.Setup(s => s.UpdateAsync(id, dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(id, dto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.UpdateAsync(id, dto), Times.Once);
        }

        [Fact]
        public async Task Update_WhenMissing_ShouldReturnNotFound()
        {
            // Arrange
            var id = TestDataGenerator.NewComponentDto().Id;
            var dto = TestDataGenerator.NewCreateDto();
            _serviceMock.Setup(s => s.UpdateAsync(id, dto))
                        .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Update(id, dto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _serviceMock.Verify(s => s.UpdateAsync(id, dto), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenExists_ShouldReturnNoContent()
        {
            // Arrange
            var id = TestDataGenerator.NewComponentDto().Id;

            _serviceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenMissing_ShouldReturnNotFound()
        {
            // Arrange
            var id = TestDataGenerator.NewComponentDto().Id;
            _serviceMock.Setup(s => s.DeleteAsync(id))
                        .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }
    }
}
