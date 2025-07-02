using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Mapping;
using TrainComponentManagement.BLL.Services.Implementation;
using TrainComponentManagement.DAL.Data;
using TrainComponentManagement.DAL.Repositories.Implementation;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TrainComponentManagement.Tests.Services
{

    public class ComponentServiceTests : IDisposable
    {
        private readonly TrainComponentContext _context;
        private readonly ComponentRepository _repo;
        private readonly ComponentService _service;
        private readonly IMapper _mapper;
        private readonly InMemoryIdempotencyService _idem = new();

        public ComponentServiceTests()
        {
            var options = new DbContextOptionsBuilder<TrainComponentContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .ConfigureWarnings(w =>
                                  w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                              .Options;

            _context = new TrainComponentContext(options);
            _context.Database.EnsureCreated();

            _context.Components.RemoveRange(_context.Components);
            _context.SaveChanges();

            _repo = new ComponentRepository(_context);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ComponentProfile>())
                           .CreateMapper();
            _service = new ComponentService(_repo, _mapper, _context, _idem);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndReturnDto()
        {
            // Arrange
            var dto = TestDataGenerator.NewCreateDto();
            var key = Guid.NewGuid().ToString();

            // Act
            var result = await _service.CreateAsync(dto, key);
            var all = await _repo.GetAllAsync();

            // Assert
            all.Should().ContainSingle(c => c.UniqueNumber == result.UniqueNumber);
            (await _idem.ExistsAsync(key)).Should().BeTrue();
            (await _idem.GetResultAsync<ComponentDto>(key))
                .Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task CreateAsync_SameKey_IsIdempotent()
        {
            // Arrange
            var dto = TestDataGenerator.NewCreateDto();
            var key = Guid.NewGuid().ToString();

            // Act
            var first = await _service.CreateAsync(dto, key);
            var second = await _service.CreateAsync(dto, key);
            var all = await _repo.GetAllAsync();

            // Assert
            second.Id.Should().Be(first.Id);
            all.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExisting()
        {
            // Arrange
            var created = await _service.CreateAsync(TestDataGenerator.NewCreateDto(), Guid.NewGuid().ToString());
            var updDto = TestDataGenerator.NewCreateDto();

            // Act
            await _service.UpdateAsync(created.Id, updDto);
            var after = await _repo.GetByIdAsync(created.Id);

            // Assert
            after!.Name.Should().Be(updDto.Name);
            after.UniqueNumber.Should().Be(updDto.UniqueNumber);
        }

        [Fact]
        public void UpdateAsync_NonExisting_ShouldThrow()
        {
            // Arrange
            var nonExistingId = 9999;
            var dto = TestDataGenerator.NewCreateDto();

            // Act
            Func<Task> act = () => _service.UpdateAsync(nonExistingId, dto);

            // Assert
            act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveExisting()
        {
            // Arrange
            var created = await _service.CreateAsync(TestDataGenerator.NewCreateDto(), Guid.NewGuid().ToString());

            // Act
            await _service.DeleteAsync(created.Id);
            var after = await _repo.GetByIdAsync(created.Id);

            // Assert
            after.Should().BeNull();
        }

        [Fact]
        public void DeleteAsync_NonExisting_ShouldThrow()
        {
            // Arrange
            var nonExistingId = 8888;

            // Act
            Func<Task> act = () => _service.DeleteAsync(nonExistingId);

            // Assert
            act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDto()
        {
            // Arrange
            var created = await _service.CreateAsync(TestDataGenerator.NewCreateDto(), Guid.NewGuid().ToString());

            // Act
            var fetched = await _service.GetAsync(created.Id);

            // Assert
            fetched.Should().BeEquivalentTo(created);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAll()
        {
            // Arrange
            await _service.CreateAsync(TestDataGenerator.NewCreateDto(), Guid.NewGuid().ToString());
            await _service.CreateAsync(TestDataGenerator.NewCreateDto(), Guid.NewGuid().ToString());

            // Act
            var list = await _service.GetAllAsync();

            // Assert
            list.Should().HaveCount(2);
        }
    }   
}
