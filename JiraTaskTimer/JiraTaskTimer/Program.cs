using AnotherJiraRestClient;
using JiraTaskTimer.Client.Events;
using JiraTaskTimer.Client.Interface;
using JiraTaskTimer.Client.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer
{
    public class Program : IProgram
    {
        private JiraClient jiraClient;
        //private List<Project> projects;
        private List<JTTProjectModel> projectData = new List<JTTProjectModel>();

        private LoginCallback OnLoginCallback;

        public void Login(IUser tempUser, LoginCallback callback)
        {
            var account = new JiraAccount(ResourceProperties.serverUrl, tempUser.Username, tempUser.Password);
            jiraClient = new JiraClient(account);
            var mergedCredentials = $"{tempUser.Username}:{tempUser.Password}";
            var byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
            var encodedCredentials = Convert.ToBase64String(byteCredentials);
            OnLoginCallback = callback;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Set("Authorization", "Basic " + encodedCredentials);
                webClient.DownloadStringAsync(new Uri(ResourceProperties.serverUrl));
                webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            }
        }


        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                OnLoginFailed();
            else
                OnLoginSuccess();
        }


        private void OnLoginFailed()
        {
            OnLoginCallback?.Invoke(LoginStatus.Failed);
        }


        private async void OnLoginSuccess()
        {
            List<Project> projects = await Task.Run(() => GetProjectList());
            GetIssuesByProject(projects);
            OnLoginCallback?.Invoke(LoginStatus.Success);
        }


        private List<Project> GetProjectList()
        {
            return jiraClient.GetProjects();
        }

        //private async Task<List<Project>> GetProjectListAsync()
        //{
        //    return await GetProjects();
        //}


        private void GetIssuesByProject(List<Project> projects)
        {
            foreach (var project in projects)
            {
                // TODO: use pagination to control the number of issues returned
                Issues issuesList = jiraClient.GetIssuesByProject(project.key, 0, 100);
                projectData.Add(new JTTProjectModel(project, issuesList));
            }
        }


        public List<JTTProjectModel> GetProjects()
        {
            return projectData;
        }


        public Worklog LogTime(string issueKey, string timeSpentFormatted)
        {
            const string comment = "I've worked on an issue";
            return jiraClient.AddWorkLog(issueKey, timeSpentFormatted, comment);
        }


        public bool UpdateLogTime(string issueKey, string timeSpentFormatted, string worklogId)
        {
            const string comment = "I've worked on an issue";
            return jiraClient.UpdateWorklogById(issueKey, worklogId, timeSpentFormatted, comment);
        }


        public void GetWorkLogs(string issueKey)
        {
            jiraClient.GetWorklogs(issueKey);
        }
    }
}
