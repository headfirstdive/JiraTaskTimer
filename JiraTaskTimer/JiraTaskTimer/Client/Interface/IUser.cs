﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Interface
{
    public interface IUser
    {
        string Username { get; }
        string Password { get; }
    }
}
