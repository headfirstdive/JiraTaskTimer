
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JiraTaskTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnSessionEnding(object sender, ExitEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
