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
            if (iAt < 0 || iAt >= formsForHash.listForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return "Error";
            }

            return formsForHash.listForms[iAt].sWordForm();

            //  TODO: comment

        }

        protected string GetForm(string sDisplayHash, EM_Subparadigm eSubparadigm)
        {
            //            string sFormHash = sDisplayHashToFormHash(sDisplayHash, eSubparadigm);
            string sFormHash = sDisplayHash;
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var formsForHash = m_DictFormStatus[sFormHash];
            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.listForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return "Error";
            }

            return formsForHash.listForms[iAt].sWordForm();

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
                List<CWordFormManaged> lstForms = null;
                if (!m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out lstForms))
                {
                    continue;
                }

                foreach (var wf in lstForms)
                {
                    formsPerHash.listForms.Add(wf);
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

/*
        private EM_ReturnCode CreateIrregularWordForm(string sForm, string sGramHash, ref CWordFormManaged wf)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            EM_PartOfSpeech eIgnore = EM_PartOfSpeech.POS_UNDEFINED;
            EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            EM_Number eNumber = EM_Number.NUM_UNDEFINED;
            EM_Gender eGender = EM_Gender.GENDER_UNDEFINED;
            EM_Case eCase = EM_Case.CASE_UNDEFINED;
            EM_Person ePerson = EM_Person.PERSON_UNDEFINED;
            EM_Animacy eAnimacy = EM_Animacy.ANIM_UNDEFINED;
            eRet = Helpers.eParseGramHash(sGramHash, ref eIgnore, ref eSp, ref eNumber, ref eGender, ref eCase, 
                                          ref ePerson, ref eAnimacy);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                var msg = "Internal error: unable to parse gram hash.";
                MessageBox.Show(msg);
                return eRet;
            }

            eRet = m_Lexeme.eCreateWordForm(ref wf);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to create a word form.");
                return eRet;
            }

            wf.SetPos(EM_PartOfSpeech.POS_VERB);
            wf.SetSubparadigm(eSp);
            wf.SetPos(EM_PartOfSpeech.POS_VERB);
            wf.SetNumber(eNumber);
            wf.SetGender(eGender);
            wf.SetCase(eCase);
            wf.SetAnimacy(eAnimacy);
            wf.SetPerson(ePerson);

            string sOutForm = "";
            Dictionary<int, EM_StressType> dictStressPos;
            Helpers.StressMarksToPosList(sForm, out sOutForm, out dictStressPos);
            wf.SetWordForm(sOutForm);
            eRet = wf.eSetIrregularStressPositions(dictStressPos);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                var msg = "Internal error: unable to save stress positions";
                MessageBox.Show(msg);
                return eRet;
            }

            return EM_ReturnCode.H_NO_ERROR;

        }       //  CreateIrregularWordForm()

        public void SaveForms(Object obj)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            foreach (KeyValuePair<string, List<string>> entry in m_DictOriginalForms)
            {
                List<string> originalForms = entry.Value;
                FormDescriptor formDescriptor;
                if (!m_DictFormStatus.TryGetValue(entry.Key, out formDescriptor))
                {
                    continue;
                }

                List<Tuple<string, string>> originalComments = null;
                m_DictOriginalComments.TryGetValue(entry.Key, out originalComments);

                List<string> changedForms = formDescriptor.listForms;
                List<Tuple<string, string>> changedComments = formDescriptor.listComments;
                if (changedForms == originalForms && changedComments == originalComments)
                {
                    continue;
                }

                List<Tuple<string, string>> listComments = null;
                if (formDescriptor.listComments != null)
                {
                    if (formDescriptor.listComments.Count != changedForms.Count)
                    {
                        MessageBox.Show("Internal error: mismatch between form and comment lists.");
                        continue;
                    }

                    listComments = formDescriptor.listComments;
                }

                // Purge all irregular forms with this gram hash from the DB
                eRet = m_Lexeme.eDeleteIrregularForm(entry.Key);
                if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_FALSE)
                {
                    var msg = "Internal error: unable to save wordform object";
                    MessageBox.Show(msg);
                    continue;
                }

                CWordFormManaged wf = null;
                for (int iAt = 0; iAt < changedForms.Count; ++iAt)
                {
                    try
                    {
                        string sForm = changedForms[iAt];
                        eRet = CreateIrregularWordForm(sForm, entry.Key, ref wf);
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            var msg = "Internal error: unable to create word form object.";
                            MessageBox.Show(msg);
                            return;
                        }

                        if (iAt > 0)
                        {
                            wf.SetIsVariant(true);
                        }
                        else
                        {
                            wf.SetIsVariant(false);
                        }

                        if (formDescriptor.listComments != null)
                        {
                            if (formDescriptor.listComments.Count != changedForms.Count)
                            {
                                MessageBox.Show("Internal error: mismatch between form and comment lists.");
                                continue;
                            }

                            string sLeftComment = listComments[iAt].Item1;
                            string sRightComment = listComments[iAt].Item2;
                            if (sLeftComment.Length > 0)
                            {
                                wf.SetLeadComment(sLeftComment);
                            }

                            if (sRightComment.Length > 0)
                            {
                                wf.SetTrailingComment(sRightComment);
                            }
                        }

                        eRet = m_Lexeme.eSaveIrregularForm(wf.sGramHash(), ref wf);
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            var msg = "Internal error: unable to save word form.";
                            MessageBox.Show(msg);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format("Exception: {0}.", ex.Message);
                        MessageBox.Show(msg);
                        return;
                    }

                }
            }       // foreach ...

            foreach (string sKey in m_DictFormStatus.Keys)
            {
                List<string> originalForms;
                if (m_DictOriginalForms.TryGetValue(sKey, out originalForms))
                {
                    FormDescriptor formDescriptor;
                    if (m_DictFormStatus.TryGetValue(sKey, out formDescriptor))
                    {
                        List<string> changedForms = formDescriptor.listForms;
                        if (changedForms != originalForms)
                        {
                            m_DictOriginalForms[sKey] = changedForms;
                        }
                    }
                }
            }           // foreach

            MessageBox.Show("Формы сохранены.");

        }       //  SaveForms()
*/

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
