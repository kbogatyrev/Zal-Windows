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

        private Dictionary<CLexemeManaged, NounViewModel> m_NounViewModels = null;
        private Dictionary<CLexemeManaged, AdjViewModel> m_AdjViewModels = null;
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
            bool? bnRet = edd.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

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

            m_LexemeGridViewModel.Clear();

            for (int iL = 0; iL < m_MainModel.NLexemes; ++iL)
            {
                CLexemeManaged l = m_MainModel.GetLexemeAt(iL);
                if (null == l)
                {
                    MessageBox.Show("Internal error: lexeme descriptor is corrupt.");
                    return;
                }

                LexemeViewModel lvm = new LexemeViewModel(l);
                lvm.RemoveLexemeEvent += new LexemeViewModel.RemoveLexemeHandler(RemoveLexeme);
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

        void ShowNoun(CLexemeManaged l)
        {
            if (null == m_NounViewModels)
            {
                m_NounViewModels = new Dictionary<CLexemeManaged, NounViewModel>();
            }

            if (!m_NounViewModels.TryGetValue(l, out NounViewModel nvm))
            {
                nvm = new NounViewModel(l);
                nvm.BackButtonEvent += new NounViewModel.BackButtonHandler(GoBack);
                m_NounViewModels[l] = nvm;
            }

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = nvm;
        }

        void ShowAdj(CLexemeManaged l)
        {
            if (null == m_AdjViewModels)
            {
                m_AdjViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
            }

            if (!m_AdjViewModels.TryGetValue(l, out AdjViewModel avm))
            {
                avm = new AdjViewModel(l);
                avm.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                m_AdjViewModels[l] = avm;
            }

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = avm;
        }

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

        void RemoveLexeme(LexemeViewModel lvm)
        {
            m_LexemeGridViewModel.Remove(lvm);
            m_MainModel.RemoveLexeme(lvm.Lexeme);
        }

    }   //  class MainViewModel 

}   //  namespace ZalTestApp

