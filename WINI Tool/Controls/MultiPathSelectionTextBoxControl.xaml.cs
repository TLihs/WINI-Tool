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

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für MultiPathSelectionTextBoxControl.xaml
    /// </summary>
    public partial class MultiPathSelectionTextBoxControl : UserControl
    {
        private PathSelectionTextBoxControl.SelectionTypes _selectionType;

        public UIElementCollection Children => StackPanel_RemovablePaths.Children;
        public PathSelectionTextBoxControl.SelectionTypes SelectionType
        {
            get => _selectionType;
            set
            {
                foreach (object child in Children)
                    if (child.GetType() == typeof(RemovablePathSelectionTextBoxControl))
                        ((RemovablePathSelectionTextBoxControl)child).SelectionType = value;
            }
        }
        public List<string> SelectedPaths
        {
            get
            {
                List<string> paths = new List<string>();
                foreach (RemovablePathSelectionTextBoxControl child in Children)
                    if (!string.IsNullOrWhiteSpace(child.SelectedPath))
                        paths.Add(child.SelectedPath);

                if (paths.Count > 0)
                    return paths;
                else
                    return null;
            }
        }
        
        public MultiPathSelectionTextBoxControl()
        {
            _selectionType = PathSelectionTextBoxControl.SelectionTypes.FileSelection;

            InitializeComponent();
        }

        private void Button_AddPath_Click(object sender, RoutedEventArgs e)
        {
            LogDebug("Button_AddPath_Click([object], [RoutedEventArgs])");

            RemovablePathSelectionTextBoxControl newcontrol = new RemovablePathSelectionTextBoxControl()
            {
                SelectionType = _selectionType
            };
            int buttonindex = Children.IndexOf(Button_AddPath);
            Children.Insert(buttonindex, newcontrol);
            newcontrol.RemoveButtonClicked += OnRemoveRequest;
        }

        private void OnRemoveRequest(object sender, EventArgs e)
        {
            LogDebug("OnRemoveRequest([object], [EventArgs])");

            if (sender != null && sender.GetType() == typeof(RemovablePathSelectionTextBoxControl))
            {
                ((RemovablePathSelectionTextBoxControl)sender).RemoveButtonClicked -= OnRemoveRequest;
                Children.Remove((RemovablePathSelectionTextBoxControl)sender);
            }
        }
    }
}
