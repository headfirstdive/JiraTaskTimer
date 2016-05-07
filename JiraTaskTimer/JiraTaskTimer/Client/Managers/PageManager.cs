using System.Windows.Controls;
using JiraTaskTimer.Client.Interface;

namespace JiraTaskTimer.Client.Managers
{
    public class PageManager : IPageManager
    {
        private readonly Frame activeWindowFrame;
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
