using JiraTaskTimer.Client.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using JiraTaskTimer.Client.Events;
using JiraTaskTimer.Client.Data;

namespace JiraTaskTimer.UI.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly ProgramManagerProvider programManagerProvider;
        private readonly PageManagerProvider pageManagerProvider;

        public LoginPage(ProgramManagerProvider programManagerProvider, PageManagerProvider pageManagerProvider)
        {
            this.programManagerProvider = programManagerProvider;
            this.pageManagerProvider    = pageManagerProvider;
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            //TODO add time-out and error functionality
#if DEBUG
            if (string.IsNullOrEmpty(usernameField.Text) || string.IsNullOrEmpty(passwordField.Password))
            {
                programManagerProvider.GetProgramManager().Login(new User("user", "password"), OnUserLoginComplete);
            }
            else
            {
                programManagerProvider.GetProgramManager()
                  .Login(new User(usernameField.Text, passwordField.Password), OnUserLoginComplete);
            }
#elif !DEBUG
            if (!string.IsNullOrEmpty(usernameField.Text) && !string.IsNullOrEmpty(passwordField.Password))
            {
                programManagerProvider.GetProgramManager()
                    .Login(new User(usernameField.Text, passwordField.Password), OnUserLoginComplete);
            }
#endif
        }


        private void OnUserLoginComplete(LoginStatus status)
        {
            switch (status)
            {
                case LoginStatus.Success:
                    var taskPage = new TasksPage(programManagerProvider);
                    pageManagerProvider.GetPageManager().SetPage(taskPage);
                    break;
                case LoginStatus.Failed:
                    Console.WriteLine("Error");
                    break;
            }
        }
    }
}
