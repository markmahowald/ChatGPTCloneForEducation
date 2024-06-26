﻿using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChatGPTInteractionAPI.Classes;
using System.Collections;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using System.Data;


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
            await SendConversationToOpenAI(initialMessage, conversation);
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

            // Get the response from OpenAI
            await SendConversationToOpenAI(userInput, conversation);

            // Save the updated conversation
            await SaveConversationToFile(conversation);

            // Return the updated conversation
            return conversation;
        }

        public async Task<string> SendConversationToOpenAI(string prompt, Conversation conversation)
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

            //assemble all current messages into a list

            //make sure that the http client has the api key
            //this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
            conversation.AddMessage("user", prompt);  // Save user message
            conversation.Messages.Select(x => $"{x.Role}: {x.Content}");
            var messageCollection = conversation.Messages.Select(x => new { role = x.Role, content = x.Content }).ToArray();


            var requestBody = new
            {
                model = "gpt-4",
                max_tokens = 150,
                messages = messageCollection
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

            try
            {
                // Check if the "Conversations" directory exists, if not, create it
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                // Use the GUID as the filename
                string fileName = $"{conversation.Id}.txt";
                string filePath = Path.Combine(folderPath, fileName);


                // Serialize the conversation object to JSON
                string json = JsonConvert.SerializeObject(conversation, Formatting.Indented);

                // Write the JSON to a text file in the "Conversations" directory
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                //todo: this would be a good place for logging. 

                Console.WriteLine($"Error saving conversation: {ex.Message}");

            }
        }

        public async Task<List<OpenAiApiBody>> GetAllConversationsAsync()
        {
            var conversations = new List<OpenAiApiBody>();
            // Define the path to the conversation file
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Conversations");
            
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, "*.txt");

                foreach (var file in files)
                {
                    var json = await File.ReadAllTextAsync(file);
                    var conversation = JsonConvert.DeserializeObject<Conversation>(json);
                    conversations.Add(conversation.GenerateOpenAiBody());
                }
            }


            return conversations;
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

