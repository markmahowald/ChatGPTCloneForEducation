using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChatGPTInteractionAPI.Classes;

namespace ChatGPTInteractionAPI.Services
{
    public class ChatService
    {
        private  HttpClient _httpClient;
        private  string _openAiApiKey;

        public Conversation StartConversation(string initialMessage)
        {
            Conversation conversation = new Conversation();
            conversation.Id = Guid.NewGuid();
            conversation.AddMessage("user", initialMessage);
            SendMessageToOpenAI(initialMessage, conversation);
            SaveConversationToFile(conversation);
            return conversation;

        }
        
        public async Task<Conversation> ContinueConversation(Guid conversationId, string userInput)
        {
            // Define the path to the conversation file
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Conversations");
            string filePath = Path.Combine(folderPath, $"{conversationId}.txt");

            // Check if the conversation file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The conversation with the specified ID does not exist.");
            }

            // Load the conversation from the file
            string json = await File.ReadAllTextAsync(filePath);
            Conversation conversation = JsonConvert.DeserializeObject<Conversation>(json);

            // Add user message to the conversation
            conversation.AddMessage("user", userInput);

            // Get the response from OpenAI
            await SendMessageToOpenAI(userInput, conversation);

            // Save the updated conversation
            await SaveConversationToFile(conversation);

            // Return the updated conversation
            return conversation;
        }

        public async Task<string> SendMessageToOpenAI(string prompt, Conversation conversation)
        {
            if (this._openAiApiKey == null)
            {
                this._openAiApiKey = Environment.GetEnvironmentVariable("OpenAiPersonalKey");

            }

            var data = new
            {
                model = "text-davinci-002",
                prompt = prompt,
                max_tokens = 150
            };

            string jsonData = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://api.openai.com/v1/completions", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject jObject = JObject.Parse(responseBody);
                string responseText = jObject["choices"][0]["text"].ToString();
                conversation.AddMessage("user", prompt);  // Save user message
                conversation.AddMessage("ai", responseText);  // Save AI response
                return responseText;
            }
            else
            {
                // Handle errors
                return $"Error: {response.ReasonPhrase}";
            }
        }


        public async Task SaveConversationToFile(Conversation conversation)
        {
            // Get the directory path for "Conversations" folder in the current directory
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Conversations");

            // Check if the "Conversations" directory exists, if not, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a unique file name based on the current timestamp
            // Use the GUID as the filename
            string fileName = $"{conversation.Id}.txt";
            string filePath = Path.Combine(folderPath, fileName);


            // Serialize the conversation object to JSON
            string json = JsonConvert.SerializeObject(conversation, Formatting.Indented);

            // Write the JSON to a text file in the "Conversations" directory
            await File.WriteAllTextAsync(filePath, json);
        }

        public class OpenAIResponse
        {
            public Choice[] choices { get; set; }
        }

        public class Choice
        {
            public string text { get; set; }
        }
    }
}

