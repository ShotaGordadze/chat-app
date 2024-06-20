using Application.Commands;
using Application.NewFolder;
using Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWebApi.Controllers;

[ApiController]
[Route("[Controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageAsync([FromBody] string message)
    {
        Message result;

        try
        {
             result = await _mediator.Send(new SendMessageCommand(message));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(result);
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
