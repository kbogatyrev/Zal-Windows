using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for RegressionGridView.xaml
    /// </summary>
    public partial class RegressionGridView : UserControl
    {
//        int iCurrentIdx = 0;
    
        public RegressionGridView()
        {
            InitializeComponent();
        }

        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            //            rgvm.CheckRow(rgvm.CurrentIdx);
            DataRowView currentRow = (DataRowView)this.ZalRegressionDataGrid.CurrentItem;
            if (currentRow != null)
            {
                rgvm.CheckRow((string)currentRow.Row.ItemArray[2]);
            }
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            DataRowView currentRow = (DataRowView)this.ZalRegressionDataGrid.CurrentItem;
            if (currentRow != null)
            {
                rgvm.UnheckRow((string)currentRow.Row.ItemArray[2]);
            }
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

        private void ZalRegressionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegressionGridViewModel rgvm = (RegressionGridViewModel)DataContext;
            if (rgvm != null)
            {
                DataRowView currentRow = (DataRowView)this.ZalRegressionDataGrid.CurrentItem;
                if (currentRow != null)
                {
                    rgvm.CurrentLexHash = (string)currentRow.Row.ItemArray[2];
                }
            }
        }
    }
}
