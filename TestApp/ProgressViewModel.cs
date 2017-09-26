using System;
using System.Windows;
using System.Windows.Input;

namespace ZalTestApp
{
    public class ProgressViewModel : ViewModelBase
    {
        private int m_iProgress = 0;
        public int Progress
        {
            get
            {
                return m_iProgress;
            }
            set
            {
                m_iProgress = value;
                OnPropertyChanged("Progress");
            }
        }

        public ProgressViewModel()
        {
        }

    }
}
