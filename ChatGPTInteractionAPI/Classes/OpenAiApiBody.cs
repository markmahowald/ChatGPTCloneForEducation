namespace ChatGPTInteractionAPI.Classes
{
    public class OpenAiApiBody
    {
        public string Model {get; set; }
        public int MaxTokens { get; set; }
        public List<Message> Messages { get; set; }
        //    max_tokens = 150,
        //    messages = _conversationHistory
    }
}
