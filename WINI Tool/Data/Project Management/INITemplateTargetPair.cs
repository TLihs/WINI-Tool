// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WINI_Tool.Data.Base;
using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Project_Management
{
    public class INITemplateTargetPair
    {
        private INIReader _templateReader;
        private List<INIReader> _targetReaders;

        public INIReader TemplateReader => _templateReader;
        public INIReader TargetReader(int index) => _targetReaders[index];
        public INIReader[] TargerReaders => _targetReaders.ToArray();
        public bool StandaloneTemplate => _targetReaders.Count == 0;

        private INITemplateTargetPair(INIReader templateReader, List<INIReader> targetReaders = null)
        {
            _templateReader = templateReader;
            if (targetReaders != null)
                _targetReaders = targetReaders;
            else
                _targetReaders = new List<INIReader>();
        }

        public static INITemplateTargetPair Create(string templatePath, List<string> targetPaths = null)
        {
            LogDebug("INITemplateTargetPair::[static]Create({0}, ..)", templatePath);

            bool createFailed = false;

            if (!File.Exists(templatePath))
            {
                LogGenericError("Template file not found: '{0}'", templatePath);
                createFailed = true;
            }
            
            INIReader templateReader = INIReader.Create(templatePath);
            List<INIReader> targetReaders = null;

            if (targetPaths != null && targetPaths.Count > 0)
            {
                foreach (string path in targetPaths)
                {
                    if (!File.Exists(path))
                    {
                        LogWarning("Target file not found: '{0}'", path);
                    }
                    else
                    {
                        INIReader targetReader = INIReader.Create(path);

                        if (targetReader != null)
                        {
                            if (targetReaders == null)
                                targetReaders = new List<INIReader>();

                            targetReaders.Add(targetReader);
                        }
                    }
                }

                if (targetReaders == null)
                {
                    LogGenericError("None of the target files found");
                    targetReaders = null;
                    createFailed = true;
                }
            }

            INITemplateTargetPair templatetargetpair = null;
            
            if (!createFailed)
                templatetargetpair = new INITemplateTargetPair(templateReader, targetReaders);

            return templatetargetpair;
        }
    }
}
