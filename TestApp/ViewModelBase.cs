﻿/*
    Based on Jeremy Alles' blog entry: 
        http://www.japf.fr/2009/03/thinking-with-mvvm-data-templates-contentcontrol/
    
        see also https://joshsmithonwpf.wordpress.com/
 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using MainLibManaged;

namespace ZalTestApp
{
    /// <summary>
    /// Base class for all ViewModel classes in the application. Provides support for 
    /// property changes notification. Original implementation by Josh Smith.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected MainModel m_MainModel = null;
        protected CLexemeManaged m_Lexeme = null;

        protected Guid m_InstanceId;

        protected Dictionary<string, FormsForGramHash> m_DictFormStatus = new Dictionary<string, FormsForGramHash>();
        //                      ^-- gram hash  --> list of possibly multiple forms for that hash

        public ViewModelBase()
        {
            m_InstanceId = Guid.NewGuid();
        }

        public string InstanceId
        {
            get
            {
                return m_InstanceId.ToString();
            }
        }

        bool m_bIsDerived = false;
        public bool IsDerived
        {
            get
            {
                return m_bIsDerived;
            }

            set
            {
                m_bIsDerived = value;
            }
        }

        protected void SetForm(string sHash, string sForm)
        {
            if (!m_DictFormStatus.ContainsKey(sHash))
            {
                return;
            }

            string sStressedForm = "";
            Helpers.AssignDiacritics(sForm, ref sStressedForm);

            var formsForHash = m_DictFormStatus[sHash];
            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return;
            }

            string sFormWithStressRemoved = "";
            Dictionary<int, EM_StressType> dctStressPositions;
            Helpers.StressMarksToPosList(sStressedForm, out sFormWithStressRemoved, out dctStressPositions);
            formsForHash.lstForms[iAt].WordFormManaged.SetWordForm(sFormWithStressRemoved);
            var eRet = formsForHash.lstForms[iAt].WordFormManaged.eSetStressPositions(dctStressPositions);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to modify stress positions.");
            }
            formsForHash.lstForms[iAt].IsUnsaved = true;
            OnPropertyChanged(sHash);
        }

        public void FormScrollUp(Object obj)
        {
            string sGramHash = obj as string;
            FormsForGramHash formsPerHash;
            if (m_DictFormStatus.TryGetValue(sGramHash, out formsPerHash))
            {
                if (formsPerHash.lstForms.Count > 1 && formsPerHash.iCurrentForm > 0)
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
                if (formsPerHash.lstForms.Count > 1 && formsPerHash.iCurrentForm < formsPerHash.lstForms.Count - 1)
                {
                    ++formsPerHash.iCurrentForm;
                    OnPropertyChanged(sGramHash);
                }
            }
        }

        public static string sDisplayHashToFormHash(string sDisplayHash, EM_Subparadigm eSubparadigm)
        {
            EM_PartOfSpeech ePartOfSpeech = Helpers.SubparadigmToPOS(eSubparadigm);
            if (EM_PartOfSpeech.POS_UNDEFINED == ePartOfSpeech)
            {
                MessageBox.Show(String.Format("Unable to determine part of speech for subparadigm {0}", eSubparadigm));
                return "";
            }

            List<string> lstDisplayPropNames = null;
            List<string> lstPropNames = null;
            string sFormHash = null;

            switch (ePartOfSpeech)
            {
                case EM_PartOfSpeech.POS_NOUN:
                    lstDisplayPropNames = Helpers.m_listPropNamesNoun;
                    lstPropNames = Helpers.m_listPropNamesNoun;
                    break;

                case EM_PartOfSpeech.POS_PRONOUN:
                    lstDisplayPropNames = Helpers.m_listPropNamesNoun;
                    lstPropNames = Helpers.m_listPropNamesPronoun;
                    break;

                case EM_PartOfSpeech.POS_NUM:
                    lstDisplayPropNames = Helpers.m_listPropNamesNoun;
                    lstPropNames = Helpers.m_listPropNamesNumeral;
                    break;

                case EM_PartOfSpeech.POS_ADJ:
                    lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                    lstPropNames = Helpers.m_listPropNamesAdj;
                    break;

                case EM_PartOfSpeech.POS_PRONOUN_ADJ:
                    lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                    lstPropNames = Helpers.m_listPropNamesPronAdj;
                    break;

                case EM_PartOfSpeech.POS_NUM_ADJ:
                    lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                    lstPropNames = Helpers.m_listPropNamesNumAdj;
                    break;

                case EM_PartOfSpeech.POS_VERB:

                    switch (eSubparadigm)
                    {
                        // If form hash points to a participle then form hash == display hash
                        // this only happens on initial verb forms page
                        case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                            if ("PPresA_M_Sg_N" == sDisplayHash)
                            {
                                sFormHash = sDisplayHash;
                            }
                            else
                            {
                                lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                                lstPropNames = Helpers.m_listPropNamesPartPresAct;
                            }
                            break;

                        case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                            if ("PPresPL_M_Sg_N" == sDisplayHash)
                            {
                                sFormHash = sDisplayHash;
                            }
                            else
                            {
                                lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                                lstPropNames = Helpers.m_listPropNamesPartPresPass;
                            }
                            break;

                        case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                            if ("PPastA_M_Sg_N" == sDisplayHash)
                            {
                                sFormHash = sDisplayHash;
                            }
                            else
                            {
                                lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                                lstPropNames = Helpers.m_listPropNamesPartPastAct;
                            }
                            break;

                        case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                            if ("PPastPL_M_Sg_N" == sDisplayHash)
                            {
                                sFormHash = sDisplayHash;
                            }
                            else
                            {
                                lstDisplayPropNames = Helpers.m_listPropNamesAdj;
                                lstPropNames = Helpers.m_listPropNamesPartPastPass;
                            }
                            break;

                        case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                        case EM_Subparadigm.SUBPARADIGM_PAST_TENSE:
                        case EM_Subparadigm.SUBPARADIGM_IMPERATIVE:
                        case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT:
                        case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                        case EM_Subparadigm.SUBPARADIGM_ASPECT_PAIR:
                            sFormHash = sDisplayHash;
                            break;

                        default:
                            MessageBox.Show(String.Format("Subparadigm {0} was not recognized.", eSubparadigm.ToString()));
                            break;
                    }       //  switch (eSubparadigm)

                    break;

                default:
                    MessageBox.Show(String.Format("Subparadigm {0} was not recognized.", eSubparadigm.ToString()));
                    break;

            }       //  switch (ePartOfSpeech)

            if (lstDisplayPropNames != null && lstDisplayPropNames.Exists(element => element == sDisplayHash))
            {
                int iKeyIdx = lstDisplayPropNames.IndexOf(sDisplayHash);
                if (iKeyIdx < 0)
                {
                    MessageBox.Show(String.Format("Display hash {0} not recognized.", sDisplayHash));
                    return "";
                }

                if (iKeyIdx < lstPropNames.Count)
                {
                    sFormHash = lstPropNames[iKeyIdx];
                }
                else
                {
                    return "";
                }
            }

            return sFormHash;

        }       //  sDisplayHashToFormHash()

        public bool HasComments(string sDisplayHash, EM_Subparadigm eSubparadigm)
        {
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, eSubparadigm);
            FormsForGramHash formsForHash = null;
            if (!m_DictFormStatus.TryGetValue(sFormHash, out formsForHash))
            {
                return false;
            }

            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.lstForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return false;
            }

            string sLeadComment = formsForHash.lstForms[iAt].WordFormManaged.sLeadComment();
            string sTrailingComment = formsForHash.lstForms[iAt].WordFormManaged.sTrailingComment();

            if (0 == sLeadComment.Length && 0 == sTrailingComment.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Warns the developer if this object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected EM_ReturnCode CreateIrregularWordForm(string sGramHash,
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
            //            eRet = wf.eSetIrregularStressPositions(dictStressPos);
            eRet = wf.eSaveIrregularForm();
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                var msg = "Internal error: unable to save irregular form.";
                MessageBox.Show(msg);
                return eRet;
            }

            return EM_ReturnCode.H_NO_ERROR;

        }       //  CreateIrregularWordForm()

        protected void SaveForms(Object obj)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            foreach (KeyValuePair<string, FormsForGramHash> entry in m_DictFormStatus)
            {
                FormsForGramHash formsPerHash = entry.Value;
                if (formsPerHash.lstForms.Count < 1)
                {
                    MessageBox.Show("Internal error: no forms for {0}.", entry.Key);
                    continue;
                }

//                string sGramHash = sDisplayHashToFormHash(entry.Key);

                bool isVariant = false;
                foreach (var fd in formsPerHash.lstForms)
                {
                    if (!fd.IsUnsaved)
                    {
                        continue;
                    }

                    try
                    {
                        fd.WordFormManaged.SetIsVariant(isVariant);
                        isVariant = true;       // for subsequent forms if they exist

                        // TODO: comments

                        eRet = fd.WordFormManaged.eSaveIrregularForm();
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            var msg = "Internal error: unable to save word form.";
                            MessageBox.Show(msg);
                            continue;
                        }
                        fd.IsUnsaved = false;
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format("Exception: {0}.", ex.Message);
                        MessageBox.Show(msg);
                        return;
                    }
                }
            }       // foreach()

            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                MessageBox.Show("Формы сохранены.");
            }
            else
            {
                MessageBox.Show("Ошибки при записи форм в базу данных.");
            }


        }       //  SaveForms()

        ////////////////////////////////////////////////////////
        /*
        //       public delegate bool ChangedFormHandler();
                public struct FormDescriptor
                {
                    public List<string> listForms { get; set; }
                    public List<Tuple<string, string>> listComments { get; set; }   // leading / trailing comments, irregular form only
                    //            public bool bCanEdit { get; set; }
        //            public ChangedFormHandler handler { get; set; }
                    public bool IsIrregular { get; set; }
                    public bool IsEdited { get; set; }
                    public int iCurrentFormNumber { get; set; }     // 0 unless we have more than 1 form per gramm hash

        //            public FormDescriptor(List<string> forms, List<Tuple<string, string>> comments, bool bIrregular, bool bEdited, ChangedFormHandler h)
                    public FormDescriptor(List<string> forms, List<Tuple<string, string>> comments, bool bIrregular, bool bEdited)
                    {
                        listForms = forms;
                        listComments = comments;
                        IsIrregular = bIrregular;
                        IsEdited = bEdited;
        //                handler = h;
                        iCurrentFormNumber = 0;
                    }
                }
        */
    }

    public class FormDescriptor
    {
        private string m_sStressedWordform;
        public string StressedWordform
        {
            get
            {
                string sWordformWithStress = m_WordformData.sWordForm();
                Helpers.MarkStress(ref sWordformWithStress, m_WordformData);
                return sWordformWithStress;
            }
        }

        private bool m_bIsUnsaved;
        public bool IsUnsaved 
        {
            get
            {
                return m_bIsUnsaved;
            }
            set 
            {
                m_bIsUnsaved = value;
            }
        }

        private CWordFormManaged m_WordformData;
        public CWordFormManaged WordFormManaged 
        {
            get
            {
                return m_WordformData;
            }

            set
            {
                m_WordformData = value;
            }
        }

        public FormDescriptor()
        {
//            m_sStressedWordform = "";
            m_bIsUnsaved = false;
        }

        public bool AddWordformObject(CWordFormManaged wfObj)
        {
            if (null == wfObj)
            {
                var msg = "Internal error: word form instance is null.";
                MessageBox.Show(msg);
                return false;
            }

            m_WordformData = wfObj;

            return true;
        }
    }    
    
    // List of forms for given gram hash + sequential number of currently displayed form
    public class FormsForGramHash
    {
        public List<FormDescriptor> lstForms;
        public int iCurrentForm;

        public FormsForGramHash()
        {
            lstForms = new List<FormDescriptor>();
            iCurrentForm = -1;
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> m_Action;
        public RelayCommand(Action<object> action)
        {
            m_Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                m_Action(parameter);
            }
            else
            {
                m_Action("");
            }
        }
    }

}
