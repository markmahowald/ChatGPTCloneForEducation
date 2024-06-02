using ChatGPTInteractionAPI.Classes;

namespace ChatGPTInteractionAPI.Services
{
    public interface IChatService
    {
        /// <summary>
        /// Sends a message to the OpenAI API and retrieves the response.
        /// </summary>
        /// <param name="sessionId">The session identifier for the conversation.</param>
        /// <param name="message">The message to send to the OpenAI API.</param>
        /// <returns>The response from the OpenAI as a string.</returns>
        Task<string> ContinueConversation(string sessionId, string message);

        Task<(string id, string response)> StartConversation(string initialMessage);

    }
}
