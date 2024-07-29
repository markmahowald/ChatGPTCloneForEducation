using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using ChatGPTForPersonalEducation_WPF.Services;
using System.Net.Http;

namespace ChatGPTForPersonalEducation_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = new MainWindow(ServiceProvider);
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ChatService>(client =>
            {
                client.BaseAddress = new Uri("http://your-api-base-url/"); // Replace with your API base URL
            });

            services.AddTransient<MainWindow>();
        }
    }
}