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

        EMark GetFormStatus(string sFormHash)
        {
            string sLexemeHash = m_Lexeme.sParadigmHash();
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

        public bool AdjL_M_Sg_N_HasComments
        {
            get { return HasComments("AdjL_M_Sg_N", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_N_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_N"); }
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

        public bool AdjL_M_Sg_A_Anim_HasComments
        {
            get { return HasComments("AdjL_M_Sg_A_Anim", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_A_Anim_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_G"); }        // sic, no separate hash for Acc anim
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

        public bool AdjL_M_Sg_A_Inanim_HasComments
        {
            get { return HasComments("AdjL_M_Sg_A_Inanim", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_A_Inanim_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_N"); }        // sic, no separate hash for Acc inanim
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

        public bool AdjL_M_Sg_G_HasComments
        {
            get { return HasComments("AdjL_M_Sg_G", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_G_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_G"); }
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

        public bool AdjL_M_Sg_P_HasComments
        {
            get { return HasComments("AdjL_M_Sg_P", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_P_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_P"); }
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

        public bool AdjL_M_Sg_D_HasComments
        {
            get { return HasComments("AdjL_M_Sg_D", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_D_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_D"); }
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

        public bool AdjL_M_Sg_I_HasComments
        {
            get { return HasComments("AdjL_M_Sg_I", m_eSubparadigm); }
        }

        public ECellStatus AdjL_M_Sg_I_CellStatus
        {
            get { return GetCellStatus("AdjL_M_Sg_I"); }
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

        public bool AdjL_F_Sg_N_HasComments
        {
            get { return HasComments("AdjL_F_Sg_N", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_N_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_N"); }
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

        public bool AdjL_F_Sg_A_HasComments
        {
            get { return HasComments("AdjL_F_Sg_A", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_A_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_A"); }
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

        public bool AdjL_F_Sg_G_HasComments
        {
            get { return HasComments("AdjL_F_Sg_G", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_G_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_G"); }
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

        public bool AdjL_F_Sg_P_HasComments
        {
            get { return HasComments("AdjL_F_Sg_P", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_P_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_P"); }
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

        public bool AdjL_F_Sg_D_HasComments
        {
            get { return HasComments("AdjL_F_Sg_D", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_D_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_D"); }
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

        public bool AdjL_F_Sg_I_HasComments
        {
            get { return HasComments("AdjL_F_Sg_I", m_eSubparadigm); }
        }

        public ECellStatus AdjL_F_Sg_I_CellStatus
        {
            get { return GetCellStatus("AdjL_F_Sg_I"); }
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

        public bool AdjL_N_Sg_N_HasComments
        {
            get { return HasComments("AdjL_N_Sg_N", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_N_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_N"); }
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

        public bool AdjL_N_Sg_A_HasComments
        {
            get { return HasComments("AdjL_N_Sg_A", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_A_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_A"); }
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

        public bool AdjL_N_Sg_G_HasComments
        {
            get { return HasComments("AdjL_N_Sg_G", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_G_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_G"); }
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

        public bool AdjL_N_Sg_P_HasComments
        {
            get { return HasComments("AdjL_N_Sg_P", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_P_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_P"); }
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

        public bool AdjL_N_Sg_D_HasComments
        {
            get { return HasComments("AdjL_N_Sg_D", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_D_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_D"); }
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

        public bool AdjL_N_Sg_I_HasComments
        {
            get { return HasComments("AdjL_N_Sg_I", m_eSubparadigm); }
        }

        public ECellStatus AdjL_N_Sg_I_CellStatus
        {
            get { return GetCellStatus("AdjL_N_Sg_I"); }
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

        public bool AdjL_Pl_N_HasComments
        {
            get { return HasComments("AdjL_Pl_N", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_N_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_N"); }
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

        public bool AdjL_Pl_A_Anim_HasComments
        {
            get { return HasComments("AdjL_Pl_A_Anim", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_A_Anim_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_G"); }         // sic, no separate hash for Acc anim
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

        public bool AdjL_Pl_A_Inanim_HasComments
        {
            get { return HasComments("AdjL_Pl_A_Inanim", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_A_Inanim_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_N"); }        // sic, no separate hash for Acc inanim
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

        public bool AdjL_Pl_G_HasComments
        {
            get { return HasComments("AdjL_Pl_G", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_G_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_G"); }
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

        public bool AdjL_Pl_P_HasComments
        {
            get { return HasComments("AdjL_Pl_P", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_P_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_P"); }
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

        public bool AdjL_Pl_D_HasComments
        {
            get { return HasComments("AdjL_Pl_D", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_D_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_D"); }
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

        public bool AdjL_Pl_I_HasComments
        {
            get { return HasComments("AdjL_Pl_I", m_eSubparadigm); }
        }

        public ECellStatus AdjL_Pl_I_CellStatus
        {
            get { return GetCellStatus("AdjL_Pl_I"); }
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

        public bool AdjS_M_HasComments
        {
            get { return HasComments("AdjS_M", m_eSubparadigm); }
        }

        public ECellStatus AdjS_M_CellStatus
        {
            get { return GetCellStatus("AdjS_M"); }
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

        public bool AdjS_F_HasComments
        {
            get { return HasComments("AdjS_F", m_eSubparadigm); }
        }

        public ECellStatus AdjS_F_CellStatus
        {
            get { return GetCellStatus("AdjS_F"); }
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

        public bool AdjS_N_HasComments
        {
            get { return HasComments("AdjS_N", m_eSubparadigm); }
        }

        public ECellStatus AdjS_N_CellStatus
        {
            get { return GetCellStatus("AdjS_N"); }
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

        public bool AdjS_Pl_HasComments
        {
            get { return HasComments("AdjS_Pl", m_eSubparadigm); }
        }

        public ECellStatus AdjS_Pl_CellStatus
        {
            get { return GetCellStatus("AdjS_Pl"); }
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

        public ECellStatus AdjComp_CellStatus
        {
            get { return GetCellStatus("AdjComp"); }
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
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_A_Inanim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_A_Anim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_M_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_M_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_F_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_F_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_A_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_A", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_N_Sg_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_N_Sg_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_A_Inanim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_A_Inanim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_A_Anim_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_A_Anim", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_G_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_G", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_P_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_P", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_D_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_D", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjL_Pl_I_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjL_Pl_I", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjS_M_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjS_M", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjS_F_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjS_F", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjS_N_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjS_N", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjS_Pl_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjS_Pl", out f); return f != null && f.lstForms.Count > 1; }
        }

        public bool AdjComp_HasMultipleForms
        {
            get { FormsForGramHash f = null; m_DictFormStatus.TryGetValue("AdjComp", out f); return f != null && f.lstForms.Count > 1; }
        }
        
        #endregion

        #region Data_Acquisition

        private void InitFormDictionary()
        {
            string sLexemeHash = m_Lexeme.sParadigmHash();

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
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Unable to find paradigm hash string; illegal hash value: {0}", sHash));
                    continue;
                }

                var sDisplayHash = Helpers.sFormHashToDisplayHash(sHash);
                m_DictFormStatus[sDisplayHash] = formsPerHash;
            }
        }       //  private void InitFormDictionary()

        #endregion

        public AdjViewModel(CLexemeManaged lexeme, EM_Subparadigm eSubparadigm, MainModel m)
        {
            m_Lexeme = lexeme;
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

            PropertyChanged += adjViewModel_PropertyChanged;

            EditEnabled = true;

//            string sLexemeHash = lexeme.sHash();

        }       //  AdjViewModel()

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
                // Remove?
                if ("мс-п" == m_Lexeme.sInflectionType())
                {
                    listKeys = Helpers.m_listPropNamesPronAdj;
                }
                else if ("мс" == m_Lexeme.sInflectionType())
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
                if ("мс-п" == m_Lexeme.sInflectionType())
                {
                    listKeys = Helpers.m_listPropNamesPronAdj;
                }
                else
                {
                    listKeys = Helpers.m_listPropNamesAdj;
                }
            }
            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NUM_ADJ)
            {
                if ("числ-п" == m_Lexeme.sInflectionType())
                {
                    listKeys = Helpers.m_listPropNamesNumAdj;
                }
                else
                {
                    listKeys = Helpers.m_listPropNamesAdj;
                }
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


#endregion
    }       //  public class AdjViewModel ...
}       //  namespace ZalTestApp
