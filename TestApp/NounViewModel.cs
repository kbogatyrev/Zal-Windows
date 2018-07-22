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
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void EditButtonHandler();

        MainModel m_MainModel = null;
        CLexemeManaged m_Lexeme = null;

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

        Dictionary<string, List<string>> m_DictOriginalForms = new Dictionary<string, List<string>>();
 
        Dictionary<string, FormDescriptor> m_DictFormStatus = new Dictionary<string, FormDescriptor>()
        {
            {  "Noun_Sg_N", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_A", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_G", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_P", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_D", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_L", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_I", new FormDescriptor(null, false, null) },
            {  "Noun_Sg_Part", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_N", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_A", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_G", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_P", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_D", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_L", new FormDescriptor(null, false, null) },
            {  "Noun_Pl_I", new FormDescriptor(null, false, null) }
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

        #region Bindings

        private bool m_bReadOnly;
        public bool ReadOnly
        {
            get
            {
                return m_bReadOnly;
            }
            set
            {
                m_bReadOnly = value;
                OnPropertyChanged("ReadOnly");
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

        public string Noun_Sg_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_N"] = fd;
                OnPropertyChanged("Noun_Sg_N");
            }
        }

        public bool Noun_Sg_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_N"] = fd;
                OnPropertyChanged("Noun_Sg_N_IsReadOnly");
            }
        }

        public string Noun_Sg_A
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_A"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_A"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_A"] = fd;
                OnPropertyChanged("Noun_Sg_A");
            }
        }

        public bool Noun_Sg_A_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_A"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_A"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_A"] = fd;
                OnPropertyChanged("Noun_Sg_A_IsReadOnly");
            }
        }

        public string Noun_Sg_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_G"] = fd;
                OnPropertyChanged("Noun_Sg_G");
            }
        }

        public bool Noun_Sg_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_G"] = fd;
                OnPropertyChanged("Noun_Sg_G_IsReadOnly");
            }
        }

        public string Noun_Sg_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_P"] = fd;
                OnPropertyChanged("Noun_Sg_P");
            }
        }

        public bool Noun_Sg_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_P"] = fd;
                OnPropertyChanged("Noun_Sg_P_IsReadOnly");
            }
        }

        public string Noun_Sg_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_D"] = fd;
                OnPropertyChanged("Noun_Sg_D");
            }
        }

        public bool Noun_Sg_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_D"] = fd;
                OnPropertyChanged("Noun_Sg_D_IsReadOnly");
            }
        }

        public string Noun_Sg_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_I"] = fd;
                OnPropertyChanged("Noun_Sg_I");
            }
        }

        public bool Noun_Sg_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_I"] = fd;
                OnPropertyChanged("Noun_Sg_I_IsReadOnly");
            }
        }

        public string Noun_Sg_Part
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_Part"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_Part"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_Part"] = fd;
                OnPropertyChanged("Noun_Sg_Part");
            }
        }

        public bool Noun_Sg_Part_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_Part"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_Part"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_Part"] = fd;
                OnPropertyChanged("Noun_Sg_Part_IsReadOnly");
            }
        }

        public string Noun_Sg_L
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_L"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Sg_L"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Sg_L"] = fd;
                OnPropertyChanged("Noun_Sg_L");
            }
        }

        public bool Noun_Sg_L_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Sg_L"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Sg_L"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Sg_L"] = fd;
                OnPropertyChanged("Noun_Sg_L_IsReadOnly");
            }
        }

        private bool m_bIs_L2_optional = false;
        public bool Is_L2_optional
        {
            get
            {
                return m_bIs_L2_optional;
            }
            set
            {
                m_bIs_L2_optional = value;
                OnPropertyChanged("Is_L2_optional");
            }
        }

        private bool m_bIs_L2_optional_IsReadOnly = true;
        public bool IsL2_optional_IsReadOnly
        {
            get
            {
                return m_bIs_L2_optional_IsReadOnly;
            }
            set
            {
                m_bIs_L2_optional_IsReadOnly = value;
                OnPropertyChanged("Is_L2_optional_IsReadOnly");
            }
        }

        private string m_sNoun_Sg_L2_Prepositions;
        public string Noun_Sg_L2_Prepositions
        {
            get
            {
                return m_sNoun_Sg_L2_Prepositions;
            }
            set
            {
                m_sNoun_Sg_L2_Prepositions = value;
                OnPropertyChanged("Noun_Sg_L2_Prepositions");
            }
        }

        private bool m_bNoun_Sg_L2_Prepositions_IsReadOnly = true;
        public bool Noun_Sg_L2_Prepositions_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_L2_Prepositions_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_L2_Prepositions_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_L2_Prepositions_IsReadOnly");
            }
        }

        public string Noun_Pl_N
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_N"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_N"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_N"] = fd;
                OnPropertyChanged("Noun_Pl_N");
            }
        }

        public bool Noun_Pl_N_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_N"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_N"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_N"] = fd;
                OnPropertyChanged("Noun_Pl_N_IsReadOnly");
            }
        }

        public string Noun_Pl_A
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_A"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_A"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_A"] = fd;
                OnPropertyChanged("Noun_Pl_A");
            }
        }

        public bool Noun_Pl_A_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_A"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_A"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_A"] = fd;
                OnPropertyChanged("Noun_Pl_A_IsReadOnly");
            }
        }

        public string Noun_Pl_G
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_G"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_G"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_G"] = fd;
                OnPropertyChanged("Noun_Pl_G");
            }
        }

        public bool Noun_Pl_G_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_G"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_G"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_G"] = fd;
                OnPropertyChanged("Noun_Pl_G_IsReadOnly");
            }
        }

        public string Noun_Pl_P
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_P"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_P"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_P"] = fd;
                OnPropertyChanged("Noun_Pl_P");
            }
        }

        public bool Noun_Pl_P_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_P"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_P"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_P"] = fd;
                OnPropertyChanged("Noun_Pl_P_IsReadOnly");
            }
        }

        public string Noun_Pl_D
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_D"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_D"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_D"] = fd;
                OnPropertyChanged("Noun_Pl_D");
            }
        }

        public bool Noun_Pl_D_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_D"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_D"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_D"] = fd;
                OnPropertyChanged("Noun_Pl_D_IsReadOnly");
            }
        }

        public string Noun_Pl_I
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_I"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_I"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_I"] = fd;
                OnPropertyChanged("Noun_Pl_I");
            }
        }

        public bool Noun_Pl_I_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_I"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_I"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_I"] = fd;
                OnPropertyChanged("Noun_Pl_I_IsReadOnly");
            }
        }

        public string Noun_Pl_L
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Pl_L"].listForms);
                return text;
            }
            set
            {
                Helpers.AssignDiacritics(value, ref value);
                var fd = m_DictFormStatus["Noun_Pl_L"];
                fd.listForms = Helpers.CommaSeparatedStringToList(value);
                m_DictFormStatus["Noun_Pl_L"] = fd;
                OnPropertyChanged("Noun_Pl_L");
            }
        }

        public bool Noun_Pl_L_IsReadOnly
        {
            get
            {
                return !m_DictFormStatus["Noun_Pl_L"].bCanEdit;
            }
            set
            {
                var fd = m_DictFormStatus["Noun_Pl_L"];
                fd.bCanEdit = !value;
                m_DictFormStatus["Noun_Pl_L"] = fd;
                OnPropertyChanged("Noun_Pl_L_IsReadOnly");
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

        #region Property_Delegates

        private void InitFormHandlers(CLexemeManaged lexeme)
        {
            string sLexemeHash = lexeme.sHash();
            List<string> hashes = new List<string>(m_DictFormStatus.Keys);

            foreach (var sHash in hashes)
            {
                FormDescriptor fd = m_DictFormStatus[sHash];
                List<string> listForms = null;
                m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out listForms);
                fd.listForms = listForms;
                fd.handler = () =>
                {
                    FormDescriptor fd1 = m_DictFormStatus[sHash];
                    if (!fd1.bCanEdit)
                    {
                        return true;
                    }

                    var sFormString = Helpers.sListToCommaSeparatedString(fd1.listForms);
                    Helpers.AssignDiacritics(sFormString, ref sFormString);
//                    OnPropertyChanged(hash);
                    return true;
                };

                m_DictFormStatus[sHash] = fd;
                m_DictOriginalForms[sHash] = listForms;
            }

            return;

        }   //  InitFormHandlers()

        #endregion

        public NounViewModel(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            EditCommand = new RelayCommand(new Action<object>(EditForm));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));

            m_MainModel = m;

            InitFormHandlers(lexeme);

            PropertyChanged += nounViewModel_PropertyChanged;

            EditEnabled = true;

            m_Lexeme = lexeme;
        }

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
        }       //  EditForm()

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
 
        }       //  SaveForms()

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
    }       //  public class NounViewModel ...
}
