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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für RemovablePathSelectionTextBoxControl.xaml
    /// </summary>
    public partial class RemovablePathSelectionTextBoxControl : UserControl
    {
        public event EventHandler RemoveButtonClicked;

        public string SelectedPath => PathSelectionTextBox_Content.SelectedPath;
        public PathSelectionTextBoxControl.SelectionTypes SelectionType
        {
            get => PathSelectionTextBox_Content.SelectionType;
            set
            {
                PathSelectionTextBox_Content.SelectionType = value;
            }
        }

        public RemovablePathSelectionTextBoxControl()
        {
            InitializeComponent();
        }

        private void Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonClicked?.Invoke(this, new EventArgs());
        }
    }
}
