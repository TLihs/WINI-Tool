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

namespace WINI_Tool.Data
{
    public class INIReader
    {
        private string _filePath;
        private List<string> _lineContent;
        private List<INISection> _sections;
        private FileSystemWatcher _watcher;

        public string FilePath => _filePath;

        private INIReader()
        {
            _lineContent = new List<string>();
            _sections = new List<INISection>();
        }

        public static INIReader Create(string filePath, INIContentControl control = null)
        {
            INIReader reader = new INIReader();

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
            INISection section = AddSection("<default>", 0);
            INIGroup group = null;
            _lineContent = new List<string>();
            _sections = new List<INISection>();
            List<string> commentBuffer = new List<string>();
            long position = 0;
            
            for (long linenumber = 0; linenumber < lines.Length; linenumber++)
            {
                string line = lines[linenumber];

                if (string.IsNullOrWhiteSpace(line))
                {
                    position++;
                    continue;
                }

                if (RXSectionName.IsMatch(line))
                {
                    Match sectionmatch = RXSectionName.Match(line);
                    if (sectionmatch.Groups[0].Length == 0)
                    {
                        INISection newsection = AddSection(sectionmatch.Groups[1].Value, linenumber + 1);
                        if (newsection != null)
                            section = newsection;
                    }
                }
                else if (RXKeyValuePair.IsMatch(line))
                {
                    if (group == null)
                        group = section.Add
                    Match keyvaluematch = RXKeyValuePair.Match(line);
                    section.AddKeyValuePair(keyvaluematch.Groups[1].Value, keyvaluematch.Groups[2].Value, linenumber, keyvaluematch.Groups[0].Length > 0, line);
                }
                else if (RXGroupName.IsMatch(line))
                {

                }
                else if (RXComment.IsMatch(line))
                {
                    commentBuffer.Add(line);
                }

                position += line.Length + 1;
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

        }

        private bool LoadControl(INIContentControl control)
        {
            if (control == null)
                return false;

            
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

        public INISection AddSection(string sectionName, long lineNumber)
        {
            if (_sections.ContainsKey(sectionName))
            {
                long line = _sections[sectionName].Line;
                Debug.Print(string.Format("INIReader::AddSection(%s, %d) - section already exists at line %d", sectionName, lineNumber, line));
                return null;
            }

            INISection section = INISection.Create(sectionName, lineNumber);
            if (section != null)
                _sections.Add(sectionName, section);

            return section;
        }

        public void RemoveSection(string sectionName)
        {

        }

        public INISection GetSection(string sectionName)
        {
            return _sections.Values.FirstOrDefault(section => section.Name == sectionName.ToLower());
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
