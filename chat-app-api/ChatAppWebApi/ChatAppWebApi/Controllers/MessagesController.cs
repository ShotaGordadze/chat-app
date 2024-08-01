using Application.Commands.MessageCommands;
using Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAppWebApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _contextAccessor;

    public MessagesController(IMediator mediator, IHttpContextAccessor contextAccessor)
    {
        _mediator = mediator;
        _contextAccessor = contextAccessor;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessageAsync([FromBody] string message)
    {
        Message result;

        var user = _contextAccessor.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;

            try
            {
                result = await _mediator.Send(new SendMessageCommand(message, username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        return BadRequest("User is not Authenticated");
    }

    [HttpGet]
    public async Task<IActionResult> ConsumeMessageAsync()
    {
        Message result;

        try
        {
            result = await _mediator.Send(new ConsumeMessageCommand());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(result);
    }
}
