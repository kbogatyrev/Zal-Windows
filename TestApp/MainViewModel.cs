using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using Microsoft.Win32;

namespace ZalTestApp
{
    class MainViewModel : ViewModelBase
    {
        private LinkedList<ViewModelBase> m_BreadCrumbs = null;
        private LexemeGridViewModel m_LexemeGridViewModel = null;
        private RegressionGridViewModel m_RegressionGridViewModel = null;

        private Dictionary<CLexemeManaged, NounViewModel> m_NounViewModels = new Dictionary<CLexemeManaged, NounViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_AdjViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, VerbViewModel> m_VerbViewModels = new Dictionary<CLexemeManaged, VerbViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPresActViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPastActViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPresPassViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();
        private Dictionary<CLexemeManaged, AdjViewModel> m_PartPastPassViewModels = new Dictionary<CLexemeManaged, AdjViewModel>();

        private LinkedListNode<ViewModelBase> m_CurrentViewModel = null;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return m_CurrentViewModel.Value;
            }
            //            set
            //            {
            //                m_CurrentViewModel = value;
            //                this.OnPropertyChanged("CurrentViewModel");
            //            }
        }

        public bool IsBackButtonEnabled
        {
            get
            {
                if (null == m_CurrentViewModel)
                {
                    return false;
                }

                if (null == m_CurrentViewModel.Previous)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsFrontButtonEnabled
        {
            get
            {
                if (null == m_CurrentViewModel)
                {
                    return false;
                }

                if (null == m_CurrentViewModel.Next)
                {
                    return false;
                }

                return true;
            }
        }

        private MainModel m_MainModel;

        #region Properties

        private bool m_bDbOpen;
        public bool DbOpen
        {
            get
            {
                return m_bDbOpen;
            }
            set
            {
                m_bDbOpen = value;
                OnPropertyChanged("DbOpen");
            }
        }

        #endregion

        #region ICommand

        private ICommand m_BackButtonCommand;
        public ICommand BackButtonCommand
        {
            get
            {
                return m_BackButtonCommand;
            }
            set
            {
                m_BackButtonCommand = value;
            }
        }

        private ICommand m_ForwardButtonCommand;
        public ICommand ForwardButtonCommand
        {
            get
            {
                return m_ForwardButtonCommand;
            }
            set
            {
                m_ForwardButtonCommand = value;
            }
        }

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

        private ICommand m_ImportRegressionDataCommand;
        public ICommand ImportRegressionDataCommand
        {
            get
            {
                return m_ImportRegressionDataCommand;
            }
            set
            {
                m_ImportRegressionDataCommand = value;
            }
        }

        private ICommand m_ExportRegressionDataCommand;
        public ICommand ExportRegressionDataCommand
        {
            get
            {
                return m_ExportRegressionDataCommand;
            }
            set
            {
                m_ExportRegressionDataCommand = value;
            }
        }
        #endregion

        public MainViewModel()
        {
            BackButtonCommand = new RelayCommand(new Action<object>(GoBack));
            ForwardButtonCommand = new RelayCommand(new Action<object>(GoForward));
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
            RunRegressionTestCommand = new RelayCommand(new Action<object>(RunRegression));
            ImportRegressionDataCommand = new RelayCommand(new Action<object>(ImportRegressionData));
            ExportRegressionDataCommand = new RelayCommand(new Action<object>(ExportRegressionData));

            m_MainModel = new MainModel();
            m_MainModel.Path = "";
            m_BreadCrumbs = new LinkedList<ViewModelBase>();
            m_CurrentViewModel = m_BreadCrumbs.AddLast(this);

            m_bDbOpen = false;
        }

        public void GoBack(object obj)
        {
            if (null == m_BreadCrumbs)
            {
                return;
            }

            if (m_BreadCrumbs.Count == 0)
            {
                return;
            }

            if (null == m_CurrentViewModel)
            {
                return;
            }

            if (null == m_CurrentViewModel.Previous)
            {
                return;
            }

            m_CurrentViewModel = m_CurrentViewModel.Previous;
            UpdateView();
        }

        public void GoForward(object obj)
        {
            if (null == m_BreadCrumbs)
            {
                return;
            }

            if (m_BreadCrumbs.Count == 0)
            {
                return;
            }

            if (null == m_CurrentViewModel)
            {
                return;
            }

            if (null == m_CurrentViewModel.Next)
            {
                return;
            }

            m_CurrentViewModel = m_CurrentViewModel.Next;
            UpdateView();
        }

        public void OpenDictionary(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|SQLite 3 files (*.db3)|*.db3";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;
            if (true == openFileDialog.ShowDialog())
            {
                eRet = m_MainModel.OpenDictionary(openFileDialog.FileName);
            }

            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                DbOpen = true;
            }
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

            m_CurrentViewModel = m_BreadCrumbs.AddLast(m_LexemeGridViewModel);
            UpdateView();

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
//                nvm.BackButtonEvent += new NounViewModel.BackButtonHandler(GoBack);
                m_NounViewModels[l] = nvm;
            }

            m_CurrentViewModel = m_BreadCrumbs.AddLast(nvm);
            UpdateView();
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
//                avm.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                m_AdjViewModels[lexeme] = avm;
            }

            m_CurrentViewModel = m_BreadCrumbs.AddLast(avm);
            UpdateView();
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
//                vvm.BackButtonEvent += new VerbViewModel.BackButtonHandler(GoBack);
                vvm.ShowParticipleFormsEvent += new VerbViewModel.ShowParticipleForms(ShowParticiple);
                m_VerbViewModels[lexeme] = vvm;
            }

            m_CurrentViewModel = m_BreadCrumbs.AddLast(vvm);
            UpdateView();
        }

        void ShowParticiple(CLexemeManaged lexeme, EM_Subparadigm sp)
        {
            switch (sp)
            {
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    if (!m_PartPresActViewModels.TryGetValue(lexeme, out AdjViewModel avmPresAct))
                    {
                        avmPresAct = new AdjViewModel(lexeme, sp, m_MainModel);
//                        avmPresAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPresActViewModels[lexeme] = avmPresAct;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avmPresAct);
                    UpdateView();
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    if (!m_PartPastActViewModels.TryGetValue(lexeme, out AdjViewModel avmPastAct))
                    {
                        avmPastAct = new AdjViewModel(lexeme, sp, m_MainModel);
//                        avmPastAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPastActViewModels[lexeme] = avmPastAct;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avmPastAct);
                    UpdateView();
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    if (!m_PartPresPassViewModels.TryGetValue(lexeme, out AdjViewModel avmPresPass))
                    {
                        avmPresPass = new AdjViewModel(lexeme, sp, m_MainModel);
//                        avmPresPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPresPassViewModels[lexeme] = avmPresPass;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avmPresPass);
                    UpdateView();
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    if (!m_PartPastPassViewModels.TryGetValue(lexeme, out AdjViewModel avmPastPass))
                    {
                        avmPastPass = new AdjViewModel(lexeme, sp, m_MainModel);
//                        avmPastPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPastPassViewModels[lexeme] = avmPastPass;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avmPastPass);
                    UpdateView();
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
            m_MainModel.Clear();
            if (m_LexemeGridViewModel != null)
            {
                m_LexemeGridViewModel.Clear();
            }

            if (null == m_RegressionGridViewModel)
            {
                m_RegressionGridViewModel = new RegressionGridViewModel(m_MainModel);
//                m_RegressionGridViewModel.BackButtonEvent += new RegressionGridViewModel.BackButtonHandler(GoBack);
            }
            m_CurrentViewModel = m_BreadCrumbs.AddLast(m_RegressionGridViewModel);
            UpdateView();
        }

        public void ImportRegressionData(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (true == openFileDialog.ShowDialog())
            {
                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
                m_MainModel.ImportRegressionData(openFileDialog.FileName, progress);
            }
        }

        public void ExportRegressionData(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (true == openFileDialog.ShowDialog())
            {
                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
                m_MainModel.ExportRegressionData(openFileDialog.FileName, progress);
            }
        }

        void RemoveLexeme(LexemeViewModel lvm)
        {
            m_LexemeGridViewModel.Remove(lvm);
            m_MainModel.RemoveLexeme(lvm.Lexeme);
        }

        void UpdateProgress(int iPercent, bool bDone)
        {

        }

        private void UpdateView()
        {
            this.OnPropertyChanged("CurrentViewModel");
            this.OnPropertyChanged("IsBackButtonEnabled");
            this.OnPropertyChanged("IsFrontButtonEnabled");
        }

    }   //  class MainViewModel 

    #region RegressionDataImportThread
    public class RegressionDataImportThread
    {
        private RegressionGridViewModel m_Caller;

        public RegressionDataImportThread(RegressionGridViewModel rvm)
        {
            m_Caller = rvm;
        }

        public void ThreadProc()
        {
            try
            {
//                m_Caller.&&&&
            }
            catch (Exception ex)
            {
                //                MainLib.ZalError err = new MainLib.ZalError();
                string sMsg = ex.Message;
                sMsg += "\n";
                //                sMsg += err.LastError;
                MessageBox.Show(sMsg, "Error");
                return;
            }

        }   //  ThreadProc()

    }   //  public class VerifierThread
    #endregion

}   //  namespace ZalTestApp
