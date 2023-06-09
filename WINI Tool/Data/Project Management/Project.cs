﻿// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Project_Management
{
    public class Project
    {
        public INITemplateTargetPair TemplateTargetPair { get; private set; }
        public string ProjectName { get; set; }
        public ProjectSettings Settings { get; }

        private Project(string projectPath, string projectName, string iniTemplatePath, string[] iniTargetPaths)
        {

        }

        public static Project Create(string projectPath, string projectName, string iniTemplatePath, string[] iniTargetPaths)
        {
            LogDebug("Project::[static]Create({0}, {1}, {2})", projectPath, projectName);

            Project project = null;
            bool createFailed = false;

            if (!Uri.IsWellFormedUriString(projectPath, UriKind.RelativeOrAbsolute))
            {
                LogGenericError("Invalid path format: '{0}'", projectPath);
                createFailed = true;
            }
            else if (!File.Exists(projectPath))
            {
                File.Create(projectPath);
            }

            if (!createFailed)
                project = new Project(projectPath, projectName, iniTemplatePath, iniTargetPaths);

            return project;
        }

        public static Project Load(string projectFilePath)
        {
            LogDebug("Project::[static]Load({0})", projectFilePath);

            Project project = null;
            bool loadFailed = false;

            if (!File.Exists(projectFilePath))
            {
                LogGenericError(new FileNotFoundException("Project file not found: " + projectFilePath));
                loadFailed = true;
            }

            if (!loadFailed)
                project = ProjectFileFormatter.Load(projectFilePath);

            return project;
        }
    }
}
