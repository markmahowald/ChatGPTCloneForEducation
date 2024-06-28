using ChatGPTInteractionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
            List<SharedClassesAndUtility.OpenAiApiBody> conversations = await _chatService.GetAllConversationsAsync();
            return Ok(conversations);
            }
    }
}
