using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for LastNameView.xaml
    /// </summary>
    public partial class LastNameView : UserControl
    {
        public LastNameView()
        {
            InitializeComponent();
        }

        // Remove wordform formatting in prep for editing
        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            LastNameViewModel nvm = (LastNameViewModel)DataContext;
            var cell = (TextBox)sender;
            string sFormString = "";
            nvm.OnGotFocus(cell.Name, ref sFormString);     //  TextBox name = gramm hash 
            cell.Text = sFormString;
        }

        // Restore wf formatting
        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            LastNameViewModel nvm = (LastNameViewModel)DataContext;
            var cell = (TextBox)sender;
            nvm.SetForm(cell.Name, cell.Text);
        }
    }
}
