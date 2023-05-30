using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using WINI_Tool.Controls;

using static WINI_Tool.Support.Constants;

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

        private INIReader(string filePath, INIContentControl contentControl)
        {
            _filePath = filePath;
            _contentControl = contentControl;
        }

        public static INIReader Create(string filePath, INIContentControl control = null)
        {
            INIReader reader = new INIReader(filePath, control);

            if (!reader.LoadFile(filePath))
                return null;
            if (!reader.LoadControl(control))
                return null;

            return reader;
        }

        public void Unload()
        {

        }

        private bool LoadFile(string filePath)
        {
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
            return false;
        }

        private bool LoadControl(INIContentControl control)
        {
            if (control == null)
                return false;

            return true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
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
