using ChatGPTInteractionAPI.Classes;
using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ChatGPTInteractionAPI.Classes.ControllerSupportingClasses;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/retrieve_all_sessions")]
    [ApiController]
    public class GetAllConversationsController : Controller
    {
        private readonly ChatService _chatService;

        public GetAllConversationsController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _chatService.GetAllConversationsAsync();
            return Ok(conversations);
        }
    }
}
