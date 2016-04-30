using JiraTaskTimer.Client.Interface;
using JiraTaskTimer.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Data
{
    public class JTTaskListProvider
    {
        private static JTTaskList timerTasks = new JTTaskList(); 

        public JTTaskList GetTimerTasks()
        {
            return timerTasks;
        }


        public void SetTimerTaskList(JTTaskList tasksList)
        {
            timerTasks = tasksList;
        }


        public void UpdateTaskInList(JTTask task)
        {
            for (var i = 0; i < timerTasks.taskList.Count; i++)
            {
                if (task.taskId != timerTasks.taskList[i].taskId) continue;
                timerTasks.taskList[i] = task;
                break;
            }
        }

    }
}
