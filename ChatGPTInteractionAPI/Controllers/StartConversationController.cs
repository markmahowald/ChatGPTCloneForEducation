using ChatGPTInteractionAPI.Classes;
using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ChatGPTInteractionAPI.Classes.ControllerSupportingClasses;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/new_session")]
    [ApiController]
    public class StartConversationController : Controller
    {
           private readonly ChatService _chatService;

            public StartConversationController(ChatService chatService)
            {
                _chatService = chatService;
            }

            [HttpPost]
            public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
            {
            try
                {
                if (request == null || string.IsNullOrWhiteSpace(request.InitialMessage))
                {
                    return BadRequest("Initial message is required.");
                }

                // Assuming StartConversation method returns a Conversation object
                Conversation conversation = await _chatService.StartConversation(request.InitialMessage);

                // You might want to return the full conversation object, or just an identifier
                // Here, I'm assuming you return an identifier, adjust as necessary
                if (conversation.Messages.Last().Role == "user")
                {
                    return StatusCode(500, "An error occurred while processing your request." + "OpenAi has not replied");
                }
                return Ok(new { ConversationId = conversation.Id, Messages = conversation.Messages });

            }
            catch (Exception ex)
                {
                    // Log the exception details here
                    return StatusCode(500, "An error occurred while processing your request."+ex.Message);
                }
            }
        }
    
}
