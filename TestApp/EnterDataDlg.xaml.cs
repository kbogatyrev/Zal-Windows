﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZalTestApp
{
    /// <summary>
    /// Interaction logic for EnterDataDlg.xaml
    /// </summary>
    public partial class EnterDataDlg : Window
    {
        public EnterDataDlg()
        {
            InitializeComponent();
            EnterDataViewModel viewModel = new EnterDataViewModel();
            this.DataContext = viewModel;
            if (null == viewModel.CloseWindowAction)
            {
                viewModel.CloseWindowAction = new Action(() => this.Close());
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
