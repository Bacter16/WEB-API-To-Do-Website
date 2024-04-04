using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ToDoListDBContext _database;

    private readonly PasswordHasher passwordHasher = new PasswordHasher();

    private const string TokenSecret = "This-is-a-secret-ey-for-authentication";

 /*   private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);*/

    public AuthController(ToDoListDBContext context)
    {
        _database = context;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Username == loginModel.Username);

        if (user != null && IsValidUser(loginModel.Password, user))
        {
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    private bool IsValidUser(string password, User user)
    {
        if (user == null)
        {
            return false;
        }

        return passwordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
    }


    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(TokenSecret);

        // Add more claims here
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] RegisterModel registerModel)
    {

        var valid_user = await IsRegistrationDataValid(registerModel);

        if (valid_user is not null)
        {
            var user = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                PasswordHash = valid_user.PasswordHash,
                PasswordSalt = valid_user.PasswordSalt
            };
            _database.Users.Add(user);
            await _database.SaveChangesAsync();
            return Ok(new { Message = "Registration successful" });
        }

        return BadRequest(new { Message = "Invalid registration data" });
    }

    private async Task<User> IsRegistrationDataValid(RegisterModel registerModel)
    {
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Username == registerModel.Username);

        if (user != null)
        {
            return null;
        }
        if (!IsValidEmail(registerModel.Email))
        {
            return null;
        }

        var password_and_salt = passwordHasher.HashPassword(registerModel.Password);

        var valid_user = new User
        {
            Username = "",
            Email = "",
            PasswordHash = password_and_salt.hash,
            PasswordSalt = password_and_salt.salt
            };

        return valid_user;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
