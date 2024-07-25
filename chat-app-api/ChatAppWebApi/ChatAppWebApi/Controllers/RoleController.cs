using Application.Commands.RoleCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWebApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddRole")]
    public async Task<IActionResult> AddAsync(string roleName)
    {
        var cmd = new AddRoleCommand(roleName);
        var result = await _mediator.Send(cmd);

        if (result)
        {
            return Ok(result);
        }

        return BadRequest("Couldn't create a role");
    }
}
