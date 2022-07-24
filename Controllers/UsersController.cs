using alurageekapi.Models;
using alurageekapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace alurageekapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _usersService;

    public UsersController(UserService usersService) =>
        _usersService = usersService;

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<List<User>> Get() =>
        await _usersService.GetAsync();

    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
    {   
        //Verifica e retorna usuário
        var user = await _usersService.Get(model.Email, model.Password);

        if (user is null)
            return new 
            {
                errors = "Usuário ou senha incorretos.",
                success = false
            };
        
        //Gerar Bearer
        var token = TokenService.GenerateToken(user);    

        //Esconde Propriedades
        user.Password = "";

        return new
        {
            data = user,            
            token = token,
            message = "Usuário logado.",
            success = true
        };
    }

    [AllowAnonymous]
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.Register(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }
}