namespace ChatGPTInteractionAPI.Classes.ControllerSupportingClasses
{
    public class ContinueConversationRequest
    {
        public Guid ConversationId { get; set; }
        public string Message { get; set; }
    }
}
