using Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SHG.Application;

namespace Application.Commands.AuthCommands;

public record SignInCommand(string UserName, string Password) : IRequest<AccessTokenDto?>;

public class SignInCommandHandler : IRequestHandler<SignInCommand, AccessTokenDto?>
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public SignInCommandHandler(UserManager<User> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<AccessTokenDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var idpUser = await _userManager.FindByNameAsync(request.UserName);

        if (idpUser is { UserName: not null } && await _userManager.CheckPasswordAsync(idpUser, request.Password))
        {
            return await _tokenService.GenerateJwtToken(idpUser);
        }

        return null;
    }
}
