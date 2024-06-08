using System.Reflection.Metadata.Ecma335;

namespace ChatGPTInteractionAPI.Classes
{
    public class Conversation
    {
        public List<Message> Messages { get; set; } = new List<Message>();
        public Guid Id { get; set; }
        public string Model { get; private set; } = "gpt-4";
        public int MaxTokens { get; private set; } = 150;

        public void AddMessage(string role, string content)
        {
            Messages.Add(new Message { Role = role, Content = content });
        }
        public OpenAiApiBody GenerateOpenAiBody()
        {
            var result = new OpenAiApiBody()
            {
                Model = this.Model,
                MaxTokens = this.MaxTokens

            };
            result.Messages = this.Messages.ToList();
            return result;
        }
    }
}
