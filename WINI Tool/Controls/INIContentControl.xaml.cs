// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using WINI_Tool.Data.Base;
using WINI_Tool.Data.Search;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Controls
{
    /// <summary>
    /// Interaktionslogik für INIContentControl.xaml
    /// </summary>
    public partial class INIContentControl : UserControl
    {
        private INIReader _reader;
        
        public INIContentControl()
        {
            LogDebug("INIContentControl::INIContentControl()");
            
            InitializeComponent();

            RichTextBox_INIContent.AcceptsReturn = true;
            RichTextBox_INIContent.AcceptsTab = true;
            RichTextBox_INIContent.AutoWordSelection = true;
            RichTextBox_INIContent.Document.IsOptimalParagraphEnabled = false;
            RichTextBox_INIContent.Document.Typography.Fraction = System.Windows.FontFraction.Stacked;
        }

        public void UnloadReader()
        {
            LogDebug("INIContentControl::UnloadReader()");

            ClearContent();
            _reader = null;
        }

        public void SetReader(INIReader reader)
        {
            LogDebug("INIContentControl::SetReader()");

            ClearContent();
            _reader = reader;
        }

        private void ClearContent()
        {
            LogDebug("INIContentControl::ClearContent()");

            RichTextBox_INIContent.Document.Blocks.Clear();
        }

        public bool SearchAndFocus(string searchText, bool caseSensitive = false, bool wholeWordOnly = false, bool fromCaretOnly = false)
        {
            LogDebug("INIContentControl::SearchAndFocus({0}, {1}, {2}, {3})", 
                searchText, 
                caseSensitive ? "case sensitive" : "case insensitive", 
                wholeWordOnly ? "whole word only" : "any result",
                fromCaretOnly ? "from caret position" : "whole document");
            
            TextPointer currentcaretposition = RichTextBox_INIContent.CaretPosition;
            TextPointer newcaretposition = SearchInRange(currentcaretposition, RichTextBox_INIContent.Document.ContentEnd, searchText, caseSensitive, wholeWordOnly);

            if (!fromCaretOnly && newcaretposition == null)
                newcaretposition = SearchInRange(RichTextBox_INIContent.Document.ContentStart, currentcaretposition, searchText, caseSensitive, wholeWordOnly);

            if (newcaretposition != null)
            {
                RichTextBox_INIContent.CaretPosition = newcaretposition;
                RichTextBox_INIContent.Selection.Select(newcaretposition, newcaretposition.GetPositionAtOffset(searchText.Length));
                Rect characterrect = newcaretposition.GetCharacterRect(LogicalDirection.Forward);
                RichTextBox_INIContent.ScrollToHorizontalOffset(RichTextBox_INIContent.HorizontalOffset + characterrect.Left - RichTextBox_INIContent.ActualWidth / 2d);
                RichTextBox_INIContent.ScrollToVerticalOffset(RichTextBox_INIContent.VerticalOffset + characterrect.Top - RichTextBox_INIContent.ActualHeight / 2d);
            }
            
            return newcaretposition != null;
        }

        public System.Collections.Generic.List<TextSearchResultInfo> Search(string searchText, bool caseSensitive = false, bool wholeWordOnly = false)
        {
            LogDebug("INIContentControl::Search({0}, {1}, {2})",
                searchText,
                caseSensitive ? "case sensitive" : "case insensitive",
                wholeWordOnly ? "whole word only" : "any result");

            System.Collections.Generic.List<TextSearchResultInfo> resultlist = new System.Collections.Generic.List<TextSearchResultInfo>();
            TextPointer newcaretposition = RichTextBox_INIContent.Document.ContentStart;
            string filename = new FileInfo(_reader.FilePath).Name;

            while (newcaretposition != null)
            {
                newcaretposition = SearchInRange(newcaretposition, RichTextBox_INIContent.Document.ContentEnd, searchText, caseSensitive, wholeWordOnly);

                if (newcaretposition != null)
                {
                    TextPointer linestartposition = newcaretposition.GetLineStartPosition(0);
                    int column = Math.Max(linestartposition.GetOffsetToPosition(newcaretposition) - 1, 0);
                    TextPointer snippetstartposition = column < 10 ? linestartposition : newcaretposition.GetPositionAtOffset(-10);
                    TextPointer snippetendposition = newcaretposition.GetPositionAtOffset(searchText.Length);
                    string snippet = new TextRange(snippetstartposition, snippetendposition).Text;
                    resultlist.Add(new TextSearchResultInfo(true, GetRow(newcaretposition), column, filename, snippet));
                }
            }

            return resultlist;
        }

        private TextPointer SearchInRange(TextPointer start, TextPointer end, string searchText, bool caseSensitive = false, bool wholeWordOnly = false)
        {
            TextRange textrange = new TextRange(start, end);

            bool found = false;
            string text;
            string searchtext;
            if (!caseSensitive)
            {
                text = textrange.Text.ToLower();
                searchtext = searchText.ToLower();
            }
            else
            {
                text = textrange.Text;
                searchtext = searchText;
            }

            int matchindex = text.IndexOf(searchtext);

            if (matchindex >= 0)
            {
                if (wholeWordOnly)
                {
                    if (matchindex > 0)
                    {
                        if (string.IsNullOrWhiteSpace(text.Substring(matchindex - 1, 1)) && string.IsNullOrWhiteSpace(text.Substring(matchindex + searchtext.Length + 1, 1)))
                            found = true;
                    }
                    else if (string.IsNullOrWhiteSpace(text.Substring(matchindex + searchtext.Length + 1, 1)))
                        found = true;
                }
            }

            return found ? textrange.Start.GetPositionAtOffset(matchindex) : null;
        }

        private int GetRow(TextPointer textposition)
        {
            TextRange textrange = new TextRange(textposition.DocumentStart, textposition);
            return textrange.Text.Sum(character => character == '\n' ? 1 : 0);
        }

        private void RichTextBox_INIContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogDebug("INIContentControl::RichTextBox_INIContent_TextChanged()");

            foreach (TextChange change in e.Changes)
            {
                int offset = change.Offset;
                int addedlength = change.AddedLength;
                int removedlength = change.RemovedLength;
                //LogDebug(string.Format("Text changed at {0}. Added length: {1}. Remove length: {2}", offset, addedlength, removedlength));
            }
        }

        private void RichTextBox_INIContent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            LogDebug("INIContentControl::RichTextBox_INIContent_PreviewKeyDown()");

            switch (e.Key)
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
                            if (!MainWindow.TextSearchControl.IsVisible)
                                MainWindow.TextSearchControl.Show();

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
