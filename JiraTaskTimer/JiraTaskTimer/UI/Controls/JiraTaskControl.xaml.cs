using AnotherJiraRestClient;
using JiraTaskTimer.Client.Data;
using JiraTaskTimer.Client.Managers;
using JiraTaskTimer.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JiraTaskTimer.Time;

namespace JiraTaskTimer.UI.Controls
{
    internal enum RecordingState
    {
        Idle,
        Recording
    }

    /// <summary>
    /// Interaction logic for JiraTask.xaml
    /// </summary>
    public partial class JiraTaskControl : UserControl
    {
        public JTTask taskData { get { return taskItem; } }

        private readonly List<JTTProjectModel> projectModels;
        private readonly JTTaskListProvider timerTaskProvider;
        private readonly ProgramManagerProvider programManagerProvider;
        private readonly JTTask taskItem;
        private readonly JTSerializer taskSerializer;
        private readonly JiraStopWatch jiraStopWatch = new JiraStopWatch();

        private RecordingState recording = RecordingState.Idle;
        private Issue selectedIssue;
        private Worklog activeWorklog;

        public JiraTaskControl(
            List<JTTProjectModel> projectModels, 
            JTTaskListProvider timerTaskProvider, 
            ProgramManagerProvider programManagerProvider,
            JTTask taskItem, 
            JTSerializer taskSerializer)
        {
            this.projectModels = projectModels;
            this.timerTaskProvider = timerTaskProvider;
            this.programManagerProvider = programManagerProvider;
            this.taskItem = taskItem;
            this.taskSerializer = taskSerializer;

            InitializeComponent();
            Setup();
            AddProjectsToMenu();
        }

        private void Setup()
        {
            UpdateTimerLabel(0, 0, 0);
        }


        public void SetInactive()
        {
            timerLabel.Content = "- - -";
            IsEnabled = false;
        }


        /// <summary>
        /// Use the ProjectModels retrieved on startup to set all the options for the ProjectMenu
        /// </summary>
        void AddProjectsToMenu()
        {
            ProjectMenu.ItemsSource         = projectModels;
            ProjectMenu.DisplayMemberPath   = "project.name";
            // Set the cached item if there is a cached project in the taskItem
            if (!string.IsNullOrEmpty(taskItem.projectId))
                SetSelectedProjectFromCache();
        }


        /// <summary>
        /// Set the list of issues for the issues menu
        /// </summary>
        private void AddIssuesToMenu(Issues issuesList)
        {
            IssueMenu.ItemsSource       = issuesList.issues;
            IssueMenu.DisplayMemberPath = "fields.summary";
            if (!string.IsNullOrEmpty(taskItem.issueId))
                SetSelectedIssueFromCache(issuesList);
        }


        /// <summary>
        /// Get the list of issues based on the selected project
        /// </summary>
        /// <param name="selectedProject"></param>
        private void GetIssuesList(Project selectedProject)
        {
            var issuesList = projectModels.First(x => x.project.name == selectedProject.name).issuesList;
            AddIssuesToMenu(issuesList);
        }

        /// <summary>
        /// If there is serialized data for projects in this task control, set its value from the xml
        /// </summary>
        private void SetSelectedProjectFromCache()
        {
            var selectedProjectModel = projectModels.FirstOrDefault(o => o.project.id == taskItem.projectId);
            if (selectedProjectModel == null) return;
            ProjectMenu.SelectedItem = selectedProjectModel;
            GetIssuesList(selectedProjectModel.project);
        }

        
        /// <summary>
        /// If there is serialized data for issues in this task control, set its value from xml
        /// </summary>
        /// <param name="issuesList"></param>
        private void SetSelectedIssueFromCache(Issues issuesList)
        {
            var selectedIssueFromCache = issuesList.issues.FirstOrDefault(o => o.id == taskItem.issueId);
            if (selectedIssueFromCache == null) return;
            IssueMenu.SelectedItem = selectedIssueFromCache;
        }


        /// <summary>
        /// When the user selects an option from the projects menu
        /// load the issues list and update the serialized data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProjectSelected(object sender, SelectionChangedEventArgs e)
        {
            var projectModel = ProjectMenu.SelectedItem as JTTProjectModel;
            if(projectModel == null) return;
            taskItem.projectId = projectModel.project.id;
            GetIssuesList(projectModel.project);
            OnDataChanged();
        }


        /// <summary>
        /// When the user selects an option from the issues menu
        /// update the serialized data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIssueSelected(object sender, SelectionChangedEventArgs e)
        {
            selectedIssue = IssueMenu.SelectedItem as Issue;
            if (selectedIssue == null) return;
            taskItem.issueId = selectedIssue.id;
            OnDataChanged();
        }


        /// <summary>
        /// When the data changes, update the task list and serialized it
        /// </summary>
        private void OnDataChanged()
        {
            timerTaskProvider.UpdateTaskInList(taskItem);
            taskSerializer.SerializeData(timerTaskProvider);
        }

        /// <summary>
        /// When the record button is clicked toggle between start and stop
        /// </summary>
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            //programManagerProvider.GetProgramManager().GetWorkLogs(selectedIssue.key);
            //OnServerUpdate("12m");
            if (recording == RecordingState.Idle) StartRecording();
            else if(recording == RecordingState.Recording) StopRecording();
        }


        private void StartRecording()
        {
            recording = RecordingState.Recording;
            jiraStopWatch.StartTimer(OnTimerTick, OnServerUpdate);
        }


        private void StopRecording()
        {
            recording = RecordingState.Idle;
            jiraStopWatch.StopTimer();
        }


        private void OnTimerTick(TimeSpan elapsedTime)
        {
            UpdateTimerLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }


        private void UpdateTimerLabel(int hours, int minutes, int seconds)
        {
            timerLabel.Content = $"{hours}h : {minutes}m : {seconds}s";
        }


        private void OnServerUpdate(string elapsedTimeFormatted)
        {
            Console.WriteLine("elapsedTimeFormatted: " + elapsedTimeFormatted);
            if (activeWorklog == null)
                activeWorklog = programManagerProvider.GetProgramManager()
                    .LogTime(selectedIssue.key, elapsedTimeFormatted);
            else
            {
                var updateSuccess = programManagerProvider.GetProgramManager()
                    .UpdateLogTime(selectedIssue.key, elapsedTimeFormatted, activeWorklog.id);
            }
        }

    }
}
