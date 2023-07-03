// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Project_Management
{
    public class ProjectSettings
    {
        public Project Project { get; }

        private ProjectSettings(Project project)
        {
            Project = project;
        }

        public static ProjectSettings Create(Project project)
        {
            LogDebug("ProjectSettings::[static]Create({0})", project.ProjectName);

            bool createFailed = false;

            if (project == null)
                createFailed = true;

            return createFailed ? null : new ProjectSettings(project);
        }
    }
}
