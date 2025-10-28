using System;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;

using MainLibManaged;
using System.ComponentModel;

namespace ZalTestApp
{
    public class LastNameViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

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

        private ICommand m_ShowFormCommentCommand;
        public ICommand ShowFormComment
        {
            get
            {
                return m_ShowFormCommentCommand;
            }
            set
            {
                m_ShowFormCommentCommand = value;
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

        //        private CLexemeManaged m_Lexeme;
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

        EM_Subparadigm m_eSubparadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
        public EM_Subparadigm Subparadigm
        {
            get
            {
                return m_eSubparadigm;
            }

            set
            {
                m_eSubparadigm = value;
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

        private bool m_bWasTouched = false;
        public bool WasTouched
        {
            get
            {
                return m_bWasTouched;
            }
            set
            {
                m_bWasTouched = value;
                OnPropertyChanged("WasTouched");
            }
        }

        private string GetForm(string sFormHash, EM_Subparadigm eSubparadigm)
        {
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
                //                MessageBox.Show("Internal error: Illegal form index.");
                //                return "Error";
            }

            return formsForHash.lstForms[iAt].StressedWordform;

            //  TODO: comment

        }

        EMark GetFormStatus(string sDisplayHash)
        {
            var sFormHash = sDisplayHashToFormHash(sDisplayHash, m_Lexeme.ePartOfSpeech(), m_eSubparadigm);
            string sLexemeHash = m_Inflection.sParadigmHash();
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

        #region Bindings_Last_Name_Masculine_Sg

        public string LastName_M_Sg_N
        {
            get { return GetForm("LastName_M_Sg_N", Subparadigm); }
            set { SetForm("LastName_M_Sg_N", value); }
        }

        private EMark m_eLastName_M_Sg_N_Marks = EMark.None;
        public EMark LastName_M_Sg_N_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_N"); }
            set { m_eLastName_M_Sg_N_Marks = value; OnPropertyChanged("LastName_M_Sg_N_Marks"); }
        }

        public bool LastName_M_Sg_N_HasComments
        {
            get { return HasComments("LastName_M_Sg_N", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_N_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_N", m_eSubparadigm); }
        }

        public string LastName_M_Sg_A
        {
            get { return GetForm("LastName_M_Sg_A", Subparadigm); }
            set { SetForm("LastName_M_Sg_A", value); }
        }

        private EMark m_eLastName_M_Sg_A_Marks = EMark.None;
        public EMark LastName_M_Sg_A_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_A"); }
            set { m_eLastName_M_Sg_A_Marks = value; OnPropertyChanged("LastName_M_Sg_A_Marks"); }
        }

        public bool LastName_M_Sg_A_HasComments
        {
            get { return HasComments("LastName_M_Sg_A", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_A_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_G", m_eSubparadigm); }        // sic, no separate hash for Acc 
        }
        public string LastName_M_Sg_G
        {
            get { return GetForm("LastName_M_Sg_G", Subparadigm); }
            set { SetForm("LastName_M_Sg_G", value); }
        }

        private EMark m_eLastName_M_Sg_G_Marks = EMark.None;
        public EMark LastName_M_Sg_G_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_G"); }
            set { m_eLastName_M_Sg_G_Marks = value; OnPropertyChanged("LastName_M_Sg_G_Marks"); }
        }

        public bool LastName_M_Sg_G_HasComments
        {
            get { return HasComments("LastName_M_Sg_G", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_G_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_G", m_eSubparadigm); }
        }

        public string LastName_M_Sg_P
        {
            get { return GetForm("LastName_M_Sg_P", Subparadigm); }
            set { SetForm("LastName_M_Sg_P", value); }
        }

        private EMark m_eLastName_M_Sg_P_Marks = EMark.None;
        public EMark LastName_M_Sg_P_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_P"); }
            set { m_eLastName_M_Sg_G_Marks = value; OnPropertyChanged("LastName_M_Sg_P_Marks"); }
        }

        public bool LastName_M_Sg_P_HasComments
        {
            get { return HasComments("LastName_M_Sg_P", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_P_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_P", m_eSubparadigm); }
        }

        public string LastName_M_Sg_D
        {
            get { return GetForm("LastName_M_Sg_D", Subparadigm); }
            set { SetForm("LastName_M_Sg_D", value); }
        }

        private EMark m_eLastName_M_Sg_D_Marks = EMark.None;
        public EMark LastName_M_Sg_D_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_D"); }
            set { m_eLastName_M_Sg_D_Marks = value; OnPropertyChanged("LastName_M_Sg_D_Marks"); }
        }

        public bool LastName_M_Sg_D_HasComments
        {
            get { return HasComments("LastName_M_Sg_D", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_D_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_D", m_eSubparadigm); }
        }

        public string LastName_M_Sg_I
        {
            get { return GetForm("LastName_M_Sg_I", Subparadigm); }
            set { SetForm("LastName_M_Sg_I", value); }
        }

        private EMark m_eLastName_M_Sg_I_Marks = EMark.None;
        public EMark LastName_M_Sg_I_Marks
        {
            get { return GetFormStatus("LastName_M_Sg_I"); }
            set { m_eLastName_M_Sg_I_Marks = value; OnPropertyChanged("LastName_M_Sg_I_Marks"); }
        }

        public bool LastName_M_Sg_I_HasComments
        {
            get { return HasComments("LastName_M_Sg_I", m_eSubparadigm); }
        }

        public ECellStatus LastName_M_Sg_I_CellStatus
        {
            get { return GetCellStatus("LastName_M_Sg_I", m_eSubparadigm); }
        }

        #endregion

        #region Bindings_Last_Name_Feminine_Sg

        public string LastName_F_Sg_N
        {
            get { return GetForm("LastName_F_Sg_N", Subparadigm); }
            set { SetForm("LastName_F_Sg_N", value); }
        }

        private EMark m_eLastName_F_Sg_N_Marks = EMark.None;
        public EMark LastName_F_Sg_N_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_N"); }
            set { m_eLastName_F_Sg_N_Marks = value; OnPropertyChanged("LastName_F_Sg_N_Marks"); }
        }

        public bool LastName_F_Sg_N_HasComments
        {
            get { return HasComments("LastName_F_Sg_N", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_N_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_N", m_eSubparadigm); }
        }

        public string LastName_F_Sg_A
        {
            get { return GetForm("LastName_F_Sg_A", Subparadigm); }
            set { SetForm("LastName_F_Sg_A", value); }
        }

        private EMark m_eLastName_F_Sg_A_Marks = EMark.None;
        public EMark LastName_F_Sg_A_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_A"); }
            set { m_eLastName_F_Sg_A_Marks = value; OnPropertyChanged("LastName_F_Sg_A_Marks"); }
        }

        public bool LastName_F_Sg_A_HasComments
        {
            get { return HasComments("LastName_F_Sg_A", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_A_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_A", m_eSubparadigm); }
        }

        public string LastName_F_Sg_G
        {
            get { return GetForm("LastName_F_Sg_G", Subparadigm); }
            set { SetForm("LastName_F_Sg_G", value); }
        }

        private EMark m_eLastName_F_Sg_G_Marks = EMark.None;
        public EMark LastName_F_Sg_G_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_G"); }
            set { m_eLastName_F_Sg_G_Marks = value; OnPropertyChanged("LastName_F_Sg_G_Marks"); }
        }

        public bool LastName_F_Sg_G_HasComments
        {
            get { return HasComments("LastName_F_Sg_G", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_G_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_G", m_eSubparadigm); }
        }

        public string LastName_F_Sg_P
        {
            get { return GetForm("LastName_F_Sg_P", Subparadigm); }
            set { SetForm("LastName_F_Sg_P", value); }
        }

        private EMark m_eLastName_F_Sg_P_Marks = EMark.None;
        public EMark LastName_F_Sg_P_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_P"); }
            set { m_eLastName_F_Sg_P_Marks = value; OnPropertyChanged("LastName_F_Sg_P_Marks"); }
        }

        public bool LastName_F_Sg_P_HasComments
        {
            get { return HasComments("LastName_F_Sg_P", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_P_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_P", m_eSubparadigm); }
        }

        public string LastName_F_Sg_D
        {
            get { return GetForm("LastName_F_Sg_D", Subparadigm); }
            set { SetForm("LastName_F_Sg_D", value); }
        }

        private EMark m_eLastName_F_Sg_D_Marks = EMark.None;
        public EMark LastName_F_Sg_D_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_D"); }
            set { m_eLastName_F_Sg_D_Marks = value; OnPropertyChanged("LastName_F_Sg_D_Marks"); }
        }

        public bool LastName_F_Sg_D_HasComments
        {
            get { return HasComments("LastName_F_Sg_D", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_D_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_D", m_eSubparadigm); }
        }

        public string LastName_F_Sg_I
        {
            get { return GetForm("LastName_F_Sg_I", Subparadigm); }
            set { SetForm("LastName_F_Sg_I", value); }
        }

        private EMark m_eLastName_F_Sg_I_Marks = EMark.None;
        public EMark LastName_F_Sg_I_Marks
        {
            get { return GetFormStatus("LastName_F_Sg_I"); }
            set { m_eLastName_F_Sg_I_Marks = value; OnPropertyChanged("LastName_F_Sg_I_Marks"); }
        }

        public bool LastName_F_Sg_I_HasComments
        {
            get { return HasComments("LastName_F_Sg_I", m_eSubparadigm); }
        }

        public ECellStatus LastName_F_Sg_I_CellStatus
        {
            get { return GetCellStatus("LastName_F_Sg_I", m_eSubparadigm); }
        }

        #endregion

        #region Bindings_Last_Name_Plural

        public string LastName_Pl_N
        {
            get { return GetForm("LastName_Pl_N", Subparadigm); }
            set { SetForm("LastName_Pl_N", value); }
        }

        private EMark m_eLastName_Pl_N_Marks = EMark.None;
        public EMark LastName_Pl_N_Marks
        {
            get { return GetFormStatus("LastName_Pl_N"); }
            set { m_eLastName_Pl_N_Marks = value; OnPropertyChanged("LastName_Pl_N_Marks"); }
        }

        public bool LastName_Pl_N_HasComments
        {
            get { return HasComments("LastName_Pl_N", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_N_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_N", m_eSubparadigm); }
        }

        public string LastName_Pl_A
        {
            get { return GetForm("LastName_Pl_A", Subparadigm); }
            set { SetForm("LastName_Pl_A", value); }
        }

        private EMark m_eLastName_Pl_A_Marks = EMark.None;
        public EMark LastName_Pl_A_Marks
        {
            get { return GetFormStatus("LastName_Pl_A"); }
            set { m_eLastName_Pl_A_Marks = value; OnPropertyChanged("LastName_Pl_A_Marks"); }
        }

        public bool LastName_Pl_A_HasComments
        {
            get { return HasComments("LastName_Pl_A", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_A_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_G", m_eSubparadigm); }    
        }

        public string LastName_Pl_G
        {
            get { return GetForm("LastName_Pl_G", Subparadigm); }
            set { SetForm("LastName_Pl_G", value); }
        }

        private EMark m_eLastName_Pl_G_Marks = EMark.None;
        public EMark LastName_Pl_G_Marks
        {
            get { return GetFormStatus("LastName_Pl_G"); }
            set { m_eLastName_Pl_G_Marks = value; OnPropertyChanged("LastName_Pl_G_Marks"); }
        }

        public bool LastName_Pl_G_HasComments
        {
            get { return HasComments("LastName_Pl_G", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_G_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_G", m_eSubparadigm); }
        }

        public string LastName_Pl_P
        {
            get { return GetForm("LastName_Pl_P", Subparadigm); }
            set { SetForm("LastName_Pl_P", value); }
        }

        private EMark m_eLastName_Pl_P_Marks = EMark.None;
        public EMark LastName_Pl_P_Marks
        {
            get { return GetFormStatus("LastName_Pl_P"); }
            set { m_eLastName_Pl_P_Marks = value; OnPropertyChanged("LastName_Pl_P_Marks"); }
        }

        public bool LastName_Pl_P_HasComments
        {
            get { return HasComments("LastName_Pl_P", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_P_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_P", m_eSubparadigm); }
        }

        public string LastName_Pl_D
        {
            get { return GetForm("LastName_Pl_D", Subparadigm); }
            set { SetForm("LastName_Pl_D", value); }
        }

        private EMark m_eLastName_Pl_D_Marks = EMark.None;
        public EMark LastName_Pl_D_Marks
        {
            get { return GetFormStatus("LastName_Pl_D"); }
            set { m_eLastName_Pl_D_Marks = value; OnPropertyChanged("LastName_Pl_D_Marks"); }
        }

        public bool LastName_Pl_D_HasComments
        {
            get { return HasComments("LastName_Pl_D", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_D_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_D", m_eSubparadigm); }
        }

        public string LastName_Pl_I
        {
            get { return GetForm("LastName_Pl_I", Subparadigm); }
            set { SetForm("LastName_Pl_I", value); }
        }

        private EMark m_eLastName_Pl_I_Marks = EMark.None;
        public EMark LastName_Pl_I_Marks
        {
            get { return GetFormStatus("LastName_Pl_I"); }
            set { m_eLastName_Pl_I_Marks = value; OnPropertyChanged("LastName_Pl_I_Marks"); }
        }

        public bool LastName_Pl_I_HasComments
        {
            get { return HasComments("LastName_Pl_I", m_eSubparadigm); }
        }

        public ECellStatus LastName_Pl_I_CellStatus
        {
            get { return GetCellStatus("LastName_Pl_I", m_eSubparadigm); }
        }

        #endregion

        #region Bindings_HasMultipleForms

        public bool LastName_M_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_M_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_M_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_M_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_M_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_M_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_M_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_F_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_F_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool LastName_Pl_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("LastName_Pl_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        #endregion

        #region Data_Acquisition

        private void InitFormDictionary()
        {
            string sLexemeHash = m_Inflection.sParadigmHash();

            List<string> listGramHashes = null;
            GetGramHashes(ref listGramHashes);
            if (null == listGramHashes)
            {
                MessageBox.Show("Internal error: unable to find gram hashes list.");
                return;
            }

            foreach (string sHash in listGramHashes)
            {
                FormsForGramHash formsPerHash = new FormsForGramHash();
                List<FormDescriptor> lstForms = null;
                if (!m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out lstForms))
                {
                    continue;
                }

                formsPerHash.lstForms = lstForms;
                formsPerHash.iCurrentForm = 0;

                var keyIdx = listGramHashes.IndexOf(sHash);
                if (keyIdx < 0)
                {
                    MessageBox.Show(String.Format("Unable to find gram hash key; illegal hash value: ", sHash));
                    continue;
                }

                string sParadigmHash = null;    // use standard long adj gram hashes regardless of part of speech
                try
                {
                    sParadigmHash = Helpers.m_listPropNamesAdj[keyIdx];
                }
                catch
                {
                    MessageBox.Show(String.Format("Unable to find paradigm hash string; illegal hash value: {0}", sHash));
                    continue;
                }

                var sDisplayHash = Helpers.sFormHashToDisplayHash(sHash);
                m_DictFormStatus[sDisplayHash] = formsPerHash;
            }
        }       //  private void InitFormDictionary()

        #endregion

        public LastNameViewModel(CLexemeManaged lexeme, CInflectionManaged inflection, EM_Subparadigm eSubparadigm, MainModel m)
        {
            m_Lexeme = lexeme;
            m_Inflection = inflection;
            m_MainModel = m;

            BackCommand = new RelayCommand(new Action<object>(GoBack));
            ShowFormComment = new RelayCommand(new Action<object>(ShowFormCommentImpl));
            //            EditCommand = new RelayCommand(new Action<object>(EditForm));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));
            FormScrollUpCommand = new RelayCommand(new Action<object>(FormScrollUp));
            FormScrollDownCommand = new RelayCommand(new Action<object>(FormScrollDown));

            m_eSubparadigm = eSubparadigm;
            IsDerived = false;

            //            m_DictFormStatus = new Dictionary<string, FormsForGramHash>();
            InitFormDictionary();

            PropertyChanged += lastNameViewModel_PropertyChanged;

//            EditEnabled = true;

            //            string sLexemeHash = lexeme.sHash();

        }       //  LastNameViewModel()

        public void ShowFormCommentImpl(Object obj)
        {
            /*
            Comments wndComments = new Comments();
            wndComments.Owner = Application.Current.MainWindow;
            var sDisplayHash = obj as string;
//            string sFormHash = sDisplayHashToFormHash(sDisplayHash, m_eSubparadigm);
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
            */

            return;
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void lastNameViewModel_PropertyChanged(object sender, PropertyChangedEventArgs arg)
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

            //            try
            //            {
            //                ChangedFormHandler handler = null;
            //                FormDescriptor fd = m_DictFormStatus[sFormHash];
            //                handler = fd.handler;
            //                var ret = handler();
            //            }
            //            catch (Exception ex)
            //            {
            //                var msg = "Internal error: unable to invoke word form change handler: ";
            //                msg += ex.Message;
            //                MessageBox.Show(msg);
            //            }
        }

        #region Helpers

        private void GetGramHashes(ref List<string> listKeys)
        {
            listKeys = null;
            IsDerived = false;
            listKeys = Helpers.m_listPropNamesLastNames;
            if (null == listKeys)
            {
                MessageBox.Show("Internal error: unable to determine gram hashes.");
            }
        }       //  GetGramHashes()


        #endregion
    }       //  public class LastNameViewModel ...
}       //  namespace ZalTestApp
