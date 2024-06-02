using ChatGPTInteractionAPI.Classes;
using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/new_session")]
    [ApiController]
    public class StartConversationController : Controller
    {
           private readonly IChatService _chatService;

            public StartConversationController(IChatService chatService)
            {
                _chatService = chatService;
            }

            [HttpPost]
            public async Task<IActionResult> StartConversation([FromBody] MessageRequest request)
            {
                try
                {
                    var sessionData = await _chatService.StartConversation(request.Message);
                    return Ok(new { SessionId = sessionData.id, Response = sessionData.response });
                }
                catch (Exception ex)
                {
                    // Log the exception details here
                    return StatusCode(500, "An error occurred while processing your request."+ex.Message);
                }
            }
        }
    
}
