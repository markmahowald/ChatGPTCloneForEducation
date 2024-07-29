using ChatGPTForPersonalEducation_WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedClassesAndUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatGPTForPersonalEducation_WPF.ViewModels
{
    public class MainWindowViewModel
    {
        public readonly ChatService ChatService;
        public Guid CurrentSessionId;
        public ObservableCollection<Conversation> Conversations = new ObservableCollection<Conversation>();
        public Conversation SelectedConversation = new Conversation();
        public ObservableCollection<Message> SelectedConversationMessages = new ObservableCollection<Message>();

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            this.ChatService = (ChatService?)serviceProvider.GetService(typeof(ChatService));

        }
        
            public async Task LoadInitialDataAsync()
        {
            try
            {
                var convoList = await this.ChatService.GetAllSessionsAsync();
                // Assuming there's a ListBox or some UI element to display these conversations
                this.Conversations.Clear();
                foreach (Conversation c in convoList)
                {
                    this.Conversations.Add(c);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load conversations: {ex.Message}");

            }
        }

        // Logic to send a message and handle the response
        public async Task<bool> SendMessage(string userInput)
        {

            if (string.IsNullOrEmpty(userInput))
            {
                this.SelectedConversationMessages.Add(new Message() { Role = "SYSTEM", Content = "Please enter some text before sending." });
                return false;
            }

            try
            {
                //check to see if this is a new conversation or an existing one. 
                bool existingChat = ((this.SelectedConversation.Messages.Count > 0) ? true : false);
                Conversation results;
                if (existingChat)
                {
                    Conversation selectedConversationWithReply = await this.ChatService.ContinueSessionAsync(this.SelectedConversation.Id, userInput);
                    this.SelectedConversationMessages.Add(selectedConversationWithReply.Messages[selectedConversationWithReply.Messages.Count() - 2]);
                    this.SelectedConversationMessages.Add(selectedConversationWithReply.Messages.Last());
                    this.Conversations.Add(selectedConversationWithReply);
                }

                else
                {
                    //this is a new chat
                    results = await this.ChatService.StartNewSessionAsync(userInput);
                    this.SelectedConversation.Id = results.Id;
                    this.SelectedConversation = results;
                    this.SelectedConversationMessages.Clear();
                    this.Conversations.Add(results);
                    foreach (Message m in results.Messages)
                    {
                        this.SelectedConversationMessages.Add(m);

                    }

                }

                foreach (Conversation c in this.Conversations)
                {
                    c.TriggerAllPropertyChanged();
                }
                return true;
            }
            catch (Exception ex)
            {
                this.SelectedConversationMessages.Add(new Message() { Role = "SYSTEM", Content = $"An error occurred: {ex.Message}"});
                return true;
            }
        }

        public async Task SelectConversationAsync(Conversation conversationItem)
        {
            if (conversationItem is not null)
            {
                await RefreshConversationAsync(conversationItem.Id);
            }
        }

        public void StartNewConversation()
        {
            SelectedConversation = new Conversation();
            SelectedConversationMessages.Clear();
        }

        public async Task RefreshConversationAsync(Guid conversationId)
        {
            try
            {
                var updatedConversation = await this.ChatService.GetConversationByIdAsync(conversationId);
                if (updatedConversation != null)
                {
                    SelectedConversationMessages.Clear();
                    foreach (var message in updatedConversation.Messages)
                    {
                        SelectedConversationMessages.Add(message);
                    }
                    SelectedConversation = updatedConversation;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh conversation: {ex.Message}");
            }
        }
    }
}
