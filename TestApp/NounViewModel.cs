using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Linq;

namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void EditButtonHandler();

        MainModel m_MainModel = null;
        CLexemeManaged m_Lexeme = null;

/*
        private delegate bool ChangedFormHandler();
        struct FormDescriptor
        {
            public List<string> listForms { get; set; }
//            public bool bCanEdit { get; set; }
            public ChangedFormHandler handler { get; set; }
            public bool IsIrregular { get; set; }
            public bool IsEdited { get; set; }

            public FormDescriptor(List<string> list, bool bIrregular, bool bEdited, ChangedFormHandler h)
            {
                listForms = list;
                IsIrregular = bIrregular;
                IsEdited = bEdited;
                handler = h;
            }
        }
*/

        List<string> m_listPropNamesNoun = new List<string>()
        {
            "Noun_Sg_N", "Noun_Sg_A", "Noun_Sg_G", "Noun_Sg_P", "Noun_Sg_D", "Noun_Sg_L",
            "Noun_Sg_I", "Noun_Sg_Part", "Noun_Pl_N", "Noun_Pl_A", "Noun_Pl_G", "Noun_Pl_P",
            "Noun_Pl_D", "Noun_Pl_L", "Noun_Pl_I"
        };

        List<string> m_listPropNamesPronoun = new List<string>()
        {
            "Pronoun_Sg_N", "Pronoun_Sg_A", "Pronoun_Sg_G", "Pronoun_Sg_P", "Pronoun_Sg_D", "Pronoun_Sg_L",
            "Pronoun_Sg_I", "Pronoun_Sg_Part", "Pronoun_Pl_N", "Pronoun_Pl_A", "Pronoun_Pl_G", "Pronoun_Pl_P",
            "Pronoun_Pl_D", "Pronoun_Pl_L", "Pronoun_Pl_I"
        };

        List<string> m_listPropNamesNumeral = new List<string>()
        {
            "Numeral_Sg_N", "Numeral_Sg_A", "Numeral_Sg_G", "Numeral_Sg_P", "Numeral_Sg_D", "Numeral_Sg_L",
            "Numeral_Sg_I", "Numeral_Sg_Part", "Numeral_Pl_N", "Numeral_Pl_A", "Numeral_Pl_G", "Numeral_Pl_P",
            "Numeral_Pl_D", "Numeral_Pl_L", "Numeral_Pl_I"
        };

        Dictionary<string, List<string>> m_DictOriginalForms = new Dictionary<string, List<string>>();
        Dictionary<string, FormDescriptor> m_DictFormStatus = new Dictionary<string, FormDescriptor>();

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

        #endregion

        #region Bindings_Nouns

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
            fd.listForms = Helpers.CommaSeparatedStringToList(sForms);
            List<string> l = new List<string>();
            List<Tuple<string, string>> c = new List<Tuple<string, string>>();
            Helpers.CommaSeparatedStringToList(sForms, out l, out c);
            fd.listForms = l;
            fd.listComments = c;
            m_DictFormStatus[sHash] = fd;
        }

        public string Noun_Sg_N
        {
            get { return GetForms("Noun_Sg_N"); }
            set { SetForms("Noun_Sg_N", value); }
        }

        public string Noun_Sg_A
        {
            get { return GetForms("Noun_Sg_A"); }
            set { SetForms("Noun_Sg_A", value); }
        }

        public string Noun_Sg_G
        {
            get { return GetForms("Noun_Sg_G"); }
            set { SetForms("Noun_Sg_G", value); }
        }

        public string Noun_Sg_P
        {
            get { return GetForms("Noun_Sg_P"); }
            set { SetForms("Noun_Sg_P", value); }
        }

        public string Noun_Sg_D
        {
            get { return GetForms("Noun_Sg_D"); }
            set { SetForms("Noun_Sg_D", value); }
        }

        public string Noun_Sg_I
        {
            get { return GetForms("Noun_Sg_I"); }
            set { SetForms("Noun_Sg_I", value); }
        }

        public string Noun_Sg_Part
        {
            get { return GetForms("Noun_Sg_Part"); }
            set { SetForms("Noun_Sg_Part", value); }
        }

        public string Noun_Sg_L
        {
            get { return GetForms("Noun_Sg_L"); }
            set { SetForms("Noun_Sg_L", value); }
        }

        private bool m_bIs_L2_optional = false;
        public bool Is_L2_optional
        {
            get { return m_bIs_L2_optional; }
            set
            {
                m_bIs_L2_optional = value;
                OnPropertyChanged("Is_L2_optional");
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

        public string Noun_Pl_N
        {
            get { return GetForms("Noun_Pl_N"); }
            set { SetForms("Noun_Pl_N", value); }
        }

        public string Noun_Pl_A
        {
            get { return GetForms("Noun_Pl_A"); }
            set { SetForms("Noun_Pl_A", value); }
        }

        public string Noun_Pl_G
        {
            get { return GetForms("Noun_Pl_G"); }
            set { SetForms("Noun_Pl_G", value); }
        }

        public string Noun_Pl_P
        {
            get { return GetForms("Noun_Pl_P"); }
            set { SetForms("Noun_Pl_P", value); }
        }

        public string Noun_Pl_D
        {
            get { return GetForms("Noun_Pl_D"); }
            set { SetForms("Noun_Pl_D", value); }
        }

        public string Noun_Pl_I
        {
            get { return GetForms("Noun_Pl_I"); }
            set { SetForms("Noun_Pl_I", value); }
        }

        public string Noun_Pl_L
        {
            get { return GetForms("Noun_Pl_L"); }
            set { SetForms("Noun_Pl_L", value); }
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

        private EMark m_eNoun_Sg_L_Marks = EMark.None;
        public EMark Noun_Sg_L_Marks
        {
            get { return GetFormStatus("Noun_Sg_L"); }
            set { m_eNoun_Sg_L_Marks = value; OnPropertyChanged("Noun_Sg_L_Marks"); }
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
                        listKeys = m_listPropNamesNoun;
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        listKeys = m_listPropNamesPronoun;
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        listKeys = m_listPropNamesNumeral;
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
                    FormDescriptor fd = new FormDescriptor(null, null, false, false, null);
                    List<string> listForms = null;
                    m_MainModel.GetFormsByGramHash(sLexemeHash, sHash, out listForms);
                    fd.listForms = listForms;

                    if (m_MainModel.bIsIrregular(sLexemeHash, sHash))
                    {
                        List<Tuple<string, string>> listComments;
                        bool bRet = m_MainModel.GetFormComments(sLexemeHash, sHash, out listComments);
                        if (!bRet || listComments.Count != listForms.Count)
                        {
                            MessageBox.Show("Internal error: unable to retrieve from comments.");
                        }
                        fd.listComments = listComments;
                    }

                    fd.handler = () =>
                    {
                        FormDescriptor fd1 = m_DictFormStatus[sFormHashToDisplayHash(sHash)];
//                        if (!fd1.bCanEdit)
//                        {
//                            return true;
//                        }

                        var sFormString = Helpers.sListToCommaSeparatedString(fd1.listForms);
                        Helpers.AssignDiacritics(sFormString, ref sFormString);
                    //                    OnPropertyChanged(hash);
                    return true;
                    };

                    m_DictFormStatus[sFormHashToDisplayHash(sHash)] = fd;
                    m_DictOriginalForms[sFormHashToDisplayHash(sHash)] = listForms;
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
            EditCommand = new RelayCommand(new Action<object>(EditForm));
            EditFormCommentCommand = new RelayCommand(new Action<object>(EditFormComment));
            SaveFormsCommand = new RelayCommand(new Action<object>(SaveForms));

            m_MainModel = m;

            m_Lexeme = lexeme;

            InitFormHandlers();

            PropertyChanged += nounViewModel_PropertyChanged;

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

        public void EditFormComment(Object obj)
        {
            // Currently disabled
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
                    eSp = EM_Subparadigm.SUBPARADIGM_NUMERAL;
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
                        eRet = CreateIrregularWordForm(entry.Key, sForm, ref wf);
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
                }
            }       // for (int iAt = 0...

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

        private string sFormHashToDisplayHash(string sFormHash)
        {
            int iKeyIdx = -1;

            try
            {
                switch (m_Lexeme.ePartOfSpeech())
                {
                    case EM_PartOfSpeech.POS_NOUN:
                        iKeyIdx = m_listPropNamesNoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        iKeyIdx = m_listPropNamesPronoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        iKeyIdx = m_listPropNamesNumeral.IndexOf(sFormHash);
                        break;

                    default:
                        string sMsg = "Part of speech :";
                        sMsg += m_Lexeme.ePartOfSpeech();
                        sMsg += " was not recognized. ";
                        MessageBox.Show(sMsg);
                        return "";
                }

                return m_listPropNamesNoun[iKeyIdx];
            }
            catch (Exception ex)
            {
                string sMsg = "Exception while retrieving form hash: ";
                sMsg += ex.Message;
                MessageBox.Show(sMsg);
                return "";
            }

            return "";

        }       //  sFormHashToDisplayHash()

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

    }       //  public class NounViewModel ...
}
