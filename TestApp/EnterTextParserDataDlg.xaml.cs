using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for EnterTextParserDataDlgViewModel.xaml
    /// </summary>
    public partial class EnterTextParserDataDlg : Window
    {
        public EnterTextParserDataDlg(EnterTextParserDataDlgViewModel ctx)
        {
            InitializeComponent();
            this.DataContext = ctx;
        }
    }

/*
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

            var model = (EnterTextParserDataDlgViewModel)this.DataContext;

            DialogResult = true;
            this.Close();
        }
*/
}
