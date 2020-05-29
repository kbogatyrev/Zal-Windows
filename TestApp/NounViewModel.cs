using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
        private EM_Subparadigm m_eSubparadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;

        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void EditButtonHandler();

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

        private ICommand m_EditFormCommentCommand;
        public ICommand EditFormCommentCommand
        {
            get
            {
                return m_EditFormCommentCommand;
            }
            set
            {
                m_EditFormCommentCommand = value;
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

        #region AccessorsAndMutators

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

        private string GetForm(string sHash)
        {
            //            string sFormHash = sDisplayHashToFormHash(sHash);
            string sFormHash = sHash;
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

            //  TODO: comment

            return formsForHash.lstForms[iAt].StressedWordform;
        }

        EMark GetFormStatus(string sDisplayHash)
        {
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, m_eSubparadigm);
            FormsForGramHash formsForHash = null;
            if (!m_DictFormStatus.TryGetValue(sFormHash, out formsForHash))
            {
                return EMark.None;
            }

            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return EMark.None;
            }

            if (formsForHash.lstForms[iAt].WordFormManaged.bIsEdited())
            {
                return EMark.IsEdited;
            }
            else if (formsForHash.lstForms[iAt].WordFormManaged.bIrregular())
            {
                return EMark.IsIrregular;
            }

            return EMark.None;
        }

        #endregion

        #region Bindings_Nouns

        public string Noun_Sg_N
        {
            get { return GetForm("Noun_Sg_N"); }
            set { SetForm("Noun_Sg_N", value); }
        }

        public string Noun_Sg_A
        {
            get { return GetForm("Noun_Sg_A"); }
            set { SetForm("Noun_Sg_A", value); }
        }

        public string Noun_Sg_G
        {
            get { return GetForm("Noun_Sg_G"); }
            set { SetForm("Noun_Sg_G", value); }
        }

        public string Noun_Sg_P
        {
            get { return GetForm("Noun_Sg_P"); }
            set { SetForm("Noun_Sg_P", value); }
        }

        public string Noun_Sg_D
        {
            get { return GetForm("Noun_Sg_D"); }
            set { SetForm("Noun_Sg_D", value); }
        }

        public string Noun_Sg_I
        {
            get { return GetForm("Noun_Sg_I"); }
            set { SetForm("Noun_Sg_I", value); }
        }

        public string Noun_Sg_Part
        {
            get { return GetForm("Noun_Sg_Part"); }
            set { SetForm("Noun_Sg_Part", value); }
        }

        public string Noun_Sg_P2
        {
            get { return GetForm("Noun_Sg_P2"); }
            set { SetForm("Noun_Sg_P2", value); }
        }

        private bool m_bIs_P2_optional = false;
        public bool Is_P2_optional
        {
            get { return m_bIs_P2_optional; }
            set
            {
                m_bIs_P2_optional = value;
                OnPropertyChanged("Is_P2_optional");
            }
        }

        private string m_sNoun_Sg_P2_Prepositions;
        public string Noun_Sg_P2_Prepositions
        {
            get
            {
                return m_sNoun_Sg_P2_Prepositions;
            }
            set
            {
                m_sNoun_Sg_P2_Prepositions = value;
                OnPropertyChanged("Noun_Sg_P2_Prepositions");
            }
        }

        public string Noun_Pl_N
        {
            get { return GetForm("Noun_Pl_N"); }
            set { SetForm("Noun_Pl_N", value); }
        }

        public string Noun_Pl_A
        {
            get { return GetForm("Noun_Pl_A"); }
            set { SetForm("Noun_Pl_A", value); }
        }

        public string Noun_Pl_G
        {
            get { return GetForm("Noun_Pl_G"); }
            set { SetForm("Noun_Pl_G", value); }
        }

        public string Noun_Pl_P
        {
            get { return GetForm("Noun_Pl_P"); }
            set { SetForm("Noun_Pl_P", value); }
        }

        public string Noun_Pl_D
        {
            get { return GetForm("Noun_Pl_D"); }
            set { SetForm("Noun_Pl_D", value); }
        }

        public string Noun_Pl_I
        {
            get { return GetForm("Noun_Pl_I"); }
            set { SetForm("Noun_Pl_I", value); }
        }

        public string Noun_Pl_L
        {
            get { return GetForm("Noun_Pl_L"); }
            set { SetForm("Noun_Pl_L", value); }
        }

        private EMark m_eNoun_Sg_N_Marks = EMark.None;
        public EMark Noun_Sg_N_Marks
        {
            get { return GetFormStatus("Noun_Sg_N"); }
            set { m_eNoun_Sg_N_Marks = value; OnPropertyChanged("Noun_Sg_N_Marks"); }
        }

        private EMark m_eNoun_Sg_A_Marks = EMark.None;
        public EMark Noun_Sg_A_Marks
        {
            get { return GetFormStatus("Noun_Sg_A"); }
            set { m_eNoun_Sg_A_Marks = value; OnPropertyChanged("Noun_Sg_A_Marks"); }
        }

        private EMark m_eNoun_Sg_G_Marks = EMark.None;
        public EMark Noun_Sg_G_Marks
        {
            get { return GetFormStatus("Noun_Sg_G"); }
            set { m_eNoun_Sg_G_Marks = value; OnPropertyChanged("Noun_Sg_G_Marks"); }
        }

        private EMark m_eNoun_Sg_P_Marks = EMark.None;
        public EMark Noun_Sg_P_Marks
        {
            get { return GetFormStatus("Noun_Sg_P"); }
            set { m_eNoun_Sg_P_Marks = value; OnPropertyChanged("Noun_Sg_P_Marks"); }
        }

        private EMark m_eNoun_Sg_D_Marks = EMark.None;
        public EMark Noun_Sg_D_Marks
        {
            get { return GetFormStatus("Noun_Sg_D"); }
            set { m_eNoun_Sg_D_Marks = value; OnPropertyChanged("Noun_Sg_D_Marks"); }
        }

        private EMark m_eNoun_Sg_P2_Marks = EMark.None;
        public EMark Noun_Sg_P2_Marks
        {
            get { return GetFormStatus("Noun_Sg_P2"); }
            set { m_eNoun_Sg_P2_Marks = value; OnPropertyChanged("Noun_Sg_P2_Marks"); }
        }

        private EMark m_eNoun_Sg_I_Marks = EMark.None;
        public EMark Noun_Sg_I_Marks
        {
            get { return GetFormStatus("Noun_Sg_I"); }
            set { m_eNoun_Sg_I_Marks = value; OnPropertyChanged("Noun_Sg_I_Marks"); }
        }

        private EMark m_eNoun_Sg_Part_Marks = EMark.None;
        public EMark Noun_Sg_Part_Marks
        {
            get { return GetFormStatus("Noun_Sg_Part"); }
            set { m_eNoun_Sg_Part_Marks = value; OnPropertyChanged("Noun_Sg_Part_Marks"); }
        }

        private EMark m_eNoun_Pl_N_Marks = EMark.None;
        public EMark Noun_Pl_N_Marks
        {
            get { return GetFormStatus("Noun_Pl_N"); }
            set { m_eNoun_Pl_N_Marks = value; OnPropertyChanged("Noun_Pl_N_Marks"); }
        }

        private EMark m_eNoun_Pl_A_Marks = EMark.None;
        public EMark Noun_Pl_A_Marks
        {
            get { return GetFormStatus("Noun_Pl_A"); }
            set { m_eNoun_Pl_A_Marks = value; OnPropertyChanged("Noun_Pl_A_Marks"); }
        }

        private EMark m_eNoun_Pl_G_Marks = EMark.None;
        public EMark Noun_Pl_G_Marks
        {
            get { return GetFormStatus("Noun_Pl_G"); }
            set { m_eNoun_Pl_G_Marks = value; OnPropertyChanged("Noun_Pl_G_Marks"); }
        }

        private EMark m_eNoun_Pl_P_Marks = EMark.None;
        public EMark Noun_Pl_P_Marks
        {
            get { return GetFormStatus("Noun_Pl_P"); }
            set { m_eNoun_Pl_P_Marks = value; OnPropertyChanged("Noun_Pl_P_Marks"); }
        }

        private EMark m_eNoun_Pl_D_Marks = EMark.None;
        public EMark Noun_Pl_D_Marks
        {
            get { return GetFormStatus("Noun_Pl_D"); }
            set { m_eNoun_Pl_D_Marks = value; OnPropertyChanged("Noun_Pl_D_Marks"); }
        }

        private EMark m_eNoun_Pl_L_Marks = EMark.None;
        public EMark Noun_Pl_L_Marks
        {
            get { return GetFormStatus("Noun_Pl_L"); }
            set { m_eNoun_Pl_L_Marks = value; OnPropertyChanged("Noun_Pl_L_Marks"); }
        }

        private EMark m_eNoun_Pl_I_Marks = EMark.None;
        public EMark Noun_Pl_I_Marks
        {
            get { return GetFormStatus("Noun_Pl_I"); }
            set { m_eNoun_Pl_I_Marks = value; OnPropertyChanged("Noun_Pl_I_Marks"); }
        }

        private string m_sFormComment;
        public string FormComment
        {
            get { return m_sFormComment; }
            set { m_sFormComment = value; OnPropertyChanged("FormComment"); }
        }

        public bool Noun_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_Part_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_Part", out f); return f != null && f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Sg_P2_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Sg_P2", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Noun_Pl_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Noun_Pl_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        #endregion

        #region Property_Delegates

        private bool InitFormHandlers()
        {
            try
            {
                string sLexemeHash = m_Lexeme.sHash();
                List<string> listKeys = null;

                switch (m_Lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        listKeys = Helpers.m_listPropNamesNoun;
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        listKeys = Helpers.m_listPropNamesPronoun;
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        listKeys = Helpers.m_listPropNamesNumeral;
                        break;

                    default:
                        MessageBox.Show("Illegal part of speech value.");
                        return false;
                }

                if (null == listKeys)
                {
                    MessageBox.Show("Internal error: unable to determine gram hashes.");
                }

                foreach (var sHash in listKeys)
                {
                    FormsForGramHash formsPerHash = new FormsForGramHash();
                    List<FormDescriptor> lstForms = null;
                    if (!m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out lstForms))
                    {
                        continue;
                    }

                    foreach (var wf in lstForms)
                    {
                        switch (m_Lexeme.ePartOfSpeech())
                        {
                            case EM_PartOfSpeech.POS_NOUN:
                                m_eSubparadigm = EM_Subparadigm.SUBPARADIGM_NOUN;
                                break;

                            case EM_PartOfSpeech.POS_PRONOUN:
                                m_eSubparadigm = EM_Subparadigm.SUBPARADIGM_PRONOUN;
                                break;

                            case EM_PartOfSpeech.POS_NUM:
                                m_eSubparadigm = EM_Subparadigm.SUBPARADIGM_NUM;
                                break;

                            default:
                                MessageBox.Show("Illegal part of speech value.");
                                return false;
                        }
                        
                        formsPerHash.lstForms.Add(wf);
                    }

                    if (formsPerHash.lstForms.Count > 0)
                    {
                        formsPerHash.iCurrentForm = 0;
                    }

                    m_DictFormStatus[sHash] = formsPerHash;
                }
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to initialize form handlers: ";
                msg += ex.Message;
                MessageBox.Show(msg);
                return false;
            }

            return true;

        }   //  InitFormHandlers()

        #endregion

        public NounViewModel(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            EditFormCommentCommand = new RelayCommand(new Action<object>(EditFormComment));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));
            FormScrollUpCommand = new RelayCommand(new Action<object>(FormScrollUp));
            FormScrollDownCommand = new RelayCommand(new Action<object>(FormScrollDown));

            m_MainModel = m;

            m_Lexeme = lexeme;

            InitFormHandlers();
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

/*
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
        }       //  EditForm()
*/
        public void EditFormComment(Object obj)
        {
            // Currently disabled
            // TODO
            return;

            //            string sComment = obj as string;
            string sComment = FormComment;

            EnterDataDlg edd = new EnterDataDlg();
            edd.Owner = Application.Current.MainWindow;

            EnterDataViewModel eddvm = (EnterDataViewModel)edd.DataContext;
            eddvm.Type = EnterDataViewModel.EType.CommentEntry;
            eddvm.DlgTitle = "Коммент";
            eddvm.DataString = sComment;
            bool? bnRet = edd.ShowDialog();
            if (bnRet != true)
            {
                return;
            }

            FormComment = eddvm.DataString;
        }

        /*
                private EM_ReturnCode CreateIrregularWordForm(string sGramHash, 
                                                              string sForm, 
                                                              ref CWordFormManaged wf)
                {
                    EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

                    eRet = m_Lexeme.eCreateWordForm(ref wf);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        MessageBox.Show("Unable to create a word form.");
                        return eRet;
                    }

                    wf.SetPos(m_Lexeme.ePartOfSpeech());

                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        wf.SetNumber(eNumber);
                    }

                    EM_Case eCase = EM_Case.CASE_UNDEFINED;
                    EM_Animacy eAnimacy = EM_Animacy.ANIM_UNDEFINED;
                    eRet = Helpers.eGramHashToCase(sGramHash, ref eCase, ref eAnimacy);
                    if (eRet == EM_ReturnCode.H_NO_ERROR)
                    {
                        wf.SetCase(eCase);
                        wf.SetAnimacy(eAnimacy);
                    }

                    EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;

                    switch (m_Lexeme.ePartOfSpeech())
                    {
                        case EM_PartOfSpeech.POS_NOUN:
                            eSp = EM_Subparadigm.SUBPARADIGM_NOUN;
                            break;

                        case EM_PartOfSpeech.POS_PRONOUN:
                            eSp = EM_Subparadigm.SUBPARADIGM_PRONOUN;
                            break;

                        case EM_PartOfSpeech.POS_NUM:
                            eSp = EM_Subparadigm.SUBPARADIGM_NUM;
                            break;

                        default:
                            MessageBox.Show("Illegal part of speech value.");
                            return EM_ReturnCode.H_ERROR_UNEXPECTED;
                    }

                    wf.SetSubparadigm(eSp);

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
        */
        /*

                public void SaveForms(Object obj)
                {
                    EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

                    foreach (KeyValuePair<string, FormsForGramHash> entry in m_DictFormStatus)
                    {
                        FormsForGramHash formsPerHash = entry.Value;
                        if (formsPerHash.listForms.Count < 1)
                        {
                            MessageBox.Show("Internal error: no forms for {0}.", entry.Key);
                            continue;
                        }

                        // Purge all irregular forms with this gram hash from the DB
                        string sGramHash = sDisplayHashToFormHash(entry.Key);
                        eRet = m_Lexeme.eDeleteIrregularForm(sGramHash);
                        if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_FALSE)
                        {
                            var msg = "Internal error: unable to save wordform object";
                            MessageBox.Show(msg);
                            continue;
                        }

                        CWordFormManaged wf = null;
                        bool isVariant = false;
                        foreach (var descriptor in formsPerHash.listForms)
                        {
                            try
                            {
                                string sForm = descriptor.sFormText;
                                eRet = CreateIrregularWordForm(sGramHash, sForm, ref wf);
                                if (eRet != EM_ReturnCode.H_NO_ERROR)
                                {
                                    var msg = "Internal error: unable to create word form object.";
                                    MessageBox.Show(msg);
                                    continue;
                                }

                                wf.SetIsVariant(isVariant);

                                isVariant = true;       // for subsequent forms if they exist

        // TODO: comments

                                eRet = m_Lexeme.eSaveIrregularForm(wf.sGramHash(), ref wf);
                                if (eRet != EM_ReturnCode.H_NO_ERROR)
                                {
                                    var msg = "Internal error: unable to save word form.";
                                    MessageBox.Show(msg);
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                var msg = string.Format("Exception: {0}.", ex.Message);
                                MessageBox.Show(msg);
                                return;
                            }
                        }
                    }       // for (int iAt = 0...

                    MessageBox.Show("Формы сохранены.");

                }       //  SaveForms()
        */
        private string sFormHashToDisplayHash(string sFormHash)
        {
            int iKeyIdx = -1;

            try
            {
                switch (m_Lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        iKeyIdx = Helpers.m_listPropNamesNoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        iKeyIdx = Helpers.m_listPropNamesPronoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        iKeyIdx = Helpers.m_listPropNamesNumeral.IndexOf(sFormHash);
                        break;

                    default:
                        MessageBox.Show(String.Format("Part of speech: {0} was not recognized.", m_Lexeme.ePartOfSpeech()));
                        return "";
                }

                return Helpers.m_listPropNamesNoun[iKeyIdx];
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Exception while retrieving form hash: {0}.", sFormHash));
                return "";
            }
        }       //  sFormHashToDisplayHash()

/*
        private string sDisplayHashToFormHash(string sDisplayHash)
        {
            int iKeyIdx = m_listPropNamesNoun.IndexOf(sDisplayHash);
            if (iKeyIdx < 0)
            {
                string sMsg = $"Display hash {iKeyIdx} not recognized.";
                MessageBox.Show(sMsg);
                return "";
            }

            string sFormHash = "";
            try
            {
                switch (m_Lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        sFormHash = m_listPropNamesNoun[iKeyIdx];
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        sFormHash = m_listPropNamesPronoun[iKeyIdx];
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        sFormHash = m_listPropNamesNumeral[iKeyIdx];
                        break;

                    default:
                        string sMsg = "Part of speech :";
                        sMsg += m_Lexeme.ePartOfSpeech();
                        sMsg += " was not recognized. ";
                        MessageBox.Show(sMsg);
                        return "";
                }

                return sFormHash;
            }
            catch (Exception ex)
            {
                string sMsg = "Exception while retrieving form hash: ";
                sMsg += ex.Message;
                MessageBox.Show(sMsg);
                return "";
            }

            return "";

        }       //  sDisplayHashToFormHash()
        
        public void FormScrollUp(Object obj)
        {
            string sGramHash = obj as string;
            FormsForGramHash formsPerHash;
            if (m_DictFormStatus.TryGetValue(sGramHash, out formsPerHash))
            {
                if (formsPerHash.listForms.Count > 1 && formsPerHash.iCurrentForm > 0)
                {
                    --formsPerHash.iCurrentForm;
                    OnPropertyChanged(sGramHash);
                }
            }
        }

        public void FormScrollDown(Object obj)
        {
            string sGramHash = obj as string;
            FormsForGramHash formsPerHash;
            if (m_DictFormStatus.TryGetValue(sGramHash, out formsPerHash))
            {
                if (formsPerHash.listForms.Count > 1 && formsPerHash.iCurrentForm < formsPerHash.listForms.Count - 1)
                {
                    ++formsPerHash.iCurrentForm;
                    OnPropertyChanged(sGramHash);
                }
            }
        }

        public void nounViewModel_PropertyChanged(object sender, PropertyChangedEventArgs arg)
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
        }
*/
    }       //  public class NounViewModel ...
}
