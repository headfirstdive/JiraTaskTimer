using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using JiraTaskTimer.Client.Data;
using JiraTaskTimer.Client.Models;
using JiraTaskTimer.UI.Controls;

namespace JiraTaskTimer.Client.Jira
{
    public class JiraServerSync
    {
        private readonly List<JiraTaskControl> activeTaskControls = new List<JiraTaskControl>();
        private readonly ProgramManagerProvider programManagerProvider;
        private readonly DispatcherTimer syncIntervalTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 5) };

        private List<JTTProjectModel> projectDataCached;

        public JiraServerSync(ProgramManagerProvider programManagerProvider)
        {
            this.programManagerProvider = programManagerProvider;
            Start();
        }

        /// <summary>
        /// Add a task control to the list of tasks that want to get updates from the server
        /// </summary>
        /// <param name="taskControl"></param>
        public void SubscribeTask(JiraTaskControl taskControl)
        {
            activeTaskControls.Add(taskControl);
        }

        /// <summary>
        /// Remove a task control from the server sync list
        /// </summary>
        /// <param name="taskControl"></param>
        public void UnsubscribeTask(JiraTaskControl taskControl)
        {
            activeTaskControls.Remove(taskControl);
        }

        /// <summary>
        /// Start the Server syncing interval
        /// </summary>
        private void Start()
        {
            syncIntervalTimer.Tick += OnSyncInterval;
            syncIntervalTimer.Start();
        }

        /// <summary>
        /// Automatically call sync
        /// </summary>
        private void OnSyncInterval(object sender, EventArgs e)
        {
            Sync();
        }

        /// <summary>
        /// Sync the client with data on jira servers
        /// </summary>
        public void Sync()
        {
            projectDataCached = programManagerProvider.GetProgramManager().GetProjects();
            programManagerProvider.GetProgramManager().UpdateProjectData(OnProjectDataReceived);
        }


        /// <summary>
        /// Sync the controls when the data comes back from the server
        /// </summary>
        /// <param name="projectData"></param>
        private void OnProjectDataReceived(List<JTTProjectModel> projectData)
        {
            // Check the local project data for any projects that are missing or new from the server
            var mismatchedProjects = projectDataCached
                .Where(x => projectData
                .All(o => o.project.id != x.project.id));
            UpdateMismatchedTaskByProjects(mismatchedProjects);

            // TODO Clean up messy nested foreach. Use Linq
            // Check to see if any of the task controls have outdated issues
            foreach (var taskControl in activeTaskControls)
            {
                bool taskIssueMissing = true;
                foreach (var data in projectData)
                {
                    if (taskControl.taskData.projectId == data.project.id)
                    {
                        foreach (var issue in data.issuesList.issues)
                        {
                            if (taskControl.taskData.issueId == issue.id)
                                taskIssueMissing = false;
                        }
                    }
                }
                if(taskIssueMissing) 
                    taskControl.SetInactive();
            }
        }

        /// <summary>
        /// Find the task controls that have id's in the mismatched projects list
        /// </summary>
        private void UpdateMismatchedTaskByProjects(IEnumerable<JTTProjectModel> mismatchedProjects)
        {
            foreach (var mismatchedProject in mismatchedProjects)
            {
                var matchingTaskControl = activeTaskControls
                    .FirstOrDefault(x => x.taskData.projectId == mismatchedProject.project.id);
                matchingTaskControl?.SetInactive();
            }
        }
    }
}
