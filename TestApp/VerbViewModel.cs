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

        Dictionary<string, FormDescriptor> m_DictFormStatus = new Dictionary<string, FormDescriptor>()
        {
            {  "Infinitive", new FormDescriptor(null, false, null) },
            {  "Pres_Sg_1", new FormDescriptor(null, false, null) },
            {  "Pres_Sg_2", new FormDescriptor(null, false, null) },
            {  "Pres_Sg_3", new FormDescriptor(null, false, null) },
            {  "Pres_Pl_1", new FormDescriptor(null, false, null) },
            {  "Pres_Pl_2", new FormDescriptor(null, false, null) },
            {  "Pres_Pl_3", new FormDescriptor(null, false, null) },
            {  "Past_M", new FormDescriptor(null, false, null) },
            {  "Past_F", new FormDescriptor(null, false, null) },
            {  "Past_N", new FormDescriptor(null, false, null) },
            {  "Past_Pl", new FormDescriptor(null, false, null) },
            {  "Impv_Sg_2", new FormDescriptor(null, false, null) },
            {  "Impv_Pl_2", new FormDescriptor(null, false, null) },
            {  "PPresA_M_Sg_N", new FormDescriptor(null, false, null) },
            {  "VAdvPres", new FormDescriptor(null, false, null) },
            {  "PPastA_M_Sg_N", new FormDescriptor(null, false, null) },
            {  "VAdvPast", new FormDescriptor(null, false, null) },
            {  "PPresPL_M_Sg_N", new FormDescriptor(null, false, null) },
            {  "PPastPL_M_Sg_N", new FormDescriptor(null, false, null) }
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
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Infinitive"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Infinitive"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Infinitive"] = fd;
                OnPropertyChanged("Infinitive");
            }
        }

        public bool Infinitive_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Infinitive"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Infinitive"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Infinitive"] = fd;
                OnPropertyChanged("Infinitive_IsReadOnly");
            }
        }

        #region Bindings_PresentTense

        public string Pres_Sg_1
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Sg_1"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_1"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Sg_1"] = fd;
                OnPropertyChanged("Pres_Sg_1");
            }
        }

        public bool Pres_Sg_1_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Sg_1"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_1"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Sg_1"] = fd;
                OnPropertyChanged("Pres_Sg_1_IsReadOnly");
            }
        }

        public string Pres_Sg_2
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Sg_2"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_2"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Sg_2"] = fd;
                OnPropertyChanged("Pres_Sg_2");
            }
        }

        public bool Pres_Sg_2_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Sg_2"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_2"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Sg_2"] = fd;
                OnPropertyChanged("Pres_Sg_2_IsReadOnly");
            }
        }

        public string Pres_Sg_3
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Sg_3"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_3"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Sg_3"] = fd;
                OnPropertyChanged("Pres_Sg_3");
            }
        }

        public bool Pres_Sg_3_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Sg_3"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Sg_3"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Sg_3"] = fd;
                OnPropertyChanged("Pres_Sg_3_IsReadOnly");
            }
        }

        public string Pres_Pl_1
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Pl_1"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_1"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Pl_1"] = fd;
                OnPropertyChanged("Pres_Pl_1");
            }
        }

        public bool Pres_Pl_1_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Pl_1"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_1"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Pl_1"] = fd;
                OnPropertyChanged("Pres_Pl_1_IsReadOnly");
            }
        }

        public string Pres_Pl_2
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Pl_2"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_2"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Pl_2"] = fd;
                OnPropertyChanged("Pres_Pl_2");
            }
        }

        public bool Pres_Pl_2_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Pl_2"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_2"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Pl_2"] = fd;
                OnPropertyChanged("Pres_Pl_2_IsReadOnly");
            }
        }

        public string Pres_Pl_3
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Pres_Pl_3"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_3"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Pres_Pl_3"] = fd;
                OnPropertyChanged("Pres_Pl_3");
            }
        }

        public bool Pres_Pl_3_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Pres_Pl_3"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Pres_Pl_3"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Pres_Pl_3"] = fd;
                OnPropertyChanged("Pres_Pl_3_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_PastTense
        public string Past_M
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Past_M"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Past_M"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Past_M"] = fd;
                OnPropertyChanged("Past_M");
            }
        }

        public bool Past_M_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Past_M"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Past_M"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Past_M"] = fd;
                OnPropertyChanged("Past_M_IsReadOnly");
            }
        }

        public string Past_F
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Past_F"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Past_F"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Past_F"] = fd;
                OnPropertyChanged("Past_F");
            }
        }

        public bool Past_F_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Past_F"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Past_F"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Past_F"] = fd;
                OnPropertyChanged("Past_F_IsReadOnly");
            }
        }

        public string Past_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Past_N"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Past_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Past_N"] = fd;
                OnPropertyChanged("Past_N");
            }
        }

        public bool Past_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Past_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Past_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Past_N"] = fd;
                OnPropertyChanged("Past_N_IsReadOnly");
            }
        }

        public string Past_Pl
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Past_Pl"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Past_Pl"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Past_Pl"] = fd;
                OnPropertyChanged("Past_Pl");
            }
        }

        public bool Past_Pl_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Past_Pl"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Past_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Past_N"] = fd;
                OnPropertyChanged("Past_N_IsReadOnly");
            }
        }
        #endregion

        #region Bindings_Imperative
        public string Impv_Sg_2
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Impv_Sg_2"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Impv_Sg_2"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Impv_Sg_2"] = fd;
                OnPropertyChanged("Impv_Sg_2");
            }
        }

        public bool Impv_Sg_2_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Impv_Sg_2"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Impv_Sg_2"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Impv_Sg_2"] = fd;
                OnPropertyChanged("Impv_Sg_2_IsReadOnly");
            }
        }

        public string Impv_Pl_2
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Impv_Pl_2"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["Impv_Pl_2"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Impv_Pl_2"] = fd;
                OnPropertyChanged("Impv_Pl_2");
            }
        }

        public bool Impv_Pl_2_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Impv_Pl_2"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Impv_Pl_2"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Impv_Pl_2"] = fd;
                OnPropertyChanged("Impv_Pl_2_IsReadOnly");
            }
        }

        #endregion

        #region Bindings_Non-Finite

        public string PPresA_M_Sg_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresA_M_Sg_N"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["PPresA_M_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["PPresA_M_Sg_N"] = fd;
                OnPropertyChanged("PPresA_M_Sg_N");
            }
        }

        public bool PPresA_M_Sg_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["PPresA_M_Sg_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["PPresA_M_Sg_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["PPresA_M_Sg_N"] = fd;
                OnPropertyChanged("PPresA_M_Sg_N_IsReadOnly");
            }
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
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastA_M_Sg_N"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["PPastA_M_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["PPastA_M_Sg_N"] = fd;
                OnPropertyChanged("PPastA_M_Sg_N");
            }
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
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPresPL_M_Sg_N"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["PPresPL_M_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["PPresPL_M_Sg_N"] = fd;
                OnPropertyChanged("PPresPL_M_Sg_N");
            }
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
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["PPastPL_M_Sg_N"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["PPastPL_M_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["PPastPL_M_Sg_N"] = fd;
                OnPropertyChanged("PPastPL_M_Sg_N");
            }
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
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["VAdvPres"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["VAdvPres"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["VAdvPres"] = fd;
                OnPropertyChanged("VAdvPres");
            }
        }

        public string VAdvPast
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["VAdvPast"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
                var fd = m_DictFormStatus["VAdvPast"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["VAdvPast"] = fd;
                OnPropertyChanged("VAdvPast");
            }
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

            foreach (var hash in hashes)
            {
                FormDescriptor fd = m_DictFormStatus[hash];
                List<string> listForms = null;
                m_MainModel.GetFormsByGramHash(sLexemeHash, hash, out listForms);
                fd.listForms = listForms;
                fd.handler = () =>
                {
                    if (!fd.bCanEdit)
                    {
                        return true;
                    }

                    var sFormString = Helpers.sListToCommaSeparatedString(fd.listForms);
                    Helpers.AssignDiacritics(sFormString, ref sFormString);

                    OnPropertyChanged(hash);
                    return true;
                };

                m_DictFormStatus[hash] = fd;
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
                PPresPExists = PPastPL_M_Sg_N.Length > 0;
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
