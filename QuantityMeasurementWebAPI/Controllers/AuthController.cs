using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly JwtService _jwt;

    public AuthController(IUserRepository repository, JwtService jwt)
    {
        _repository = repository;
        _jwt = jwt;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        try
        {
            Console.WriteLine($"Signup attempt: {request?.Email}");
            
            if (request == null)
            {
                Console.WriteLine("Request is null");
                return BadRequest("Request data is missing");
            }
            
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                var passwordStatus = string.IsNullOrEmpty(request.Password) ? "missing" : "provided";
                Console.WriteLine($"Missing fields - Name: {request.Name}, Email: {request.Email}, Password: {passwordStatus}");
                return BadRequest("All fields are required");
            }

            var existing = await _repository.GetByEmailAsync(request.Email);
            if (existing != null) return BadRequest("Email already exists");

            var user = new UserEntity
            {
                Name = request.Name,
                Email = request.Email,
                Password = PasswordHelper.HashPassword(request.Password),
                Role = request.Role ?? "User"
            };

            await _repository.AddUserAsync(user);
            Console.WriteLine($"User created successfully: {user.Email}");
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Signup error: {ex.Message}");
            return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            Console.WriteLine($"Login attempt: {request?.Email}");
            
            if (request == null)
            {
                Console.WriteLine("Request is null");
                return BadRequest("Request data is missing");
            }
            
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                var passwordStatus = string.IsNullOrEmpty(request.Password) ? "missing" : "provided";
                Console.WriteLine($"Missing fields - Email: {request.Email}, Password: {passwordStatus}");
                return BadRequest("All fields are required");
            }

            var user = await _repository.GetByEmailAsync(request.Email);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.Password))
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user.Id, user.Email, user.Role ?? "User");
            Console.WriteLine($"Login successful: {user.Email}");
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public record SignupRequest(string Name, string Email, string Password, string? Role);
public record LoginRequest(string Email, string Password);