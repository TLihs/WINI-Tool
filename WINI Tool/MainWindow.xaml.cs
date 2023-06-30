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
using System.Windows.Navigation;
using System.Windows.Shapes;

using WINI_Tool.Controls;
using WINI_Tool.Data.Base;
using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextSearchControl TextSearchControl => new TextSearchControl();

        private INIReader _iniTemplate;
        private INIReader _iniTarget;

        public MainWindow()
        {
            InitializeComponent();

            _iniTemplate = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Template.ini");
            _iniTemplate.SetContentControl(INIContentControl_Template);
            _iniTarget = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Target.ini");
            _iniTarget.SetContentControl(INIContentControl_Target);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            TextSearchControl.CloseFinally();
            TextSearchControl.Close();
            base.OnClosing(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F:
                    ModifierKeys activemodifierkeys = e.KeyboardDevice.Modifiers;
                    if (activemodifierkeys.HasFlag(ModifierKeys.Control))
                    {
                        if (activemodifierkeys.HasFlag(ModifierKeys.Shift))
                        {
                            if (INIContentControl_Template.IsKeyboardFocusWithin)
                            {
                                if (!INIContentControl_Template.RichTextBox_INIContent.Selection.IsEmpty)
                                    TextSearchControl.TextBox_SearchInput.Text = INIContentControl_Template.RichTextBox_INIContent.Selection.Text;
                            }
                            else if (INIContentControl_Target.IsKeyboardFocusWithin)
                            {
                                if (!INIContentControl_Target.RichTextBox_INIContent.Selection.IsEmpty)
                                    TextSearchControl.TextBox_SearchInput.Text = INIContentControl_Target.RichTextBox_INIContent.Selection.Text;
                            }
                            else
                                TextSearchControl.TextBox_SearchInput.Text = string.Empty;

                            if (!TextSearchControl.IsVisible)
                                TextSearchControl.Show();

                            TextSearchControl.Focus();
                        }
                        else
                        {

                        }

                        e.Handled = true;
                    }
                    break;
            }

            base.OnPreviewKeyDown(e);
        }
    }
}
