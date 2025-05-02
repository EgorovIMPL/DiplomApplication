using Contract;
using Microsoft.AspNetCore.Mvc;

namespace DiplomApplication.Controllers;

public class UserController : Controller
{
    [HttpPost("/createuser")]
    public IActionResult CreateUser(CreateUserDto user)
    {
        Console.WriteLine(user.SteamId);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.Name);
        return Ok();
    }
}