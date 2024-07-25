using Application.Commands.AuthCommands;
using ChatAppWebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWebApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUpAsync(SignUpModel model)
    {
        var cmd = new SignUpCommand(model.Name, model.Lastname, model.Email, model.Password, model.Username);
        var user = await _mediator.Send(cmd);

        if (user != null)
        {
            return Ok(user);
        }

        return BadRequest("Couldn't create an user");
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignInAsync(SignInModel model)
    {
        var userToken = await _mediator.Send(new SignInCommand(model.Username, model.Password));

        if (userToken is null)
        {
            return BadRequest();
        }

        return Ok(userToken);
    }
}
