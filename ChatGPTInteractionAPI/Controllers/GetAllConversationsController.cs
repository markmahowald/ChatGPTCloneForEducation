using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ChatGPTInteractionAPI.Controllers
{
    [Route("api/retrieve_sessions")]
    [ApiController]
    public class GetAllConversationsController : Controller
    {
        private readonly ChatService _chatService;

        public GetAllConversationsController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("retrieve_all_sessions")]
        public async Task<IActionResult> GetAllConversations()
        {
            List<SharedClassesAndUtility.OpenAiApiBody> conversations = await _chatService.GetAllConversationsAsync();
            return Ok(conversations);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversationById(Guid id)
        {
            var conversation = await _chatService.GetConversationByIdAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            return Ok(conversation);
        }
    }
}

