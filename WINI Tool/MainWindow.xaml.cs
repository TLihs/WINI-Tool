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
using WINI_Tool.Data;

namespace WINI_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private INIReader _iniTemplate;
        private INIReader _iniTarget;
        
        public MainWindow()
        {
            InitializeComponent();

            _iniTemplate = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Template.ini", INIContentControl_Template);
            _iniTarget = INIReader.Create(@"C:\Users\Admin\Documents\WINI Tool - Target.ini", INIContentControl_Target);
        }
    }
}
