using ChatGPTInteractionAPI.Classes;
using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/sessions/{sessionId}")]
    [ApiController]
    public class ContinueConversationController : Controller
    {
        private readonly IChatService _chatService;

        public ContinueConversationController(IChatService chatService)
        {
            _chatService = chatService;
        }

       // [HttpPost(Name= "ContinueConversation")]
        [HttpPost]
        public async Task<IActionResult> SendMessage(string sessionId, [FromBody] MessageRequest request)
        {
            try
            {
                var response = await _chatService.ContinueConversation(sessionId, request.Message);
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
