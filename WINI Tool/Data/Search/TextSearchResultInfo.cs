// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WINI_Tool.Data.Search
{
    public struct TextSearchResultInfo
    {
        public bool Found { get; }
        public int Row { get; }
        public int Column { get; }
        public string FileName { get; }
        public string LineTextSnippet { get; }

        public TextSearchResultInfo(bool found, int row, int column, string fileName, string lineTextSnippet)
        {
            Found = found;
            Row = row;
            Column = column;
            FileName = fileName;
            LineTextSnippet = lineTextSnippet;
        }
    }
}
