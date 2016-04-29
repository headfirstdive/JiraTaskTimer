using AnotherJiraRestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiraTaskTimer.Client.Events;
using static JiraTaskTimer.Program;
using JiraTaskTimer.Client.Models;

namespace JiraTaskTimer.Client.Interface
{
    public interface IProgram
    {
        void Login(IUser tempUser, LoginCallback callback);
        List<JTTProjectModel> GetProjects();
        Worklog LogTime(string issueKey, string timeSpentFormatted);
        bool UpdateLogTime(string issueKey, string timeSpentFormatted, string worklogId);
        void GetWorkLogs(string issueKey);
    }
}
