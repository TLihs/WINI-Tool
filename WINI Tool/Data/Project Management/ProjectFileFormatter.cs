// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Project_Management
{
    public class ProjectFileFormatter
    {
        private static readonly string _DEFAULTSAVEFILEPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WINI Tool Projects");

        private string _filePath;
        private Project _project;
        
        private ProjectFileFormatter(Project project)
        {
            _project = project;
        }

        public static ProjectFileFormatter Create(Project project)
        {
            LogDebug("ProjectFileFormatter::[static]Create()");

            if (project == null)
            {
                LogGenericError(new ArgumentNullException(nameof(project)));
                return null;
            }

            return new ProjectFileFormatter(project);
        }

        private void CreateSaveFileTemplate()
        {
            LogDebug("ProjectFileFormatter::CreateSaveFileTemplate()");

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                CheckCharacters = true,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                WriteEndDocumentOnClose = true
            };
            XmlWriter writer = XmlWriter.Create(_filePath, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("WINIToolProject");
            writer.Close();
        }

        public void Save()
        {
            LogDebug("ProjectFileFormatter::Save()");

            if (string.IsNullOrEmpty(_filePath))
            {
                SaveAs();
                return;
            }

            XDocument document = XDocument.Load(_filePath);
            XElement root = document.Root;
            root.RemoveAll();
            root.Add(new XElement("ProjectName", _project.ProjectName));
            root.Add(new XElement("TemplatePath"));
            root.Add(new XElement("TargetPath"));
        }

        public void SaveAs()
        {
            LogDebug("ProjectFileFormatter::SaveAs()");

            if (!Directory.Exists(_DEFAULTSAVEFILEPATH))
                Directory.CreateDirectory(_DEFAULTSAVEFILEPATH);
            
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "WINI Tool Save File (*.wts)||*.wts",
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = "wts",
                CreatePrompt = false,
                OverwritePrompt = true,
                Title = "Save WINI Tool Project As",
                ValidateNames = true,
                InitialDirectory = _DEFAULTSAVEFILEPATH
            };
            if (dialog.ShowDialog() != true)
                return;
            
            _filePath = dialog.FileName;
            CreateSaveFileTemplate();

            Save();
        }
    }
}
