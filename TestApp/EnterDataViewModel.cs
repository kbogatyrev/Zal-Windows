using System;
using System.Windows;
using System.Windows.Input;

namespace ZalTestApp
{
    public class EnterDataViewModel : ViewModelBase
    {
        public Action CloseWindowAction { get; set; }

        public enum EType { LexemeSearch, CommentEntry };

        private EType m_eType = EType.LexemeSearch;
        public EType Type
        {
            get
            {
                return m_eType;
            }
            set
            {
                if (value != m_eType)
                {
                    m_eType = value;
                }
                OnPropertyChanged("Type");
            }
        }

        private string m_sData = null;
        public string DataString
        {
            get
            {
                return m_sData;
            }
            set
            {
                if (value != m_sData)
                {
                    m_sData = value;
                }
                OnPropertyChanged("DataString");
            }
        }

        private string m_sDlgTitle = "Исходная форма";
        public string DlgTitle
        {
            get
            {
                return m_sDlgTitle;
            }
            set
            {
                if (value != m_sDlgTitle)
                {
                    m_sDlgTitle = value;
                }
                OnPropertyChanged("DlgTitle");
            }
        }

        private ICommand m_OKCommand;
        public ICommand OKCommand
        {
            get
            {
                return m_OKCommand;
            }
            set
            {
                m_OKCommand = value;
            }
        }

        private ICommand m_CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return m_CancelCommand;
            }
            set
            {
                m_CancelCommand = value;
            }
        }

        public EnterDataViewModel()
        {
            OKCommand = new RelayCommand(new System.Action<object>(ReturnOK));
            CancelCommand = new RelayCommand(new System.Action<object>(ReturnCancel));
        }

        public void ReturnOK(object arg)
        {
            CloseWindowAction();
        }

        public void ReturnCancel(object arg)
        {
            CloseWindowAction();
            DataString = "";
        }
    }
}
