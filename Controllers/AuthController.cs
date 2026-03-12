using Microsoft.AspNetCore.Mvc;
using TaskFlowAPI.Data;
using TaskFlowAPI.DTOs;
using TaskFlowAPI.Helpers;
using TaskFlowAPI.Models;
namespace TaskFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context,JwtHelper jwtHelper)
        {
            _context=context;
            _jwtHelper=jwtHelper;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            if(_context.Users.Any(u=>u.Username==dto.Username))
            return BadRequest("User already exisits");
            var user=new User
            {
                Username=dto.Username,
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role="User"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public IActionResult Login (LoginDTO dto)
        {
            var user=_context.Users.FirstOrDefault(u=>u.Username==dto.Username);
            if (user==null)
                return Unauthorized("Invalid username or password");
            var passwordValid=BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash);
            if(!passwordValid)
                return Unauthorized("Invalid username or password");
            var token=_jwtHelper.GenerateToken(user.Username,user.Role);
            return Ok(new {token});
            
        }
    }
}