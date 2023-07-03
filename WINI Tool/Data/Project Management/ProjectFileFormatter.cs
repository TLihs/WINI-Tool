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
using WINI_Tool.Data.Base;

using static WINI_Tool.Support.ExceptionHandling;
using static WINI_Tool.Support.XMLHandling;

namespace WINI_Tool.Data.Project_Management
{
    public class ProjectFileFormatter
    {
        private static readonly string _DEFAULTSAVEFILEPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WINI Tool Projects");

        public string FilePath { get; private set; }
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
            XmlWriter writer = XmlWriter.Create(FilePath, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("WINIToolProject");
            writer.Close();
        }

        public void Save()
        {
            LogDebug("ProjectFileFormatter::Save()");

            if (string.IsNullOrEmpty(FilePath))
            {
                SaveAs();
                return;
            }

            XDocument document = XDocument.Load(FilePath);
            XElement root = document.Root;
            root.RemoveAll();
            root.Add(new XElement("ProjectName", _project.ProjectName));
            root.Add(new XElement("TemplatePath", _project.TemplateTargetPair.TemplateReader.FilePath));
            XElement targetpaths = new XElement("Targets");
            foreach (INIReader reader in _project.TemplateTargetPair.TargerReaders)
                targetpaths.Add(new XElement("TargetPath", reader.FilePath));
            root.Add(targetpaths);
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
            
            FilePath = dialog.FileName;
            CreateSaveFileTemplate();

            Save();
        }

        public static Project Load(string filePath)
        {
            LogDebug("ProjectFileFormatter::Load({0})", filePath);

            if (!File.Exists(filePath))
            {
                LogGenericError(new FileNotFoundException("File not found: '" + filePath + "'"));
                return null;
            }
            
            XDocument document = XDocument.Load(filePath);
            XElement root = document.Root;
            string name = TryReadValue(root, "ProjectName", "");
            string templatepath = TryReadValue(root, "TemplatePath", "");
            List<string> targetpaths = TryReadListValue(root, "Targets", "TargetPath", "");

            return Project.Create(filePath, name, templatepath, targetpaths.ToArray());
        }
    }
}
