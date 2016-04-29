using AnotherJiraRestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Models
{
    /// <summary>
    /// Class that holds info for a project and its issues
    /// </summary>
    public class JTTProjectModel
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
