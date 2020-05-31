using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class VerbViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void ShowParticipleForms(CLexemeManaged l, EM_Subparadigm sp, ViewModelBase lvm);
        public event ShowParticipleForms ShowParticipleFormsEvent;

        ViewModelBase m_LexemeViewModel;

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

        private ICommand m_EditCommand;
        public ICommand EditCommand
        {
            get
            {
                return m_EditCommand;
            }
            set
            {
                m_EditCommand = value;
            }
        }

        private ICommand m_SaveFormsCommand;
        public ICommand SaveFormsCommand
        {
            get
            {
                return m_SaveFormsCommand;
            }
            set
            {
                m_SaveFormsCommand = value;
            }
        }

        private ICommand m_ShowPresActFormsCommand;
        public ICommand ShowPresActFormsCommand
        {
            get
            {
                return m_ShowPresActFormsCommand;
            }
            set
            {
                m_ShowPresActFormsCommand = value;
            }
        }

        private ICommand m_ShowPastActFormsCommand;
        public ICommand ShowPastActFormsCommand
        {
            get
            {
                return m_ShowPastActFormsCommand;
            }
            set
            {
                m_ShowPastActFormsCommand = value;
            }
        }

        private ICommand m_ShowPresPassFormsCommand;
        public ICommand ShowPresPassFormsCommand
        {
            get
            {
                return m_ShowPresPassFormsCommand;
            }
            set
            {
                m_ShowPresPassFormsCommand = value;
            }
        }

        private ICommand m_ShowPastPassFormsCommand;
        public ICommand ShowPastPassFormsCommand
        {
            get
            {
                return m_ShowPastPassFormsCommand;
            }
            set
            {
                m_ShowPastPassFormsCommand = value;
            }
        }

        private ICommand m_FormScrollUpCommand;
        public ICommand FormScrollUpCommand
        {
            get
            {
                return m_FormScrollUpCommand;
            }
            set
            {
                m_FormScrollUpCommand = value;
            }
        }

        private ICommand m_FormScrollDownCommand;
        public ICommand FormScrollDownCommand
        {
            get
            {
                return m_FormScrollDownCommand;
            }
            set
            {
                m_FormScrollDownCommand = value;
            }
        }
        #endregion

        string GetForms(string sFormHash)
        {
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var formsForHash = m_DictFormStatus[sFormHash];
            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return "Error";
            }

            return formsForHash.lstForms[iAt].WordFormManaged.sWordForm();

            //  TODO: comment

        }

        protected string GetForm(string sDisplayHash, EM_Subparadigm eSubparadigm)
        {
            string sFormHash = sDisplayHash;
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var formsForHash = m_DictFormStatus[sFormHash];
            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return "Error";
            }

            return formsForHash.lstForms[iAt].StressedWordform;

            //  TODO: comment

        }

        EMark GetFormStatus(string sFormHash)
        {
            string sLexemeHash = m_Lexeme.sHash();
            if (m_MainModel.bIsEdited(sLexemeHash, sFormHash))
            {
                return EMark.IsEdited;
            }
            else if (m_MainModel.bIsIrregular(sLexemeHash, sFormHash))
            {
                return EMark.IsIrregular;
            }
            return EMark.None;
        }
/*
        void SetForms(string sHash, string sForms)
        {
            if (!m_DictFormStatus.ContainsKey(sHash))
            {
                return;
            }
            Helpers.AssignDiacritics(sForms, ref sForms);
            var fd = m_DictFormStatus[sHash];
            List<string> l = new List<string>();
            List<Tuple<string, string>> c = new List<Tuple<string, string>>();
            Helpers.CommaSeparatedStringToList(sForms, out l, out c);
            fd.listForms = l;
            fd.listComments = c;
            m_DictFormStatus[sHash] = fd;
        }

*/
        public CLexemeManaged Parent
        {
            get
            {
                return m_Lexeme;
            }
            set
            {
                m_Lexeme = value;
            }
        }

        private string m_sSourceForm;
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

        public string Infinitive
        {
            get { return GetForm("Infinitive", EM_Subparadigm.SUBPARADIGM_INFINITIVE); }
            set { SetForm("Infinitive", value); }
        }

        private EMark m_eInfinitive_Marks = EMark.None;
        public EMark Infinitive_Marks
        {
            get { return GetFormStatus("Infinitive"); }
            set { m_eInfinitive_Marks = value; OnPropertyChanged("Infinitive_Marks"); }
        }

        #region Bindings_PresentTense

        public string Pres_Sg_1
        {
            get { return GetForm("Pres_Sg_1", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Sg_1", value); }
        }

        private EMark m_ePres_Sg_1_Marks = EMark.None;
        public EMark Pres_Sg_1_Marks
        {
            get { return GetFormStatus("Pres_Sg_1"); }
            set { m_ePres_Sg_1_Marks = value; OnPropertyChanged("Pres_Sg_1_Marks"); }
        }

        public string Pres_Sg_2
        {
            get { return GetForm("Pres_Sg_2", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Sg_2", value); }
        }

        private EMark m_ePres_Sg_2_Marks = EMark.None;
        public EMark Pres_Sg_2_Marks
        {
            get { return GetFormStatus("Pres_Sg_2"); }
            set { m_ePres_Sg_2_Marks = value; OnPropertyChanged("Pres_Sg_2_Marks"); }
        }

        public string Pres_Sg_3
        {
            get { return GetForm("Pres_Sg_3", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Sg_3", value); }
        }

        private EMark m_ePres_Sg_3_Marks = EMark.None;
        public EMark Pres_Sg_3_Marks
        {
            get { return GetFormStatus("Pres_Sg_3"); }
            set { m_ePres_Sg_3_Marks = value; OnPropertyChanged("Pres_Sg_3_Marks"); }
        }

        public string Pres_Pl_1
        {
            get { return GetForm("Pres_Pl_1", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Pl_1", value); }
        }

        private EMark m_ePres_Pl_1_Marks = EMark.None;
        public EMark Pres_Pl_1_Marks
        {
            get { return GetFormStatus("Pres_Pl_1"); }
            set { m_ePres_Pl_1_Marks = value; OnPropertyChanged("Pres_Pl_1_Marks"); }
        }

        public string Pres_Pl_2
        {
            get { return GetForm("Pres_Pl_2", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Pl_2", value); }
        }

        private EMark m_ePres_Pl_2_Marks = EMark.None;
        public EMark Pres_Pl_2_Marks
        {
            get { return GetFormStatus("Pres_Pl_2"); }
            set { m_ePres_Pl_2_Marks = value; OnPropertyChanged("Pres_Pl_2_Marks"); }
        }

        public string Pres_Pl_3
        {
            get { return GetForm("Pres_Pl_3", EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE); }
            set { SetForm("Pres_Pl_3", value); }
        }

        private EMark m_ePres_Pl_3_Marks = EMark.None;
        public EMark Pres_Pl_3_Marks
        {
            get { return GetFormStatus("Pres_Pl_3"); }
            set { m_ePres_Pl_3_Marks = value; OnPropertyChanged("Pres_Pl_3_Marks"); }
        }

        #endregion

        #region Bindings_PastTense
        public string Past_M
        {
            get { return GetForm("Past_M", EM_Subparadigm.SUBPARADIGM_PAST_TENSE); }
            set { SetForm("Past_M", value); }
        }

        private EMark m_ePast_M_Marks = EMark.None;
        public EMark Past_M_Marks
        {
            get { return GetFormStatus("Past_M"); }
            set { m_ePast_M_Marks = value; OnPropertyChanged("Past_M_Marks"); }
        }

        public string Past_F
        {
            get { return GetForm("Past_F", EM_Subparadigm.SUBPARADIGM_PAST_TENSE); }
            set { SetForm("Past_F", value); }
        }

        private EMark m_ePast_F_Marks = EMark.None;
        public EMark Past_F_Marks
        {
            get { return GetFormStatus("Past_F"); }
            set { m_ePast_F_Marks = value; OnPropertyChanged("Past_F_Marks"); }
        }

        public string Past_N
        {
            get { return GetForm("Past_N", EM_Subparadigm.SUBPARADIGM_PAST_TENSE); }
            set { SetForm("Past_N", value); }
        }

        private EMark m_ePast_N_Marks = EMark.None;
        public EMark Past_N_Marks
        {
            get { return GetFormStatus("Past_N"); }
            set { m_ePast_N_Marks = value; OnPropertyChanged("Past_N_Marks"); }
        }

        public string Past_Pl
        {
            get { return GetForm("Past_Pl", EM_Subparadigm.SUBPARADIGM_PAST_TENSE); }
            set { SetForm("Past_Pl", value); }
        }

        private EMark m_ePast_Pl_Marks = EMark.None;
        public EMark Past_Pl_Marks
        {
            get { return GetFormStatus("Past_Pl"); }
            set { m_ePast_Pl_Marks = value; OnPropertyChanged("Past_Pl_Marks"); }
        }

        #endregion

        #region Bindings_Imperative
        public string Impv_Sg_2
        {
            get { return GetForm("Impv_Sg_2", EM_Subparadigm.SUBPARADIGM_IMPERATIVE); }
            set { SetForm("Impv_Sg_2", value); }
        }

        private EMark m_eImpv_Sg_2_Marks = EMark.None;
        public EMark Impv_Sg_2_Marks
        {
            get { return GetFormStatus("Impv_Sg_2"); }
            set { m_eImpv_Sg_2_Marks = value; OnPropertyChanged("Impv_Sg_2_Marks"); }
        }

        public string Impv_Pl_2
        {
            get { return GetForm("Impv_Pl_2", EM_Subparadigm.SUBPARADIGM_IMPERATIVE); }
            set { SetForm("Impv_Pl_2", value); }
        }

        private EMark m_eImpv_Pl_2_Marks = EMark.None;
        public EMark Impv_Pl_2_Marks
        {
            get { return GetFormStatus("Impv_Pl_2"); }
            set { m_eImpv_Pl_2_Marks = value; OnPropertyChanged("Impv_Pl_2_Marks"); }
        }

        #endregion

        #region Bindings_Non-Finite

        public string PPresA_M_Sg_N
        {
            get { return GetForm("PPresA_M_Sg_N", EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT); }
            set { SetForm("PPresA_M_Sg_N", value); }
        }

        private EMark m_ePPresA_M_Sg_N_Marks = EMark.None;
        public EMark PPresA_M_Sg_N_Marks
        {
            get { return GetFormStatus("PPresA_M_Sg_N"); }
            set { m_ePPresA_M_Sg_N_Marks = value; OnPropertyChanged("PPresA_M_Sg_N_Marks"); }
        }

        private bool m_bPPresAExists;
        public bool PPresAExists
        {
            get
            {
                return m_bPPresAExists;
            }
            set
            {
                m_bPPresAExists = value;
                OnPropertyChanged("PPresAExists");
            }
        }

        public string PPastA_M_Sg_N
        {
            get { return GetForm("PPastA_M_Sg_N", EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT); }
            set { SetForm("PPastA_M_Sg_N", value); }
        }

        private EMark m_ePPastA_M_Sg_N_Marks = EMark.None;
        public EMark PPastA_M_Sg_N_Marks
        {
            get { return GetFormStatus("PPastA_M_Sg_N"); }
            set { m_ePPastA_M_Sg_N_Marks = value; OnPropertyChanged("PPastA_M_Sg_N_Marks"); }
        }

        private bool m_bPPastAExists;
        public bool PPastAExists
        {
            get
            {
                return m_bPPastAExists;
            }
            set
            {
                m_bPPastAExists = value;
                OnPropertyChanged("PPastAExists");
            }
        }

        public string PPresPL_M_Sg_N
        {
            get { return GetForm("PPresPL_M_Sg_N", EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG); }
            set { SetForm("PPresPL_M_Sg_N", value); }
        }

        private EMark m_ePPresPL_M_Sg_N_Marks = EMark.None;
        public EMark PPresPL_M_Sg_N_Marks
        {
            get { return GetFormStatus("PPresPL_M_Sg_N"); }
            set { m_ePPresPL_M_Sg_N_Marks = value; OnPropertyChanged("PPresPL_M_Sg_N_Marks"); }
        }

        private bool m_bPPresPExists;
        public bool PPresPExists
        {
            get
            {
                return m_bPPresPExists;
            }
            set
            {
                m_bPPresPExists = value;
                OnPropertyChanged("PPresPExists");
            }
        }

        public string PPastPL_M_Sg_N
        {
            get { return GetForm("PPastPL_M_Sg_N", EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG); }
            set { SetForm("PPastPL_M_Sg_N", value); }
        }

        private EMark m_ePPastPL_M_Sg_N_Marks = EMark.None;
        public EMark PPastPL_M_Sg_N_Marks
        {
            get { return GetFormStatus("PPastPL_M_Sg_N"); }
            set { m_ePPastPL_M_Sg_N_Marks = value; OnPropertyChanged("PPastPL_M_Sg_N_Marks"); }
        }

        private bool m_bPPastPExists;
        public bool PPastPExists
        {
            get
            {
                return m_bPPastPExists;
            }
            set
            {
                m_bPPastPExists = value;
                OnPropertyChanged("PPastPExists");
            }
        }

        public string VAdvPres
        {
            get { return GetForm("VAdvPres", EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT); }
            set { SetForm("VAdvPres", value); }
        }

        private EMark m_eVAdvPres_Marks = EMark.None;
        public EMark VAdvPres_Marks
        {
            get { return GetFormStatus("VAdvPres"); }
            set { m_eVAdvPres_Marks = value; OnPropertyChanged("VAdvPres_Marks"); }
        }

        public string VAdvPast
        {
            get { return GetForm("VAdvPast", EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST); }
            set { SetForm("VAdvPast", value); }
        }

        private EMark m_eVAdvPast_Marks = EMark.None;
        public EMark VAdvPast_Marks
        {
            get { return GetFormStatus("VAdvPast"); }
            set { m_eVAdvPast_Marks = value; OnPropertyChanged("VAdvPast_Marks"); }
        }

        #endregion

        private bool m_bEditEnabled = false;
        public bool EditEnabled
        {
            get
            {
                return m_bEditEnabled;
            }
            set
            {
                m_bEditEnabled = value;
                OnPropertyChanged("EditEnabled");
            }
        }

        private void InitFormDictionary(CLexemeManaged lexeme)
        {
            string sLexemeHash = m_Lexeme.sHash();

            List<string> listGramHashes = Helpers.m_listPropNamesVerbMainScreen;
            foreach (string sHash in listGramHashes)
            {
                FormsForGramHash formsPerHash = new FormsForGramHash();
                List<FormDescriptor> lstForms = null;
                if (!m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out lstForms))
                {
                    continue;
                }

                foreach (var wf in lstForms)
                {
                    formsPerHash.lstForms.Add(wf);
                }

                formsPerHash.iCurrentForm = 0;
/*
                var keyIdx = listGramHashes.IndexOf(sHash);
                if (keyIdx < 0)
                {
                    MessageBox.Show(String.Format("Unable to find gram hash key; illegal hash value: ", sHash));
                    continue;
                }

                string sParadigmHash = null;    // use standard adj gram hashes regardless of part of speech
                try
                {
                    sParadigmHash = Helpers.m_listPropNamesAdj[keyIdx];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Unable to find paradigm hash string; illegal hash value: {0}", sHash));
                    continue;
                }
*/
                m_DictFormStatus[sHash] = formsPerHash;
            }

            try
            {
//                var PPresA_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresA_M_Sg_N"].listForms);
                PPresAExists = PPresA_M_Sg_N.Length > 0;

//                var PPastA_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastA_M_Sg_N"].listForms);
                PPastAExists = PPastA_M_Sg_N.Length > 0;

//                var PPresPL_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresPL_M_Sg_N"].listForms);
                PPresPExists = PPresPL_M_Sg_N.Length > 0;

//                var PPastPL_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastPL_M_Sg_N"].listForms);
                PPastPExists = PPastPL_M_Sg_N.Length > 0;
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to retrieve participial form: ";
                msg += ex.Message;
                MessageBox.Show(msg);
            }

            return;

        }   //  InitFormDictionary()

        private void GetGramHashes(ref List<string> listKeys)
        {
            listKeys = Helpers.m_listPropNamesVerbMainScreen;


            if (null == listKeys)
            {
                MessageBox.Show("Internal error: unable to determine gram hashes.");
            }
        }       //  GetGramHashes()


        /*
                private string sFormHashToDisplayHash(string sFormHash)
                {
                    int iKeyIdx = -1;
                    try
                    {
                        if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_ADJ)
                        {
                            if ("мс" == m_Lexeme.sInflectionType())
                            {
                                iKeyIdx = m_listPropNamesPronAdj.IndexOf(sFormHash);
                            }
                            else
                            {
                                iKeyIdx = m_listPropNamesAdj.IndexOf(sFormHash);
                            }
                        }

                        if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ)
                        {
                            iKeyIdx = m_listPropNamesPronAdj.IndexOf(sFormHash);
                        }
                        else if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
                        {
                            IsDerived = true;

                            switch (m_eSubparadigm)
                            {
                                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                                    iKeyIdx = m_listPropNamesPartPresAct.IndexOf(sFormHash);
                                    break;

                                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                                    iKeyIdx = m_listPropNamesPartPastAct.IndexOf(sFormHash);
                                    break;

                                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                                    iKeyIdx = m_listPropNamesPartPresPass.IndexOf(sFormHash);
                                    break;

                                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                                    iKeyIdx = m_listPropNamesPartPastPass.IndexOf(sFormHash);
                                    break;

                                default:
                                    MessageBox.Show("Illegal subparadigm.");
                                    break;
                            }
                        }

                        return m_listPropNamesAdj[iKeyIdx];
                    }
                    catch (Exception ex)
                    {
                        string sMsg = "Unable to find form hash. ";
                        sMsg += ex.Message;
                        MessageBox.Show(sMsg);
                    }

                    return "";
                }
        */

        public VerbViewModel(CLexemeManaged lexeme, MainModel m, ViewModelBase lvm)
        {
            m_MainModel = m;
            m_Lexeme = lexeme;
            m_LexemeViewModel = lvm;

            BackCommand = new RelayCommand(new Action<object>(GoBack));
            ShowPresActFormsCommand = new RelayCommand(new Action<object>(ShowPresActForms));
            ShowPastActFormsCommand = new RelayCommand(new Action<object>(ShowPastActForms));
            ShowPresPassFormsCommand = new RelayCommand(new Action<object>(ShowPresPassForms));
            ShowPastPassFormsCommand = new RelayCommand(new Action<object>(ShowPastPassForms));
            FormScrollUpCommand = new RelayCommand(new Action<object>(FormScrollUp));
            FormScrollDownCommand = new RelayCommand(new Action<object>(FormScrollDown));

            EditCommand = new RelayCommand(new Action<object>(EditForm));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));

            InitFormDictionary(lexeme);

            PropertyChanged += verbViewModel_PropertyChanged;

            EditEnabled = true;
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void ShowPresActForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT, m_LexemeViewModel);
        }

        public void ShowPastActForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT, m_LexemeViewModel);
        }

        public void ShowPresPassForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG, m_LexemeViewModel);
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT, m_LexemeViewModel);
        }

        public void ShowPastPassForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG, m_LexemeViewModel);
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT, m_LexemeViewModel);
        }

        public void EditForm(Object obj)
        {
            var sPropName = obj as string;

            try
            {
                var fd = m_DictFormStatus[sPropName];
//                fd.bCanEdit = !fd.bCanEdit;
//                m_DictFormStatus[sPropName] = fd;
//                sPropName += "_IsReadOnly";
//                OnPropertyChanged(sPropName);
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to invoke word form change handler: ";
                msg += ex.Message;
                MessageBox.Show(msg);
            }
        }

        public void verbViewModel_PropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            var sFormHash = arg.PropertyName.ToString();
            if (null == sFormHash || 0 == sFormHash.Length)
            {
                return;
            }
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return;
            }

            try
            {
//                ChangedFormHandler handler = null;
//                FormDescriptor fd = m_DictFormStatus[sFormHash];
//                handler = fd.handler;
//                var ret = handler();
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to invoke word form change handler: ";
                msg += ex.Message;
                MessageBox.Show(msg);
            }
        }

    }       //  VerbViewModel 

}       //  namespace ZalTestApp
