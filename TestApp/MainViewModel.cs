using System;
using System.Windows;
using System.Windows.Input;

using MainLibManaged;

namespace ZalTestApp
{
    class MainViewModel : ViewModelBase
    {
        private LexemeGridViewModel m_LexemeGridViewModel = null;
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

        #region ICommand
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
/*
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
*/
        private ICommand m_BackToLexemeGridCommand;
        public ICommand BackToLexemeGridCommand
        {
            get
            {
                return m_BackToLexemeGridCommand;
            }
            set
            {
                m_BackToLexemeGridCommand = value;
            }
        }

        #endregion

        public MainViewModel()
        {
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
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

            if (null == m_LexemeGridViewModel)
            {
                m_LexemeGridViewModel = new LexemeGridViewModel();
            }
            
            for (int iL = 0; iL < m_MainModel.NLexemes; ++iL)
            {
                LexemeViewModel lvm = new LexemeViewModel(m_MainModel.GetLexemeAt(iL));
                m_LexemeGridViewModel.Add(lvm);
                lvm.ShowNounFormsEvent += new LexemeViewModel.ShowNounFormsHandler(ShowNoun);
            }

            CurrentViewModel = m_LexemeGridViewModel;
        }

        void ShowNoun(CLexemeManaged l)
        {
            if (null == m_NounViewModel)
            {
                m_NounViewModel = new NounViewModel();
            }
            CurrentViewModel = m_NounViewModel;
        }

    }

}
