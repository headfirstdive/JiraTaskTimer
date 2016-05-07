using AnotherJiraRestClient;

namespace JiraTaskTimer.Client.Interface
{
    public interface IJTTProjectModel
    {
        Issues issuesList { get; }
        Project project { get; }
    }
}