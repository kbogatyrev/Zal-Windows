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

        private delegate bool ChangedFormHandler();

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

        Dictionary<string, FormDescriptor> m_DictFormStatus;
        Dictionary<string, List<string>> m_DictOriginalForms = new Dictionary<string, List<string>>();

        List<string> m_listPropNamesAdj = new List<string>()
        {
            "AdjL_M_Sg_N", "AdjL_M_Sg_A_Inanim", "AdjL_M_Sg_A_Anim", "AdjL_M_Sg_G", "AdjL_M_Sg_P", "AdjL_M_Sg_D",
            "AdjL_M_Sg_I", "AdjL_F_Sg_N", "AdjL_F_Sg_A", "AdjL_F_Sg_G", "AdjL_F_Sg_P", "AdjL_F_Sg_D", "AdjL_F_Sg_I",
            "AdjL_N_Sg_N", "AdjL_N_Sg_A", "AdjL_N_Sg_G", "AdjL_N_Sg_P", "AdjL_N_Sg_D", "AdjL_N_Sg_I", "AdjL_Pl_N",
            "AdjL_Pl_A_Inanim", "AdjL_Pl_A_Anim", "AdjL_Pl_G", "AdjL_Pl_P", "AdjL_Pl_D", "AdjL_Pl_I",
            "AdjS_M", "AdjS_F", "AdjS_N", "AdjS_Pl"
        };

        List<string> m_listPropNamesPronAdj = new List<string>()
        {
            "PronAdj_M_Sg_N", "PronAdj_M_Sg_A_Inanim", "PronAdj_M_Sg_A_Anim", "PronAdj_M_Sg_G", "PronAdj_M_Sg_P", "PronAdj_M_Sg_D",
            "PronAdj_M_Sg_I", "PronAdj_F_Sg_N", "PronAdj_F_Sg_A", "PronAdj_F_Sg_G", "PronAdj_F_Sg_P", "PronAdj_F_Sg_D", "PronAdj_F_Sg_I",
            "PronAdj_N_Sg_N", "PronAdj_N_Sg_A", "PronAdj_N_Sg_G", "PronAdj_N_Sg_P", "PronAdj_N_Sg_D", "PronAdj_N_Sg_I", "PronAdj_Pl_N",
            "PronAdj_Pl_A_Inanim", "PronAdj_Pl_A_Anim",  "PronAdj_Pl_G", "PronAdj_Pl_P", "PronAdj_Pl_D", "PronAdj_Pl_I"
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
            "PPastPL_Pl_P", "PPastPL_Pl_D", "PPastPL_Pl_I", "PPastPS_M", "PPastPS_F", "PPastPS_N", "PPastPS_Pl",
            "PPastPS_M", "PPastPS_F", "PPastPS_N", "PPastPS_Pl"
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

        #region Bindings_Long_Masculine_Sg

        public string AdjL_M_Sg_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_N"] = fd;
                OnPropertyChanged("AdjL_M_Sg_N");
            }
        }

        public bool AdjL_M_Sg_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_N"] = fd;
                OnPropertyChanged("AdjL_M_Sg_N_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_A_Anim
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_A_Anim"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_A_Anim"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_A_Anim"] = fd;
                OnPropertyChanged("AdjL_M_Sg_A_Anim");
            }
        }

        public bool AdjL_M_Sg_A_Anim_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_A_Anim"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_A_Anim"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_A_Anim"] = fd;
                OnPropertyChanged("AdjL_M_Sg_A_Anim_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_A_Inanim
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_A_Inanim"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_A_Inanim"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_A_Inanim"] = fd;
                OnPropertyChanged("AdjL_M_Sg_A_Inanim");
            }
        }

        public bool AdjL_M_Sg_A_Inanim_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_A_Inanim"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_A_Inanim"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_A_Inanim"] = fd;
                OnPropertyChanged("AdjL_M_Sg_A_Inanim_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_G"] = fd;
                OnPropertyChanged("AdjL_M_Sg_G");
            }
        }

        public bool AdjL_M_Sg_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_G"] = fd;
                OnPropertyChanged("AdjL_M_Sg_G_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_P"] = fd;
                OnPropertyChanged("AdjL_M_Sg_P");
            }
        }

        public bool AdjL_M_Sg_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_P"] = fd;
                OnPropertyChanged("AdjL_M_Sg_P_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_D"] = fd;
                OnPropertyChanged("AdjL_M_Sg_D");
            }
        }

        public bool AdjL_M_Sg_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_D"] = fd;
                OnPropertyChanged("AdjL_M_Sg_D_IsReadOnly");
            }
        }

        public string AdjL_M_Sg_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_M_Sg_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_M_Sg_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_M_Sg_I"] = fd;
                OnPropertyChanged("AdjL_M_Sg_I");
            }
        }

        public bool AdjL_M_Sg_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_M_Sg_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_M_Sg_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_M_Sg_I"] = fd;
                OnPropertyChanged("AdjL_M_Sg_I_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Long_Feminine_Sg

        public string AdjL_F_Sg_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_N"] = fd;
                OnPropertyChanged("AdjL_F_Sg_N");
            }
        }

        public bool AdjL_F_Sg_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_N"] = fd;
                OnPropertyChanged("AdjL_F_Sg_N_IsReadOnly");
            }
        }

        public string AdjL_F_Sg_A
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_A"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_A"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_A"] = fd;
                OnPropertyChanged("AdjL_F_Sg_A");
            }
        }

        public bool AdjL_F_Sg_A_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_A"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_A"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_A"] = fd;
                OnPropertyChanged("AdjL_F_Sg_A_IsReadOnly");
            }
        }

        public string AdjL_F_Sg_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_G"] = fd;
                OnPropertyChanged("AdjL_F_Sg_G");
            }
        }

        public bool AdjL_F_Sg_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_G"] = fd;
                OnPropertyChanged("AdjL_F_Sg_G_IsReadOnly");
            }
        }

        public string AdjL_F_Sg_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_P"] = fd;
                OnPropertyChanged("AdjL_F_Sg_P");
            }
        }

        public bool AdjL_F_Sg_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_P"] = fd;
                OnPropertyChanged("AdjL_F_Sg_P_IsReadOnly");
            }
        }

        public string AdjL_F_Sg_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_D"] = fd;
                OnPropertyChanged("AdjL_F_Sg_D");
            }
        }

        public bool AdjL_F_Sg_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_D"] = fd;
                OnPropertyChanged("AdjL_F_Sg_D_IsReadOnly");
            }
        }

        public string AdjL_F_Sg_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_F_Sg_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_F_Sg_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_F_Sg_I"] = fd;
                OnPropertyChanged("AdjL_F_Sg_I");
            }
        }

        public bool AdjL_F_Sg_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_F_Sg_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_F_Sg_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_F_Sg_I"] = fd;
                OnPropertyChanged("AdjL_F_Sg_I_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Long_Neuter_Sg

        public string AdjL_N_Sg_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_N"] = fd;
                OnPropertyChanged("AdjL_N_Sg_N");
            }
        }

        public bool AdjL_N_Sg_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_N"] = fd;
                OnPropertyChanged("AdjL_N_Sg_N_IsReadOnly");
            }
        }

        public string AdjL_N_Sg_A
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_A"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_A"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_A"] = fd;
                OnPropertyChanged("AdjL_N_Sg_A");
            }
        }

        public bool AdjL_N_Sg_A_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_A"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_A"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_A"] = fd;
                OnPropertyChanged("AdjL_N_Sg_A_IsReadOnly");
            }
        }

        public string AdjL_N_Sg_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_G"] = fd;
                OnPropertyChanged("AdjL_N_Sg_G");
            }
        }

        public bool AdjL_N_Sg_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_G"] = fd;
                OnPropertyChanged("AdjL_N_Sg_G_IsReadOnly");
            }
        }

        public string AdjL_N_Sg_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_P"] = fd;
                OnPropertyChanged("AdjL_N_Sg_P");
            }
        }

        public bool AdjL_N_Sg_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_P"] = fd;
                OnPropertyChanged("AdjL_N_Sg_P_IsReadOnly");
            }
        }

        public string AdjL_N_Sg_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_D"] = fd;
                OnPropertyChanged("AdjL_N_Sg_D");
            }
        }

        public bool AdjL_N_Sg_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_D"] = fd;
                OnPropertyChanged("AdjL_N_Sg_D_IsReadOnly");
            }
        }

        public string AdjL_N_Sg_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_N_Sg_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_N_Sg_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_N_Sg_I"] = fd;
                OnPropertyChanged("AdjL_N_Sg_I");
            }
        }

        public bool AdjL_N_Sg_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_N_Sg_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_N_Sg_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_N_Sg_I"] = fd;
                OnPropertyChanged("AdjL_N_Sg_I_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Long_Plural

        public string AdjL_Pl_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_N"] = fd;
                OnPropertyChanged("AdjL_Pl_N");
            }
        }

        public bool AdjL_Pl_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_N"] = fd;
                OnPropertyChanged("AdjL_Pl_N_IsReadOnly");
            }
        }

        public string AdjL_Pl_A_Anim
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_A_Anim"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_A_Anim"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_A_Anim"] = fd;
                OnPropertyChanged("AdjL_Pl_A_Anim");
            }
        }

        public bool AdjL_Pl_A_Anim_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_A_Anim"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_A_Anim"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_A_Anim"] = fd;
                OnPropertyChanged("AdjL_Pl_A_Anim_IsReadOnly");
            }
        }

        public string AdjL_Pl_A_Inanim
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_A_Inanim"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_A_Inanim"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_A_Inanim"] = fd;
                OnPropertyChanged("AdjL_Pl_A_Inanim");
            }
        }

        public bool AdjL_Pl_A_Inanim_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_A_Inanim"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_A_Inanim"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_A_Inanim"] = fd;
                OnPropertyChanged("AdjL_Pl_A_Inanim_IsReadOnly");
            }
        }

        public string AdjL_Pl_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_G"] = fd;
                OnPropertyChanged("AdjL_Pl_G");
            }
        }

        public bool AdjL_Pl_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_G"] = fd;
                OnPropertyChanged("AdjL_Pl_G_IsReadOnly");
            }
        }

        public string AdjL_Pl_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_P"] = fd;
                OnPropertyChanged("AdjL_Pl_P");
            }
        }

        public bool AdjL_Pl_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_P"] = fd;
                OnPropertyChanged("AdjL_Pl_P_IsReadOnly");
            }
        }

        public string AdjL_Pl_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_D"] = fd;
                OnPropertyChanged("AdjL_Pl_D");
            }
        }

        public bool AdjL_Pl_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_D"] = fd;
                OnPropertyChanged("AdjL_Pl_D_IsReadOnly");
            }
        }

        public string AdjL_Pl_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjL_Pl_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjL_Pl_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjL_Pl_I"] = fd;
                OnPropertyChanged("AdjL_Pl_I");
            }
        }

        public bool AdjL_Pl_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["AdjL_Pl_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["AdjL_Pl_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjL_Pl_I"] = fd;
                OnPropertyChanged("AdjL_Pl_I_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Short

        public string AdjS_M
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_M"))
                {
                    return "";
                }

                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjS_M"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjS_M"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjS_M"] = fd;
                OnPropertyChanged("AdjS_M");
            }
        }

        public bool AdjS_M_IsReadOnly
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_M"))
                {
                    return true;
                }
                return !m_DictFormStatus["AdjS_M"].bCanEdit;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_M"))
                {
                    return;
                }

                var fd = m_DictFormStatus["AdjS_M"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjS_M"] = fd;
                OnPropertyChanged("AdjS_M_IsReadOnly");
            }
        }

        public string AdjS_F
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_F"))
                {
                    return "";
                }
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjS_F"].listForms);
                return text;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_F"))
                {
                    return;
                }
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjS_F"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjS_F"] = fd;
                OnPropertyChanged("AdjS_F");
            }
        }

        public bool AdjS_F_IsReadOnly
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_F"))
                {
                    return true;
                }
                return !m_DictFormStatus["AdjS_F"].bCanEdit;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_F"))
                {
                    return;
                }
                var fd = m_DictFormStatus["AdjS_F"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjS_F"] = fd;
                OnPropertyChanged("AdjS_F_IsReadOnly");
            }
        }

        public string AdjS_N
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_N"))
                {
                    return "";
                }
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjS_N"].listForms);
                return text;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_N"))
                {
                    return;
                }
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjS_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjS_N"] = fd;
                OnPropertyChanged("AdjS_N");
            }
        }

        public bool AdjS_N_IsReadOnly
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_N"))
                {
                    return true;
                }
                return !m_DictFormStatus["AdjS_N"].bCanEdit;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_N"))
                {
                    return;
                }
                var fd = m_DictFormStatus["AdjS_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjS_N"] = fd;
                OnPropertyChanged("AdjS_N_IsReadOnly");
            }
        }

        public string AdjS_Pl
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_Pl"))
                {
                    return "";
                }
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjS_Pl"].listForms);
                return text;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_Pl"))
                {
                    return;
                }
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjS_Pl"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjS_Pl"] = fd;
                OnPropertyChanged("AdjS_Pl");
            }
        }

        public bool AdjS_Pl_IsReadOnly
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_Pl"))
                {
                    return true;
                }
                return !m_DictFormStatus["AdjS_Pl"].bCanEdit;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjS_Pl"))
                {
                    return;
                }
                var fd = m_DictFormStatus["AdjS_Pl"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjS_Pl"] = fd;
                OnPropertyChanged("AdjS_Pl_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Comparative

        public string AdjComp
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjComp"))
                {
                    return "";
                }
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["AdjComp"].listForms);
                return text;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjComp"))
                {
                    return;
                }
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["AdjComp"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["AdjComp"] = fd;
                OnPropertyChanged("AdjComp");
            }
        }

        public bool AdjComp_IsReadOnly
        {
            get
            {
                if (!m_DictFormStatus.ContainsKey("AdjComp"))
                {
                    return true;
                }
                return !m_DictFormStatus["AdjComp"].bCanEdit;
            }
            set
            {
                if (!m_DictFormStatus.ContainsKey("AdjComp"))
                {
                    return;
                }
                var fd = m_DictFormStatus["AdjComp"];
                fd.bCanEdit = !value;
                m_DictFormStatus["AdjComp"] = fd;
                OnPropertyChanged("AdjComp_IsReadOnly");
            }
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
                var keyIdx = listGramHashes.IndexOf(sHash);
                if (keyIdx < 0)
                {
                    string sMsg = "Unable to find gram hash key; illegal hash value: ";
                    sMsg += sHash;
                    MessageBox.Show(sMsg);
                    continue;
                }

                var sKey = listGramHashes[keyIdx];
                FormDescriptor fd = new FormDescriptor(null, false, null);
                List<string> listForms = null;
                var bRet = m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out listForms);
                if (!bRet)
                {
                    continue;
                }

                fd.listForms = listForms;
                fd.handler = () =>
                {
                    FormDescriptor fd1 = m_DictFormStatus[sHash];
                    if (!fd1.bCanEdit)
                    {
                        return true;
                    }

                    var sFormString1 = Helpers.sListToCommaSeparatedString(fd1.listForms);
                    Helpers.AssignDiacritics(sFormString1, ref sFormString1);
                    //                    OnPropertyChanged(hash);
                    return true;
                };

                var sFormString = Helpers.sListToCommaSeparatedString(fd.listForms);
                Helpers.AssignDiacritics(sFormString, ref sFormString);

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
            EditCommand = new RelayCommand(new Action<object>(EditForm));
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

        public void EditForm(Object obj)
        {
            var sPropName = obj as string;

            try
            {
                var fd = m_DictFormStatus[sPropName];
                fd.bCanEdit = !fd.bCanEdit;
                m_DictFormStatus[sPropName] = fd;
                sPropName += "_IsReadOnly";
                OnPropertyChanged(sPropName);
            }
            catch (Exception ex)
            {
                var msg = "Internal error: unable to invoke word form change handler: ";
                msg += ex.Message;
                MessageBox.Show(msg);
            }
        }

        public void SaveForms(Object obj)
        {
            foreach (KeyValuePair<string, List<string>> entry in m_DictOriginalForms)
            {
                List<string> originalForms = entry.Value;
                FormDescriptor formDescriptor;
                if (m_DictFormStatus.TryGetValue(entry.Key, out formDescriptor))
                {
                    List<string> changedForms = formDescriptor.listForms;
                    if (changedForms != originalForms)
                    {
                        foreach (string sForm in changedForms)
                        {
                            var idx = changedForms.IndexOf(sForm);
                            if (idx < 0)
                            {
                                var msg = "Internal error: form index out of range";
                                MessageBox.Show(msg);
                                return;
                            }

                            CWordFormManaged wf = null;
                            var eRet = m_Lexeme.eWordFormFromHash(entry.Key, changedForms.IndexOf(sForm), ref wf);
                            if (eRet != EM_ReturnCode.H_NO_ERROR)
                            {
                                var msg = "Internal error: unable to create wordform object";
                                MessageBox.Show(msg);
                                return;
                            }

                            string sOutForm = "";
                            Dictionary<int, EM_StressType> dictStressPos;
                            Helpers.StressMarksToPosList(sForm, out sOutForm, out dictStressPos);
                            wf.SetWordForm(sOutForm);
                            eRet = wf.eSetIrregularStressPositions(dictStressPos);
                            if (eRet != EM_ReturnCode.H_NO_ERROR)
                            {
                                var msg = "Internal error: unable to save stress positions";
                                MessageBox.Show(msg);
                                return;
                            }
                            eRet = wf.eSaveIrregularForm();
                            if (eRet != EM_ReturnCode.H_NO_ERROR)
                            {
                                var msg = "Internal error: unable to save wordform object";
                                MessageBox.Show(msg);
                                return;
                            }
                        }
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
                listKeys = m_listPropNamesAdj;
            }

            if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ)
            {
                listKeys = m_listPropNamesPronAdj;
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

        #endregion

    }       //  public class AdjViewModel ...
}       //  namespace ZalTestApp
