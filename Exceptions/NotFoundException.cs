namespace RealTimeChatAPI.Exceptions;

public class NotFoundException(string name, string identifier)
    : Exception($"{name} with identifier: {identifier} not found")
{
}
