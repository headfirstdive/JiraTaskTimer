using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JiraTaskTimer.Client.Managers
{
    public class PageManager
    {
        readonly private Frame activeWindowFrame;

        private Page activePage;

        public PageManager(Frame targetFrame)
        {
            activeWindowFrame = targetFrame;
        }


        public void SetPage(Page page)
        {
            activePage = page;
            activeWindowFrame.Content = page;
        }

    }
}
