/*
    Based on Jeremy Alles' blog entry: 
        http://www.japf.fr/2009/03/thinking-with-mvvm-data-templates-contentcontrol/
    
        see also https://joshsmithonwpf.wordpress.com/
 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Linq;
using MainLibManaged;

namespace ZalTestApp
{
    public enum ECellStatus { Normal, Missing, Difficult, Assumed };

    public sealed class StyleConverterCell : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            string uri = sAssemblyName + ";component/Skins.xaml";

            ResourceDictionary resourceDictionary =
                (ResourceDictionary)Application.LoadComponent(new System.Uri(uri, System.UriKind.Relative));

            if (ECellStatus.Normal == (ECellStatus)value)
            {
                return resourceDictionary["FormText"] as Style;
            }
            else if (ECellStatus.Missing == (ECellStatus)value)
            {
                return resourceDictionary["EmptyFormTextBox"] as Style;
            }
            else if (ECellStatus.Difficult == (ECellStatus)value)
            {
                return resourceDictionary["DifficultFormTextBox"] as Style;
            }
            else if (ECellStatus.Assumed == (ECellStatus)value)
            {
                return resourceDictionary["AssumedFormTextBox"] as Style;
            }
            else
            {
                return resourceDictionary["FormText"] as Style;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

        public void OnGotFocus(string sHash, ref string sFormString)
        {
            FormsForGramHash formsForHash = null;
            if (m_DictFormStatus.TryGetValue(sHash, out formsForHash))
            {
                sFormString = "";
                foreach (var formDescriptor in formsForHash.lstForms)
                {
                    if (sFormString.Length > 0)
                    {
                        sFormString += "; ";
                    }
                    var sFormattedWord = formDescriptor.StressedWordform;
                    Helpers.RestoreInlineStressMarks(ref sFormattedWord);
                    sFormString += sFormattedWord;
                }
            }
        }

        public void SetForm(string sDisplayHash, string sCellContents)
        {
            if (m_Lexeme.bSpryazhSm())
            {
                MessageBox.Show("Глаголы с пометой \"спряж. см.\" нельзя редактировать.");
                OnPropertyChanged(sDisplayHash);
                OnPropertyChanged(sDisplayHash + "_HasMultipleForms");
                return;
            }

            var sFormHash = sDisplayHashToFormHash(sDisplayHash, m_Lexeme.ePartOfSpeech());

            FormsForGramHash formsForHash = null; 
            if (m_DictFormStatus.TryGetValue(sDisplayHash, out formsForHash))
            {
//                formsForHash.lstForms.Clear();
            }
            else
            {
                formsForHash = new FormsForGramHash();
                formsForHash.iCurrentForm = 0;
                m_DictFormStatus[sDisplayHash] = formsForHash;
            }

            string sOldForms = "";
            foreach (var form in formsForHash.lstForms)
            {
                if (sOldForms.Length > 0)
                {
                    sOldForms += "; ";
                }

                string sForm = form.StressedWordform;
                Helpers.RestoreInlineStressMarks(ref sForm);
                sOldForms += sForm;
            }

            if (sCellContents == sOldForms)
            {
                OnPropertyChanged(sDisplayHash);
                OnPropertyChanged(sDisplayHash + "_HasMultipleForms");
                return;
            }

            string sMsgSave = "Сохранить нерегулярную форму?";
            string sMsgRestore = "Восстановить регулярную форму?";
            var mbRetSave = MessageBox.Show(String.Format("{0}", (sCellContents.Length > 0) ? sMsgSave : sMsgRestore),
                "", MessageBoxButton.YesNo);
            if (MessageBoxResult.No == mbRetSave)
            {
                sCellContents = sOldForms;
                OnPropertyChanged(sDisplayHash);
                OnPropertyChanged(sDisplayHash + "_HasMultipleForms");
                return;
            }

            formsForHash.lstForms.Clear();

            char[] arrSeparators = { ';' };
            List<string> lstWordForms = new List<string>(sCellContents.Split(arrSeparators, StringSplitOptions.RemoveEmptyEntries));
            formsForHash.lstForms.Clear();
            bool bIsVariant = false;
            if (0 == lstWordForms.Count)
            {
//                var mbRetDel = MessageBox.Show("Восстановить регулярную форму?", "Отмена", MessageBoxButton.YesNo);
//                if (MessageBoxResult.Yes == mbRetDel)
//                {
                    var eDel = m_Lexeme.eDeleteIrregularForm(sFormHash);
                    if (eDel != EM_ReturnCode.H_NO_ERROR)
                    {
                        MessageBox.Show("Ошибки при удалении формы.");
                    }
                    OnPropertyChanged(sDisplayHash);
                    OnPropertyChanged(sDisplayHash + "_HasMultipleForms");
                    return;
//                }
            }

            var eRet = m_Lexeme.eRemoveWordForms(sFormHash);
            //                if (eRet != EM_ReturnCode.H_NO_ERROR)
            //                {
            //                    MessageBox.Show(String.Format("Internal error: failed to remove forms for {0}.", sFormHash));
            //                    return;
            //                }

            foreach (var item in lstWordForms)
            {
                var sForm = item.Trim();
                string sComment = "";
                var iCommentOffset = sForm.IndexOf('(');
                if (iCommentOffset >= 0)
                {
                    var regex = new Regex(@"\(.+\)");
                    MatchCollection matches = regex.Matches(sForm.Substring(iCommentOffset));
                    if (matches.Count > 0)
                    {
                        sForm = sForm.Substring(0, iCommentOffset);
                        sComment = matches[0].Value;
                    }
                }
                string sStressedForm = "";
                Helpers.AssignDiacritics(sForm, ref sStressedForm);
                var fd = new FormDescriptor();
                CWordFormManaged wf = null;
                CreateIrregularWordForm(sFormHash, sStressedForm, sComment, ref wf);
                wf.SetIsVariant(bIsVariant);
                fd.WordFormManaged = wf;
                fd.IsUnsaved = true;
                formsForHash.lstForms.Add(fd);

                m_Lexeme.AddWordForm(ref wf);

                eRet = m_Lexeme.eSaveIrregularForms(sFormHash);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show(String.Format("Internal error: failed to save forms for {0}.", sFormHash));
                    return;
                }

                OnPropertyChanged(sDisplayHash);
                OnPropertyChanged(sDisplayHash+"_HasMultipleForms");
            }
        }       //  SetForm()

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

        public static string sDisplayHashToFormHash(string sDisplayHash, 
                                                    EM_PartOfSpeech ePartOfSpeech, 
                                                    EM_Subparadigm eSubparadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED)
        {
            if (EM_PartOfSpeech.POS_UNDEFINED == ePartOfSpeech)
            {
                MessageBox.Show(String.Format("Part of speech {0} not recognized.", ePartOfSpeech));
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
                    EM_PartOfSpeech ePOS = EM_PartOfSpeech.POS_UNDEFINED;
                    EM_Subparadigm eSP = eSubparadigm;
                    if (EM_Subparadigm.SUBPARADIGM_UNDEFINED == eSP)
                    {
                        var eRet = Helpers.eGramHashToSubparadigm(sDisplayHash, ref ePOS, ref eSP);
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            MessageBox.Show(String.Format("Display hash {0} was not recognized.", sDisplayHash));
                            return sDisplayHash;
                        }
                    }

                    switch (eSP)
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
            return false;

// ***                   CURRENTLY DISABLED                   ***

            /*
            string sFormHash = sDisplayHashToFormHash(sDisplayHash, m_Lexeme.ePartOfSpeech(), eSubparadigm);
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
            */
        }

        public ECellStatus GetCellStatus(string sFormHash)
        {
            string sLexemeHash = m_Lexeme.sParadigmHash();
            if (m_MainModel.bIsMissing(sLexemeHash, sFormHash))
            {
                return ECellStatus.Missing;
            }
            else if (m_MainModel.bIsDifficult(sLexemeHash, sFormHash))
            {
                return ECellStatus.Difficult;
            }
            else if (m_MainModel.bIsAssumed(sLexemeHash, sFormHash))
            {
                return ECellStatus.Assumed;
            }

            return ECellStatus.Normal;
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

        protected EM_ReturnCode CreateIrregularWordForm(string sDisplayHash,
                                                        string sForm,
                                                        string sComment,
                                                        ref CWordFormManaged wf)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            //            var sGramHash = sDisplayHashToFormHash(sDisplayHash, 
            var sGramHash = sDisplayHash;

            eRet = m_Lexeme.eCreateWordForm(ref wf);
            if (EM_ReturnCode.H_NO_ERROR != eRet)
            {
                MessageBox.Show("Unable to create a word form.");
                return eRet;
            }

            wf.SetPos(m_Lexeme.ePartOfSpeech());
            EM_PartOfSpeech eIgnoreMe = EM_PartOfSpeech.POS_UNDEFINED;
            EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            eRet = Helpers.eGramHashToSubparadigm(sGramHash, ref eIgnoreMe, ref eSp);
            wf.SetSubparadigm(eSp);

            switch (eSp)
            {
                case EM_Subparadigm.SUBPARADIGM_NOUN:
                case EM_Subparadigm.SUBPARADIGM_NUM:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN:
                {
                    EM_Case eCase = EM_Case.CASE_UNDEFINED;
                    EM_Animacy eIgnoreAnimacy = EM_Animacy.ANIM_UNDEFINED;
                    eRet = Helpers.eGramHashToCase(sGramHash, ref eCase, ref eIgnoreAnimacy);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetCase(eCase);
                    }
                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetNumber(eNumber);
                    }
                    break;
                }

                case EM_Subparadigm.SUBPARADIGM_LONG_ADJ:
                case EM_Subparadigm.SUBPARADIGM_NUM_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    {
                        EM_Gender eGender = EM_Gender.GENDER_UNDEFINED;
                    eRet = Helpers.eGramHashToGender(sGramHash, ref eGender);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetGender(eGender);
                    }

                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetNumber(eNumber);
                    }

                    EM_Case eCase = EM_Case.CASE_UNDEFINED;
                    EM_Animacy eAnimacy = EM_Animacy.ANIM_UNDEFINED;
                    eRet = Helpers.eGramHashToCase(sGramHash, ref eCase, ref eAnimacy);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetCase(eCase);
                        wf.SetAnimacy(eAnimacy);
                    }
                    break;                
                }

                case EM_Subparadigm.SUBPARADIGM_SHORT_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                {
                    EM_Gender eGender = EM_Gender.GENDER_UNDEFINED;
                    eRet = Helpers.eGramHashToGender(sGramHash, ref eGender);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetGender(eGender);
                    }

                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetNumber(eNumber);
                    }
                    break;
                }

                case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                case EM_Subparadigm.SUBPARADIGM_IMPERATIVE:
                {
                    EM_Person ePerson = EM_Person.PERSON_UNDEFINED;
                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToPersonAndNumber(sGramHash, ref ePerson, ref eNumber);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetPerson(ePerson);
                        wf.SetNumber(eNumber);
                    }
                    break;
                }

                case EM_Subparadigm.SUBPARADIGM_PAST_TENSE:
                {
                    EM_Gender eGender = EM_Gender.GENDER_UNDEFINED;
                    eRet = Helpers.eGramHashToGender(sGramHash, ref eGender);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetGender(eGender);
                    }

                    EM_Number eNumber = EM_Number.NUM_UNDEFINED;
                    eRet = Helpers.eGramHashToNumber(sGramHash, ref eNumber);
                    if (EM_ReturnCode.H_NO_ERROR == eRet)
                    {
                        wf.SetNumber(eNumber);
                    }
                    break;
                }

                case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT:
                case EM_Subparadigm.SUBPARADIGM_COMPARATIVE:
                case EM_Subparadigm.SUBPARADIGM_INFINITIVE:
                {
                    break;
                }

                default:
                    MessageBox.Show("Illegal subparadigm value.");
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
            }

            string sOutForm = "";
            Dictionary<int, EM_StressType> dictStressPos;
            Helpers.StressMarksToSyllabicPosList(sForm, out sOutForm, out dictStressPos);
            wf.SetWordForm(sOutForm);
            wf.SetIrregular(true);
            eRet = wf.eSetStressPositions(dictStressPos);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                var msg = "Internal error: unable to assign stress positions.";
                MessageBox.Show(msg);
                return eRet;
            }

            if (sComment.Length > 0)
            {
                wf.SetTrailingComment(sComment);
            }

            return EM_ReturnCode.H_NO_ERROR;

        }       //  CreateIrregularWordForm()

        protected void SaveForms(Object obj)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            foreach (var entry in m_DictFormStatus)
            {
                var lstFormsForHash = entry.Value.lstForms;
                IEnumerable<FormDescriptor> select = lstFormsForHash.Where(form => form.IsUnsaved);
                if (select.Count() > 0)
                {
                    var sFormHash = sDisplayHashToFormHash(entry.Key, m_Lexeme.ePartOfSpeech());
                    eRet = m_Lexeme.eSaveIrregularForms(sFormHash);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        MessageBox.Show(String.Format("Internal error: failed to save forms for {0}.", entry.Key));
                    }
                }
            }

            foreach (var entry in m_DictFormStatus)
            {
                FormsForGramHash formsPerHash = entry.Value;
                if (formsPerHash.lstForms.Count < 1)
                {
//                    MessageBox.Show(String.Format("Internal error: no forms for {0}.", entry.Key));
                    continue;
                }

                //                string sGramHash = sDisplayHashToFormHash(entry.Key);

                foreach (var fd in formsPerHash.lstForms)
                {
                    fd.IsUnsaved = false;
                }
            }

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
