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

        MainModel m_MainModel = null;

//        private delegate bool ChangedFormHandler();

        Dictionary<string, FormDescriptor> m_DictFormStatus;
        Dictionary<string, List<string>> m_DictOriginalForms = new Dictionary<string, List<string>>();

        List<string> m_listPropNamesAdj = new List<string>()
        {
            "AdjL_M_Sg_N", "AdjL_M_Sg_A_Inanim", "AdjL_M_Sg_A_Anim", "AdjL_M_Sg_G", "AdjL_M_Sg_P", "AdjL_M_Sg_D",
            "AdjL_M_Sg_I", "AdjL_F_Sg_N", "AdjL_F_Sg_A", "AdjL_F_Sg_G", "AdjL_F_Sg_P", "AdjL_F_Sg_D", "AdjL_F_Sg_I",
            "AdjL_N_Sg_N", "AdjL_N_Sg_A", "AdjL_N_Sg_G", "AdjL_N_Sg_P", "AdjL_N_Sg_D", "AdjL_N_Sg_I", "AdjL_Pl_N",
            "AdjL_Pl_A_Inanim", "AdjL_Pl_A_Anim", "AdjL_Pl_G", "AdjL_Pl_P", "AdjL_Pl_D", "AdjL_Pl_I",
            "AdjS_M", "AdjS_F", "AdjS_N", "AdjS_Pl", "AdjComp"
        };

        List<string> m_listPropNamesPronAdj = new List<string>()
        {
            "PronAdj_M_Sg_N", "PronAdj_M_Sg_A_Inanim", "PronAdj_M_Sg_A_Anim", "PronAdj_M_Sg_G", "PronAdj_M_Sg_P", "PronAdj_M_Sg_D",
            "PronAdj_M_Sg_I", "PronAdj_F_Sg_N", "PronAdj_F_Sg_A", "PronAdj_F_Sg_G", "PronAdj_F_Sg_P", "PronAdj_F_Sg_D", "PronAdj_F_Sg_I",
            "PronAdj_N_Sg_N", "PronAdj_N_Sg_A", "PronAdj_N_Sg_G", "PronAdj_N_Sg_P", "PronAdj_N_Sg_D", "PronAdj_N_Sg_I", "PronAdj_Pl_N",
            "PronAdj_Pl_A_Inanim", "PronAdj_Pl_A_Anim",  "PronAdj_Pl_G", "PronAdj_Pl_P", "PronAdj_Pl_D", "PronAdj_Pl_I"
        };

        List<string> m_listPropNamesNumAdj = new List<string>()
        {
            "NumAdj_M_Sg_N", "NumAdj_M_Sg_A_Inanim", "NumAdj_M_Sg_A_Anim", "NumAdj_M_Sg_G", "NumAdj_M_Sg_P", "NumAdj_M_Sg_D",
            "NumAdj_M_Sg_I", "NumAdj_F_Sg_N", "NumAdj_F_Sg_A", "NumAdj_F_Sg_G", "NumAdj_F_Sg_P", "NumAdj_F_Sg_D", "NumAdj_F_Sg_I",
            "NumAdj_N_Sg_N", "NumAdj_N_Sg_A", "NumAdj_N_Sg_G", "NumAdj_N_Sg_P", "NumAdj_N_Sg_D", "NumAdj_N_Sg_I", "NumAdj_Pl_N",
            "NumAdj_Pl_A_Inanim", "NumAdj_Pl_A_Anim",  "NumAdj_Pl_G", "NumAdj_Pl_P", "NumAdj_Pl_D", "NumAdj_Pl_I"
        };

        List<string> m_listPropNamesPartPresAct = new List<string>()
        {
            "PPresA_M_Sg_N", "PPresA_M_Sg_A_Inanim", "PPresA_M_Sg_A_Anim", "PPresA_M_Sg_G", "PPresA_M_Sg_P", "PPresA_M_Sg_D",
            "PPresA_M_Sg_I", "PPresA_F_Sg_N", "PPresA_F_Sg_A", "PPresA_F_Sg_G", "PPresA_F_Sg_P", "PPresA_F_Sg_D", "PPresA_F_Sg_I",
            "PPresA_N_Sg_N", "PPresA_N_Sg_A", "PPresA_N_Sg_G", "PPresA_N_Sg_P", "PPresA_N_Sg_D", "PPresA_N_Sg_I", "PPresA_Pl_N",
            "PPresA_Pl_A_Inanim", "PPresA_Pl_A_Anim", "PPresA_Pl_G", "PPresA_Pl_P", "PPresA_Pl_D", "PPresA_Pl_I"
        };

        List<string> m_listPropNamesPartPastAct = new List<string>()
        {
            "PPastA_M_Sg_N", "PPastA_M_Sg_A_Inanim", "PPastA_M_Sg_A_Anim", "PPastA_M_Sg_G", "PPastA_M_Sg_P", "PPastA_M_Sg_D",
            "PPastA_M_Sg_I", "PPastA_F_Sg_N", "PPastA_F_Sg_A", "PPastA_F_Sg_G", "PPastA_F_Sg_P", "PPastA_F_Sg_D", "PPastA_F_Sg_I",
            "PPastA_N_Sg_N", "PPastA_N_Sg_A", "PPastA_N_Sg_G", "PPastA_N_Sg_P", "PPastA_N_Sg_D", "PPastA_N_Sg_I", "PPastA_Pl_N",
            "PPastA_Pl_A_Anim", "PPastA_Pl_A_Inanim", "PPastA_Pl_G", "PPastA_Pl_P", "PPastA_Pl_D", "PPastA_Pl_I"
        };

        List<string> m_listPropNamesPartPresPass = new List<string>()
        {
            "PPresPL_M_Sg_N", "PPresPL_M_Sg_A_Anim", "PPresPL_M_Sg_A_Inanim", "PPresPL_M_Sg_G", "PPresPL_M_Sg_P",
            "PPresPL_M_Sg_D", "PPresPL_M_Sg_I", "PPresPL_F_Sg_N", "PPresPL_F_Sg_A", "PPresPL_F_Sg_G", "PPresPL_F_Sg_P",
            "PPresPL_F_Sg_D", "PPresPL_F_Sg_I", "PPresPL_N_Sg_N", "PPresPL_N_Sg_A", "PPresPL_N_Sg_G", "PPresPL_N_Sg_P",
            "PPresPL_N_Sg_D", "PPresPL_N_Sg_I", "PPresPL_Pl_N", "PPresPL_Pl_A_Anim", "PPresPL_Pl_A_Inanim", "PPresPL_Pl_G",
            "PPresPL_Pl_P", "PPresPL_Pl_D", "PPresPL_Pl_I", "PPresPS_M", "PPresPS_F", "PPresPS_N", "PPresPS_Pl"
        };

        List<string> m_listPropNamesPartPastPass = new List<string>()
        {
            "PPastPL_M_Sg_N", "PPastPL_M_Sg_A_Anim", "PPastPL_M_Sg_A_Inanim", "PPastPL_M_Sg_G", "PPastPL_M_Sg_P",
            "PPastPL_M_Sg_D", "PPastPL_M_Sg_I", "PPastPL_F_Sg_N", "PPastPL_F_Sg_A", "PPastPL_F_Sg_G", "PPastPL_F_Sg_P",
            "PPastPL_F_Sg_D", "PPastPL_F_Sg_I", "PPastPL_N_Sg_N", "PPastPL_N_Sg_A", "PPastPL_N_Sg_G", "PPastPL_N_Sg_P",
            "PPastPL_N_Sg_D", "PPastPL_N_Sg_I", "PPastPL_Pl_N", "PPastPL_Pl_A_Anim", "PPastPL_Pl_A_Inanim", "PPastPL_Pl_G",
            "PPastPL_Pl_P", "PPastPL_Pl_D", "PPastPL_Pl_I", "PPastPS_M", "PPastPS_F", "PPastPS_N", "PPastPS_Pl"
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

        #endregion

        private CLexemeManaged m_Lexeme;
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

        string GetForms(string sFormHash)
        {
            if (!m_DictFormStatus.ContainsKey(sFormHash))
            {
                return "";
            }

            var descriptor = m_DictFormStatus[sFormHash];
            if (m_MainModel.bIsIrregular(m_Lexeme.sHash(), sFormHash))
            {
                descriptor.IsIrregular = true;
            }
            else
            {
                descriptor.IsIrregular = false;
            }

            string sText = null;
            if (null == descriptor.listComments)
            {
                sText = Helpers.sListToCommaSeparatedString(descriptor.listForms);
            }
            else
            {
                sText = Helpers.sListToCommaSeparatedString(descriptor.listForms, descriptor.listComments);
            }

            return sText;
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
            List<string> l = new List<string>();
            List<Tuple<string, string>> c = new List<Tuple<string, string>>();
            Helpers.CommaSeparatedStringToList(sForms, out l, out c);
            fd.listForms = l;
            fd.listComments = c;
            m_DictFormStatus[sHash] = fd;
        }

        #region Bindings_Long_Masculine_Sg

        public string AdjL_M_Sg_N
        {
            get { return GetForms("AdjL_M_Sg_N"); }
            set { SetForms("AdjL_M_Sg_N", value); }
        }

        private EMark m_eAdjL_M_Sg_N_Marks = EMark.None;
        public EMark AdjL_M_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_N"); }
            set { m_eAdjL_M_Sg_N_Marks = value; OnPropertyChanged("AdjL_M_Sg_N_Marks"); }
        }

        public string AdjL_M_Sg_A_Anim
        {
            get { return GetForms("AdjL_M_Sg_A_Anim"); }
            set { SetForms("AdjL_M_Sg_A_Anim", value); }
        }

        private EMark m_eAdjL_M_Sg_A_Anim_Marks = EMark.None;
        public EMark AdjL_M_Sg_A_Anim_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_A_Anim"); }
            set { m_eAdjL_M_Sg_A_Anim_Marks = value; OnPropertyChanged("AdjL_M_Sg_A_Anim_Marks"); }
        }

        public string AdjL_M_Sg_A_Inanim
        {
            get { return GetForms("AdjL_M_Sg_A_Inanim"); }
            set { SetForms("AdjL_M_Sg_A_Inanim", value); }
        }

        private EMark m_eAdjL_M_Sg_A_Inanim_Marks = EMark.None;
        public EMark AdjL_M_Sg_A_Inanim_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_A_Inanim"); }
            set { m_eAdjL_M_Sg_A_Inanim_Marks = value; OnPropertyChanged("AdjL_M_Sg_A_Inanim_Marks"); }
        }

        public string AdjL_M_Sg_G
        {
            get { return GetForms("AdjL_M_Sg_G"); }
            set { SetForms("AdjL_M_Sg_G", value); }
        }

        private EMark m_eAdjL_M_Sg_G_Marks = EMark.None;
        public EMark AdjL_M_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_G"); }
            set { m_eAdjL_M_Sg_G_Marks = value; OnPropertyChanged("AdjL_M_Sg_G_Marks"); }
        }

        public string AdjL_M_Sg_P
        {
            get { return GetForms("AdjL_M_Sg_P"); }
            set { SetForms("AdjL_M_Sg_P", value); }
        }

        private EMark m_eAdjL_M_Sg_P_Marks = EMark.None;
        public EMark AdjL_M_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_P"); }
            set { m_eAdjL_M_Sg_G_Marks = value; OnPropertyChanged("AdjL_M_Sg_P_Marks"); }
        }

        public string AdjL_M_Sg_D
        {
            get { return GetForms("AdjL_M_Sg_D"); }
            set { SetForms("AdjL_M_Sg_D", value); }
        }

        private EMark m_eAdjL_M_Sg_D_Marks = EMark.None;
        public EMark AdjL_M_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_M_Sg_D"); }
            set { m_eAdjL_M_Sg_D_Marks = value; OnPropertyChanged("AdjL_M_Sg_D_Marks"); }
        }

        public string AdjL_M_Sg_I
        {
            get { return GetForms("AdjL_M_Sg_I"); }
            set { SetForms("AdjL_M_Sg_I", value); }
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
            get { return GetForms("AdjL_F_Sg_N"); }
            set { SetForms("AdjL_F_Sg_N", value); }
        }

        private EMark m_eAdjL_F_Sg_N_Marks = EMark.None;
        public EMark AdjL_F_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_N"); }
            set { m_eAdjL_F_Sg_N_Marks = value; OnPropertyChanged("AdjL_F_Sg_N_Marks"); }
        }

        public string AdjL_F_Sg_A
        {
            get { return GetForms("AdjL_F_Sg_A"); }
            set { SetForms("AdjL_F_Sg_A", value); }
        }

        private EMark m_eAdjL_F_Sg_A_Marks = EMark.None;
        public EMark AdjL_F_Sg_A_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_A"); }
            set { m_eAdjL_F_Sg_A_Marks = value; OnPropertyChanged("AdjL_F_Sg_A_Marks"); }
        }

        public string AdjL_F_Sg_G
        {
            get { return GetForms("AdjL_F_Sg_G"); }
            set { SetForms("AdjL_F_Sg_G", value); }
        }

        private EMark m_eAdjL_F_Sg_G_Marks = EMark.None;
        public EMark AdjL_F_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_G"); }
            set { m_eAdjL_F_Sg_G_Marks = value; OnPropertyChanged("AdjL_F_Sg_G_Marks"); }
        }

        public string AdjL_F_Sg_P
        {
            get { return GetForms("AdjL_F_Sg_P"); }
            set { SetForms("AdjL_F_Sg_P", value); }
        }

        private EMark m_eAdjL_F_Sg_P_Marks = EMark.None;
        public EMark AdjL_F_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_P"); }
            set { m_eAdjL_F_Sg_P_Marks = value; OnPropertyChanged("AdjL_F_Sg_P_Marks"); }
        }

        public string AdjL_F_Sg_D
        {
            get { return GetForms("AdjL_F_Sg_D"); }
            set { SetForms("AdjL_F_Sg_D", value); }
        }

        private EMark m_eAdjL_F_Sg_D_Marks = EMark.None;
        public EMark AdjL_F_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_F_Sg_D"); }
            set { m_eAdjL_F_Sg_D_Marks = value; OnPropertyChanged("AdjL_F_Sg_D_Marks"); }
        }

        public string AdjL_F_Sg_I
        {
            get { return GetForms("AdjL_F_Sg_I"); }
            set { SetForms("AdjL_F_Sg_I", value); }
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
            get { return GetForms("AdjL_N_Sg_N"); }
            set { SetForms("AdjL_N_Sg_N", value); }
        }

        private EMark m_eAdjL_N_Sg_N_Marks = EMark.None;
        public EMark AdjL_N_Sg_N_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_N"); }
            set { m_eAdjL_N_Sg_N_Marks = value; OnPropertyChanged("AdjL_N_Sg_N_Marks"); }
        }

        public string AdjL_N_Sg_A
        {
            get { return GetForms("AdjL_N_Sg_A"); }
            set { SetForms("AdjL_N_Sg_A", value); }
        }

        private EMark m_eAdjL_N_Sg_A_Marks = EMark.None;
        public EMark AdjL_N_Sg_A_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_A"); }
            set { m_eAdjL_N_Sg_A_Marks = value; OnPropertyChanged("AdjL_N_Sg_A_Marks"); }
        }

        public string AdjL_N_Sg_G
        {
            get { return GetForms("AdjL_N_Sg_G"); }
            set { SetForms("AdjL_N_Sg_G", value); }
        }

        private EMark m_eAdjL_N_Sg_G_Marks = EMark.None;
        public EMark AdjL_N_Sg_G_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_G"); }
            set { m_eAdjL_N_Sg_G_Marks = value; OnPropertyChanged("AdjL_N_Sg_G_Marks"); }
        }

        public string AdjL_N_Sg_P
        {
            get { return GetForms("AdjL_N_Sg_P"); }
            set { SetForms("AdjL_N_Sg_P", value); }
        }

        private EMark m_eAdjL_N_Sg_P_Marks = EMark.None;
        public EMark AdjL_N_Sg_P_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_P"); }
            set { m_eAdjL_N_Sg_P_Marks = value; OnPropertyChanged("AdjL_N_Sg_P_Marks"); }
        }

        public string AdjL_N_Sg_D
        {
            get { return GetForms("AdjL_N_Sg_D"); }
            set { SetForms("AdjL_N_Sg_D", value); }
        }

        private EMark m_eAdjL_N_Sg_D_Marks = EMark.None;
        public EMark AdjL_N_Sg_D_Marks
        {
            get { return GetFormStatus("AdjL_N_Sg_D"); }
            set { m_eAdjL_N_Sg_D_Marks = value; OnPropertyChanged("AdjL_N_Sg_D_Marks"); }
        }

        public string AdjL_N_Sg_I
        {
            get { return GetForms("AdjL_N_Sg_I"); }
            set { SetForms("AdjL_N_Sg_I", value); }
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
            get { return GetForms("AdjL_Pl_N"); }
            set { SetForms("AdjL_Pl_N", value); }
        }

        private EMark m_eAdjL_Pl_N_Marks = EMark.None;
        public EMark AdjL_Pl_N_Marks
        {
            get { return GetFormStatus("AdjL_Pl_N"); }
            set { m_eAdjL_Pl_N_Marks = value; OnPropertyChanged("AdjL_Pl_N_Marks"); }
        }

        public string AdjL_Pl_A_Anim
        {
            get { return GetForms("AdjL_Pl_A_Anim"); }
            set { SetForms("AdjL_Pl_A_Anim", value); }
        }

        private EMark m_eAdjL_Pl_A_Anim_Marks = EMark.None;
        public EMark AdjL_Pl_A_Anim_Marks
        {
            get { return GetFormStatus("AdjL_Pl_A_Anim"); }
            set { m_eAdjL_Pl_A_Anim_Marks = value; OnPropertyChanged("AdjL_Pl_A_Anim_Marks"); }
        }

        public string AdjL_Pl_A_Inanim
        {
            get { return GetForms("AdjL_Pl_A_Inanim"); }
            set { SetForms("AdjL_Pl_A_Inanim", value); }
        }

        private EMark m_eAdjL_Pl_A_Inanim_Marks = EMark.None;
        public EMark AdjL_Pl_A_Inanim_Marks
        {
            get { return GetFormStatus("AdjL_Pl_A_Inanim"); }
            set { m_eAdjL_Pl_A_Inanim_Marks = value; OnPropertyChanged("AdjL_Pl_A_Inanim_Marks"); }
        }

        public string AdjL_Pl_G
        {
            get { return GetForms("AdjL_Pl_G"); }
            set { SetForms("AdjL_Pl_G", value); }
        }

        private EMark m_eAdjL_Pl_G_Marks = EMark.None;
        public EMark AdjL_Pl_G_Marks
        {
            get { return GetFormStatus("AdjL_Pl_G"); }
            set { m_eAdjL_Pl_G_Marks = value; OnPropertyChanged("AdjL_Pl_G_Marks"); }
        }

        public string AdjL_Pl_P
        {
            get { return GetForms("AdjL_Pl_P"); }
            set { SetForms("AdjL_Pl_P", value); }
        }

        private EMark m_eAdjL_Pl_P_Marks = EMark.None;
        public EMark AdjL_Pl_P_Marks
        {
            get { return GetFormStatus("AdjL_Pl_P"); }
            set { m_eAdjL_Pl_P_Marks = value; OnPropertyChanged("AdjL_Pl_P_Marks"); }
        }

        public string AdjL_Pl_D
        {
            get { return GetForms("AdjL_Pl_D"); }
            set { SetForms("AdjL_Pl_D", value); }
        }

        private EMark m_eAdjL_Pl_D_Marks = EMark.None;
        public EMark AdjL_Pl_D_Marks
        {
            get { return GetFormStatus("AdjL_Pl_D"); }
            set { m_eAdjL_Pl_D_Marks = value; OnPropertyChanged("AdjL_Pl_D_Marks"); }
        }

        public string AdjL_Pl_I
        {
            get { return GetForms("AdjL_Pl_I"); }
            set { SetForms("AdjL_Pl_I", value); }
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
            get { return GetForms("AdjS_M"); }
            set { SetForms("AdjS_M", value); }
        }

        private EMark m_eAdjS_M_Marks = EMark.None;
        public EMark AdjS_M_Marks
        {
            get { return GetFormStatus("AdjS_M"); }
            set { m_eAdjS_M_Marks = value; OnPropertyChanged("AdjS_M_Marks"); }
        }

        public string AdjS_F
        {
            get { return GetForms("AdjS_F"); }
            set { SetForms("AdjS_F", value); }
        }

        private EMark m_eAdjS_F_Marks = EMark.None;
        public EMark AdjS_F_Marks
        {
            get { return GetFormStatus("AdjS_F"); }
            set { m_eAdjS_F_Marks = value; OnPropertyChanged("AdjS_F_Marks"); }
        }

        public string AdjS_N
        {
            get { return GetForms("AdjS_N"); }
            set { SetForms("AdjS_N", value); }
        }

        private EMark m_eAdjS_N_Marks = EMark.None;
        public EMark AdjS_N_Marks
        {
            get { return GetFormStatus("AdjS_N"); }
            set { m_eAdjS_N_Marks = value; OnPropertyChanged("AdjS_N_Marks"); }
        }

        public string AdjS_Pl
        {
            get { return GetForms("AdjS_Pl"); }
            set { SetForms("AdjS_Pl", value); }
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
            get { return GetForms("AdjComp"); }
            set { SetForms("AdjComp", value); }
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
                FormDescriptor fd = new FormDescriptor(null, null, false, false, null);
                List<string> listForms = null;
                bool bRet = m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out listForms);
                if (!bRet)
                {
                    continue;
                }

                fd.listForms = listForms;

                if (m_MainModel.bIsIrregular(sLexemeHash, sHash))
                {
                    List<Tuple<string, string>> listComments;
                    bRet = m_MainModel.GetFormComments(sLexemeHash, sHash, out listComments);
                    if (!bRet || listComments.Count != listForms.Count)
                    {
                        MessageBox.Show("Internal error: unable to retrieve form comments.");
                    }
                    fd.listComments = listComments;
                }

                fd.handler = () =>
                {
                    FormDescriptor fd1 = m_DictFormStatus[sFormHashToDisplayHash(sHash)];
                    var sFormString1 = Helpers.sListToCommaSeparatedString(fd1.listForms);
                    Helpers.AssignDiacritics(sFormString1, ref sFormString1);
                    //                    OnPropertyChanged(hash);
                    return true;
                };

                var sFormString = Helpers.sListToCommaSeparatedString(fd.listForms);
                Helpers.AssignDiacritics(sFormString, ref sFormString);

                var keyIdx = listGramHashes.IndexOf(sHash);
                if (keyIdx < 0)
                {
                    string sMsg = "Unable to find gram hash key; illegal hash value: ";
                    sMsg += sHash;
                    MessageBox.Show(sMsg);
                    continue;
                }

                string sParadigmHash = null;    // use standard adj gram hashes regardless of part of speech
                try
                {
                    sParadigmHash = m_listPropNamesAdj[keyIdx];
                }
                catch (Exception ex)
                {
                    string sMsg = "Unable to find paradigm hash string; illegal hash value: ";
                    sMsg += sHash;
                    sMsg += "; ";
                    sMsg += ex.Message;
                    MessageBox.Show(sMsg);
                }

                m_DictFormStatus.Add(sParadigmHash, fd);
                m_DictOriginalForms.Add(sParadigmHash, fd.listForms);
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

            m_eSubparadigm = eSubparadigm;
            IsDerived = false;

            m_DictFormStatus = new Dictionary<string, FormDescriptor>();
            InitFormDictionary();

            PropertyChanged += adjViewModel_PropertyChanged;

            EditEnabled = true;

//            string sLexemeHash = lexeme.sHash();

        }       //  AdjViewModel()

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

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

        #region Helpers

        private void GetGramHashes(ref List<string> listKeys)
        {
            listKeys = null;

            IsDerived = false;

            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_ADJ)
            {
                if ("мс" == m_Lexeme.sInflectionType())
                {
                    listKeys = m_listPropNamesPronAdj;
                }
                else
                {
                    listKeys = m_listPropNamesAdj;
                }
            }

            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ)
            {
                listKeys = m_listPropNamesPronAdj;
            }
            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NUM_ADJ)
            {
                listKeys = m_listPropNamesNumAdj;
            }
            else if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
            {
                IsDerived = true;

                switch (m_eSubparadigm)
                {
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                        listKeys = m_listPropNamesPartPresAct;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                        listKeys = m_listPropNamesPartPastAct;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                        listKeys = m_listPropNamesPartPresPass;
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                        listKeys = m_listPropNamesPartPastPass;
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

        #endregion

    }       //  public class AdjViewModel ...
}       //  namespace ZalTestApp
