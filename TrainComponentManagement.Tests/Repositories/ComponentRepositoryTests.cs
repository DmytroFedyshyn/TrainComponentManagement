using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TrainComponentManagement.DAL.Data;
using TrainComponentManagement.DAL.Models;
using TrainComponentManagement.DAL.Repositories.Implementation;

namespace TrainComponentManagement.Tests.Repositories
{
    public class ComponentRepositoryTests : IDisposable
    {
        private readonly TrainComponentContext _context;
        private readonly ComponentRepository _repository;

        public ComponentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TrainComponentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new TrainComponentContext(options);
            _context.Database.EnsureCreated();

            _context.Components.RemoveRange(_context.Components);
            _context.SaveChanges();

            _repository = new ComponentRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var entity = new Component
            {
                Name = "Test",
                UniqueNumber = "UN123",
                CanAssignQuantity = true,
                Quantity = 5
            };

            // Act
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            var fromDb = await _context.Components.FindAsync(entity.Id);

            // Assert
            fromDb.Should().NotBeNull();
            fromDb!.Name.Should().Be("Test");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            // Arrange
            var entity = new Component
            {
                Name = "ById",
                UniqueNumber = "ID001",
                CanAssignQuantity = false
            };
            _context.Components.Add(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(entity.Id);

            // Assert
            result.Should().NotBeNull();
            result!.UniqueNumber.Should().Be("ID001");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            _context.Components.AddRange(
                new Component { Name = "A", UniqueNumber = "UA", CanAssignQuantity = false },
                new Component { Name = "B", UniqueNumber = "UB", CanAssignQuantity = true, Quantity = 2 }
            );
            await _context.SaveChangesAsync();

            // Act
            var list = await _repository.GetAllAsync();

            // Assert
            list.Should().HaveCount(2);
            list.Select(e => e.UniqueNumber).Should().BeEquivalentTo(new[] { "UA", "UB" });
        }

        [Fact]
        public async Task GetByUniqueNumberAsync_ShouldReturnMatching()
        {
            // Arrange
            _context.Components.Add(new Component
            {
                Name = "FindMe",
                UniqueNumber = "FIND123",
                CanAssignQuantity = false
            });
            await _context.SaveChangesAsync();

            // Act
            var entity = await _repository.GetByUniqueNumberAsync("FIND123");

            // Assert
            entity.Should().NotBeNull();
            entity!.Name.Should().Be("FindMe");
        }

        [Fact]
        public async Task Remove_ShouldDeleteEntity()
        {
            // Arrange
            var entity = new Component
            {
                Name = "ToDelete",
                UniqueNumber = "DEL456",
                CanAssignQuantity = false
            };
            _context.Components.Add(entity);
            await _context.SaveChangesAsync();

            // Act
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
            var fromDb = await _repository.GetByIdAsync(entity.Id);

            // Assert
            fromDb.Should().BeNull();
        }

        [Fact]
        public async Task AddRangeAsync_And_RemoveRange_ShouldHandleBatchOperations()
        {
            // Arrange
            var items = Enumerable.Range(1, 3)
                .Select(i => new Component
                {
                    Name = $"C{i}",
                    UniqueNumber = $"UN{i:000}",
                    CanAssignQuantity = false
                })
                .ToList();

            // Act
            await _repository.AddRangeAsync(items);
            await _repository.SaveChangesAsync();
            var allEntitiesAfterAdd = await _repository.GetAllAsync();

            _repository.RemoveRange(items);
            await _repository.SaveChangesAsync();
            var allEntitiesAfterRemove = await _repository.GetAllAsync();

            // Assert
            allEntitiesAfterAdd.Should().HaveCount(3);
            allEntitiesAfterRemove.Should().BeEmpty();
        }
    }
}
