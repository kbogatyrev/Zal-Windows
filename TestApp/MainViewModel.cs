using System;
using System.Windows;
using System.Windows.Input;

namespace ZalTestApp
{
    class MainViewModel : ViewModelBase
    {
        private LexemeGridViewModel m_LexemeGridViewModel = null;
        private LexemeViewModel m_LexemeViewModel = null;
        private NounViewModel m_NounViewModel = null;
        private ViewModelBase m_CurrentViewModelModel = null;
 
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return m_CurrentViewModelModel;
            }
            set
            {
                m_CurrentViewModelModel = value;
                this.OnPropertyChanged("CurrentViewModel");
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
            m_CurrentViewModelModel = this;
        }
 
        public void OpenDictionary(object obj)
        {
            m_MainModel.OpenDictionary(obj.ToString());
        }

        public void SearchByInitialForm(object obj)
        {
            EnterDataDlg edd = new EnterDataDlg();
            edd.Owner = Application.Current.MainWindow;
            edd.ShowDialog();
            EnterDataViewModel eddvm =  (EnterDataViewModel)edd.DataContext;
            m_MainModel.SearchByInitalForm(eddvm.DataString);
            if (m_MainModel.NLexemes < 1)
            {
                return;
            }

            ShowLexeme(null);
        }

        public void ShowLexeme(object obj)
        {
            if (null == m_LexemeGridViewModel)
            {
                m_LexemeViewModel = new LexemeViewModel();
                if (null == m_LexemeGridViewModel)
                {
                    m_LexemeGridViewModel = new LexemeGridViewModel();
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                    m_LexemeGridViewModel.Add(m_LexemeViewModel);
                }

            }
            CurrentViewModel = m_LexemeGridViewModel;
        }

        public void ShowNoun(object obj)
        {
            if (null == m_NounViewModel)
            {
                m_NounViewModel = new NounViewModel();
            }
            CurrentViewModel = m_NounViewModel;
        }
    }
}
