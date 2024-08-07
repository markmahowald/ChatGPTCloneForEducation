﻿using ChatGPTForPersonalEducation_WPF.Services;
using ChatGPTForPersonalEducation_WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SharedClassesAndUtility;
using System.Collections.ObjectModel;
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

        private MainWindowViewModel _viewModel;



        //TODO: MM 6/12/24 - this constructor is a hacky work arround for the app attempting to spin up a main window with a default constructor. fix it to not be necessary in the future. 
        public MainWindow()
        {
            this.Close();
        }
        public MainWindow(IServiceProvider serviceProvider)
        {
            _viewModel = new MainWindowViewModel(serviceProvider);
           
            InitializeComponent();
        }

        // Handler for the window loaded event
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               _viewModel.LoadInitialDataAsync();
                this.Title = "Mark-GPT";

            }
            catch (Exception)
            {

                this.Close();
            }

            this.MessagesListBox.ItemsSource = this._viewModel.Conversations;
            this.SelectedConversationMessagesListBox.ItemsSource = this._viewModel.SelectedConversationMessages;

        }

    

        // Handler for the send button click
        private async void SendInput_Click(object sender, RoutedEventArgs e)
        {
            var userInput = this.UserInputTextBox.Text.Trim();
            bool sent = await _viewModel.SendMessage(userInput);

            if (!sent)
            {
                MessageBox.Show("Please enter some text before sending.");
            }
            else
            {
                this.UserInputTextBox.Clear();
                this.Title = $"Mark-GPT - Conversation {_viewModel.SelectedConversation.TopicDescription}";
            }
            this.CurrentConversationScrollViewer.ScrollToBottom();
        }

        // Handler for key presses in the input text box to check for Enter key
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                var userInput = this.UserInputTextBox.Text.Trim();
                bool sent = _viewModel.SendMessage(userInput).GetAwaiter().GetResult();

                if (!sent)
                {
                    MessageBox.Show("Please enter some text before sending.");
                }
                else 
                {
                    this.UserInputTextBox.Clear();
                }

                // Display the response in your UI
                e.Handled = true; // Prevents the Enter key from creating a new line
            }
        }

      

   

        // Optional: Handler for application closing event
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MM 6/24/24 nothing necessary here yet. 
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void ExistingConversation_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is TextBlock textBlock)
            {
                var conversationItem = textBlock.DataContext as Conversation;
                if (conversationItem != null)
                {
                    await _viewModel.SelectConversationAsync(conversationItem);
                }
            }
        }

        private void NewConversation_Click(object sender, RoutedEventArgs e)
        {

            this._viewModel.SelectedConversation = new Conversation();
            this._viewModel.StartNewConversation();
        }
    }

}
