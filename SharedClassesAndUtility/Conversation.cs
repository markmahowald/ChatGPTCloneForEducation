using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharedClassesAndUtility
{
    public class Conversation : INotifyPropertyChanged
    {
        private List<Message> _messages = new List<Message>();
        private Guid _id;
        private string _topicDescription;

        public List<Message> Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Model { get; private set; } = "gpt-4";
        public int MaxTokens { get; private set; } = 150;

        public string TopicDescription
        {
            get => _topicDescription;
            set
            {
                if (_topicDescription != value)
                {
                    _topicDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TriggerAllPropertyChanged()
        {
            OnPropertyChanged(nameof(Messages));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(MaxTokens));
            OnPropertyChanged(nameof(TopicDescription));
        }

        public void AddMessage(string role, string content)
        {
            Messages.Add(new Message { Role = role, Content = content });
            OnPropertyChanged(nameof(Messages));
        }

        public OpenAiApiBody GenerateOpenAiBody()
        {
            var result = new OpenAiApiBody()
            {
                Model = this.Model,
                MaxTokens = this.MaxTokens
            };
            result.Conversation = this;
            return result;
        }
    }
}
