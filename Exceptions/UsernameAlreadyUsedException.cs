namespace RealTimeChatAPI.Exceptions;

public class UsernameAlreadyUsedException(string username)
    : Exception($"Username '{username}' already used, choose another one.")
{
}
