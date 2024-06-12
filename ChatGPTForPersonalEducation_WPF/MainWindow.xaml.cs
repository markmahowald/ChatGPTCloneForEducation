using ChatGPTForPersonalEducation_WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatGPTForPersonalEducation_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChatService _chatService;
        private Guid _currentSessionId;

        //TODO: MM 6/12/24 - this constructor is a hacky work arround for the app attempting to spin up a main window with a default constructor. fix it to not be necessary in the future. 
        public MainWindow()
        {
            this.Close();
        }
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _chatService = (ChatService?)serviceProvider.GetService(typeof(ChatService));
        }
        private async void StartNewSession_Click(object sender, RoutedEventArgs e)
        {
            var conversation = await _chatService.StartNewSessionAsync(this.UserInputTextBox.Text);
            _currentSessionId = conversation.Id;
            ResponseTextBlock.Text = "New session started. Session ID: " + _currentSessionId;
        }
        private async void ContinueSession_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSessionId == Guid.Empty)
            {
                ResponseTextBlock.Text = "Please start a new session first.";
                return;
            }

            var userInput = UserInputTextBox.Text;
            var conversation = await _chatService.ContinueSessionAsync(_currentSessionId, userInput);
            ResponseTextBlock.Text = "Session continued. Latest response: " + conversation.Messages[^1].Content;
        }

        private async void RetrieveAllSessions_Click(object sender, RoutedEventArgs e)
        {
            var allSessions = await _chatService.GetAllSessionsAsync();
            ResponseTextBlock.Text = "All sessions retrieved. Count: " + allSessions.Count;
        }
    }
}