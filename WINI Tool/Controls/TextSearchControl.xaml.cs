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

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für TextSearchControl.xaml
    /// </summary>
    public partial class TextSearchControl : Window
    {
        private bool _hideOnClose;
        private string[] _searchLocations = new string[] { "Template INI", "Target INI", "Both INIs" };

        public string SearchInputText => TextBox_SearchInput.Text;
        public int SearchLocationIndex => ComboBox_SearchLocation.SelectedIndex;


        public TextSearchControl()
        {
            _hideOnClose = true;

            InitializeComponent();

            foreach (string location in _searchLocations)
                ComboBox_SearchLocation.Items.Add(location);
            ComboBox_SearchLocation.SelectedIndex = 0;
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
    }
}
