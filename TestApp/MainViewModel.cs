using System;
using System.Windows;
using System.Windows.Input;

namespace ZalTestApp
{
    class MainViewModel : ViewModelBase
    {
        private LexemeViewModel m_LexemeViewModel = null;
        private NounViewModel m_NounViewModel = null;
        private ViewModelBase m_CurrentViewModel = null;
 
        public ViewModelBase CurrentView
        {
            get
            {
                return m_CurrentViewModel;
            }
            set
            {
                m_CurrentViewModel = value;
                this.OnPropertyChanged("CurrentView");
            }
        }

        private MainModel m_MainModel;
 
        private ICommand m_OpenDictionaryCommand;
        public ICommand OpenDictionaryCommand
        {
            get
            {
                return m_OpenDictionaryCommand;
            }
            set
            {
                m_OpenDictionaryCommand = value;
            }
        }

        private ICommand m_SearchByInitialFormCommand;
        public ICommand SearchByInitialFormCommand
        {
            get
            {
                return m_SearchByInitialFormCommand;
            }
            set
            {
                m_SearchByInitialFormCommand = value;
            }
        }

        private ICommand m_ShowLexemeCommand;
        public ICommand ShowLexemeCommand
        {
            get
            {
                return m_ShowLexemeCommand;
            }
            set
            {
                m_ShowLexemeCommand = value;
            }
        }

        private ICommand m_ShowNounCommand;
        public ICommand ShowNounCommand
        {
            get
            {
                return m_ShowNounCommand;
            }
            set
            {
                m_ShowNounCommand = value;
            }
        }

        public MainViewModel()
        {
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
            ShowLexemeCommand = new RelayCommand(new Action<object>(ShowLexeme));
            ShowNounCommand = new RelayCommand(new Action<object>(ShowNoun));
            m_MainModel = new MainModel();
            m_MainModel.Path = "";
            m_CurrentViewModel = this;
        }
 
        public void OpenDictionary(object obj)
        {
            m_MainModel.OpenDictionary(obj.ToString());
        }

        public void SearchByInitialForm(object obj)
        {
            m_MainModel.SearchByInitalForm(obj.ToString());
        }

        public void ShowLexeme(object obj)
        {
            if (null == m_LexemeViewModel)
            {
                m_LexemeViewModel = new LexemeViewModel();
            }
            CurrentView = m_LexemeViewModel;
        }

        public void ShowNoun(object obj)
        {
            if (null == m_NounViewModel)
            {
                m_NounViewModel = new NounViewModel();
            }
            CurrentView = m_NounViewModel;
        }
    }
}
