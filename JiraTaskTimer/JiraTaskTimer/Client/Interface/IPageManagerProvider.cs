using System.Windows.Controls;

namespace JiraTaskTimer.Client.Interface
{
    public interface IPageManagerProvider
    {
        void SetPageManager(Frame targetFrame);
        IPageManager GetPageManager();
    }
}
