﻿using MediatR;

namespace RealTimeChatAPI.Services.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest
{
    public string Name { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
