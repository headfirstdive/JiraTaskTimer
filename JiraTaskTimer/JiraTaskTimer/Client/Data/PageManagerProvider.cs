using JiraTaskTimer.Client.Managers;
using System.Windows.Controls;
using JiraTaskTimer.Client.Interface;

namespace JiraTaskTimer.Client.Data
{
    public class PageManagerProvider : IPageManagerProvider
    {
        private IPageManager pageManager;

        public void SetPageManager(Frame targetFrame)
        {
            pageManager = new PageManager(targetFrame);
        }

        public IPageManager GetPageManager()
        {
            return pageManager;
        }
    }
}
