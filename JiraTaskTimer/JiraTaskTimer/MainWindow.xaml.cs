using JiraTaskTimer.Client.Data;
using JiraTaskTimer.Client.Interface;
using JiraTaskTimer.Client.Managers;
using JiraTaskTimer.Client.Models;
using JiraTaskTimer.UI.Pages;
using NSubstitute;
using System.Windows;
using System.Windows.Controls;

namespace JiraTaskTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProgramManagerProvider programProvider     = new ProgramManagerProvider();
        private readonly PageManagerProvider pageManagerProvider    = new PageManagerProvider();

        public MainWindow()
        {
            InitializeComponent();

            var loginPage = new LoginPage(programProvider, pageManagerProvider);
            pageManagerProvider.SetPageManager(frame);
            pageManagerProvider.GetPageManager().SetPage(loginPage);
        }

    }
}
