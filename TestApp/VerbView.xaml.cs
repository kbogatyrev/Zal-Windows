using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for VerbView.xaml
    /// </summary>
    public partial class VerbView : UserControl
    {
        public VerbView()
        {
            InitializeComponent();
        }

        // Remove wordform formatting in prep for editing
        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            VerbViewModel vvm = (VerbViewModel)DataContext;
            var cell = (TextBox)sender;
            string sFormString = "";
            vvm.OnGotFocus(cell.Name, ref sFormString);     //  TextBox name = gramm hash 
            cell.Text = sFormString;
        }

        // Restore wf formatting
        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            VerbViewModel vvm = (VerbViewModel)DataContext;
            var cell = (TextBox)sender;
            vvm.SetForm(cell.Name, cell.Text);
        }
    }
}
