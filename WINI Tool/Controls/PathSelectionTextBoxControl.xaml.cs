// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinControls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

using static WINI_Tool.Support.ExceptionHandling;
using WINI_Tool.Data.Project_Management;
using WINI_Tool.Support;
using System.IO;

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für PathSelectionTextBoxControl.xaml
    /// </summary>
    public partial class PathSelectionTextBoxControl : WinControls.UserControl
    {
        public enum SelectionTypes
        {
            FileSelection = 0,
            FolderSelection = 1
        }

        private string _extensions;
        private string _initialDirectory;

        public SelectionTypes SelectionType { get; set; }
        public string Title { get; set; }
        public string SelectedPath => TextBox_Content.Text;
        public string Extensions
        {
            get => _extensions;
            set
            {
                switch (SelectionType)
                {
                    case SelectionTypes.FileSelection:
                        _extensions = value;
                        break;
                    case SelectionTypes.FolderSelection:
                        LogWarning("PathSelectionTextBoxControl::{set}Extensions - Can't set extensions for folders.");
                        return;
                }
            }
        }
        public string InitialDirectory
        {
            get => _initialDirectory;
            set
            {
                if (value == null)
                {
                    LogWarning("PathSelectionTextBoxControl::{set}InitialDirectory - Value is null. Directory is set to default.");
                    _initialDirectory = ProjectManager.DefaultProjectsDirectory;
                }
                else
                {
                    if (!Directory.Exists(value))
                    {
                        LogWarning("PathSelectionTextBoxControl::{set}InitialDirectory - Directory does not exist. Creating ...");

                        try
                        {
                            Directory.CreateDirectory(value);
                        }
                        catch (Exception ex)
                        {
                            LogGenericError(ex);
                            return;
                        }
                    }

                    _initialDirectory = value;
                }
            }
        }
        
        public PathSelectionTextBoxControl()
        {
            _extensions = null;
            _initialDirectory = null;

            InitializeComponent();
        }

        private string ShowFilePathSelectionDialog()
        {
            if (_extensions == null)
                _extensions = "All files (*.*)|*.*||";

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = _extensions,
                CheckFileExists = false,
                CheckPathExists = true,
                ReadOnlyChecked = false,
                InitialDirectory = InitialDirectory,
                AutoUpgradeEnabled = true,
                DereferenceLinks = true,
                Multiselect = false,
                FilterIndex = 0,
                Title = Title,
                ShowReadOnly = false,
                ShowHelp = false
            };

            switch (dialog.ShowDialog())
            {
                case DialogResult.OK:
                    return dialog.FileName;
                default:
                    return "";
            }
        }

        private string ShowFolderPathSelectionDialog()
        {
            if (_extensions == null)
                _extensions = "All files (*.*)|*.*||";

            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                Description = Title,
                RootFolder = Environment.SpecialFolder.Personal,
                ShowNewFolderButton = true
            };

            switch (dialog.ShowDialog())
            {
                case DialogResult.OK:
                    return dialog.SelectedPath;
                default:
                    return "";
            }
        }

        private void Button_PathSelection_Click(object sender, RoutedEventArgs e)
        {
            LogDebug("PathSelectionTextBoxControl::Button_PathSelection_Click(..., ...)");

            string selectedpath = "";
            
            switch (SelectionType)
            {
                case SelectionTypes.FileSelection:
                    selectedpath = ShowFilePathSelectionDialog();
                    break;
                case SelectionTypes.FolderSelection:
                    selectedpath = ShowFolderPathSelectionDialog();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(selectedpath))
                TextBox_Content.Text = selectedpath;
        }
    }
}
