// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WINI_Tool.Data.Project_Management;

namespace WINI_Tool.Windows
{
    /// <summary>
    /// Interaktionslogik für ProjectCreationWindow.xaml
    /// </summary>
    public partial class ProjectCreationWindow : Window
    {
        public ProjectCreationWindow()
        {
            InitializeComponent();
        }

        private void TextBox_Create_Click(object sender, RoutedEventArgs e)
        {
            //ProjectManager.CreateNewProject();
        }

        private void TextBox_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
