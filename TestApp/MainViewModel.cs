﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Collections.ObjectModel;

using MainLibManaged;

namespace ZalTestApp
{
    public class ViewPage
    {
        public ViewPage(string sHeader, ViewModelBase lexeme, ViewModelBase page)
        {
            m_sHeader = sHeader;
            m_LexemeInfo = lexeme;
            m_Page = page;
        }

        string m_sHeader;
        ViewModelBase m_LexemeInfo;
        ViewModelBase m_Page;

        public string Header
        {
            get
            {
                return m_sHeader;
            }
        }

        public ViewModelBase Page
        {
            get
            {
                return m_Page;
            }
        }

        public ViewModelBase LexemeInfo
        {
            get
            {
                return m_LexemeInfo;
            }
        }

        public string Type
        {
            get
            {
                return "Lexeme";
            }
        }
    }

    class MainViewModel : ViewModelBase
    {
        private LinkedList<ViewModelBase> m_BreadCrumbs = null;

        private ViewPage m_RegressionGridViewModel = null;
        private Dictionary<CLexemeManaged, ViewPage> m_NounViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_AdjViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_VerbViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_PartPresActViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_PartPastActViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_PartPresPassViewModels = new Dictionary<CLexemeManaged, ViewPage>();
        private Dictionary<CLexemeManaged, ViewPage> m_PartPastPassViewModels = new Dictionary<CLexemeManaged, ViewPage>();

        private ObservableCollection<ViewPage> m_Pages = new ObservableCollection<ViewPage>();
        public ObservableCollection<ViewPage> Pages
        {
            get
            {
                return m_Pages;
            }
        }

        private LinkedListNode<ViewModelBase> m_CurrentViewModel = null;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return m_CurrentViewModel.Value;
            }

//            set
//            {
//                m_CurrentViewModel.Value = value;
//            }
        }

        LexemeViewModel m_CurrentLexeme;
        public LexemeViewModel CurrentLexeme
        {
            get
            {
                return m_CurrentLexeme;
            }

            //            set
            //            {
            //                m_CurrentViewModel.Value = value;
            //            }
        }

        private ViewPage m_CurrentViewPage;
        public ViewPage CurrentViewPage
        {
            get
            {
                return m_CurrentViewPage;
            }

            set
            {
                m_CurrentViewPage = value;
            }
        }

        public string CurrentHeader
        {
            get
            {
                if (m_CurrentViewPage != null)
                {
                    return m_CurrentViewPage.Header;
                }
                else
                {
                    MessageBox.Show("Internal error: current page is null.");
                    return "";
                }
            }
        }

        public ViewModelBase LexemeInfo
        {
            get
            {
                return m_CurrentViewPage.LexemeInfo;
            }
        }

        public string Type
        {
            get
            {
                return "Lexeme";
            }
        }

        private int m_iCurrentTab;
        public int CurrentTab
        {
            get
            {
                return m_iCurrentTab;
            }
            set
            {
                m_iCurrentTab = value;
            }
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

        public bool IsForwardButtonEnabled
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

        private RelayCommand closeCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                if (this.closeCommand == null)
                {
                    this.closeCommand = new RelayCommand(w => CloseCommandMethod(w));
                }
                return this.closeCommand;
            }
        }

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

        private ICommand m_ShowRegressionPageCommand;
        public ICommand ShowRegressionPageCommand
        {
            get
            {
                return m_ShowRegressionPageCommand;
            }
            set
            {
                m_ShowRegressionPageCommand = value;
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
            m_Pages = new ObservableCollection<ViewPage>();
            BackButtonCommand = new RelayCommand(new Action<object>(GoBack));
            ForwardButtonCommand = new RelayCommand(new Action<object>(GoForward));
            OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
            SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
            ShowRegressionPageCommand = new RelayCommand(new Action<object>(ShowRegression));
            ImportRegressionDataCommand = new RelayCommand(new Action<object>(ImportRegressionData));
            ExportRegressionDataCommand = new RelayCommand(new Action<object>(ExportRegressionData));

            m_MainModel = new MainModel();
            m_MainModel.Path = "";
            m_BreadCrumbs = new LinkedList<ViewModelBase>();
//            m_LexemeGridViewModel = new ViewPage ("парадигмы", new LexemeGridViewModel());
//            m_Pages.Add(m_LexemeGridViewModel);
//            m_iSelectedIndex = 0;
//            m_CurrentViewModel = m_BreadCrumbs.AddLast(m_LexemeGridViewModel.Page);

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

            EnterDataViewModel eddvm = (EnterDataViewModel)edd.DataContext;
            m_MainModel.SearchByInitialForm(eddvm.DataString);
            if (m_MainModel.NLexemes < 1)
            {
                return;
            }

            foreach (CLexemeManaged lexeme in m_MainModel)
            { 
                if (null == lexeme)
                {
                    MessageBox.Show("Internal error: lexeme descriptor is corrupt.");
                    return;
                }

                bool bIsNewLexeme = true;
                foreach (ViewPage page in m_Pages)
                {
                    LexemeViewModel knownLvm = (LexemeViewModel)page.LexemeInfo;
                    if (knownLvm.Lexeme == null)
                        continue;
                    if (lexeme.llLexemeId() == knownLvm.Lexeme.llLexemeId())
                    {
                        bIsNewLexeme = false;
                        break;
                    }
                }

                if (!bIsNewLexeme)
                {
                    continue;
                }

                LexemeViewModel lvm = new LexemeViewModel(lexeme);
//                lvm.RemoveLexemeEvent += new LexemeViewModel.RemoveLexemeHandler(RemoveLexeme);
                //                ((LexemeGridViewModel)m_LexemeGridViewModel.Page).Add(lvm);
                m_CurrentLexeme = lvm;

                switch (lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        //                        lvm.ShowNounFormsEvent += new LexemeViewModel.ShowNounFormsHandler(ShowNoun);
                        ShowNoun(lexeme, lvm);
                        break;
                    case EM_PartOfSpeech.POS_ADJ:
                        ShowAdj(lexeme, lvm);
//                        lvm.ShowAdjFormsEvent += new LexemeViewModel.ShowAdjFormsHandler(ShowAdj);
                        break;
                    case EM_PartOfSpeech.POS_VERB:
                        ShowVerb(lexeme, lvm);
//                        lvm.ShowVerbFormsEvent += new LexemeViewModel.ShowVerbFormsHandler(ShowVerb);
                        break;
                    default:
                        MessageBox.Show("Illegal part of speech value in lexeme descriptor.");
                        return;
                }
            }

            //            m_CurrentViewModel = m_BreadCrumbs.AddAfter(m_CurrentViewModel, m_LexemeGridViewModel);
            UpdateView();

        }   // SearchByInitialForm()

        void ShowNoun(CLexemeManaged l, LexemeViewModel lvm)
        {
            if (null == m_NounViewModels)
            {
                m_NounViewModels = new Dictionary<CLexemeManaged, ViewPage>();
            }

            if (!m_NounViewModels.TryGetValue(l, out ViewPage nvp))
            {
                NounViewModel nvm = new NounViewModel(l, m_MainModel);
                //                nvm.BackButtonEvent += new NounViewModel.BackButtonHandler(GoBack);
                nvp = new ViewPage(l.sSourceForm(), lvm, nvm);
                m_Pages.Add(nvp);
                m_NounViewModels[l] = nvp;
                m_iCurrentTab = m_Pages.Count - 1;                
            }
            m_CurrentViewModel = m_BreadCrumbs.AddLast(nvp.Page);
            m_CurrentViewPage = nvp; 
//            UpdateView();
        }

        void ShowAdj(CLexemeManaged lexeme, LexemeViewModel lvm)
        {
            if (null == m_AdjViewModels)
            {
                m_AdjViewModels = new Dictionary<CLexemeManaged, ViewPage>();
            }

            if (!m_AdjViewModels.TryGetValue(lexeme, out ViewPage avp))
            {
                AdjViewModel avm = new AdjViewModel(lexeme, EM_Subparadigm.SUBPARADIGM_LONG_ADJ, m_MainModel);
                avp = new ViewPage(lexeme.sSourceForm(), lvm, avm);
                //                avm.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                m_Pages.Add(avp);
                m_AdjViewModels[lexeme] = avp;
                m_iCurrentTab = m_Pages.Count - 1;
            }

            m_CurrentViewModel = m_BreadCrumbs.AddLast(avp.Page);
            m_CurrentViewPage = avp;
            UpdateView();
        }

        void ShowVerb(CLexemeManaged lexeme, LexemeViewModel lvm)
        {
            if (null == m_VerbViewModels)
            {
                m_VerbViewModels = new Dictionary<CLexemeManaged, ViewPage>();
            }

            if (!m_VerbViewModels.TryGetValue(lexeme, out ViewPage vvp))
            {
                VerbViewModel vvm = new VerbViewModel(lexeme, m_MainModel, lvm);
                vvp = new ViewPage(lexeme.sSourceForm(), lvm, vvm);
//                vvm.BackButtonEvent += new VerbViewModel.BackButtonHandler(GoBack);
                vvm.ShowParticipleFormsEvent += new VerbViewModel.ShowParticipleForms(ShowParticiple);
                m_Pages.Add(vvp);
                m_VerbViewModels[lexeme] = vvp;
                m_iCurrentTab = m_Pages.Count - 1;
            }

            m_CurrentViewModel = m_BreadCrumbs.AddLast(vvp.Page);
            m_CurrentViewPage = vvp;
            UpdateView();
        }

        void ShowParticiple(CLexemeManaged lexeme, EM_Subparadigm sp, ViewModelBase lvm)
        {
            switch (sp)
            {
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    if (!m_PartPresActViewModels.TryGetValue(lexeme, out ViewPage avpPresAct))
                    {
                        AdjViewModel avmPresAct = new AdjViewModel(lexeme, sp, m_MainModel);
                        //                        avmPresAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        avpPresAct = new ViewPage(lexeme.sSourceForm() + " прич. наст. д.", lvm, avmPresAct);
                        m_Pages.Add(avpPresAct);
                        m_PartPresActViewModels[lexeme] = avpPresAct;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPresAct.Page);
                    m_CurrentViewPage = avpPresAct;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    if (!m_PartPastActViewModels.TryGetValue(lexeme, out ViewPage avpPastAct))
                    {
                        AdjViewModel avmPastAct = new AdjViewModel(lexeme, sp, m_MainModel);
                        avpPastAct = new ViewPage(lexeme.sSourceForm() + " прич. прош. д.", lvm, avmPastAct);
                        m_Pages.Add(avpPastAct);
//                        avmPastAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPastActViewModels[lexeme] = avpPastAct;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPastAct.Page);
                    m_CurrentViewPage = avpPastAct;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    if (!m_PartPresPassViewModels.TryGetValue(lexeme, out ViewPage avpPresPass))
                    {
                        AdjViewModel avmPresPass = new AdjViewModel(lexeme, sp, m_MainModel);
                        avpPresPass = new ViewPage(lexeme.sSourceForm() + " прич. наст. страд.", lvm, avmPresPass);
                        m_Pages.Add(avpPresPass);
//                        avmPresPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_PartPresPassViewModels[lexeme] = avpPresPass;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPresPass.Page);
                    m_CurrentViewPage = avpPresPass;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    if (!m_PartPastPassViewModels.TryGetValue(lexeme, out ViewPage avpPastPass))
                    {
                        AdjViewModel avmPastPass = new AdjViewModel(lexeme, sp, m_MainModel);
                        avpPastPass = new ViewPage(lexeme.sSourceForm() + " прич. прош. страд.", lvm, avmPastPass);
                        //                        avmPastPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                        m_Pages.Add(avpPastPass);
                        m_PartPastPassViewModels[lexeme] = avpPastPass;
                    }
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPastPass.Page);
                    m_CurrentViewPage = avpPastPass;
                    break;
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                    // handled together with long forms, no need for separate processing
                    break;
                default:
                    MessageBox.Show("Unexpected subparadigm.");
                    return;
            }

            m_iCurrentTab = m_Pages.Count - 1;
            UpdateView();

        }       //  ShowParticiple()

        public void ShowRegression(object obj)
        {
//            m_MainModel.Clear();

            if (null == m_RegressionGridViewModel)
            {
                m_RegressionGridViewModel = new ViewPage("Тест", new LexemeViewModel(), new RegressionGridViewModel(m_MainModel));
                m_Pages.Add(m_RegressionGridViewModel);

//                m_RegressionGridViewModel.BackButtonEvent += new RegressionGridViewModel.BackButtonHandler(GoBack);
            }
            m_CurrentViewModel = m_BreadCrumbs.AddLast(m_RegressionGridViewModel.Page);
            m_CurrentViewPage = m_RegressionGridViewModel;
            m_iCurrentTab = m_Pages.Count - 1;
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
            openFileDialog.CheckFileExists = false;

            if (true == openFileDialog.ShowDialog())
            {
                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
                m_MainModel.ExportRegressionData(openFileDialog.FileName, progress);
            }
        }

        private void CloseCommandMethod(object arg)
        {
            ViewModelBase vm = (ViewModelBase)arg;

            List<ViewPage> pagesToRemove = new List<ViewPage>();

            foreach (ViewPage page in m_Pages)
            {
                if (page.Page.InstanceId == vm.InstanceId)
                {
                    pagesToRemove.Add(page);
                }
            }

            List<ViewPage> derivedPagesToRemove = new List<ViewPage>();
            foreach (ViewPage pageToRemove in pagesToRemove)
            {
                if (((LexemeViewModel)pageToRemove.LexemeInfo).Lexeme != null)
                {
                    foreach (ViewPage page in m_Pages)
                    {
                        if (page.Page.InstanceId == pageToRemove.Page.InstanceId)
                        {
                            continue;
                        }

                        if (((LexemeViewModel)page.LexemeInfo).Lexeme == null)
                        {
                            continue;
                        }

                        if (((LexemeViewModel)page.LexemeInfo).Lexeme == ((LexemeViewModel)pageToRemove.LexemeInfo).Lexeme)
                        {
                            derivedPagesToRemove.Add(page);
                        }
                    }
                }
            }

            foreach (var pageView in pagesToRemove)
            {
                CLexemeManaged l = ((LexemeViewModel)pageView.LexemeInfo).Lexeme;
                if (l != null)
                {
                    m_MainModel.RemoveLexeme(l);
                }

                m_Pages.Remove(pageView);
            }

            foreach (var pageView in derivedPagesToRemove)
            {
                m_Pages.Remove(pageView);
            }

        }   //  CloseCommandMethod()

        void UpdateProgress(int iPercent, bool bDone)
        {

        }

        private void UpdateView()
        {
            this.OnPropertyChanged("CurrentViewModel");
            this.OnPropertyChanged("CurrentTab");
            this.OnPropertyChanged("IsBackButtonEnabled");
            this.OnPropertyChanged("IsForwardButtonEnabled");
            this.OnPropertyChanged("Type");
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
