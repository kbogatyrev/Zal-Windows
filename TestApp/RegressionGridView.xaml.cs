using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for RegressionGridView.xaml
    /// </summary>
    public partial class RegressionGridView : UserControl
    {
        public RegressionGridView()
        {
            InitializeComponent();
        }

        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            rgvm.CheckRow(rgvm.CurrentIdx);
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            rgvm.UnheckRow(rgvm.CurrentIdx);
        }

        private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            if (rgvm != null)
            {
                rgvm.CheckAll();
            }
        }

        private void HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            if (rgvm != null)
            {
                rgvm.UncheckAll();
            }
        }
    }
}
