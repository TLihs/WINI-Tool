// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

using WINI_Tool.Support;

namespace WINI_Tool
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            ExceptionHandling.Create(true);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //if (e.Args.Length > 0)
            //    MessageBox.Show("Now opening file: \n\n" + e.Args[0]);

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
