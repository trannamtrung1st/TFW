using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Firebase.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMessage()
        {
            // The topic name can be optionally prefixed with "/topics/".
            var topic = "userABC";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
                Topic = topic,
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
            return NoContent();
        }

        [HttpPost("subscription")]
        public async Task<IActionResult> Subscribe(string token, string topic)
        {
            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new[] { token }, topic);

            return NoContent();
        }

    }
}
