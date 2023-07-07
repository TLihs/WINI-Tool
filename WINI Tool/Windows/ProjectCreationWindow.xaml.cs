// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WINI_Tool.Data.Project_Management;
using WPFExceptionHandler;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Windows
{
    /// <summary>
    /// Interaktionslogik für ProjectCreationWindow.xaml
    /// </summary>
    public partial class ProjectCreationWindow : Window
    {
        private bool _openedAsDialog = false;
        
        public ProjectCreationWindow()
        {
            InitializeComponent();
        }

        private bool ValidateEntries()
        {
            LogDebug("ValidateEntries()");

            if (string.IsNullOrWhiteSpace(TextBox_ProjectName.Text))
            {
                MsgBox(ExceptionManagement.LogEntryType.LE_WARNING, "Please choose a project name.", this);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TextBox_ProjectPath.SelectedPath))
            {
                MsgBox(ExceptionManagement.LogEntryType.LE_WARNING, "Please select a path for your new project.", this);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TextBox_TemplatePath.SelectedPath))
            {
                MsgBox(ExceptionManagement.LogEntryType.LE_WARNING, "Please select the path for the template INI file.", this);
                return false;
            }

            return true;
        }

        private void TextBox_Create_Click(object sender, RoutedEventArgs e)
        {
            LogDebug("ValidateEntries([sender], [RoutedEventArgs])");

            if (ValidateEntries())
            {
                ProjectManager.CreateNewProject(TextBox_ProjectPath.SelectedPath, TextBox_ProjectName.Text,
                    TextBox_TemplatePath.SelectedPath, MultiPathSelectionTextBox_TargetPaths.SelectedPaths.ToArray());

                if (_openedAsDialog)
                    DialogResult = true;
                Close();
            }
        }

        public new bool ShowDialog()
        {
            _openedAsDialog = true;
            return base.ShowDialog() == true ? true : false;
        }

        private void TextBox_Cancel_Click(object sender, RoutedEventArgs e)
        {
            LogDebug("ValidateEntries([sender], [RoutedEventArgs])");
            if (_openedAsDialog)
                DialogResult = false;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
