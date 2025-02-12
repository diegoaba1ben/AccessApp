using System;
using System.Threading.Tasks;
using System Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppuser.Infrastructure.Repositories.Base;
using AccessAppUser.Domain.Entities;

public class BaseRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly BaseRepository<User> _userRepository;

    public BaseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _userRepository = new BaseRepository<User>(_context);
    }
    [Fact]
    public async Task AddAsync_Should_Add_User_To_Database()
    {
        // Arrange
        var user = user.Builder()
            .WithName("Test User")
            .WithEmail("test@exaple.com")
            .WithPassword("@SecPass1234")
            .Build();

        // Act
        await _userRepository.AddAsync(user);
        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Name, retrievedUser.Name);
    }
    [Fact]
    public async Task DeleteAsync_Shuould_Remove_User_From_Database()
    {
        // Arrange
        var user = user.Builder()
            .WithName("To Delete")
            .WithEmail("delete@example.com")
            .WithPassword("@SecPass1234")
            .Build();

        await _userRepository.AddAsync(user);

        // Act
        await _userRepository.DeleteAsync(user.Id);
        var deletedUser = await _userRepository.GetByIdAsync(user.Id);

        // Assert
        Assert.Null(deletedUser);
    }
}