using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Interface
{
    public interface ITimerTask
    {
        string projectId    { get; set; }
        string taskId       { get; set; }
    }
}
