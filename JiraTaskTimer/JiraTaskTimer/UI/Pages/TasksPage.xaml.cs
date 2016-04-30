using JiraTaskTimer.Client.Data;
using JiraTaskTimer.Client.Managers;
using JiraTaskTimer.Client.Models;
using JiraTaskTimer.UI.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using JiraTaskTimer.Client.Interface;
using JiraTaskTimer.Client.Jira;

namespace JiraTaskTimer.UI.Pages
{
    /// <summary>
    /// Interaction logic for TasksPage.xaml
    /// </summary>
    public partial class TasksPage : Page
    {
        private readonly JTTaskListProvider taskListProvider = new JTTaskListProvider();
        private readonly JTSerializer serializer = new JTSerializer();
        private readonly ProgramManagerProvider programManagerProvider;
        private readonly JiraServerSync serverSync;
        //TODO Add interface
        private List<JTTProjectModel> projects;

        public TasksPage(ProgramManagerProvider programManagerProvider)
        {
            this.programManagerProvider = programManagerProvider;
            serverSync = new JiraServerSync(programManagerProvider);

            InitializeComponent();

            GetProjects();
            serializer.GetSerialedDataFromDisk(taskListProvider);
            PopulateTaskControlList();
        }

        /// <summary>
        /// Get the list of projects from the program manager, retrieved from the server 
        /// </summary>
        private void GetProjects()
        {
            projects = programManagerProvider.GetProgramManager().GetProjects();
        }


        /// <summary>
        /// Load the TaskControl list from cache
        /// </summary>
        private void PopulateTaskControlList()
        {
            for (int i = 0; i < taskListProvider.GetTimerTasks().taskList.Count; i++)
            {
                CreateTaskControl(taskListProvider.GetTimerTasks().taskList[i]);
            }
        }


        /// <summary>
        /// Add a new JTTask to the list
        /// </summary>
        private void AddItem()
        {
            var task = new JTTask {taskId = taskListProvider.GetTimerTasks().taskList.Count};
            CreateTaskControl(task);
            taskListProvider.GetTimerTasks().taskList.Add(task);
            serializer.SerializeData(taskListProvider);
        }


        /// <summary>
        /// Generate a Task Control
        /// </summary>
        /// <param name="taskItem"></param>
        private void CreateTaskControl(JTTask taskItem)
        {
            var task = new JiraTaskControl(projects, taskListProvider, programManagerProvider, taskItem, serializer);
            TasksViewer.Items.Add(task);
            serverSync.SubscribeTask(task);
        }


        /// <summary>
        /// On add task clicked, add a task
        /// </summary>
        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }
    }
}
