using JiraTaskTimer.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Models
{
    public class JTTask 
    {
        public int taskId { get; set; }
        public string projectId { get; set; }
        public string issueId   { get; set; }
    }
}
