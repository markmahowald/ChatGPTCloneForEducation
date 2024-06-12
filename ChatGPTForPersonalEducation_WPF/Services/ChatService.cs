using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedClassesAndUtility;

namespace ChatGPTForPersonalEducation_WPF.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Conversation> StartNewSessionAsync(string prompt)
        {
            StartConversationRequest startConversationRequest = new StartConversationRequest()
            { InitialMessage = prompt };
            var json = JsonConvert.SerializeObject(startConversationRequest);
            //Conversation newConvo = new Conversation();
            //newConvo.AddMessage("user", prompt);
           // var json = JsonConvert.SerializeObject(newConvo);

            // Convert the JSON string to HttpContent
            using var content = new StringContent(json, Encoding.UTF8, "application/json");


            try
            {
                // Send a POST request to the specified URI with the specified content.
                var response = await _httpClient.PostAsync("https://localhost:7171/api/new_session", content);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Optionally read the response body (if any)
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Conversation>(responseBody);

            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<Conversation> ContinueSessionAsync(Guid sessionId, string userInput)
        {
            var requestBody = new
            {
                sessionId,
                userInput
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7171/api/continue_session", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Conversation>(responseContent);
        }

        public async Task<List<Conversation>> GetAllSessionsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7171/api/retrieve_all_sessions");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Conversation>>(content);
        }
    }
}
