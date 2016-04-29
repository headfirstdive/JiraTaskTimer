using JiraTaskTimer.Client.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JiraTaskTimer.Client.Data
{
    public class PageManagerProvider
    {
        private static PageManager pageManager;

        public PageManager SetPageManager(Frame targetFrame)
        {
            pageManager = new PageManager(targetFrame);
            return pageManager;
        }

        public PageManager GetPageManager()
        {
            return pageManager;
        }
    }
}
