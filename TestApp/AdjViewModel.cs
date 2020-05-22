using System;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;

using MainLibManaged;
using System.ComponentModel;

namespace ZalTestApp
{
    public class AdjViewModel : ViewModelBase
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

        private string GetForm(string sDisplayHash, EM_Subparadigm eSubparadigm)
        {
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, eSubparadigm);
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

            return formsForHash.listForms[iAt].sFormText;

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


        #region Bindings_Long_Masculine_Sg

        public string AdjL_M_Sg_N
        {
            get { return GetForm("AdjL_M_Sg_N", Subparadigm); }
            set { SetForm("AdjL_M_Sg_N", value); }
        }

        private EMark m_eAdjL_M_Sg_N_Marks = EMark.None;
        public EMark AdjL_M_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_N"); }
            set { m_eAdjL_M_Sg_N_Marks = value; OnPropertyChanged("AdjL_M_Sg_N_Marks"); }
        }

        public string AdjL_M_Sg_A_Anim
        {
            get { return GetForm("AdjL_M_Sg_A_Anim", Subparadigm); }
            set { SetForm("AdjL_M_Sg_A_Anim", value); }
        }

        private EMark m_eAdjL_M_Sg_A_Anim_Marks = EMark.None;
        public EMark AdjL_M_Sg_A_Anim_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_A_Anim"); }
            set { m_eAdjL_M_Sg_A_Anim_Marks = value; OnPropertyChanged("AdjL_M_Sg_A_Anim_Marks"); }
        }

        public string AdjL_M_Sg_A_Inanim
        {
            get { return GetForm("AdjL_M_Sg_A_Inanim", Subparadigm); }
            set { SetForm("AdjL_M_Sg_A_Inanim", value); }
        }

        private EMark m_eAdjL_M_Sg_A_Inanim_Marks = EMark.None;
        public EMark AdjL_M_Sg_A_Inanim_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_A_Inanim"); }
            set { m_eAdjL_M_Sg_A_Inanim_Marks = value; OnPropertyChanged("AdjL_M_Sg_A_Inanim_Marks"); }
        }

        public string AdjL_M_Sg_G
        {
            get { return GetForm("AdjL_M_Sg_G", Subparadigm); }
            set { SetForm("AdjL_M_Sg_G", value); }
        }

        private EMark m_eAdjL_M_Sg_G_Marks = EMark.None;
        public EMark AdjL_M_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_G"); }
            set { m_eAdjL_M_Sg_G_Marks = value; OnPropertyChanged("AdjL_M_Sg_G_Marks"); }
        }

        public string AdjL_M_Sg_P
        {
            get { return GetForm("AdjL_M_Sg_P", Subparadigm); }
            set { SetForm("AdjL_M_Sg_P", value); }
        }

        private EMark m_eAdjL_M_Sg_P_Marks = EMark.None;
        public EMark AdjL_M_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_P"); }
            set { m_eAdjL_M_Sg_G_Marks = value; OnPropertyChanged("AdjL_M_Sg_P_Marks"); }
        }

        public string AdjL_M_Sg_D
        {
            get { return GetForm("AdjL_M_Sg_D", Subparadigm); }
            set { SetForm("AdjL_M_Sg_D", value); }
        }

        private EMark m_eAdjL_M_Sg_D_Marks = EMark.None;
        public EMark AdjL_M_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_D"); }
            set { m_eAdjL_M_Sg_D_Marks = value; OnPropertyChanged("AdjL_M_Sg_D_Marks"); }
        }

        public string AdjL_M_Sg_I
        {
            get { return GetForm("AdjL_M_Sg_I", Subparadigm); }
            set { SetForm("AdjL_M_Sg_I", value); }
        }

        private EMark m_eAdjL_M_Sg_I_Marks = EMark.None;
        public EMark AdjL_M_Sg_I_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_I"); }
            set { m_eAdjL_M_Sg_I_Marks = value; OnPropertyChanged("AdjL_M_Sg_I_Marks"); }
        }

        #endregion

        #region Bindings_Long_Feminine_Sg

        public string AdjL_F_Sg_N
        {
            get { return GetForm("AdjL_F_Sg_N", Subparadigm); }
            set { SetForm("AdjL_F_Sg_N", value); }
        }

        private EMark m_eAdjL_F_Sg_N_Marks = EMark.None;
        public EMark AdjL_F_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_N"); }
            set { m_eAdjL_F_Sg_N_Marks = value; OnPropertyChanged("AdjL_F_Sg_N_Marks"); }
        }

        public string AdjL_F_Sg_A
        {
            get { return GetForm("AdjL_F_Sg_A", Subparadigm); }
            set { SetForm("AdjL_F_Sg_A", value); }
        }

        private EMark m_eAdjL_F_Sg_A_Marks = EMark.None;
        public EMark AdjL_F_Sg_A_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_A"); }
            set { m_eAdjL_F_Sg_A_Marks = value; OnPropertyChanged("AdjL_F_Sg_A_Marks"); }
        }

        public string AdjL_F_Sg_G
        {
            get { return GetForm("AdjL_F_Sg_G", Subparadigm); }
            set { SetForm("AdjL_F_Sg_G", value); }
        }

        private EMark m_eAdjL_F_Sg_G_Marks = EMark.None;
        public EMark AdjL_F_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_G"); }
            set { m_eAdjL_F_Sg_G_Marks = value; OnPropertyChanged("AdjL_F_Sg_G_Marks"); }
        }

        public string AdjL_F_Sg_P
        {
            get { return GetForm("AdjL_F_Sg_P", Subparadigm); }
            set { SetForm("AdjL_F_Sg_P", value); }
        }

        private EMark m_eAdjL_F_Sg_P_Marks = EMark.None;
        public EMark AdjL_F_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_P"); }
            set { m_eAdjL_F_Sg_P_Marks = value; OnPropertyChanged("AdjL_F_Sg_P_Marks"); }
        }

        public string AdjL_F_Sg_D
        {
            get { return GetForm("AdjL_F_Sg_D", Subparadigm); }
            set { SetForm("AdjL_F_Sg_D", value); }
        }

        private EMark m_eAdjL_F_Sg_D_Marks = EMark.None;
        public EMark AdjL_F_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_D"); }
            set { m_eAdjL_F_Sg_D_Marks = value; OnPropertyChanged("AdjL_F_Sg_D_Marks"); }
        }

        public string AdjL_F_Sg_I
        {
            get { return GetForm("AdjL_F_Sg_I", Subparadigm); }
            set { SetForm("AdjL_F_Sg_I", value); }
        }

        private EMark m_eAdjL_F_Sg_I_Marks = EMark.None;
        public EMark AdjL_F_Sg_I_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_I"); }
            set { m_eAdjL_F_Sg_I_Marks = value; OnPropertyChanged("AdjL_F_Sg_I_Marks"); }
        }

        #endregion

        #region Bindings_Long_Neuter_Sg

        public string AdjL_N_Sg_N
        {
            get { return GetForm("AdjL_N_Sg_N", Subparadigm); }
            set { SetForm("AdjL_N_Sg_N", value); }
        }

        private EMark m_eAdjL_N_Sg_N_Marks = EMark.None;
        public EMark AdjL_N_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_N"); }
            set { m_eAdjL_N_Sg_N_Marks = value; OnPropertyChanged("AdjL_N_Sg_N_Marks"); }
        }

        public string AdjL_N_Sg_A
        {
            get { return GetForm("AdjL_N_Sg_A", Subparadigm); }
            set { SetForm("AdjL_N_Sg_A", value); }
        }

        private EMark m_eAdjL_N_Sg_A_Marks = EMark.None;
        public EMark AdjL_N_Sg_A_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_A"); }
            set { m_eAdjL_N_Sg_A_Marks = value; OnPropertyChanged("AdjL_N_Sg_A_Marks"); }
        }

        public string AdjL_N_Sg_G
        {
            get { return GetForm("AdjL_N_Sg_G", Subparadigm); }
            set { SetForm("AdjL_N_Sg_G", value); }
        }

        private EMark m_eAdjL_N_Sg_G_Marks = EMark.None;
        public EMark AdjL_N_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_G"); }
            set { m_eAdjL_N_Sg_G_Marks = value; OnPropertyChanged("AdjL_N_Sg_G_Marks"); }
        }

        public string AdjL_N_Sg_P
        {
            get { return GetForm("AdjL_N_Sg_P", Subparadigm); }
            set { SetForm("AdjL_N_Sg_P", value); }
        }

        private EMark m_eAdjL_N_Sg_P_Marks = EMark.None;
        public EMark AdjL_N_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_P"); }
            set { m_eAdjL_N_Sg_P_Marks = value; OnPropertyChanged("AdjL_N_Sg_P_Marks"); }
        }

        public string AdjL_N_Sg_D
        {
            get { return GetForm("AdjL_N_Sg_D", Subparadigm); }
            set { SetForm("AdjL_N_Sg_D", value); }
        }

        private EMark m_eAdjL_N_Sg_D_Marks = EMark.None;
        public EMark AdjL_N_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_D"); }
            set { m_eAdjL_N_Sg_D_Marks = value; OnPropertyChanged("AdjL_N_Sg_D_Marks"); }
        }

        public string AdjL_N_Sg_I
        {
            get { return GetForm("AdjL_N_Sg_I", Subparadigm); }
            set { SetForm("AdjL_N_Sg_I", value); }
        }

        private EMark m_eAdjL_N_Sg_I_Marks = EMark.None;
        public EMark AdjL_N_Sg_I_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_I"); }
            set { m_eAdjL_N_Sg_I_Marks = value; OnPropertyChanged("AdjL_N_Sg_I_Marks"); }
        }

        #endregion

        #region Bindings_Long_Plural

        public string AdjL_Pl_N
        {
            get { return GetForm("AdjL_Pl_N", Subparadigm); }
            set { SetForm("AdjL_Pl_N", value); }
        }

        private EMark m_eAdjL_Pl_N_Marks = EMark.None;
        public EMark AdjL_Pl_N_Marks
        {
            get { return GetFormStatus("AdjL_Pl_N"); }
            set { m_eAdjL_Pl_N_Marks = value; OnPropertyChanged("AdjL_Pl_N_Marks"); }
        }

        public string AdjL_Pl_A_Anim
        {
            get { return GetForm("AdjL_Pl_A_Anim", Subparadigm); }
            set { SetForm("AdjL_Pl_A_Anim", value); }
        }

        private EMark m_eAdjL_Pl_A_Anim_Marks = EMark.None;
        public EMark AdjL_Pl_A_Anim_Marks
        {
            get { return GetFormStatus("AdjL_Pl_A_Anim"); }
            set { m_eAdjL_Pl_A_Anim_Marks = value; OnPropertyChanged("AdjL_Pl_A_Anim_Marks"); }
        }

        public string AdjL_Pl_A_Inanim
        {
            get { return GetForm("AdjL_Pl_A_Inanim", Subparadigm); }
            set { SetForm("AdjL_Pl_A_Inanim", value); }
        }

        private EMark m_eAdjL_Pl_A_Inanim_Marks = EMark.None;
        public EMark AdjL_Pl_A_Inanim_Marks
        {
            get { return GetFormStatus("AdjL_Pl_A_Inanim"); }
            set { m_eAdjL_Pl_A_Inanim_Marks = value; OnPropertyChanged("AdjL_Pl_A_Inanim_Marks"); }
        }

        public string AdjL_Pl_G
        {
            get { return GetForm("AdjL_Pl_G", Subparadigm); }
            set { SetForm("AdjL_Pl_G", value); }
        }

        private EMark m_eAdjL_Pl_G_Marks = EMark.None;
        public EMark AdjL_Pl_G_Marks
        {
            get { return GetFormStatus("AdjL_Pl_G"); }
            set { m_eAdjL_Pl_G_Marks = value; OnPropertyChanged("AdjL_Pl_G_Marks"); }
        }

        public string AdjL_Pl_P
        {
            get { return GetForm("AdjL_Pl_P", Subparadigm); }
            set { SetForm("AdjL_Pl_P", value); }
        }

        private EMark m_eAdjL_Pl_P_Marks = EMark.None;
        public EMark AdjL_Pl_P_Marks
        {
            get { return GetFormStatus("AdjL_Pl_P"); }
            set { m_eAdjL_Pl_P_Marks = value; OnPropertyChanged("AdjL_Pl_P_Marks"); }
        }

        public string AdjL_Pl_D
        {
            get { return GetForm("AdjL_Pl_D", Subparadigm); }
            set { SetForm("AdjL_Pl_D", value); }
        }

        private EMark m_eAdjL_Pl_D_Marks = EMark.None;
        public EMark AdjL_Pl_D_Marks
        {
            get { return GetFormStatus("AdjL_Pl_D"); }
            set { m_eAdjL_Pl_D_Marks = value; OnPropertyChanged("AdjL_Pl_D_Marks"); }
        }

        public string AdjL_Pl_I
        {
            get { return GetForm("AdjL_Pl_I", Subparadigm); }
            set { SetForm("AdjL_Pl_I", value); }
        }

        private EMark m_eAdjL_Pl_I_Marks = EMark.None;
        public EMark AdjL_Pl_I_Marks
        {
            get { return GetFormStatus("AdjL_Pl_I"); }
            set { m_eAdjL_Pl_I_Marks = value; OnPropertyChanged("AdjL_Pl_I_Marks"); }
        }

        #endregion

        #region Bindings_Short

        public string AdjS_M
        {
            get { return GetForm("AdjS_M", Subparadigm); }
            set { SetForm("AdjS_M", value); }
        }

        private EMark m_eAdjS_M_Marks = EMark.None;
        public EMark AdjS_M_Marks
        {
            get { return GetFormStatus("AdjS_M"); }
            set { m_eAdjS_M_Marks = value; OnPropertyChanged("AdjS_M_Marks"); }
        }

        public string AdjS_F
        {
            get { return GetForm("AdjS_F", Subparadigm); }
            set { SetForm("AdjS_F", value); }
        }

        private EMark m_eAdjS_F_Marks = EMark.None;
        public EMark AdjS_F_Marks
        {
            get { return GetFormStatus("AdjS_F"); }
            set { m_eAdjS_F_Marks = value; OnPropertyChanged("AdjS_F_Marks"); }
        }

        public string AdjS_N
        {
            get { return GetForm("AdjS_N", Subparadigm); }
            set { SetForm("AdjS_N", value); }
        }

        private EMark m_eAdjS_N_Marks = EMark.None;
        public EMark AdjS_N_Marks
        {
            get { return GetFormStatus("AdjS_N"); }
            set { m_eAdjS_N_Marks = value; OnPropertyChanged("AdjS_N_Marks"); }
        }

        public string AdjS_Pl
        {
            get { return GetForm("AdjS_Pl", Subparadigm); }
            set { SetForm("AdjS_Pl", value); }
        }

        private EMark m_eAdjS_Pl_Marks = EMark.None;
        public EMark AdjS_Pl_Marks
        {
            get { return GetFormStatus("AdjS_Pl"); }
            set { m_eAdjS_Pl_Marks = value; OnPropertyChanged("AdjS_Pl_Marks"); }
        }

        #endregion

        #region Bindings_Comparative

        public string AdjComp
        {
            get { return GetForm("AdjComp", Subparadigm); }
            set { SetForm("AdjComp", value); }
        }

        private EMark m_eAdjComp_Marks = EMark.None;
        public EMark AdjComp_Marks
        {
            get { return GetFormStatus("AdjComp"); }
            set { m_eAdjComp_Marks = value; OnPropertyChanged("AdjComp_Marks"); }
        }

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

        public bool AdjL_M_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_N", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_A_Inanim", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_A_Anim", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_G", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_P", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_D", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_M_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_I", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_N", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_A", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_G", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_P", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_D", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_F_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_I", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_N", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_A", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_G", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_P", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_D", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_N_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_I", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_N", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_A_Inanim", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_A_Anim", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_G", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_P", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_D", out f); return f != null && f.listForms.Count > 1; }
        }

        public bool AdjL_Pl_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_I", out f); return f != null && f.listForms.Count > 1; }
        }

        #endregion

        #region Data_Acquisition

        private void InitFormDictionary()
        {
            string sLexemeHash = m_Lexeme.sHash();

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
                List<string> listForms = null;
                if (!m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out listForms))
                {
                    continue;
                }

                foreach (string sForm in listForms)
                {
                    FormDescriptor fd = new FormDescriptor();
                    fd.sFormText = sForm;
                    if (m_MainModel.bIsIrregular(sLexemeHash, sHash))
                    {
                        fd.IsIrregular = true;
                    }
                    else
                    {
                        fd.IsIrregular = false;
                    }

                    formsPerHash.listForms.Add(fd);
                }

                formsPerHash.iCurrentForm = 0;

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

                m_DictFormStatus[sHash] = formsPerHash;
            }
        }       //  private void InitFormDictionary()

        #endregion

        public AdjViewModel(CLexemeManaged lexeme, EM_Subparadigm eSubparadigm, MainModel m)
        {
            m_Lexeme = lexeme;
            m_MainModel = m;

            BackCommand = new RelayCommand(new Action<object>(GoBack));
//            EditCommand = new RelayCommand(new Action<object>(EditForm));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));
            FormScrollUpCommand = new RelayCommand(new Action<object>(FormScrollUp));
            FormScrollDownCommand = new RelayCommand(new Action<object>(FormScrollDown));

            m_eSubparadigm = eSubparadigm;
            IsDerived = false;

//            m_DictFormStatus = new Dictionary<string, FormsForGramHash>();
            InitFormDictionary();

            PropertyChanged += adjViewModel_PropertyChanged;

            EditEnabled = true;

//            string sLexemeHash = lexeme.sHash();

        }       //  AdjViewModel()

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

/*
        private EM_ReturnCode CreateIrregularWordForm(string sForm, string sGramHash, ref CWordFormManaged wf)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            eRet = m_Lexeme.eCreateWordForm(ref wf);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to create a word form.");
                return eRet;
            }

//            wf.SetPos(m_Lexeme.ePartOfSpeech());

            EM_Number eNumber = EM_Number.NUM_UNDEFINED;
            eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                wf.SetNumber(eNumber);
            }

            EM_Gender eGender = EM_Gender.GENDER_UNDEFINED;
            eRet = Helpers.eGramHashToGender(sGramHash, ref eGender);
            wf.SetGender(eGender);

            EM_Case eCase = EM_Case.CASE_UNDEFINED;
            EM_Animacy eAnimacy = EM_Animacy.ANIM_UNDEFINED;
            eRet = Helpers.eGramHashToCase(sGramHash, ref eCase, ref eAnimacy);
            wf.SetCase(eCase);
            wf.SetAnimacy(eAnimacy);

            EM_PartOfSpeech ePos = EM_PartOfSpeech.POS_UNDEFINED;
            ePos = EM_PartOfSpeech.POS_ADJ;
            wf.SetPos(ePos);
            EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            eRet = Helpers.eGramHashToSubparadigm(sGramHash, ref ePos, ref eSp);
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
                }       // foreach()
            }

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

        public void adjViewModel_PropertyChanged(object sender, PropertyChangedEventArgs arg)
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

            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_ADJ)
            {
                if ("мс" == m_Lexeme.sInflectionType())
                {
                    listKeys = Helpers.m_listPropNamesPronAdj;
                }
                else
                {
                    listKeys = Helpers.m_listPropNamesAdj;
                }
            }

            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ)
            {
                listKeys = Helpers.m_listPropNamesPronAdj;
            }
            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NUM_ADJ)
            {
                listKeys = Helpers.m_listPropNamesNumAdj;
            }
            else if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
            {
                IsDerived = true;

                switch (m_eSubparadigm)
                {
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                        listKeys = Helpers.m_listPropNamesPartPresAct;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                        listKeys = Helpers.m_listPropNamesPartPastAct;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                        listKeys = Helpers.m_listPropNamesPartPresPass;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                        listKeys = Helpers.m_listPropNamesPartPastPass;
                        break;

                    default:
                        MessageBox.Show("Illegal subparadigm.");
                        break;
                }
            }

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
#endregion
    }       //  public class AdjViewModel ...
}       //  namespace ZalTestApp
