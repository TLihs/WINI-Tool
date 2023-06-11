// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WINI_Tool.Data.Base;

namespace WINI_Tool.Data.Project_Management
{
    public class INITemplateTargetPair
    {
        private INIReader _templateReader;
        private INIReader _targetReader;
        private bool _standaloneTemplate;

        public INIReader TemplateReader => _templateReader;
        public INIReader TargetReader => _targetReader;
        public bool StandaloneTemplate => _standaloneTemplate;

        public INITemplateTargetPair(INIReader templateReader, INIReader targetReader = null)
        {
            _templateReader = templateReader;
            _targetReader = targetReader;

            _standaloneTemplate = _targetReader == null;
        }
    }
}
