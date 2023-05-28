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
    /// Interaktionslogik für INIContentControl.xaml
    /// </summary>
    public partial class INIContentControl : UserControl
    {
        public INIContentControl()
        {
            InitializeComponent();
        }

        private void RichTextBox_INIContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
