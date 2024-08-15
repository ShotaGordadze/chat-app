using Application.Commands.AuthCommands;
using Application.Exceptions;
using ChatAppWebApi.Models;
using Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        SignUpCommand cmd;
        User user;

        try
        {
            cmd = new SignUpCommand(model.Name, model.Lastname, model.Email, model.Password, model.Username);
            user = await _mediator.Send(cmd);
        }
        catch (UserAlreadyExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user != null)
            return Ok(user);
        else
            return BadRequest();
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignInAsync(SignInModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userToken = await _mediator.Send(new SignInCommand(model.Username, model.Password));

        if (userToken is null)
        {
            return BadRequest();
        }

        return Ok(userToken);
    }
}
