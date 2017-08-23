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

        private Dictionary<CLexemeManaged, NounViewModel> m_NounViewModels = new Dictionary<CLexemeManaged, NounViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_AdjViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, VerbViewModel> m_VerbViewModels = new Dictionary<CLexemeManaged, VerbViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPresActViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPastActViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPresPassViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPastPassViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();

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

        private ICommand m_RunRegressionTestCommand;
        public ICommand RunRegressionTestCommand
        {
            get
            {
                return m_RunRegressionTestCommand;
            }
            set
            {
                m_RunRegressionTestCommand = value;
            }
        }

        private ICommand m_ImportTestDataCommand;
        public ICommand ImportTestDataCommand
        {
            get
            {
                return m_ImportTestDataCommand;
            }
            set
            {
                m_ImportTestDataCommand = value;
            }
        }

        private ICommand m_ExportTestDataCommand;
        public ICommand ExportTestDataCommand
        {
            get
            {
                return m_ExportTestDataCommand;
            }
            set
            {
                m_ExportTestDataCommand = value;
            }
        }

        #endregion

        public MainViewModel()
        {
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
            RunRegressionTestCommand = new RelayCommand(new Action<object>(RunRegression));
            ImportTestDataCommand = new RelayCommand(new Action<object>(ImportTestData));
            ExportTestDataCommand = new RelayCommand(new Action<object>(ExportTestData));

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

            foreach (CLexemeManaged lexeme in m_MainModel)
            { 
                if (null == lexeme)
                {
                    MessageBox.Show("Internal error: lexeme descriptor is corrupt.");
                    return;
                }

                LexemeViewModel lvm = new LexemeViewModel(lexeme);
                lvm.RemoveLexemeEvent += new LexemeViewModel.RemoveLexemeHandler(RemoveLexeme);
                m_LexemeGridViewModel.Add(lvm);

                switch (lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        lvm.ShowNounFormsEvent += new LexemeViewModel.ShowNounFormsHandler(ShowNoun);
                        break;
                    case EM_PartOfSpeech.POS_ADJ:
                        lvm.ShowAdjFormsEvent += new LexemeViewModel.ShowAdjFormsHandler(ShowAdj);
                        break;
                    case EM_PartOfSpeech.POS_VERB:
                        lvm.ShowVerbFormsEvent += new LexemeViewModel.ShowVerbFormsHandler(ShowVerb);
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
                nvm = new NounViewModel(l, m_MainModel);
                nvm.BackButtonEvent += new NounViewModel.BackButtonHandler(GoBack);
                m_NounViewModels[l] = nvm;
            }

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = nvm;
        }

        void ShowAdj(CLexemeManaged lexeme)
        {
            if (null == m_AdjViewModels)
            {
                m_AdjViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
            }

            if (!m_AdjViewModels.TryGetValue(lexeme, out AdjViewModel avm))
            {
                avm = new AdjViewModel(lexeme, EM_Subparadigm.SUBPARADIGM_LONG_ADJ, m_MainModel);
                avm.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                m_AdjViewModels[lexeme] = avm;
            }

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = avm;
        }

        void ShowVerb(CLexemeManaged lexeme)
        {
            if (null == m_VerbViewModels)
            {
                m_VerbViewModels = new Dictionary<CLexemeManaged, VerbViewModel>();
            }

            if (!m_VerbViewModels.TryGetValue(lexeme, out VerbViewModel vvm))
            {
                vvm = new VerbViewModel(lexeme, m_MainModel);
                vvm.BackButtonEvent += new VerbViewModel.BackButtonHandler(GoBack);
                vvm.ShowParticipleFormsEvent += new VerbViewModel.ShowParticipleForms(ShowParticiple);
                m_VerbViewModels[lexeme] = vvm;
            }

            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = vvm;
        }

        void ShowParticiple(CLexemeManaged lexeme, EM_Subparadigm sp)
        {
            switch (sp)
            {
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    if (!m_PartPresActViewModels.TryGetValue(lexeme, out AdjViewModel avmPresAct))
                    {
                        avmPresAct = new AdjViewModel(lexeme, sp, m_MainModel);
                        avmPresAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPresActViewModels[lexeme] = avmPresAct;
                    }
                    m_BreadCrumbs.Push(m_CurrentViewModel);
                    CurrentViewModel = avmPresAct;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    if (!m_PartPastActViewModels.TryGetValue(lexeme, out AdjViewModel avmPastAct))
                    {
                        avmPastAct = new AdjViewModel(lexeme, sp, m_MainModel);
                        avmPastAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPastActViewModels[lexeme] = avmPastAct;
                    }
                    m_BreadCrumbs.Push(m_CurrentViewModel);
                    CurrentViewModel = avmPastAct;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    if (!m_PartPresPassViewModels.TryGetValue(lexeme, out AdjViewModel avmPresPass))
                    {
                        avmPresPass = new AdjViewModel(lexeme, sp, m_MainModel);
                        avmPresPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPresPassViewModels[lexeme] = avmPresPass;
                    }
                    m_BreadCrumbs.Push(m_CurrentViewModel);
                    CurrentViewModel = avmPresPass;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    if (!m_PartPastPassViewModels.TryGetValue(lexeme, out AdjViewModel avmPastPass))
                    {
                        avmPastPass = new AdjViewModel(lexeme, sp, m_MainModel);
                        avmPastPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPastPassViewModels[lexeme] = avmPastPass;
                    }
                    m_BreadCrumbs.Push(m_CurrentViewModel);
                    CurrentViewModel = avmPastPass;                    
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                    // handled together with long forms, no need for separate processing
                    break;
                default:
                    MessageBox.Show("Unexpected subparadigm.");
                    return;
            }
        }       //  ShowParticiple()

        public void RunRegression(object obj)
        {
            var rm = new RegressionGridViewModel(m_MainModel);
            rm.BackButtonEvent += new RegressionGridViewModel.BackButtonHandler(GoBack);
            m_BreadCrumbs.Push(m_CurrentViewModel);
            CurrentViewModel = rm;
        }

        public void ImportTestData(object obj)
        {
            MessageBox.Show("ImportTestData");
        }
        public void ExportTestData(object obj)
        {
            MessageBox.Show("ExportTestData");
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
