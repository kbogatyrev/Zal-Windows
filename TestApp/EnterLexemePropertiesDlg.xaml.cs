using System;
using System.Windows;

using MainLibManaged;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for EnterLexemeProperties.xaml
    /// </summary>
    public partial class EnterLexemePropertiesDlg : Window
    {
        public EnterLexemePropertiesDlg(EnterLexemePropertiesViewModel clvm)
        {
            InitializeComponent();
            this.DataContext = clvm;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

            var model = (EnterLexemePropertiesViewModel)this.DataContext;

            DialogResult = true;
            this.Close();
        }
    }
}
