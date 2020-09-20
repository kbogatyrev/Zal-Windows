using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading;
//using System.ComponentModel;

using MainLibManaged;
using System.Windows.Data;
using System.Globalization;
using ZalTestApp.Properties;

namespace ZalTestApp
{
    public enum EMark { None, IsIrregular, IsEdited };

    public sealed class StyleConverterMarks : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string uri = sAssemblyName + ";component/Skins.xaml";
            ResourceDictionary resourceDictionary =
                (ResourceDictionary)Application.LoadComponent(new System.Uri(uri, System.UriKind.Relative));

            if (EMark.None == (EMark)value)
            {
                return resourceDictionary["NoMark"] as Style;
            }
            else if (EMark.IsIrregular == (EMark)value)
            {
                return resourceDictionary["IrregularFormMark"] as Style;
            }
            else if (EMark.IsEdited == (EMark)value)
            {
                return resourceDictionary["EditedFormMark"] as Style;
            }
            else
            {
                return resourceDictionary["NoMark"] as Style;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
                if (m_LexemeInfo != null)
                {
                    return "Lexeme";
                }
                else
                {
                    return "Generic";
                }
            }
        }
    }

    public class MainViewModel : ViewModelBase
    {
        private ProgressViewModel m_ProgressViewModel = null;

        private LinkedList<ViewModelBase> m_BreadCrumbs = null;

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
                if (m_CurrentViewModel != null)
                {
                    return m_CurrentViewModel.Value;
                }
                else
                {
                    return null;
                }
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
//                    MessageBox.Show("Internal error: current page is null.");
                    return "";
                }
            }
        }

        public ViewModelBase LexemeInfo
        {
            get
            {
                if (m_CurrentViewPage != null)
                {
                    return m_CurrentViewPage.LexemeInfo;
                }
                else
                {
                    return null;
                }
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


        #region Properties

        private bool m_bDbOpen = false;
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

        private bool m_bEditDbOpen;
        public bool EditDbOpen
        {
            get
            {
                return m_bEditDbOpen;
            }
            set
            {
                m_bEditDbOpen = value;
                OnPropertyChanged("EditDbOpen");
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

        private ICommand m_OpenEditDictionaryCommand;
        public ICommand OpenEditDictionaryCommand
        {
            get
            {
                return m_OpenEditDictionaryCommand;
            }
            set
            {
                m_OpenEditDictionaryCommand = value;
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

        private ICommand m_GenerateAllFormsCommand;
        public ICommand GenerateAllFormsCommand
        {
            get
            {
                return m_GenerateAllFormsCommand;
            }
            set
            {
                m_GenerateAllFormsCommand = value;
            }
        }

        private ICommand m_ParseWordCommand;
        public ICommand ParseWordCommand
        {
            get
            {
                return m_ParseWordCommand;
            }
            set
            {
                m_ParseWordCommand = value;
            }
        }

        private ICommand m_CreateTextParserCommand;
        public ICommand CreateTextParserCommand
        {
            get
            {
                return m_CreateTextParserCommand;
            }
            set
            {
                m_CreateTextParserCommand = value;
            }
        }

        private ICommand m_NewLexemeCommand;
        public ICommand NewLexemeCommand
        {
            get
            {
                return m_NewLexemeCommand;
            }
            set
            {
                m_NewLexemeCommand = value;
            }
        }

        private ICommand m_EditLexemeCommand;
        public ICommand EditLexemeCommand
        {
            get
            {
                return m_EditLexemeCommand;
            }
            set
            {
                m_EditLexemeCommand = value;
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
            try
            {
                m_Pages = new ObservableCollection<ViewPage>();
                BackButtonCommand = new RelayCommand(new Action<object>(GoBack));
                ForwardButtonCommand = new RelayCommand(new Action<object>(GoForward));
                OpenDictionaryCommand = new RelayCommand(new Action<object>(OpenDictionary));
                //            OpenEditDictionaryCommand = new RelayCommand(new Action<object>(OpenEditDictionary));
                SearchByInitialFormCommand = new RelayCommand(new Action<object>(SearchByInitialForm));
                GenerateAllFormsCommand = new RelayCommand(new Action<object>(GenerateAllForms));
                ParseWordCommand = new RelayCommand(new Action<object>(ParseWord));
//                CreateTextParserCommand = new RelayCommand(new Action<object>(CreateTextParser));
                NewLexemeCommand = new RelayCommand(new Action<object>(NewLexeme));
                EditLexemeCommand = new RelayCommand(new Action<object>(EditLexeme));
                ShowRegressionPageCommand = new RelayCommand(new Action<object>(ShowRegression));
                ImportRegressionDataCommand = new RelayCommand(new Action<object>(ImportRegressionData));
                ExportRegressionDataCommand = new RelayCommand(new Action<object>(ExportRegressionData));

                m_bDbOpen = false;
                m_bEditDbOpen = false;

                m_MainModel = new MainModel();
                if (!m_MainModel.Initialized)
                {
                    MessageBox.Show("Unable to initialize the database.");
                    return;
                }

//                m_bDbOpen = true;
//                m_bEditDbOpen = true;

                m_BreadCrumbs = new LinkedList<ViewModelBase>();
                //            m_LexemeGridViewModel = new ViewPage ("парадигмы", new LexemeGridViewModel());
                //            m_Pages.Add(m_LexemeGridViewModel);
                //            m_iSelectedIndex = 0;
                //            m_CurrentViewModel = m_BreadCrumbs.AddLast(m_LexemeGridViewModel.Page);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

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

        //public void OpenEditDictionary(object obj)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    openFileDialog.InitialDirectory = "c:\\";
        //    openFileDialog.Filter = "All files (*.*)|*.*|SQLite 3 files (*.db3)|*.db3";
        //    openFileDialog.FilterIndex = 2;
        //    openFileDialog.RestoreDirectory = true;

        //    EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;
        //    if (true == openFileDialog.ShowDialog())
        //    {
        //        eRet = m_MainModel.OpenEditDictionary(openFileDialog.FileName);
        //    }

        //    if (EM_ReturnCode.H_NO_ERROR == eRet)
        //    {
        //        EditDbOpen = true;
        //    }
        //}

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

            foreach (string sLexemeHash in m_MainModel)
            {
                CLexemeManaged lexeme = m_MainModel.LexemeFromHash(sLexemeHash);
                if (null == lexeme)
                {
                    MessageBox.Show("Internal error: lexeme descriptor is corrupt.");
                    return;
                }

                bool bIsNewLexeme = true;
                foreach (ViewPage page in m_Pages)
                {
                    LexemeViewModel knownLvm = (LexemeViewModel)page.LexemeInfo;
                    if (null == knownLvm)
                    {
                        continue;
                    }
                    if (null == knownLvm.Lexeme)
                    {
                        continue;
                    }

                    if (lexeme.llLexemeId() == knownLvm.Lexeme.llLexemeId() && 
                            lexeme.iInflectionId() == knownLvm.Lexeme.iInflectionId())
                    {
                        bIsNewLexeme = false;
                        break;
                    }
                }

                if (!bIsNewLexeme)
                {
                    continue;
                }

                ShowParadigm(lexeme);

            }       // foreach (ViewPage ...)

            //            m_CurrentViewModel = m_BreadCrumbs.AddAfter(m_CurrentViewModel, m_LexemeGridViewModel);
            UpdateView();

        }   // SearchByInitialForm()

        public void GenerateAllForms(object obj)
        {
            m_MainModel.GenerateAllForms();

        }   // GenerateAllForms()

        public void ParseWord(object obj)
        {
            EnterDataDlg edd = new EnterDataDlg();
            edd.Owner = Application.Current.MainWindow;
            EnterDataViewModel eddvm = (EnterDataViewModel)edd.DataContext;
            eddvm.DlgTitle = "Словоформа";
            bool? bnRet = edd.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

            m_MainModel.ParseWord(eddvm.DataString);

            if (m_MainModel.NParses < 1)
            {
                MessageBox.Show("Словоформа не найдена."); 
                return;
            }

            IEnumerator<string> parseEnumerator = (IEnumerator<string>)m_MainModel.GetParseEnumerator();
            while (parseEnumerator.MoveNext())
            {
                string sGramHash = (string)parseEnumerator.Current;
                List<CWordFormManaged> listWf = m_MainModel.WordFormsFromHash(sGramHash);
                foreach (CWordFormManaged wf in listWf)
                {
                    ShowParsedForm(wf);
                }
            }

            //            m_CurrentViewModel = m_BreadCrumbs.AddAfter(m_CurrentViewModel, m_LexemeGridViewModel);
            UpdateView();

        }   // ParseWord()

/*
        public void CreateTextParser(object obj)
        {
            EnterTextParserDataDlgViewModel etpModel = new EnterTextParserDataDlgViewModel(m_MainModel);
            EnterTextParserDataDlg dlg = new EnterTextParserDataDlg(etpModel);
            dlg.Owner = Application.Current.MainWindow;

            bool? bnRet = dlg.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

            m_CurrentLexeme = null;
            ParsedTextViewModel parser = new ParsedTextViewModel(m_MainModel);
            m_CurrentViewPage = new ViewPage("Текст", null, parser);
            m_Pages.Add(m_CurrentViewPage);
            m_iCurrentTab = m_Pages.Count - 1;

        }   // CreateTextParser()
*/

        void NewLexeme(object obj)
        {
            CLexemeManaged l = null;
            var bRet = m_MainModel.bCreateLexeme(ref l);
            if (null == l || !bRet)
            {
                return;
            }

            EnterLexemePropertiesViewModel elpModel = new EnterLexemePropertiesViewModel(l, true);
            EnterLexemePropertiesDlg dlg = new EnterLexemePropertiesDlg(elpModel);
            dlg.Owner = Application.Current.MainWindow;

            bool? bnRet = dlg.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

//            bRet = m_MainModel.bSaveLexeme(l);
//            if (bRet)
//            {
//                MessageBox.Show("Лексема сохранена.");
//            }
//            else
//            {
//                MessageBox.Show("Не удалось сохранить лексему.");
//            }
        }

        void EditLexeme(object obj)
        {
            CLexemeManaged sourceLexeme = (CLexemeManaged)obj;
            CLexemeManaged editLexeme = null;
            bool bRet = m_MainModel.bEditLexeme(sourceLexeme, ref editLexeme);
            if (!bRet)
            {
                return;
            }

            EnterLexemePropertiesViewModel elpModel = new EnterLexemePropertiesViewModel(editLexeme, false);
            elpModel.ShowParadigmForEditEvent += ShowParadigm;
            EnterLexemePropertiesDlg dlg = new EnterLexemePropertiesDlg(elpModel);
            dlg.Owner = Application.Current.MainWindow;

            bool? bnRet = dlg.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

            if (elpModel.HeadwordChanged)
            {
                bRet = m_MainModel.bSaveHeadword(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить заглавную форму.");
                }
            }

            if (elpModel.AspectPairChanged)
            {
                bRet = m_MainModel.bSaveAspectPairInfo(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить информацию о видовой паре.");
                }
            }

            if (elpModel.P2Changed)
            {
                bRet = m_MainModel.bSaveP2Info(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить информацию о втором предложном.");
                }
            }

            if (elpModel.CommonDeviationChanged)
            {
                bRet = m_MainModel.bSaveCommonDeviation(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить информацию о стандартных отклонениях.");
                }
            }

            if (elpModel.InflectionGroupChanged)
            {
                bRet = m_MainModel.bSaveInflectionInfo(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить лексему.");
                }
            }

            if (elpModel.DescriptorChanged)
            {
                bRet = m_MainModel.bSaveDescriptorInfo(editLexeme);
                if (!bRet)
                {
                    MessageBox.Show("Не удалось сохранить лексему.");
                }
            }

//            bRet = m_MainModel.bSaveLexeme(editLexeme);
//            if (bRet)
//            {
//                MessageBox.Show("Лексема сохранена.");
//            }
//            else
//            {
//                MessageBox.Show("Не удалось сохранить лексему.");
//            }

        }       //  EditLexeme()

        void ShowParadigm(CLexemeManaged lexeme)
        {
            LexemeViewModel lexemeViewModel = new LexemeViewModel(lexeme);
            //                lvm.RemoveLexemeEvent += new LexemeViewModel.RemoveLexemeHandler(RemoveLexeme);
            lexemeViewModel.EditLexemeEvent += new LexemeViewModel.EditLexemeHandler(EditLexeme);
            m_CurrentLexeme = lexemeViewModel;
            ViewModelBase paradigmViewModel = null;

            switch (lexeme.ePartOfSpeech())
            {
                case EM_PartOfSpeech.POS_NOUN:
                case EM_PartOfSpeech.POS_PRONOUN:
                case EM_PartOfSpeech.POS_NUM:
                    paradigmViewModel = new NounViewModel(lexeme, m_MainModel);
                    break;
                case EM_PartOfSpeech.POS_ADJ:
                case EM_PartOfSpeech.POS_PRONOUN_ADJ:
                case EM_PartOfSpeech.POS_NUM_ADJ:
                    paradigmViewModel = new AdjViewModel(lexeme, EM_Subparadigm.SUBPARADIGM_LONG_ADJ, m_MainModel);
                    break;
                case EM_PartOfSpeech.POS_VERB:
                    VerbViewModel vvm = new VerbViewModel(lexeme, m_MainModel, lexemeViewModel);
                    vvm.ShowParticipleFormsEvent += new VerbViewModel.ShowParticipleForms(ShowParticiple);
                    paradigmViewModel = vvm;
                    break;
                case EM_PartOfSpeech.POS_ADV:
                case EM_PartOfSpeech.POS_INTERJ:
                case EM_PartOfSpeech.POS_PARTICLE:
                case EM_PartOfSpeech.POS_CONJUNCTION:
                case EM_PartOfSpeech.POS_PREDIC:
                case EM_PartOfSpeech.POS_PREPOSITION:
                case EM_PartOfSpeech.POS_PRONOUN_PREDIC:
                case EM_PartOfSpeech.POS_NULL:
                    paradigmViewModel = new ViewModelBase();
                    break;

                default:
                    MessageBox.Show("Illegal part of speech value in lexeme descriptor.");
                    return;
            }

            m_CurrentViewPage = new ViewPage(lexeme.sSourceForm(), lexemeViewModel, paradigmViewModel);
            m_Pages.Add(m_CurrentViewPage);
            m_iCurrentTab = m_Pages.Count - 1;
            //                m_CurrentViewModel = m_BreadCrumbs.AddLast(nvp.Page);
        }

        void ShowParsedForm(CWordFormManaged wf)
        {
            ViewModelBase wordFormViewModel = new WordFormViewModel(wf);

            CLexemeManaged lexeme = m_MainModel.GetLexemeFromWordform(wf);
            if (null == lexeme)
            {
                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return;
            }

            bool bReadOnly = true;
            LexemeViewModel lexemeViewModel = new LexemeViewModel(lexeme, bReadOnly);
            m_CurrentViewPage = new ViewPage(wf.sWordForm(), lexemeViewModel, wordFormViewModel);
            m_Pages.Add(m_CurrentViewPage);
            m_iCurrentTab = m_Pages.Count - 1;
            //                m_CurrentViewModel = m_BreadCrumbs.AddLast(nvp.Page);
        }

        void ShowParticiple(CLexemeManaged lexeme, EM_Subparadigm sp, ViewModelBase parent)
        {
            LexemeViewModel parentViewModel = (LexemeViewModel)parent;

            foreach (ViewPage viewPage in m_Pages)
            {
                LexemeViewModel currentLexeme = (LexemeViewModel)viewPage.LexemeInfo;
                if (parentViewModel.Lexeme == currentLexeme.Lexeme)
                {
                    if (typeof(AdjViewModel) == viewPage.Page.GetType())
                    {
                        AdjViewModel avm = (AdjViewModel)viewPage.Page;
                        if (avm.Subparadigm == sp)
                        {
                            m_CurrentViewModel = m_BreadCrumbs.AddLast(avm);
                            m_CurrentViewPage = viewPage;
                            return;
                        }
                    }
                }
            }

            switch (sp)
            {
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    AdjViewModel avmPresAct = new AdjViewModel(lexeme, sp, m_MainModel);
                    ViewPage avpPresAct = new ViewPage(lexeme.sSourceForm() + " прич. наст. д.", parent, avmPresAct);
                    m_Pages.Add(avpPresAct);
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPresAct.Page);
                    m_CurrentViewPage = avpPresAct;
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    AdjViewModel avmPastAct = new AdjViewModel(lexeme, sp, m_MainModel);
                    ViewPage avpPastAct = new ViewPage(lexeme.sSourceForm() + " прич. прош. д.", parent, avmPastAct);
                    m_Pages.Add(avpPastAct);
//                        avmPastAct.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPastAct.Page);
                    m_CurrentViewPage = avpPastAct;
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    AdjViewModel avmPresPass = new AdjViewModel(lexeme, sp, m_MainModel);
                    ViewPage avpPresPass = new ViewPage(lexeme.sSourceForm() + " прич. наст. страд.", parent, avmPresPass);
                    m_Pages.Add(avpPresPass);
//                        avmPresPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                    m_CurrentViewModel = m_BreadCrumbs.AddLast(avpPresPass.Page);
                    m_CurrentViewPage = avpPresPass;
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    AdjViewModel avmPastPass = new AdjViewModel(lexeme, sp, m_MainModel);
                    ViewPage avpPastPass = new ViewPage(lexeme.sSourceForm() + " прич. прош. страд.", parent, avmPastPass);
                    //                        avmPastPass.BackButtonEvent += new AdjViewModel.BackButtonHandler(GoBack);
                    m_Pages.Add(avpPastPass);
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

            ViewPage rvp = new ViewPage("Тест", new LexemeViewModel(), new RegressionGridViewModel(m_MainModel));

//                m_RegressionGridViewModel.BackButtonEvent += new RegressionGridViewModel.BackButtonHandler(GoBack);
            m_Pages.Add(rvp);
            m_CurrentViewModel = m_BreadCrumbs.AddLast(rvp.Page);
            m_CurrentViewPage = rvp;
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
                if (null == m_ProgressViewModel)
                {
                    m_ProgressViewModel = new ProgressViewModel();
                }

                ProgressDialog pd = new ProgressDialog(m_ProgressViewModel);
                pd.Owner = Application.Current.MainWindow;
                pd.Show();

                try
                {
                    RegressionDataImportThread rt = new RegressionDataImportThread(m_MainModel, m_ProgressViewModel, pd, openFileDialog.FileName);
                    System.Threading.Thread t = new Thread(new ThreadStart(rt.ThreadProc));
                    t.Name = "TestApp test data import thread";
                    t.IsBackground = true;
                    //              m_WorkerThread.Priority = ThreadPriority.Lowest;
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
//                    t.Join();
//                    pd.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }
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
//                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
//                m_MainModel.ExportRegressionData(openFileDialog.FileName, progress);
                if (null == m_ProgressViewModel)
                {
                    m_ProgressViewModel = new ProgressViewModel();
                }

                ProgressDialog pd = new ProgressDialog(m_ProgressViewModel);
                pd.Owner = Application.Current.MainWindow;
                pd.Show();

                try
                {
                    RegressionDataExportThread rt = new RegressionDataExportThread(m_MainModel, m_ProgressViewModel, pd, openFileDialog.FileName);
                    System.Threading.Thread t = new Thread(new ThreadStart(rt.ThreadProc));
                    t.Name = "TestApp test data export thread";
                    t.IsBackground = true;
                    //              m_WorkerThread.Priority = ThreadPriority.Lowest;
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    //                    t.Join();
                    //                    pd.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }

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
                if (pageToRemove.Page.GetType() == typeof(VerbViewModel))
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

            foreach (ViewPage pageView in pagesToRemove)
            {
                m_Pages.Remove(pageView);
                if (pageView.Page.IsDerived)
                {
                    continue;
                }

                if (pageView.LexemeInfo != null)
                {
                    CLexemeManaged l = ((LexemeViewModel)pageView.LexemeInfo).Lexeme;
                    if (l != null)
                    {
                        m_MainModel.RemoveLexeme(l);
                    }
                }
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

    #region Threading

    public class RegressionDataImportThread
    {
        private MainModel m_Caller;
        private ProgressViewModel m_ProgressViewModel;
        private ProgressDialog m_ProgressDialog;
        private string m_sPath;
        private int m_iPercentDone;
        private delegate void DelegateProgress(int iPercentDone, bool bOperationComplete);

        private void UpdateProgress(int iPercentDone, bool bOperationComplete)
        {
            m_iPercentDone = iPercentDone;
            m_ProgressViewModel.Progress = m_iPercentDone;
        }

        public RegressionDataImportThread(MainModel mm, ProgressViewModel pvm, ProgressDialog pd, string sPath)
        {
            m_Caller = mm;
            m_ProgressViewModel = pvm;
            m_ProgressDialog = pd;
            m_sPath = sPath;
        }
       
        public void ThreadProc()
        {
            try
            {
                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
                EM_ReturnCode eRet = m_Caller.ImportRegressionData(m_sPath, progress);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show("Unable to load test data", "Error");
                }
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

            Application.Current.Dispatcher.Invoke(new Action(() => { m_ProgressDialog.Close(); }));

        }   //  ThreadProc()

    }   //  public class RegressionDataThread

    public class RegressionDataExportThread
    {
        private MainModel m_Caller;
        private ProgressViewModel m_ProgressViewModel;
        private ProgressDialog m_ProgressDialog;
        private string m_sPath;
        private int m_iPercentDone;
        private delegate void DelegateProgress(int iPercentDone, bool bOperationComplete);

        private void UpdateProgress(int iPercentDone, bool bOperationComplete)
        {
            m_iPercentDone = iPercentDone;
            m_ProgressViewModel.Progress = m_iPercentDone;
        }

        public RegressionDataExportThread(MainModel mm, ProgressViewModel pvm, ProgressDialog pd, string sPath)
        {
            m_Caller = mm;
            m_ProgressViewModel = pvm;
            m_ProgressDialog = pd;
            m_sPath = sPath;
        }

        public void ThreadProc()
        {
            try
            {
                MainLibManaged.DelegateProgress progress = new MainLibManaged.DelegateProgress(UpdateProgress);
                EM_ReturnCode eRet = m_Caller.ExportRegressionData(m_sPath, progress);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show("Unable to write test data", "Error");
                }
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

            Application.Current.Dispatcher.Invoke(new Action(() => { m_ProgressDialog.Close(); }));

        }   //  ThreadProc()

    }   //  public class RegressionDataThread

    #endregion

}   //  namespace ZalTestApp
