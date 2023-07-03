// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Project_Management
{
    public static class ProjectManager
    {
        public static string DefaultProjectsDirectory { get; }
        public static List<Project> Projects = new List<Project>();
        public static Project ActiveProject { get; private set; }

        static ProjectManager()
        {
            LogDebug("ProjectManager::[static]ProjectManager()");
            string projectsdirpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WINI Tools", "Projects");
            if (!Directory.Exists(projectsdirpath))
                try
                {
                    Directory.CreateDirectory(projectsdirpath);
                    DefaultProjectsDirectory = projectsdirpath;
                }
                catch (Exception ex)
                {
                    LogGenericError(ex);
                    DefaultProjectsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                }
        }

        public static Project CreateNewProject(string projectPath, string projectName, string templatePath, string[] targetPaths)
        {
            LogDebug("ProjectManager::[static]CreateNewProject({0}, {1}, {2}, [{3}])", projectPath, projectName, templatePath, string.Join(",", targetPaths));
            bool createFailed = false;
            Project project = Project.Create(projectPath, projectName, templatePath, targetPaths);

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
            Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "ProjectManager::[static]LoadProject()");
            return;

            LogDebug("ProjectManager::[static]ProjectManager()");

        }

        public static void SetActiveProject(Project project)
        {
            LogDebug("ProjectManager::[static]SetActiveProject({0})", project == null ? "null" : project.ProjectName);
            ActiveProject = project;
        }

        public static void SearchText(string text, bool caseSensitive = false, bool wholeWordOnly = false)
        {
            Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "ProjectManager::[static]SearchText()");
            return;

            LogDebug("ProjectManager::[static]SearchText({0}, {1}, {2})",
                text, caseSensitive ? "case sensitive" : "case insensitive",
                wholeWordOnly ? "whole word only" : "all occurences");

        }

        public static void SearchAndFocusText(string text, bool caseSensitive = false, bool wholeWordOnly = false, bool fromCaretOnly = false)
        {
            Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "ProjectManager::[static]SearchAndFocusText()");
            return;

            LogDebug("ProjectManager::[static]SearchAndFocusText({0}, {1}, {2}, {3})",
                text, caseSensitive ? "case sensitive" : "case insensitive",
                wholeWordOnly ? "whole word only" : "all occurences",
                fromCaretOnly ? "only from caret to end" : "whole scope");

        }
    }
}
