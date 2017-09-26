using System.Windows;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {
        public ProgressDialog(ProgressViewModel pvm)
        {
            InitializeComponent();
            this.DataContext = pvm;
        }
    }
}
