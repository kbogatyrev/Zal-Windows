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

        MainModel m_MainModel = null;
        CLexemeManaged m_Lexeme = null;
        ViewModelBase m_LexemeViewModel;

//        private delegate bool ChangedFormHandler();
/*
        struct FormDescriptor
        {
            public List<string> listForms { get; set; }
            public bool bCanEdit { get; set; }
            public ChangedFormHandler handler { get; set; }

            public FormDescriptor(List<string> list, bool b, ChangedFormHandler h)
            {
                listForms = list;
                bCanEdit = b;
                handler = h;
            }
        }
*/

        Dictionary<string, List<string>> m_DictOriginalForms = new Dictionary<string, List<string>>();

        Dictionary<string, FormDescriptor> m_DictFormStatus = new Dictionary<string, FormDescriptor>()
        {
            {  "Infinitive", new FormDescriptor(null, false, false, null) },
            {  "Pres_Sg_1", new FormDescriptor(null, false, false, null) },
            {  "Pres_Sg_2", new FormDescriptor(null, false, false, null) },
            {  "Pres_Sg_3", new FormDescriptor(null, false, false, null) },
            {  "Pres_Pl_1", new FormDescriptor(null, false, false, null) },
            {  "Pres_Pl_2", new FormDescriptor(null, false, false, null) },
            {  "Pres_Pl_3", new FormDescriptor(null, false, false, null) },
            {  "Past_M", new FormDescriptor(null, false, false, null) },
            {  "Past_F", new FormDescriptor(null, false, false, null) },
            {  "Past_N", new FormDescriptor(null, false, false, null) },
            {  "Past_Pl", new FormDescriptor(null, false, false, null) },
            {  "Impv_Sg_2", new FormDescriptor(null, false, false, null) },
            {  "Impv_Pl_2", new FormDescriptor(null, false, false, null) },
            {  "PPresA_M_Sg_N", new FormDescriptor(null, false, false, null) },
            {  "VAdvPres", new FormDescriptor(null, false, false, null) },
            {  "PPastA_M_Sg_N", new FormDescriptor(null, false, false, null) },
            {  "VAdvPast", new FormDescriptor(null, false, false, null) },
            {  "PPresPL_M_Sg_N", new FormDescriptor(null, false, false, null) },
            {  "PPastPL_M_Sg_N", new FormDescriptor(null, false, false, null) },
        };

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
        #endregion

        string GetForms(string sFormHash)
        {
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var descriptor = m_DictFormStatus[sFormHash];
            var text = Helpers.sListToCommaSeparatedString(descriptor.listForms);
            if (m_MainModel.bIsIrregular(m_Lexeme.sHash(), sFormHash))
            {
                descriptor.IsIrregular = true;
            }
            else
            {
                descriptor.IsIrregular = false;
            }

            return text;
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

        void SetForms(string sHash, string sForms)
        {
            if (!m_DictFormStatus.ContainsKey(sHash))
            {
                return;
            }
            Helpers.AssignDiacritics(sForms, ref sForms);
            var fd = m_DictFormStatus[sHash];
            fd.listForms = Helpers.CommaSeparatedStringToList(sForms);
            m_DictFormStatus[sHash] = fd;
        }

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
            get { return GetForms("Infinitive"); }
            set { SetForms("Infinitive", value); }
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
            get { return GetForms("Pres_Sg_1"); }
            set { SetForms("Pres_Sg_1", value); }
        }

        private EMark m_ePres_Sg_1_Marks = EMark.None;
        public EMark Pres_Sg_1_Marks
        {
            get { return GetFormStatus("Pres_Sg_1"); }
            set { m_ePres_Sg_1_Marks = value; OnPropertyChanged("Pres_Sg_1_Marks"); }
        }

        public string Pres_Sg_2
        {
            get { return GetForms("Pres_Sg_2"); }
            set { SetForms("Pres_Sg_2", value); }
        }

        private EMark m_ePres_Sg_2_Marks = EMark.None;
        public EMark Pres_Sg_2_Marks
        {
            get { return GetFormStatus("Pres_Sg_2"); }
            set { m_ePres_Sg_2_Marks = value; OnPropertyChanged("Pres_Sg_2_Marks"); }
        }

        public string Pres_Sg_3
        {
            get { return GetForms("Pres_Sg_3"); }
            set { SetForms("Pres_Sg_3", value); }
        }

        private EMark m_ePres_Sg_3_Marks = EMark.None;
        public EMark Pres_Sg_3_Marks
        {
            get { return GetFormStatus("Pres_Sg_3"); }
            set { m_ePres_Sg_3_Marks = value; OnPropertyChanged("Pres_Sg_3_Marks"); }
        }

        public string Pres_Pl_1
        {
            get { return GetForms("Pres_Pl_1"); }
            set { SetForms("Pres_Pl_1", value); }
        }

        private EMark m_ePres_Pl_1_Marks = EMark.None;
        public EMark Pres_Pl_1_Marks
        {
            get { return GetFormStatus("Pres_Pl_1"); }
            set { m_ePres_Pl_1_Marks = value; OnPropertyChanged("Pres_Pl_1_Marks"); }
        }

        public string Pres_Pl_2
        {
            get { return GetForms("Pres_Pl_2"); }
            set { SetForms("Pres_Pl_2", value); }
        }

        private EMark m_ePres_Pl_2_Marks = EMark.None;
        public EMark Pres_Pl_2_Marks
        {
            get { return GetFormStatus("Pres_Pl_2"); }
            set { m_ePres_Pl_2_Marks = value; OnPropertyChanged("Pres_Pl_2_Marks"); }
        }

        public string Pres_Pl_3
        {
            get { return GetForms("Pres_Pl_3"); }
            set { SetForms("Pres_Pl_3", value); }
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
            get { return GetForms("Past_M"); }
            set { SetForms("Past_M", value); }
        }

        private EMark m_ePast_M_Marks = EMark.None;
        public EMark Past_M_Marks
        {
            get { return GetFormStatus("Past_M"); }
            set { m_ePast_M_Marks = value; OnPropertyChanged("Past_M_Marks"); }
        }

        public string Past_F
        {
            get { return GetForms("Past_F"); }
            set { SetForms("Past_F", value); }
        }

        private EMark m_ePast_F_Marks = EMark.None;
        public EMark Past_F_Marks
        {
            get { return GetFormStatus("Past_F"); }
            set { m_ePast_F_Marks = value; OnPropertyChanged("Past_F_Marks"); }
        }

        public string Past_N
        {
            get { return GetForms("Past_N"); }
            set { SetForms("Past_N", value); }
        }

        private EMark m_ePast_N_Marks = EMark.None;
        public EMark Past_N_Marks
        {
            get { return GetFormStatus("Past_N"); }
            set { m_ePast_N_Marks = value; OnPropertyChanged("Past_N_Marks"); }
        }

        public string Past_Pl
        {
            get { return GetForms("Past_Pl"); }
            set { SetForms("Past_Pl", value); }
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
            get { return GetForms("Impv_Sg_2"); }
            set { SetForms("Impv_Sg_2", value); }
        }

        private EMark m_eImpv_Sg_2_Marks = EMark.None;
        public EMark Impv_Sg_2_Marks
        {
            get { return GetFormStatus("Impv_Sg_2"); }
            set { m_eImpv_Sg_2_Marks = value; OnPropertyChanged("Impv_Sg_2_Marks"); }
        }

        public string Impv_Pl_2
        {
            get { return GetForms("Impv_Pl_2"); }
            set { SetForms("Impv_Pl_2", value); }
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
            get { return GetForms("PPresA_M_Sg_N"); }
            set { SetForms("PPresA_M_Sg_N", value); }
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
            get { return GetForms("PPastA_M_Sg_N"); }
            set { SetForms("PPastA_M_Sg_N", value); }
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
            get { return GetForms("PPresPL_M_Sg_N"); }
            set { SetForms("PPresPL_M_Sg_N", value); }
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
            get { return GetForms("PPastPL_M_Sg_N"); }
            set { SetForms("PPastPL_M_Sg_N", value); }
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
            get { return GetForms("VAdvPres"); }
            set { SetForms("VAdvPres", value); }
        }

        private EMark m_eVAdvPres_Marks = EMark.None;
        public EMark VAdvPres_Marks
        {
            get { return GetFormStatus("VAdvPres"); }
            set { m_eVAdvPres_Marks = value; OnPropertyChanged("VAdvPres_Marks"); }
        }

        public string VAdvPast
        {
            get { return GetForms("VAdvPast"); }
            set { SetForms("VAdvPast", value); }
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

        private void InitFormHandlers(CLexemeManaged lexeme)
        {
            string sLexemeHash = lexeme.sHash();
            List<string> hashes = new List<string>(m_DictFormStatus.Keys);

            try
            {
                foreach (var formHash in hashes)
                {
                    FormDescriptor fd = m_DictFormStatus[formHash];
                    List<string> listForms = null;
                    m_MainModel.GetFormsByGramHash(sLexemeHash, formHash, out listForms);
                    fd.listForms = listForms;
                    fd.handler = () =>
                    {
//                        if (!fd.bCanEdit)
//                        {
//                            return true;
//                        }

                        var sFormString = Helpers.sListToCommaSeparatedString(fd.listForms);
                        Helpers.AssignDiacritics(sFormString, ref sFormString);

                        OnPropertyChanged(formHash);
                        return true;
                    };

                    m_DictFormStatus[formHash] = fd;
                    m_DictOriginalForms[formHash] = listForms;
                }
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to initiate form handlers: ";
                msg += ex.Message;
                MessageBox.Show(msg);
                return;
            }

            try
            {
                var PPresA_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresA_M_Sg_N"].listForms);
                PPresAExists = PPresA_M_Sg_N.Length > 0;

                var PPastA_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastA_M_Sg_N"].listForms);
                PPastAExists = PPastA_M_Sg_N.Length > 0;

                var PPresPL_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresPL_M_Sg_N"].listForms);
                PPresPExists = PPresPL_M_Sg_N.Length > 0;

                var PPastPL_M_Sg_N = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastPL_M_Sg_N"].listForms);
                PPastPExists = PPastPL_M_Sg_N.Length > 0;
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to retrieve participial form: ";
                msg += ex.Message;
                MessageBox.Show(msg);
            }

            return;

        }   //  InitFormHandlers()

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

            EditCommand = new RelayCommand(new Action<object>(EditForm));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));

            InitFormHandlers(lexeme);

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

                List<string> changedForms = formDescriptor.listForms;
                if (changedForms == originalForms)
                {
                    continue;
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
                foreach (string sForm in changedForms)
                {
                    eRet = CreateIrregularWordForm(sForm, entry.Key, ref wf);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        var msg = "Internal error: unable to create word form object.";
                        MessageBox.Show(msg);
                        return;
                    }

                    eRet = m_Lexeme.eSaveIrregularForm(wf.sGramHash(), ref wf);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        var msg = "Internal error: unable to save word form.";
                        MessageBox.Show(msg);
                        return;
                    }
                }       // foreach()
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
                ChangedFormHandler handler = null;
                FormDescriptor fd = m_DictFormStatus[sFormHash];
                handler = fd.handler;
                var ret = handler();
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
