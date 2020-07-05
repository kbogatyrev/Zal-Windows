using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for NounView.xaml
    /// </summary>
    public partial class NounView : UserControl
    {
        public NounView()
        {
            InitializeComponent();
        }

        // Remove wordform formatting in prep for editing
        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            NounViewModel nvm = (NounViewModel)DataContext;
            var cell = (TextBox)sender;
            string sFormString = "";
            nvm.OnGotFocus(cell.Name, ref sFormString);     //  TextBox name = gramm hash 
            cell.Text = sFormString;
        }

        // Restore wf formatting
        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            NounViewModel nvm = (NounViewModel)DataContext;
            var cell = (TextBox)sender;
            nvm.SetForm(cell.Name, cell.Text);
        }
    }
}
