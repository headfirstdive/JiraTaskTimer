using AnotherJiraRestClient;
using JiraTaskTimer.Client.Interface;

namespace JiraTaskTimer.Client.Models
{
    /// <summary>
    /// Class that holds info for a project and its issues
    /// </summary>
    public class JTTProjectModel : IJTTProjectModel
    {
        public Project project { get; private set; }
        public Issues issuesList { get; private set; }

        public JTTProjectModel(Project project, Issues issuesList)
        {
            this.project = project;
            this.issuesList = issuesList;
        }
    }
}
