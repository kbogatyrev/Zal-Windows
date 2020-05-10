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
using System.Windows;
using MainLibManaged;

namespace ZalTestApp
{
    /// <summary>
    /// Base class for all ViewModel classes in the application. Provides support for 
    /// property changes notification. Original implementation by Josh Smith.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public static List<string> m_listPropNamesNoun = new List<string>()
        {
            "Noun_Sg_N", "Noun_Sg_A", "Noun_Sg_G", "Noun_Sg_P", "Noun_Sg_D", "Noun_Sg_P2",
            "Noun_Sg_I", "Noun_Sg_Part", "Noun_Pl_N", "Noun_Pl_A", "Noun_Pl_G", "Noun_Pl_P",
            "Noun_Pl_D", "Noun_Pl_L", "Noun_Pl_I"
        };

        public static List<string> m_listPropNamesPronoun = new List<string>()
        {
            "Pronoun_Sg_N", "Pronoun_Sg_A", "Pronoun_Sg_G", "Pronoun_Sg_P", "Pronoun_Sg_D", "Pronoun_Sg_P2",
            "Pronoun_Sg_I", "Pronoun_Sg_Part", "Pronoun_Pl_N", "Pronoun_Pl_A", "Pronoun_Pl_G", "Pronoun_Pl_P",
            "Pronoun_Pl_D", "Pronoun_Pl_L", "Pronoun_Pl_I"
        };

        public static List<string> m_listPropNamesNumeral = new List<string>()
        {
            "Numeral_Sg_N", "Numeral_Sg_A", "Numeral_Sg_G", "Numeral_Sg_P", "Numeral_Sg_D", "Numeral_Sg_P2",
            "Numeral_Sg_I", "Numeral_Sg_Part", "Numeral_Pl_N", "Numeral_Pl_A", "Numeral_Pl_G", "Numeral_Pl_P",
            "Numeral_Pl_D", "Numeral_Pl_L", "Numeral_Pl_I"
        };

        public static List<string> m_listPropNamesAdj = new List<string>()
        {
            "AdjL_M_Sg_N", "AdjL_M_Sg_A_Inanim", "AdjL_M_Sg_A_Anim", "AdjL_M_Sg_G", "AdjL_M_Sg_P", "AdjL_M_Sg_D",
            "AdjL_M_Sg_I", "AdjL_F_Sg_N", "AdjL_F_Sg_A", "AdjL_F_Sg_G", "AdjL_F_Sg_P", "AdjL_F_Sg_D", "AdjL_F_Sg_I",
            "AdjL_N_Sg_N", "AdjL_N_Sg_A", "AdjL_N_Sg_G", "AdjL_N_Sg_P", "AdjL_N_Sg_D", "AdjL_N_Sg_I", "AdjL_Pl_N",
            "AdjL_Pl_A_Inanim", "AdjL_Pl_A_Anim", "AdjL_Pl_G", "AdjL_Pl_P", "AdjL_Pl_D", "AdjL_Pl_I",
            "AdjS_M", "AdjS_F", "AdjS_N", "AdjS_Pl", "AdjComp"
        };

        public static List<string> m_listPropNamesPronAdj = new List<string>()
        {
            "PronAdj_M_Sg_N", "PronAdj_M_Sg_A_Inanim", "PronAdj_M_Sg_A_Anim", "PronAdj_M_Sg_G", "PronAdj_M_Sg_P", "PronAdj_M_Sg_D",
            "PronAdj_M_Sg_I", "PronAdj_F_Sg_N", "PronAdj_F_Sg_A", "PronAdj_F_Sg_G", "PronAdj_F_Sg_P", "PronAdj_F_Sg_D", "PronAdj_F_Sg_I",
            "PronAdj_N_Sg_N", "PronAdj_N_Sg_A", "PronAdj_N_Sg_G", "PronAdj_N_Sg_P", "PronAdj_N_Sg_D", "PronAdj_N_Sg_I", "PronAdj_Pl_N",
            "PronAdj_Pl_A_Inanim", "PronAdj_Pl_A_Anim",  "PronAdj_Pl_G", "PronAdj_Pl_P", "PronAdj_Pl_D", "PronAdj_Pl_I"
        };

        public static List<string> m_listPropNamesNumAdj = new List<string>()
        {
            "NumAdj_M_Sg_N", "NumAdj_M_Sg_A_Inanim", "NumAdj_M_Sg_A_Anim", "NumAdj_M_Sg_G", "NumAdj_M_Sg_P", "NumAdj_M_Sg_D",
            "NumAdj_M_Sg_I", "NumAdj_F_Sg_N", "NumAdj_F_Sg_A", "NumAdj_F_Sg_G", "NumAdj_F_Sg_P", "NumAdj_F_Sg_D", "NumAdj_F_Sg_I",
            "NumAdj_N_Sg_N", "NumAdj_N_Sg_A", "NumAdj_N_Sg_G", "NumAdj_N_Sg_P", "NumAdj_N_Sg_D", "NumAdj_N_Sg_I", "NumAdj_Pl_N",
            "NumAdj_Pl_A_Inanim", "NumAdj_Pl_A_Anim",  "NumAdj_Pl_G", "NumAdj_Pl_P", "NumAdj_Pl_D", "NumAdj_Pl_I"
        };

        public static List<string> m_listPropNamesPartPresAct = new List<string>()
        {
            "PPresA_M_Sg_N", "PPresA_M_Sg_A_Inanim", "PPresA_M_Sg_A_Anim", "PPresA_M_Sg_G", "PPresA_M_Sg_P", "PPresA_M_Sg_D",
            "PPresA_M_Sg_I", "PPresA_F_Sg_N", "PPresA_F_Sg_A", "PPresA_F_Sg_G", "PPresA_F_Sg_P", "PPresA_F_Sg_D", "PPresA_F_Sg_I",
            "PPresA_N_Sg_N", "PPresA_N_Sg_A", "PPresA_N_Sg_G", "PPresA_N_Sg_P", "PPresA_N_Sg_D", "PPresA_N_Sg_I", "PPresA_Pl_N",
            "PPresA_Pl_A_Inanim", "PPresA_Pl_A_Anim", "PPresA_Pl_G", "PPresA_Pl_P", "PPresA_Pl_D", "PPresA_Pl_I"
        };

        public static List<string> m_listPropNamesPartPastAct = new List<string>()
        {
            "PPastA_M_Sg_N", "PPastA_M_Sg_A_Inanim", "PPastA_M_Sg_A_Anim", "PPastA_M_Sg_G", "PPastA_M_Sg_P", "PPastA_M_Sg_D",
            "PPastA_M_Sg_I", "PPastA_F_Sg_N", "PPastA_F_Sg_A", "PPastA_F_Sg_G", "PPastA_F_Sg_P", "PPastA_F_Sg_D", "PPastA_F_Sg_I",
            "PPastA_N_Sg_N", "PPastA_N_Sg_A", "PPastA_N_Sg_G", "PPastA_N_Sg_P", "PPastA_N_Sg_D", "PPastA_N_Sg_I", "PPastA_Pl_N",
            "PPastA_Pl_A_Anim", "PPastA_Pl_A_Inanim", "PPastA_Pl_G", "PPastA_Pl_P", "PPastA_Pl_D", "PPastA_Pl_I"
        };

        public static List<string> m_listPropNamesPartPresPass = new List<string>()
        {
            "PPresPL_M_Sg_N", "PPresPL_M_Sg_A_Anim", "PPresPL_M_Sg_A_Inanim", "PPresPL_M_Sg_G", "PPresPL_M_Sg_P",
            "PPresPL_M_Sg_D", "PPresPL_M_Sg_I", "PPresPL_F_Sg_N", "PPresPL_F_Sg_A", "PPresPL_F_Sg_G", "PPresPL_F_Sg_P",
            "PPresPL_F_Sg_D", "PPresPL_F_Sg_I", "PPresPL_N_Sg_N", "PPresPL_N_Sg_A", "PPresPL_N_Sg_G", "PPresPL_N_Sg_P",
            "PPresPL_N_Sg_D", "PPresPL_N_Sg_I", "PPresPL_Pl_N", "PPresPL_Pl_A_Anim", "PPresPL_Pl_A_Inanim", "PPresPL_Pl_G",
            "PPresPL_Pl_P", "PPresPL_Pl_D", "PPresPL_Pl_I", "PPresPS_M", "PPresPS_F", "PPresPS_N", "PPresPS_Pl"
        };

        public static List<string> m_listPropNamesPartPastPass = new List<string>()
        {
            "PPastPL_M_Sg_N", "PPastPL_M_Sg_A_Anim", "PPastPL_M_Sg_A_Inanim", "PPastPL_M_Sg_G", "PPastPL_M_Sg_P",
            "PPastPL_M_Sg_D", "PPastPL_M_Sg_I", "PPastPL_F_Sg_N", "PPastPL_F_Sg_A", "PPastPL_F_Sg_G", "PPastPL_F_Sg_P",
            "PPastPL_F_Sg_D", "PPastPL_F_Sg_I", "PPastPL_N_Sg_N", "PPastPL_N_Sg_A", "PPastPL_N_Sg_G", "PPastPL_N_Sg_P",
            "PPastPL_N_Sg_D", "PPastPL_N_Sg_I", "PPastPL_Pl_N", "PPastPL_Pl_A_Anim", "PPastPL_Pl_A_Inanim", "PPastPL_Pl_G",
            "PPastPL_Pl_P", "PPastPL_Pl_D", "PPastPL_Pl_I", "PPastPS_M", "PPastPS_F", "PPastPS_N", "PPastPS_Pl"
        };

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

        protected string GetForm(string sHash)
        {
            //            string sFormHash = sDisplayHashToFormHash(sHash);
            string sFormHash = sHash;
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

        protected void SetForm(string sHash, string sForm)
        {
            if (!m_DictFormStatus.ContainsKey(sHash))
            {
                return;
            }

            Helpers.AssignDiacritics(sForm, ref sForm);

            var formsForHash = m_DictFormStatus[sHash];
            int iAt = formsForHash.iCurrentForm;
            if (iAt < 0 || iAt >= formsForHash.listForms.Count)
            {
                MessageBox.Show("Internal error: Illegal form index.");
                return;
            }

            formsForHash.listForms[iAt].sFormText = sForm;

        }

        protected string sFormHashToDisplayHash(string sFormHash, EM_Subparadigm eSubparadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED)
        {
            int iKeyIdx = -1;

            try
            {
                switch (m_Lexeme.ePartOfSpeech())
                {
                    // Nominal inflection
                    case EM_PartOfSpeech.POS_NOUN:
                        iKeyIdx = m_listPropNamesNoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_PRONOUN:
                        iKeyIdx = m_listPropNamesPronoun.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_NUM:
                        iKeyIdx = m_listPropNamesNumeral.IndexOf(sFormHash);
                        break;

                    case EM_PartOfSpeech.POS_ADJ:
                        if ("мс" == m_Lexeme.sInflectionType())
                        {
                            iKeyIdx = m_listPropNamesPronAdj.IndexOf(sFormHash);
                        }
                        else
                        {
                            iKeyIdx = m_listPropNamesAdj.IndexOf(sFormHash);
                        }

                        if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ)
                        {
                            iKeyIdx = m_listPropNamesPronAdj.IndexOf(sFormHash);
                        }
                        else if (m_Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
                        {
                            IsDerived = true;

                            switch (eSubparadigm)
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
                                    MessageBox.Show(string.Format("Part of speech {0} was not recognized", m_Lexeme.ePartOfSpeech()));
                                    return "";
                            }
                        }

                        break;
                    
                    default:
                        MessageBox.Show(string.Format("Part of speech {0} was not recognized", m_Lexeme.ePartOfSpeech()));
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

        protected string sDisplayHashToFormHash(string sDisplayHash)
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
            eRet = wf.eSetIrregularStressPositions(dictStressPos);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                var msg = "Internal error: unable to save stress positions";
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
                if (formsPerHash.listForms.Count < 1)
                {
                    MessageBox.Show("Internal error: no forms for {0}.", entry.Key);
                    continue;
                }

                // Purge all irregular forms with this gram hash from the DB
                string sGramHash = sDisplayHashToFormHash(entry.Key);
                eRet = m_Lexeme.eDeleteIrregularForm(sGramHash);
                if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_FALSE)
                {
                    var msg = "Internal error: unable to save wordform object";
                    MessageBox.Show(msg);
                    continue;
                }

                CWordFormManaged wf = null;
                bool isVariant = false;
                foreach (var descriptor in formsPerHash.listForms)
                {
                    try
                    {
                        string sForm = descriptor.sFormText;
                        eRet = CreateIrregularWordForm(sGramHash, sForm, ref wf);
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            var msg = "Internal error: unable to create word form object.";
                            MessageBox.Show(msg);
                            continue;
                        }

                        wf.SetIsVariant(isVariant);

                        isVariant = true;       // for subsequent forms if they exist

                        // TODO: comments

                        eRet = m_Lexeme.eSaveIrregularForm(wf.sGramHash(), ref wf);
                        if (eRet != EM_ReturnCode.H_NO_ERROR)
                        {
                            var msg = "Internal error: unable to save word form.";
                            MessageBox.Show(msg);
                            continue;
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

//            MessageBox.Show("Формы сохранены.");

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
        public string sFormText;
        public string sComment;                            // leading / trailing comments, irregular form only
        public bool HasTrailingComment;
        public bool IsIrregular;
        public bool IsEdited;

        public FormDescriptor()
        {
            sFormText = "";
            sComment = "";
            HasTrailingComment = false;
            IsIrregular = false;
            IsEdited = false;
        }
    }

    // List of forms for given gram hash + sequential number of currently displayed form
    public class FormsForGramHash
    {
        public List<FormDescriptor> listForms;
        public int iCurrentForm;

        public FormsForGramHash()
        {
            listForms = new List<FormDescriptor>();
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
