using JiraTaskTimer.Client.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using JiraTaskTimer.Client.Events;
using JiraTaskTimer.Client.Interface;
using JiraTaskTimer.Properties;

namespace JiraTaskTimer.UI.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly IProgramManagerProvider programManagerProvider;
        private readonly IPageManagerProvider pageManagerProvider;

        public LoginPage(IProgramManagerProvider programManagerProvider, IPageManagerProvider pageManagerProvider)
        {
            this.programManagerProvider = programManagerProvider;
            this.pageManagerProvider    = pageManagerProvider;
            InitializeComponent();
            InitializeControls();
        }

        /// <summary>
        /// Set initial state of the login controls
        /// </summary>
        private void InitializeControls()
        {
            serverField.Text = Settings.Default.jiraurl;
            usernameField.Text = Settings.Default.username;
            passwordField.Password = Settings.Default.password != string.Empty 
                ? DPAPI.Decrypt(Settings.Default.password) 
                : Settings.Default.password;
        }

        /// <summary>
        /// When the login button is clicked
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            WaitingForProgress();
            if (string.IsNullOrEmpty(usernameField.Text) 
                || string.IsNullOrEmpty(passwordField.Password)) return;
            // Call DPAPI to encrypt data with user-specific key.
            var encryptedPassword = DPAPI.Encrypt(DPAPI.KeyType.UserKey,
                passwordField.Password);

            Settings.Default.jiraurl    = serverField.Text;
            Settings.Default.username   = usernameField.Text;
            Settings.Default.password   = encryptedPassword;
            Settings.Default.Save();

            programManagerProvider.GetProgramManager()
                .Login(new User(usernameField.Text, passwordField.Password, serverField.Text), OnUserLoginComplete);
        }

        /// <summary>
        /// On login status update, handle the callback
        /// </summary>
        /// <param name="status"></param>
        private void OnUserLoginComplete(LoginStatus status)
        {
            switch (status)
            {
                case LoginStatus.Success:
                    var taskPage = new TasksPage(programManagerProvider);
                    pageManagerProvider.GetPageManager().SetPage(taskPage);
                    break;
                case LoginStatus.Failed:
                    ReturnControl();
                    //TODO Add proper error handling
                    Console.WriteLine("Error");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        /// <summary>
        /// Wait for any process to be complete
        /// Lock the controls from the user
        /// </summary>
        private void WaitingForProgress()
        {
            LoginButton.IsEnabled = false;
            //TODO add time-out and error functionality
        }

        /// <summary>
        /// Give the user back control
        /// </summary>
        private void ReturnControl()
        {
            LoginButton.IsEnabled = true;
        }
    }
}
