using System.Windows;

namespace ZalTestApp
{
    public partial class CustomResources : ResourceDictionary
    {
        public void ClickAddFormComment(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Тама!");
        }
    }
}
