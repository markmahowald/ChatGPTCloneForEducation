namespace ChatGPTInteractionAPI.Classes
{
    public class Conversation
    {
        public List<Message> Messages { get; set; } = new List<Message>();
        public Guid Id { get; set; }

        public void AddMessage(string role, string content)
        {
            Messages.Add(new Message { Role = role, Content = content });
        }
    }
}
