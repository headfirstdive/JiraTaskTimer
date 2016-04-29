using JiraTaskTimer.Client.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiraTaskTimer.Client.Managers
{
    public class JTSerializer
    {
        private const string xmlSerializedFileName = "data.xml";


        public void SerializeData(JTTaskListProvider taskListProvider)
        {
            XmlSerializer ser = new XmlSerializer(typeof(JTTaskList));
            TextWriter writer = new StreamWriter(xmlSerializedFileName);
            ser.Serialize(writer, taskListProvider.GetTimerTasks());
            writer.Close();
        }


        public bool GetSerialedDataFromDisk(JTTaskListProvider taskListProvider)
        {
            if (!File.Exists(xmlSerializedFileName)) return false;
            Stream xmlStream = new FileStream(xmlSerializedFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            XmlSerializer ser = new XmlSerializer(typeof(JTTaskList));
            var taskTimerList = (JTTaskList)ser.Deserialize(xmlStream);
            taskListProvider.SetTimerTaskList(taskTimerList);

            xmlStream.Close();
            return true;
        }
    }
}
