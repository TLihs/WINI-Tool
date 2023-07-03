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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WINI_Tool.Data.Project_Management;
using static WINI_Tool.Data.Project_Management.ProjectManager;

namespace WINI_Tool.Windows
{
    /// <summary>
    /// Interaktionslogik für TextSearchWindow.xaml
    /// </summary>
    public partial class TextSearchWindow : Window
    {
        private bool _hideOnClose;
        private string[] _searchLocations = new string[] { "Template INI", "Target INI(s)", "All INIs" };
        private bool _isReplaceMode;

        public string SearchInputText => _isReplaceMode ? TextBox_ReplaceInput.Text : TextBox_SearchInput.Text;
        public string ReplaceOutputText => _isReplaceMode ? TextBox_ReplaceOutput.Text : TextBox_SearchInput.Text;
        
        public int SearchLocationIndex => _isReplaceMode ? ComboBox_ReplaceLocation.SelectedIndex : ComboBox_SearchLocation.SelectedIndex;

        public bool SearchWholeWord => _isReplaceMode ? (bool)CheckBox_ReplaceWholeWord.IsChecked : (bool)CheckBox_SearchWholeWord.IsChecked;
        public bool SearchCaseSensitive => _isReplaceMode ? (bool)CheckBox_ReplaceCaseSensitive.IsChecked : (bool)CheckBox_SearchCaseSensitive.IsChecked;
        public bool SearchInSectionNames => _isReplaceMode ? (bool)CheckBox_ReplaceInSections.IsChecked : (bool)CheckBox_SearchInSections.IsChecked;
        public bool SearchInGroupNames => _isReplaceMode ? (bool)CheckBox_ReplaceInGroups.IsChecked : (bool)CheckBox_SearchInGroups.IsChecked;
        public bool SearchInKeyNames => _isReplaceMode ? (bool)CheckBox_ReplaceInKeys.IsChecked : (bool)CheckBox_SearchInKeys.IsChecked;
        public bool SearchInValues => _isReplaceMode ? (bool)CheckBox_ReplaceInValues.IsChecked : (bool)CheckBox_SearchInValues.IsChecked;
        public bool SearchInComments => _isReplaceMode ? (bool)CheckBox_ReplaceInComments.IsChecked : (bool)CheckBox_SearchInComments.IsChecked;


        public TextSearchWindow()
        {
            _hideOnClose = true;

            InitializeComponent();

            foreach (string location in _searchLocations)
            {
                ComboBox_SearchLocation.Items.Add(location);
                ComboBox_ReplaceLocation.Items.Add(location);
            }
            ComboBox_SearchLocation.SelectedIndex = 0;
            ComboBox_ReplaceLocation.SelectedIndex = 0;

            TabControl_Type.SelectedIndex = 0;
        }

        public void CloseFinally()
        {
            _hideOnClose = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_hideOnClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void TabControl_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isReplaceMode = TabControl_Type.SelectedIndex == 1;
        }

        private void TextBox_Search_SearchNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Search_SearchAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Replace_SearchNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Replace_SearchAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Replace_ReplaceCurrent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_Replace_ReplaceAll_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
