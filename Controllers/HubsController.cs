using Microsoft.AspNetCore.Mvc;

namespace RealTimeChatAPI.Controllers
{
    [Route("api/hubs")]
    [ApiController]
    public class HubsController : ControllerBase
    {
        /// <summary>
        /// Retrieves information about the ChatHub and its available methods and connections.
        /// </summary>
        /// <returns>A JSON object containing ChatHub information, including methods and connection events.</returns>
        [HttpGet("ChatHub/info")]
        public IActionResult GetInfo()
        {
            var chatHubInfo = new
            {
                HubName = "ChatHub",
                Methods = new[]
                {
                    new
                    {
                        MethodName = "SendMessage",
                        Arguments = "SendMessage(string userId, string message)",
                        Description = "Sends a message to the specified user."
                    },
                    new
                    {
                        MethodName = "DeliveredMessage",
                        Arguments = "DeliveredMessage(string messageId)",
                        Description = "Marks a message as delivered."
                    },
                    new
                    {
                        MethodName = "DeliveredAndReadMessage",
                        Arguments = "DeliveredAndReadMessage(string messageId)",
                        Description = "Marks a message as both delivered and read."
                    },
                    new
                    {
                        MethodName = "ReadMessages",
                        Arguments = "ReadMessages(string userChatId)",
                        Description = "Marks messages between the current user and the specified user as read."
                    },
                    new
                    {
                        MethodName = "DeliveredAllMessages",
                        Arguments = "DeliveredAllMessages()",
                        Description = "Marks all messages as delivered for the current user."
                    }
                },
                ConnectionEvents = new[]
                {
                    new
                    {
                        Name = "SendMessage",
                        Description = "Triggered when a message is sent and saved in the database."
                    },
                    new
                    {
                        Name = "ReceiveMessage",
                        Description = "Triggered when a new message is received by the user."
                    },
                    new
                    {
                        Name = "MessageStatus",
                        Description = "Triggered when the status of a message changes (e.g., delivered, read)."
                    },
                    new
                    {
                        Name = "Error",
                        Description = "Triggered when an error occurs, sending the error message to the client."
                    }
                }
            };

            return Ok(chatHubInfo);
        }
    }
}
