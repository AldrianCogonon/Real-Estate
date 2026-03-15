using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var users = _userService.LoadUsers();

        if (users.Any(u => u.Username == request.Username))
            return BadRequest(new { message = "Username exists" });

        users.Add(new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Redirect = "buy.html"
        });

        _userService.SaveUsers(users);

        return Ok(new { message = "Registration successful" });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "admin123")
        {
            return Ok(new
            {
                redirect = "admin.html"
            });
        }

        var users = _userService.LoadUsers();

        var user = users.FirstOrDefault(u =>
            u.Username == request.Username &&
            u.Password == request.Password);

        if (user == null)
            return BadRequest(new { message = "Invalid login" });

        return Ok(new
        {
            redirect = user.Redirect
        });
    }
}