// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Controls;
using WINI_Tool.Controls;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Base
{
    public class INIReader
    {
        private string _filePath;
        private LineContentBase _lineContent;
        private INIContentControl _contentControl;
        private FileSystemWatcher _watcher;

        public string FilePath => _filePath;
        public INIContentControl ContentControl => _contentControl;

        private INIReader(string filePath)
        {
            _filePath = filePath;
        }

        public static INIReader Create(string filePath)
        {
            INIReader reader = new INIReader(filePath);

            if (!reader.LoadFile(filePath))
                return null;

            return reader;
        }

        public void Unload()
        {

        }

        private bool LoadFile(string filePath)
        {
            LogDebug(string.Format("INIReader::LoadFile({0})", filePath));

            if (!File.Exists(filePath))
            {

                return false;
            }

            if (new FileInfo(filePath).Extension != "ini")
            {

                return false;
            }

            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));
            LineContentBase lastlinecontent = null;
            List<string> commentBuffer = new List<string>();
            long position = 0;
            
            for (long linenumber = 0; linenumber < lines.Length; linenumber++)
            {
                string line = lines[linenumber];

                if (_lineContent == null)
                {
                    _lineContent = new LineContentBase(this, position, line, null, null);
                }
                else
                {
                    LineContentBase templinecontent = lastlinecontent;
                    lastlinecontent = new LineContentBase(this, position, line, null, null);

                    if (templinecontent == null)
                        _lineContent.NextContent = lastlinecontent;
                    else
                        templinecontent.NextContent = lastlinecontent;
                }

                position += line.Length + ("\r\n").Length;
            }

            _filePath = filePath;

            FileInfo info = new FileInfo(filePath);
            _watcher = new FileSystemWatcher(info.Directory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                Filter = info.Name,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size
            };
            _watcher.Changed += OnFileChanged;
            
            return true;
        }

        private bool ReloadFile()
        {
            LogDebug("INIReader::ReloadFile()");

            return false;
        }

        public void SetContentControl(Control control)
        {
            LogDebug("INIReader::SetContentControl(..)");

            if (control == null)
            {
                if (_contentControl != null)
                    _contentControl.UnloadReader();

                return;
            }
            
            if (control.GetType() == typeof(INIContentControl))
            {
                if (!LoadControl((INIContentControl)control))
                    LogWarning("INIReader::SetContentControl(..) - Control could not be set.");
            }
            else
                LogWarning("INIReader::SetContentControl(..) - Control is not of any supported type.");
        }

        private bool LoadControl(INIContentControl control)
        {
            LogDebug("INIReader::LoadControl(..)");

            if (control == null)
                return false;

            return true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            LogDebug("INIReader::OnFileChanged(..)");

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Deleted:
                    Unload();
                    break;
                case WatcherChangeTypes.Renamed:
                    _filePath = e.FullPath;
                    _watcher.Filter = _filePath;
                    break;
                case WatcherChangeTypes.Changed:
                    ReloadFile();
                    break;
            }
        }

        public INISection GetSection(string sectionName)
        {
            return _lineContent.GetSection(sectionName);
        }

        public long GetValueFromSection(string sectionName, string key, int defaultValue)
        {
            INISection section = GetSection(sectionName);
            return section != null ? section.GetValue(key, defaultValue) : defaultValue;
        }

        public double GetValueFromSection(string sectionName, string key, double defaultValue)
        {
            INISection section = GetSection(sectionName);
            return section != null ? section.GetValue(key, defaultValue) : defaultValue;
        }

        public string GetValueFromSection(string sectionName, string key, string defaultValue)
        {
            INISection section = GetSection(sectionName);
            return section != null ? section.GetValue(key, defaultValue) : defaultValue;
        }

        public bool GetValueFromSection(string sectionName, string key, bool defaultValue)
        {
            INISection section = GetSection(sectionName);
            return section != null ? section.GetValue(key, defaultValue) : defaultValue;
        }
    }
}
