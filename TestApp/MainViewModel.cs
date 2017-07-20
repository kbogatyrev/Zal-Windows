using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;

namespace ZalTestApp
{
    class MainViewModel : ViewModelBase
    {
        private Stack<ViewModelBase> m_BreadCrumbs = null;
        private LexemeGridViewModel m_LexemeGridViewModel = null;
        private NounViewModel m_NounViewModel = null;
        private AdjViewModel m_AdjViewModel = null;
        private ViewModelBase m_CurrentViewModel = null;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return m_CurrentViewModel;
            }
            set
            {
                m_CurrentViewModel = value;
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

        #endregion

        public MainViewModel()
        {
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
            m_MainModel = new MainModel();
            m_MainModel.Path = "";
            m_CurrentViewModel = this;
            m_BreadCrumbs = new Stack<ViewModelBase>();
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
            m_MainModel.SearchByInitialForm(eddvm.DataString);
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
                CLexemeManaged l = m_MainModel.GetLexemeAt(iL);
                if (null == l)
                {
                    MessageBox.Show("Internal error: lexeme descriptor is corrupt.");
                    return;
                }

                LexemeViewModel lvm = new LexemeViewModel(l);
                m_LexemeGridViewModel.Add(lvm);

                switch (l.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        lvm.ShowNounFormsEvent += new LexemeViewModel.ShowNounFormsHandler(ShowNoun);
                        break;
                    case EM_PartOfSpeech.POS_ADJ:
                        lvm.ShowAdjFormsEvent += new LexemeViewModel.ShowAdjFormsHandler(ShowAdj);
                        break;
                    default:
                        MessageBox.Show("Illegal part of speech value in lexeme descriptor.");
                        return;

                }
            }
        

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = m_LexemeGridViewModel;

        }   // SearchByInitialForm()

        void GoBack()
        {
            if (null == m_BreadCrumbs)
            {
                return;
            }

            if (m_BreadCrumbs.Count == 0)
            {
                return;
            }

            CurrentViewModel = m_BreadCrumbs.Pop();
        }

        void ShowNoun(CLexemeManaged l)
        {
            if (null == m_NounViewModel)
            {
                m_NounViewModel = new NounViewModel(l);
                m_NounViewModel.BackButtonEvent += new NounViewModel.BackButtonHandler(GoBack);
            }
            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = m_NounViewModel;
        }

        void ShowAdj(CLexemeManaged l)
        {
            if (null == m_AdjViewModel)
            {
                m_AdjViewModel = new AdjViewModel(l);
                m_AdjViewModel.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
            }
            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = m_AdjViewModel;
        }

    }   //  class MainViewModel 

}   //  namespace ZalTestApp

