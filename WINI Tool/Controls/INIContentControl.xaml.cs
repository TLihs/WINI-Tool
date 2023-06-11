// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für INIContentControl.xaml
    /// </summary>
    public partial class INIContentControl : UserControl
    {
        private MainWindow _mainWindow => (MainWindow)App.Current.MainWindow;
        
        public INIContentControl()
        {
            InitializeComponent();

            RichTextBox_INIContent.AcceptsReturn = true;
            RichTextBox_INIContent.AcceptsTab = true;
            RichTextBox_INIContent.AutoWordSelection = true;
            RichTextBox_INIContent.Document.IsOptimalParagraphEnabled = false;
            RichTextBox_INIContent.Document.Typography.Fraction = System.Windows.FontFraction.Stacked;
        }

        private void RichTextBox_INIContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "INIContentControl::RichTextBox_INIContent_TextChanged(..)");
            foreach (TextChange change in e.Changes)
            {
                int offset = change.Offset;
                int addedlength = change.AddedLength;
                int removedlength = change.RemovedLength;
                LogDebug(string.Format("Text changed at {0}. Added length: {1}. Remove length: {2}", offset, addedlength, removedlength));
            }
        }

        private void RichTextBox_INIContent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Enter:
                    TextPointer pointer = RichTextBox_INIContent.Selection.Start.InsertLineBreak();
                    RichTextBox_INIContent.Selection.Select(pointer, pointer);
                    e.Handled = true;
                    break;

                case Key.F:
                    ModifierKeys activemodifierkeys = e.KeyboardDevice.Modifiers;
                    if (activemodifierkeys.HasFlag(ModifierKeys.Control))
                    {
                        if (activemodifierkeys.HasFlag(ModifierKeys.Shift))
                        {
                            if (!_mainWindow.TextSearchControl.IsVisible)
                                _mainWindow.TextSearchControl.Show();

                            if (!RichTextBox_INIContent.Selection.IsEmpty)
                                Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "INIContentControl::RichTextBox_INIContent_PreviewKeyDown - set text to TextSearchControl");
                        }
                        else
                        {

                        }

                        e.Handled = true;
                    }
                    break;
            }
        }
    }
}
