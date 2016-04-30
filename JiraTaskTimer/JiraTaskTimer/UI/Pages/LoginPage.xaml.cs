using JiraTaskTimer.Client.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using JiraTaskTimer.Client.Events;
using JiraTaskTimer.Client.Data;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            InitializeControls();
        }

        /// <summary>
        /// Set initial state of the login controls
        /// </summary>
        private void InitializeControls()
        {
            usernameField.Text = Properties.Settings.Default.username;
            usernameField.Text = Properties.Settings.Default.password != string.Empty 
                ? DPAPI.Decrypt(Properties.Settings.Default.password) 
                : Properties.Settings.Default.password;
        }

        /// <summary>
        /// When the login button is clicked
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            //TODO add time-out and error functionality

            if (string.IsNullOrEmpty(usernameField.Text) 
                || string.IsNullOrEmpty(passwordField.Password)) return;
            // Call DPAPI to encrypt data with user-specific key.
            var encryptedPassword = DPAPI.Encrypt(DPAPI.KeyType.UserKey,
                passwordField.Password);

            Properties.Settings.Default.username = usernameField.Text;
            Properties.Settings.Default.password = encryptedPassword;

            programManagerProvider.GetProgramManager()
                .Login(new User(usernameField.Text, passwordField.Password), OnUserLoginComplete);
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
                    Console.WriteLine("Error");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
