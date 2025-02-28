using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.DTOs.User;


namespace AccessAppUser.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;
        private readonly UserController _controller;
        private readonly IMapper _mapper;

        public UserControllerTests()
        {
            // Nombre único por prueba para evitar colisiones
            var databaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserReadDTO>();
                cfg.CreateMap<UserCreateDTO, User>();
                cfg.CreateMap<UserUpdateDTO, User>();
            });
            _mapper = config.CreateMapper();

            _controller = new UserController(_repository, _mapper);
        }

        /// <summary>
        /// Prueba de diagnóstico para verificar que la base de datos InMemory funciona correctamente.
        /// </summary>
        [Fact]
        public async Task TestDatabaseOperations()
        { 
            // PASO 1
            // Verifica la BD antes de empezar
            var usersBeforeClean = await _context.Users.CountAsync();
            Console.WriteLine($"Usuarios en BD antes de limpiar: {usersBeforeClean}");  

            // PASO 2
            //Limpia la BD
            Console.WriteLine("Limpiando la BD");
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
            var usersAfterClean = await _context.Users.CountAsync();
            Console.WriteLine($"Usuarios en BD después de limpiar: {usersAfterClean}");

            // PASO 3
            //  Agrega usuarios y verificar que se guarden correctamente
            var user1 = new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com", IsActive = true };
            var user2 = new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", IsActive = true };

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            var usersAfterAdd = await _context.Users.CountAsync();
            Console.WriteLine($"Usuarios en BD después de agregar: {usersAfterAdd}");

            // PASO 4
            //  Recupera usuarios
            var usersFromDb = await _context.Users.ToListAsync();
            Console.WriteLine($"Usuarios guardados en la BD"); 
            foreach (var user in usersFromDb)
            {
                Console.WriteLine($"- ID: {user.Id}, Nombre: {user.Name}, Email: {user.Email}");
            }

            Assert.Equal(2, usersFromDb.Count); // Si esto falla, InMemoryDatabase no está guardando correctamente
        }

        /// <summary>
        /// Prueba: Obtener todos los usuarios.
        /// </summary>
        [Fact]
        public async Task GetAllUsers_ReturnsOk_WithUsers()
        {

            // Arrange
            //Limpiar la base de datos
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();

            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Alice", Email = "alice@example.com", IsActive = true },
                new User { Id = Guid.NewGuid(), Name = "Bob", Email = "bob@example.com", IsActive = true }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<IEnumerable<UserReadDTO>>(okResult.Value);
            Assert.Equal(2, response.Count());
        }

        /// <summary>
        /// Prueba: Obtener usuario por ID (usuario existente).
        /// </summary>
        [Fact]
        public async Task GetUserById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Charlie", Email = "charlie@example.com", IsActive = true };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetUserById(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<UserReadDTO>(okResult.Value);
            Assert.Equal(user.Name, response.Name);
        }

        /// <summary>
        /// Prueba: Obtener usuario por ID (usuario inexistente).
        /// </summary>
        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _controller.GetUserById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        /// <summary>
        /// Prueba: Registrar usuario.
        /// </summary>
        [Fact]
        public async Task RegisterUser_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var userDto = new UserCreateDTO { Name = "David", Email = "david@example.com", Password = "password123", IsActive = true };

            // Act
            var result = await _controller.RegisterUser(userDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<UserReadDTO>(createdAtResult.Value);
            Assert.Equal(userDto.Name, response.Name);
        }

        /// <summary>
        /// Prueba: Login de usuario exitoso.
        /// </summary>
        [Fact]
public async Task LoginUser_ReturnsOk_WhenCredentialsAreCorrect()
{
    //  PASO 1: LIMPIAR LA BASE DE DATOS
    _context.Users.RemoveRange(_context.Users);
    await _context.SaveChangesAsync();
    Console.WriteLine(" Base de datos limpiada antes de la prueba.");

    // 🔹 PASO 2: CREAR USUARIO
    var user = new User
    {
        Id = Guid.NewGuid(),
        Name = "Eve",
        Email = "eve@example.com",
        Password = "P@ass123" // cumple con las validaciones
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    Console.WriteLine(" Usuario agregado a la base de datos.");

    //  PASO 3: VERIFICAR QUE EL USUARIO SE GUARDÓ
    var storedUsers = await _context.Users.ToListAsync();
    Console.WriteLine($" Usuarios en BD: {storedUsers.Count}");
    foreach (var u in storedUsers)
    {
        Console.WriteLine($" Usuario en BD -> Email: {u.Email}, Password: {u.Password}");
    }

    //  PASO 4: INTENTAR LOGIN
    var loginDto = new UserLoginDTO { Email = "eve@example.com", Password = "P@ass123" };
    Console.WriteLine($" Intentando login con -> Email: {loginDto.Email}, Password: {loginDto.Password}");

    var result = await _controller.LoginUser(loginDto);

    //  PASO 5: VERIFICAR RESPUESTA
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal("Inicio de sesión exitoso", okResult.Value);

    Console.WriteLine(" Prueba de login exitosa.");
}

        /// <summary>
        /// Prueba: Login de usuario fallido.
        /// </summary>
        [Fact]
        public async Task LoginUser_ReturnsUnauthorized_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Frank", Email = "frank@example.com", Password = "password123" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new UserLoginDTO { Email = "frank@example.com", Password = "P@ssw1234" };

            // Act
            var result = await _controller.LoginUser(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }

        /// <summary>
        /// Prueba: Actualizar usuario.
        /// </summary>
        [Fact]
        public async Task UpdateUser_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Grace", Email = "grace@example.com", Password = "password123", IsActive = true };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var updateDto = new UserUpdateDTO { Id = user.Id, Name = "Grace Updated", Email = "grace@example.com", Password = "newpassword", IsActive = true };

            // Act
            var result = await _controller.UpdateUser(user.Id, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Usuario actualizado correctamente", okResult.Value);
        }

        /// <summary>
        /// Prueba: Eliminar usuario.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Henry", Email = "henry@example.com", IsActive = true };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteUser(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Usuario eliminado correctamente", okResult.Value);
        }
    }
}

