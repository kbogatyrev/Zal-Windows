using System;
using System.Windows.Input;

namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        #region ICommand
        private ICommand m_BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return m_BackCommand;
            }
            set
            {
                m_BackCommand = value;
            }
        }
        #endregion

        #region Bindings
        private string m_sSourceForm = "псевдосинхрофазотрон";
        public string SourceForm
        {
            get
            {
                return m_sSourceForm;
            }
            set
            {
                m_sSourceForm = value;
                OnPropertyChanged("SourceForm");
            }
        }
        private string m_Noun_Sg_N = "моча\x301лка";
        public string Noun_Sg_N
        {
            get
            {
                return m_Noun_Sg_N;
            }
            set
            {
                m_sSourceForm = value;
                OnPropertyChanged("Noun_Sg_N");
            }
        }
        #endregion

        public NounViewModel()
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }
    }
}
