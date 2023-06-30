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
    public static class ProjectManager
    {
        public static List<Project> Projects = new List<Project>();
        public static Project ActiveProject { get; private set; }

        static ProjectManager()
        {

        }

        public static Project CreateNewProject(string projectPath, string projectName)
        {
            Project project = Project.Create(projectPath, projectName, true);
            bool createFailed = false;

            if (project == null)
            {
                LogGenericError("ProjectManager::CreateNewProject({0}) - Project couldn't be created.", projectPath);
                createFailed = true;
            }

            if (!createFailed)
            {
                Projects.Add(project);
                SetActiveProject(project);
            }
            return project;
        }

        public static void LoadProject(string filePath)
        {

        }

        public static void SetActiveProject(Project project)
        {
            ActiveProject = project;
        }

        public static void SearchText(string text, bool caseSensitive = false, bool wholeWordOnly = false)
        {

        }

        public static void SearchAndFocusText(string text, bool caseSensitive = false, bool wholeWordOnly = false, bool fromCaretOnly = false)
        {

        }
    }
}
