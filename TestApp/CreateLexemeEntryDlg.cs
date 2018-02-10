using System;
using System.Windows;

using MainLibManaged;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for EnterDataDlg.xaml
    /// </summary>
    public partial class CreateLexemeEntryDlg : Window
    {
        public CreateLexemeEntryDlg(CreateLexemeViewModel clvm)
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
            DialogResult = true;
            this.Close();
        }
    }
}
