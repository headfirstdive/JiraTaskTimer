using JiraTaskTimer.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Models
{
    public class User : IUser
    {
        public string Server { get; }
        public string Username { get; }
        public string Password { get; }

        public User(string username, string password, string server)
        {
            Username = username;
            Password = password;
            Server = server;
        }
    }
}
