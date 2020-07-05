using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for AdjView.xaml
    /// </summary>
    public partial class AdjView : UserControl
    {
        public AdjView()
        {
            InitializeComponent();
        }

        // Remove wordform formatting in prep for editing
        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AdjViewModel avm = (AdjViewModel)DataContext;
            var cell = (TextBox)sender;
            string sFormString = "";
            avm.OnGotFocus(cell.Name, ref sFormString);     //  TextBox name = gramm hash 
            cell.Text = sFormString;
        }

        // Restore wf formatting
        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AdjViewModel avm = (AdjViewModel)DataContext;
            var cell = (TextBox)sender;
            avm.SetForm(cell.Name, cell.Text);
        }
    }
}
