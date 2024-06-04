using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChatGPTInteractionAPI.Classes;
using System.Collections;
using Microsoft.AspNetCore.DataProtection.KeyManagement;


namespace ChatGPTInteractionAPI.Services
{
    public class ChatService
    {
        private  HttpClient _httpClient;
        private  string _openAiApiKey;

        public ChatService()
        {
                _httpClient = new HttpClient();

        }

        public async Task<Conversation> StartConversation(string initialMessage)
        {
            Conversation conversation = new Conversation();
            conversation.Id = Guid.NewGuid();
            await SendMessageToOpenAI(initialMessage, conversation);
            await SaveConversationToFile(conversation);
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
            //verify that we have the api key. 

            //var variables = Environment.GetEnvironmentVariables();
            //foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
            //{
            //    Console.WriteLine("Key: {0}, Value: {1}", env.Key, env.Value);
            //}
            if (this._openAiApiKey == null)
            {
                this._openAiApiKey = Environment.GetEnvironmentVariable("OpenAiPersonalKey");

            }

            //make sure that the http client has the api key
            //this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
            var requestBody = new
            {
                model = "gpt-4",
                max_tokens = 150,
                messages = new[]
            {
                //new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = prompt }
            },
            };

            string generatedText = "";
            this._httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this._openAiApiKey}");

            HttpResponseMessage response = await this._httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var choices = jsonDocument.RootElement.GetProperty("choices");
                
                generatedText += choices[0].GetProperty("message").GetProperty("content").GetString();
            }
            else
            {
                generatedText += $"Error(s): {await response.Content.ReadAsStringAsync()}";
            }
                           
            conversation.AddMessage("user", prompt);  // Save user message
            conversation.AddMessage("assistant", generatedText);  // Save AI response
            return generatedText;
               
            // Handle errors
            //conversation.AddMessage("system", "");  // Save user message
            //return $"Error: {response.ReasonPhrase}";
                
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

