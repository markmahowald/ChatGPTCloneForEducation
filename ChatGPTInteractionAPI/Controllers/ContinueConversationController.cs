using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SharedClassesAndUtility;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/continue_session")]
    [ApiController]
    public class ContinueConversationController : Controller
    {
        private readonly ChatService _chatService;

        public ContinueConversationController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> ContinueConversation([FromBody] ContinueConversationRequest request)
        {
            try
            {
                if (request == null || request.ConversationId == "" || string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest("Valid conversation ID and message are required.");
                }

                // Assuming ContinueConversation method returns a Conversation object
                Conversation conversation = await _chatService.ContinueConversation(new Guid( request.ConversationId), request.Message);

                if (conversation == null)
                {
                    return NotFound($"No conversation found with ID: {request.ConversationId}");
                }

                // Return the updated conversation details
                return Ok(new { ConversationId = conversation.Id, Messages = conversation.Messages });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }
    }
}
