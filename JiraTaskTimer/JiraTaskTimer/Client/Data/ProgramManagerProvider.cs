﻿using JiraTaskTimer.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTaskTimer.Client.Data
{
    public class ProgramManagerProvider : IProgramManagerProvider
    {
        private readonly IProgram programManager = new Program();

        public IProgram GetProgramManager()
        {
            return programManager;
        }
    }
}
