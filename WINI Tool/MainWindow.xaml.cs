﻿// WINI Tool
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
using WINI_Tool.Windows;
using WINI_Tool.Data.Base;
using WPFExceptionHandler;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ProjectCreationWindow _creationWindow;
        public static TextSearchWindow TextSearchWindow => new TextSearchWindow();

        private INIReader _iniTemplate;
        private INIReader _iniTarget;

        public MainWindow()
        {
            LogDebug("MainWindow::MainWindow()");

            InitializeComponent();

            _iniTemplate = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Template.ini");
            _iniTemplate.SetContentControl(INIContentControl_Template);
            _iniTarget = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Target.ini");
            _iniTarget.SetContentControl(INIContentControl_Target);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            LogDebug("MainWindow::OnClosing([CancelEventArgs])");

            TextSearchWindow.CloseFinally();
            TextSearchWindow.Close();
            base.OnClosing(e);
            App.Current.Shutdown();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            LogDebug("MainWindow::OnPreviewKeyDown({0})", Enum.GetName(typeof(Key), e.Key));

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
                                    TextSearchWindow.TextBox_SearchInput.Text = INIContentControl_Template.RichTextBox_INIContent.Selection.Text;
                            }
                            else if (INIContentControl_Target.IsKeyboardFocusWithin)
                            {
                                if (!INIContentControl_Target.RichTextBox_INIContent.Selection.IsEmpty)
                                    TextSearchWindow.TextBox_SearchInput.Text = INIContentControl_Target.RichTextBox_INIContent.Selection.Text;
                            }
                            else
                                TextSearchWindow.TextBox_SearchInput.Text = string.Empty;

                            if (!TextSearchWindow.IsVisible)
                                TextSearchWindow.Show();

                            TextSearchWindow.Focus();
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

        private void OnCreationWindowClosed(object sender, EventArgs e)
        {
            LogDebug("MainWindow::OnCreationWindowClosed([object], [EventArgs])");

            _creationWindow = null;
        }

        private void Menu_File_NewProject_Click(object sender, RoutedEventArgs e)
        {
            LogDebug("MainWindow::OnPreviewKeyDown([sender], [RoutedEventArgs])");

            if (_creationWindow == null)
            {
                _creationWindow = new ProjectCreationWindow();
                _creationWindow.Closed += OnCreationWindowClosed;
                _creationWindow.ShowDialog();
            }
            else
            {
                MsgBox(ExceptionManagement.LogEntryType.LE_ERROR_GENERIC, "Project creation window already opened.", this);
                _creationWindow.Focus();
            }
        }
    }
}
