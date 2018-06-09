﻿using System;
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                OnPropertyChanged("Noun_Sg_Part");
            }
        }

        public string Noun_Sg_L
        {
            get
            {
                var text = Helpers.sListToCommaSeparatedString(m_DictFormStatus["Noun_Sg_L"].listForms);
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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
                Helpers.AssignDiacritics(text, ref text);
                return text;
            }
            set
            {
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

//        private Dictionary<string, ChangedFormHandler> m_ChangedFormHandlers = new Dictionary<string, ChangedFormHandler>();

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

            return;

        }   //  InitFormHandlers()

        #endregion

        public NounViewModel(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            EditCommand = new RelayCommand(new Action<object>(EditForm));

            m_MainModel = m;

            InitFormHandlers(lexeme);

            PropertyChanged += nounViewModel_PropertyChanged;

            EditEnabled = true;

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
