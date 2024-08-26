using Application.Exceptions;
using Infrastructure.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;

namespace Application.Commands.AuthCommands;

public record SignUpCommand(string Name, string Lastname, string Email, string Password, string Username) : IRequest<User?>;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, User?>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public SignUpCommandHandler(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<User?> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            throw new UserAlreadyExistsException($"This user with email: '{request.Email}' already exists", request.Email);
        }

        var idpUserResult = await _userManager.CreateAsync(new User
        {
            Name = request.Name,
            Lastname = request.Lastname,
            Email = request.Email,
            UserName = request.Username,
            AccCreateDate = DateTime.UtcNow
        }, request.Password);

        var idpUser = await _userManager.FindByEmailAsync(request.Email);
        await SendConfirmationEmail(request.Email, idpUser);

        if (!idpUserResult.Succeeded)
        {
            return null;
        }

        if (idpUser != null) await _userManager.AddToRoleAsync(idpUser, "Client");

        return idpUser;
    }

    private async Task SendConfirmationEmail(string? email, User? user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
        var confirmationLink = $"http://localhost:3000/confirm-email?UserId={user!.Id}&Token={token}";
        await _emailService.SendEmailAsync(email!, "Confirm Your Email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.", true);
    }
}
