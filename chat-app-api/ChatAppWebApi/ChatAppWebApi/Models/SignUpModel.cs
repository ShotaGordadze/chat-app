namespace ChatAppWebApi.Models;

public record SignUpModel(string Name, string Lastname, string Email, string Password, string Username);

public record SignInModel(string Username, string Password);


