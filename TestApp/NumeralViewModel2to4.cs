using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class NumeralViewModel2to4 : ViewModelBase
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
            string sFormHash = sHash;
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var formsForHash = m_DictFormStatus[sFormHash];
            if (formsForHash.lstForms.Count < 1)
            {
                return "";
            }

            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                iAt = 0;
            }

            //  TODO: comment

            return formsForHash.lstForms[iAt].StressedWordform;
        }

        EMark GetFormStatus(string sDisplayHash)
        {
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, m_Lexeme.ePartOfSpeech());
            FormsForGramHash formsForHash = null;
            if (!m_DictFormStatus.TryGetValue(sFormHash, out formsForHash))
            {
                return EMark.None;
            }

            if (formsForHash.lstForms.Count < 1)
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

        #region Bindings

        public string Numeral_M_N
        {
            get { return GetForm("Numeral_M_N"); }
            set { SetForm("Numeral_M_N", value); }
        }

        public string Numeral_M_A_Inanim
        {
            get { return GetForm("Numeral_M_A_Inanim"); }
            set { SetForm("Numeral_M_A_Inanim", value); }
        }

        public string Numeral_M_A_Anim
        {
            get { return GetForm("Numeral_M_A_Anim"); }
            set { SetForm("Numeral_M_A_Anim", value); }
        }

        public string Numeral_M_G
        {
            get { return GetForm("Numeral_M_G"); }
            set { SetForm("Numeral_M_G", value); }
        }

        public string Numeral_M_P
        {
            get { return GetForm("Numeral_M_P"); }
            set { SetForm("Numeral_M_P", value); }
        }

        public string Numeral_M_D
        {
            get { return GetForm("Numeral_M_D"); }
            set { SetForm("Numeral_M_D", value); }
        }

        public string Numeral_M_I
        {
            get { return GetForm("Numeral_M_I"); }
            set { SetForm("Numeral_M_I", value); }
        }

        public string Numeral_F_N
        {
            get { return GetForm("Numeral_F_N"); }
            set { SetForm("Numeral_F_N", value); }
        }

        public string Numeral_F_A
        {
            get { return GetForm("Numeral_F_A"); }
            set { SetForm("Numeral_F_A", value); }
        }

        public string Numeral_F_G
        {
            get { return GetForm("Numeral_F_G"); }
            set { SetForm("Numeral_F_G", value); }
        }

        public string Numeral_F_P
        {
            get { return GetForm("Numeral_F_P"); }
            set { SetForm("Numeral_F_P", value); }
        }

        public string Numeral_F_D
        {
            get { return GetForm("Numeral_F_D"); }
            set { SetForm("Numeral_F_D", value); }
        }

        public string Numeral_F_I
        {
            get { return GetForm("Numeral_F_I"); }
            set { SetForm("Numeral_F_I", value); }
        }

        public string Numeral_F_L
        {
            get { return GetForm("Numeral_F_L"); }
            set { SetForm("Numeral_F_L", value); }
        }

        public string Numeral_Pl_N
        {
            get { return GetForm("Numeral_Pl_N"); }
            set { SetForm("Numeral_Pl_N", value); }
        }

        public string Numeral_Pl_A
        {
            get { return GetForm("Numeral_Pl_A"); }
            set { SetForm("Numeral_Pl_A", value); }
        }

        public string Numeral_Pl_G
        {
            get { return GetForm("Numeral_Pl_G"); }
            set { SetForm("Numeral_Pl_G", value); }
        }

        public string Numeral_Pl_P
        {
            get { return GetForm("Numeral_Pl_P"); }
            set { SetForm("Numeral_Pl_P", value); }
        }

        public string Numeral_Pl_D
        {
            get { return GetForm("Numeral_Pl_D"); }
            set { SetForm("Numeral_Pl_D", value); }
        }

        public string Numeral_Pl_I
        {
            get { return GetForm("Numeral_Pl_I"); }
            set { SetForm("Numeral_Pl_I", value); }
        }

        public string Numeral_Pl_L
        {
            get { return GetForm("Numeral_Pl_L"); }
            set { SetForm("Numeral_Pl_L", value); }
        }

        public bool Numeral_M_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_A_Inanim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_A_Anim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_M_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_M_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_F_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_F_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_A_Inanim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_A_Anim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool Numeral_Pl_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("Numeral_Pl_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public ECellStatus Numeral_M_N_CellStatus
        {
            get { return GetCellStatus("Numeral_M_N"); }
        }

        public ECellStatus Numeral_M_A_Inanim_CellStatus
        {
            get { return GetCellStatus("Numeral_M_A_Inanim"); }
        }

        public ECellStatus Numeral_M_A_Anim_CellStatus
        {
            get { return GetCellStatus("Numeral_M_A_Anim"); }
        }

        public ECellStatus Numeral_M_G_CellStatus
        {
            get { return GetCellStatus("Numeral_M_G"); }
        }

        public ECellStatus Numeral_M_P_CellStatus
        {
            get { return GetCellStatus("Numeral_M_P"); }
        }

        public ECellStatus Numeral_M_D_CellStatus
        {
            get { return GetCellStatus("Numeral_M_D"); }
        }

        public ECellStatus Numeral_M_I_CellStatus
        {
            get { return GetCellStatus("Numeral_M_I"); }
        }

        public ECellStatus Numeral_F_N_CellStatus
        {
            get { return GetCellStatus("Numeral_F_N"); }
        }

        public ECellStatus Numeral_F_A_Inanim_CellStatus
        {
            get { return GetCellStatus("Numeral_F_A_Inanim"); }
        }

        public ECellStatus Numeral_F_A_Anim_CellStatus
        {
            get { return GetCellStatus("Numeral_F_A_Anim"); }
        }

        public ECellStatus Numeral_F_G_CellStatus
        {
            get { return GetCellStatus("Numeral_F_G"); }
        }

        public ECellStatus Numeral_F_P_CellStatus
        {
            get { return GetCellStatus("Numeral_F_P"); }
        }

        public ECellStatus Numeral_F_D_CellStatus
        {
            get { return GetCellStatus("Numeral_F_D"); }
        }

        public ECellStatus Numeral_F_I_CellStatus
        {
            get { return GetCellStatus("Num24_F_I"); }
        }

        public ECellStatus Numeral_Pl_N_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_N"); }
        }

        public ECellStatus Numeral_Pl_A_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_A"); }
        }

        public ECellStatus Numeral_Pl_G_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_G"); }
        }

        public ECellStatus Numeral_Pl_P_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_P"); }
        }

        public ECellStatus Numeral_Pl_D_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_D"); }
        }

        public ECellStatus Numeral_Pl_I_CellStatus
        {
            get { return GetCellStatus("Numeral_Pl_I"); }
        }

        #endregion

        #region Property_Delegates

        private bool InitFormHandlers()
        {
            try
            {
                string sLexemeHash = m_Lexeme.sParadigmHash();
                List<string> listKeys = Helpers.m_listPropNamesNumeral;
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
                        formsPerHash.lstForms.Add(wf);
                    }

                    if (formsPerHash.lstForms.Count > 0)
                    {
                        formsPerHash.iCurrentForm = 0;
                    }

                    var sDisplayHash = Helpers.sFormHashToDisplayHash(sHash);
                    m_DictFormStatus[sDisplayHash] = formsPerHash;
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

        public NumeralViewModel2to4(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
//            ShowFormComment = new RelayCommand(new Action<object>(ShowFormCommentImpl));
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
        public void ShowFormCommentImpl(Object obj)
        {
            Comments wndComments = new Comments();
            wndComments.Owner = Application.Current.MainWindow;
            var sDisplayHash = obj as string;
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, m_Lexeme.ePartOfSpeech());
            var formsForHash = m_DictFormStatus[sFormHash];
            if (formsForHash.iCurrentForm < 0 || formsForHash.iCurrentForm >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return;
            }

            var wf = formsForHash.lstForms[formsForHash.iCurrentForm].WordFormManaged;
            var sLeftComment = wf.sLeadComment();
            var sRightComment = wf.sTrailingComment();

            if (sLeftComment != String.Empty)
            {
                wndComments.TextBlock.Text = sLeftComment;
            }

            if (sRightComment != String.Empty)
            {
                wndComments.TextBlock.Text += "; " + sRightComment;
            }
            else
            {
                wndComments.TextBlock.Text = sLeftComment;
            }

            wndComments.ShowDialog();

            return;

        }
*/
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
        */
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
