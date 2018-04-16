using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using MainLibManaged;
using System.Text;
using System.Collections.ObjectModel;

namespace ZalTestApp
{
    public class EnterLexemePropertiesViewModel : ViewModelBase
    {
        CLexemeManaged m_Lexeme;
        List<string> m_PropertiesChanged;
        HashSet<char> m_CyrillicAlphabet;
        char[] m_Vowels = { 'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
        char[] m_sStressMarks = { '/', '\\'};

        List<string> m_Errors;

        #region POS_String_to_POS_Enum

        public enum E_POS
        {
            POS_UNDEFINED = 0,
            POS_NOUN,
            POS_VERB,
            POS_ADJ,
            POS_PRONOUN,
            POS_PRONOUN_ADJ,
            POS_PRONOUN_PREDIC,
            POS_NUM,
            POS_NUM_ADJ,
            POS_ADV,
            POS_COMPAR,
            POS_PREDIC,
            POS_PREP,
            POS_CONJ,
            POS_PARTICLE,
            POS_INTERJ,     // вводн.
            POS_PARENTH,    // межд.
            POS_NULL,
            POS_COUNT
        };
        Dictionary<string, E_POS> m_StringToPos;

        #endregion

        #region Bindings

        private string m_sSourceForm;
        public string SourceForm
        {
            get
            {
                Helpers.ExtractStressMarks(ref m_sSourceForm);
                return m_sSourceForm;
            }
            set
            {
                if (value != m_sSourceForm)
                {
                    m_sSourceForm = value;
                }
                OnPropertyChanged("SourceForm");
            }
        }

        private string m_sVariant;
        public string Variant
        {
            get
            {
                return m_sVariant;
            }
            set
            {
                if (value != m_sVariant)
                {
                    m_sVariant = value;
                }
                OnPropertyChanged("Variant");
            }
        }

        private string m_sHeadwordComment;
        public string HeadwordComment
        {
            get
            {
                return m_sHeadwordComment;
            }
            set
            {
                if (value != m_sHeadwordComment)
                {
                    m_sHeadwordComment = value;
                }
                OnPropertyChanged("HeadwordComment");
            }
        }

        private string m_sRestrictedContexts;
        public string RestrictedContexts
        {
            get
            {
                return m_sRestrictedContexts;
            }
            set
            {
                if (value != m_sRestrictedContexts)
                {
                    m_sRestrictedContexts = value;
                }
                OnPropertyChanged("RestrictedContexts");
            }
        }

        private string m_sUsage;
        public string Usage
        {
            get
            {
                return m_sUsage;
            }
            set
            {
                if (value != m_sUsage)
                {
                    m_sUsage = value;
                }
                OnPropertyChanged("Usage");
            }
        }

        private string m_sMainSymbol;
        public string MainSymbol
        {
            get
            {
                return m_sMainSymbol;
            }
            set
            {
                if (value != m_sMainSymbol)
                {
                    m_sMainSymbol = value;
                }
                OnPropertyChanged("MainSymbol");
            }
        }

        private string m_sIndex;
        public string Index
        {
            get
            {
                return m_sIndex;
            }
            set
            {
                if (value != m_sIndex)
                {
                    m_sIndex = value;
                }
                OnPropertyChanged("Index");
            }
        }

        private string m_sStressType1;
        public string StressType1
        {
            get
            {
                return m_sStressType1;
            }
            set
            {
                if (value != m_sStressType1)
                {
                    m_sStressType1 = value;
                }
                OnPropertyChanged("StressType1");
            }
        }

        private string m_sStressType2;
        public string StressType2
        {
            get
            {
                return m_sStressType2;
            }
            set
            {
                if (value != m_sStressType2)
                {
                    m_sStressType2 = value;
                }
                OnPropertyChanged("StressType2");
            }
        }

        private string m_sNumberInCircle;
        public string NumberInCircle
        {
            get
            {
                return m_sNumberInCircle;
            }
            set
            {
                if (value != m_sNumberInCircle)
                {
                    m_sNumberInCircle = value;
                }
                OnPropertyChanged("NumberInCircle");
            }
        }

        private string m_sTriangle;
        public string Triangle
        {
            get
            {
                return m_sTriangle;
            }
            set
            {
                if (value != m_sTriangle)
                {
                    m_sTriangle = value;
                }
                OnPropertyChanged("Triangle");
            }
        }

        private string m_sFleetingVowel;
        public string FleetingVowel
        {
            get
            {
                return m_sFleetingVowel;
            }
            set
            {
                if (value != m_sFleetingVowel)
                {
                    m_sFleetingVowel = value;
                }
                OnPropertyChanged("FleetingVowel");
            }
        }

        private string m_sYoAlternation;
        public string YoAlternation
        {
            get
            {
                return m_sYoAlternation;
            }
            set
            {
                if (value != m_sYoAlternation)
                {
                    m_sYoAlternation = value;
                }
                OnPropertyChanged("YoAlternation");
            }
        }

        private string m_sOAlternation;
        public string OAlternation
        {
            get
            {
                return m_sOAlternation;
            }
            set
            {
                if (value != m_sOAlternation)
                {
                    m_sOAlternation = value;
                }
                OnPropertyChanged("OAlternation");
            }
        }

        private string m_sMinusSymbol;
        public string MinusSymbol
        {
            get
            {
                return m_sMinusSymbol;
            }
            set
            {
                if (value != m_sMinusSymbol)
                {
                    m_sMinusSymbol = value;
                }
                OnPropertyChanged("MinusSymbol");
            }
        }

        private string m_sXSymbol;
        public string XSymbol
        {
            get
            {
                return m_sXSymbol;
            }
            set
            {
                if (value != m_sXSymbol)
                {
                    m_sXSymbol = value;
                }
                OnPropertyChanged("XSymbol");
            }
        }

        private string m_sFramedXSymbol;
        public string FramedXSymbol
        {
            get
            {
                return m_sFramedXSymbol;
            }
            set
            {
                if (value != m_sFramedXSymbol)
                {
                    m_sFramedXSymbol = value;
                }
                OnPropertyChanged("FramedXSymbol");
            }
        }

        private string m_sTildeSymbol;
        public string TildeSymbol
        {
            get
            {
                return m_sTildeSymbol;
            }
            set
            {
                if (value != m_sTildeSymbol)
                {
                    m_sTildeSymbol = value;
                }
                OnPropertyChanged("TildeSymbol");
            }
        }

        private string m_sGen2;
        public string Gen2
        {
            get
            {
                return m_sGen2;
            }
            set
            {
                if (value != m_sGen2)
                {
                    m_sGen2 = value;
                }
                OnPropertyChanged("Gen2");
            }
        }

        private string m_sLoc2;
        public string Loc2
        {
            get
            {
                return m_sLoc2;
            }
            set
            {
                if (value != m_sLoc2)
                {
                    m_sLoc2 = value;
                }
                OnPropertyChanged("Loc2");
            }
        }

        private string m_sLoc2Preposition;
        public string Loc2Preposition
        {
            get
            {
                return m_sLoc2Preposition;
            }
            set
            {
                if (value != m_sLoc2Preposition)
                {
                    m_sLoc2Preposition = value;
                }
                OnPropertyChanged("Loc2Preposition");
            }
        }

        private string m_sVerbStemAlternation;
        public string VerbStemAlternation
        {
            get
            {
                return m_sVerbStemAlternation;
            }
            set
            {
                if (value != m_sVerbStemAlternation)
                {
                    m_sVerbStemAlternation = value;
                }
                OnPropertyChanged("VerbStemAlternation");
            }
        }

        private ObservableCollection<string> m_YesNoValues;
        public ObservableCollection<string> YesNoValues
        {
            get
            {
                return m_YesNoValues;
            }
            set
            {
                if (value != m_YesNoValues)
                {
                    m_YesNoValues = value;
                }
                OnPropertyChanged("YesNoValues");
            }
        }

        private ICommand m_OKCommand;
        public ICommand OKCommand
        {
            get
            {
                return m_OKCommand;
            }
            set
            {
                m_OKCommand = value;
            }
        }

        private ICommand m_CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return m_CancelCommand;
            }
            set
            {
                m_CancelCommand = value;
            }
        }

        #endregion

        public EnterLexemePropertiesViewModel(CLexemeManaged lexeme, bool bIsNew)
        {
            m_Lexeme = lexeme;
            m_PropertiesChanged = new List<string>();
            m_sSourceForm = "";

            String sAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя\u0300\u0301";
            m_CyrillicAlphabet = new HashSet<char>(sAlphabet.ToCharArray());

            m_Errors = new List<string>();

            CancelCommand = new RelayCommand(new Action<object>(OnCancel));
            OKCommand = new RelayCommand(new Action<object>(OnOK));
            PropertyChanged += new PropertyChangedEventHandler(lexeme_PropertyChanged);

            m_StringToPos = new Dictionary<string, E_POS>()
            {
                { "", E_POS.POS_UNDEFINED },
                { "Существительное", E_POS.POS_NOUN },
                { "Глагол", E_POS.POS_VERB },
                { "Прилагательное", E_POS.POS_ADJ },
                { "Местоимение-С", E_POS.POS_PRONOUN },
                { "Местоимение-П", E_POS.POS_PRONOUN_ADJ },
                { "POS_PRONOUN_PREDIC", E_POS.POS_PRONOUN_PREDIC },
                { "Числительное-К", E_POS.POS_NUM },
                { "Числительное-П", E_POS.POS_NUM_ADJ },
                { "Наречие", E_POS.POS_ADV },
                { "POS_COMPAR", E_POS.POS_COMPAR },
                { "POS_PREDIC", E_POS.POS_PREDIC },
                { "Предлог", E_POS.POS_PREP },
                { "Союз", E_POS.POS_CONJ },
                { "Частица", E_POS.POS_PARTICLE },
                { "POS_INTERJ", E_POS.POS_INTERJ },
                { "POS_PARENTH", E_POS.POS_PARENTH },
                { "POS_NULL", E_POS.POS_NULL },
                { "POS_COUNT", E_POS.POS_COUNT }
            };

            YesNoValues = new ObservableCollection<string>() { "Да", "Нет" };

            if (!bIsNew)
            {
                LoadData();
            }

        }       //  EnterLexemePropertiesViewModel()

        public bool bErrors()
        {
            return (m_Errors.Count > 0);
        }

        public string sErrorList()
        {
            string sMsg = "";
            foreach (string sErr in m_Errors)
            {
                if (sMsg.Length > 0)
                {
                    sMsg += System.Environment.NewLine;
                }
                sMsg += '\t' + sErr;
            }

            return sMsg;
        }

        public void OnCancel(object view)
        {
            ((Window)view).Close();
        }

        public void OnOK(object view)
        {
            var bRet = bVerifyData();
            if (!bRet)
            {
                string sMsg = "Ошибки в параметрах лексемы:";
                sMsg += System.Environment.NewLine;
                sMsg += sErrorList();
                MessageBox.Show(sMsg);
                m_Errors.Clear();
            }
            else
            {
                OffloadData();
                var eErrCode = m_Lexeme.eCheckLexemeProperties();
                if (EM_ReturnCode.H_NO_ERROR == eErrCode)
                {
                    ((Window)view).DialogResult = true;
                    ((Window)view).Close();
                }
            }
        }

        public bool bGetYesNoValue(string sPropertyName, string sValue, ref bool bResult)
        {
            string sYesValue = "Да";
            string sNoValue = "Нет";

            if (sYesValue == sValue)
            {
                bResult = true;
            }
            else if (sNoValue == sValue)
            {
                bResult = false;
            }
            else
            {
                MessageBox.Show(String.Format("Unexpected value for {1} property", sPropertyName), "Error");
                return false;
            }
            return true;
        }

        public bool bVerifyData()
        {
//            bool bRet = true;

            // Headword: only required part is source form
            if (null == m_sSourceForm || m_sSourceForm.Length < 1)
            {
                m_Errors.Add("Нет исходной формы.");
                return false;
            }

            if (!m_CyrillicAlphabet.IsSupersetOf(m_sSourceForm))
            {
                m_Errors.Add("Посторонние символы в исходной форме.");
                return false;
            }

//            var ret = m_Lexeme.eCheckLexemeProperties();
/*
            EM_MainSymbol eMainSymbol = Helpers.eMainSymbolToEnum(m_sMainSymbol);
            switch (eMainSymbol)
            {
                case EM_MainSymbol.MS_MO:
                case EM_MainSymbol.MS_ZH:
                case EM_MainSymbol.MS_ZHO:
                case EM_MainSymbol.MS_S:
                case EM_MainSymbol.MS_SO:
                    break;
                case EM_MainSymbol.MS_MO_ZHO:
                    break;
                case EM_MainSymbol.MS_MN:
                    break;
                case EM_MainSymbol.MS_MN_NEOD:
                    break;
                case EM_MainSymbol.MS_MN_ODUSH:
                    break;
                case EM_MainSymbol.MS_MN_OT:
                    break;
                case EM_MainSymbol.MS_P:
                    break;
                case EM_MainSymbol.MS_MS:
                    break;
                case EM_MainSymbol.MS_MS_P:
                    break;
                case EM_MainSymbol.MS_CHISL:
                    break;
                case EM_MainSymbol.MS_CHISL_P:
                    break;
                case EM_MainSymbol.MS_SV:
                    break;
                case EM_MainSymbol.MS_NSV:
                    break;
                case EM_MainSymbol.MS_SV_NSV:
                    break;
                case EM_MainSymbol.MS_N:
                    break;
                case EM_MainSymbol.MS_PREDL:
                    break;
                case EM_MainSymbol.MS_SOJUZ:
                    break;
                case EM_MainSymbol.MS_PREDIK:
                    break;
                case EM_MainSymbol.MS_VVODN:
                    break;
                case EM_MainSymbol.MS_SRAVN:
                    break;
                case EM_MainSymbol.MS_CHAST:
                    break;
                case EM_MainSymbol.MS_MEZHD:
                    break;
                case EM_MainSymbol.MS_END:
                    break;
                default:
                    m_Errors.Add("Неопознан основной символ.");
                    break;
//                    return false;
            }
            */
            return !bErrors();
        }

        public void LoadData()
        {
            FleetingVowel = m_Lexeme.bHasFleetingVowel() ? m_YesNoValues[0] : m_YesNoValues[1];
            YoAlternation = m_Lexeme.bHasYoAlternation() ? m_YesNoValues[0] : m_YesNoValues[1];
            OAlternation = m_Lexeme.bHasOAlternation() ? m_YesNoValues[0] : m_YesNoValues[1];
            SourceForm = m_Lexeme.sSourceForm();
            HeadwordComment = m_Lexeme.sHeadwordComment();
            Variant = m_Lexeme.sHeadwordVariant();
            //            HeadwordVariantComment = m_Lexeme.sHeadwordVariantComment();
            Usage = m_Lexeme.sUsage();
            MainSymbol = m_Lexeme.sMainSymbol();
            Index = m_Lexeme.iType().ToString();
           

            /*


            /*
            case "SetPluralOf":
            {
                if (null == m_sPluralOf || m_sm_sPluralOf.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetPluralOf(m_sPluralOf);
                }
                break;
            }
            */

            /*
            case "SetPluralOf":
            {
                if (null == m_sPluralOf || m_sPluralOf.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetPluralOf(m_sPluralOf);
                }
                break;
            }
            */
            /*
        case "SeeRef":
        {
            if (null == m_sSeeRef || m_sSeeRef.Length < 1)
            {
                continue;
            }
            else
            {
                m_Lexeme.SetSeeRef(m_sSeeRef);
            }
            break;
        }

        case "BackRef":
        {
            if (null == m_sBackRef || m_sBackRef.Length < 1)
            {
                continue;
            }
            else
            {
                m_Lexeme.SetBackRef(m_sSeeRef);
            }
            break;
        }

        case "IsUnstressed":
        {
            bRet = bSetYesNoValue("IsUnstressed", m_sIsUnstressed, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetIsUnstressed(bValue);
            }                    
            break;
        }

        case "IsVariant":
        {
            bRet = bSetYesNoValue("IsVariant", m_sIsVariant, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetIsVariant(bValue);
            }
            break;
        }
              */
            /*
        case "IsPluralOf":
        {
            bRet = bSetYesNoValue("IsPluralOf", m_sIsPluralOf, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetIsPluralOf(bValue);
            }
            break;
        }
            */
            /*
        case "Transitive":
        {
            bRet = bSetYesNoValue("Transitive", m_sTransitive, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetTransitive(bValue);
            }
            break;
        }
        */

            //    virtual void SetIsReflexive(ET_Reflexive eValue)
            //    virtual void SetMainSymbolPluralOf(const CEString& sValue)

            /*
case "AltMainSymbol":
{
    if (null == m_sAltMainSymbol || m_sAltMainSymbol.Length < 1)
    {
        continue;
    }
    else
    {
        m_Lexeme.SetAltMainSymbol(m_sAltMainSymbol);
    }
    break;
}
            */

            //                virtual void SetAspect(ET_Aspect eValue)
            //    virtual void SetInflectionType(const CEString& sValue)
            //    virtual void SetPartOfSpeech(ET_PartOfSpeech eValue)
            //    virtual void SetComment(const CEString& sValue)
            //    virtual void SetAltMainSymbolComment(const CEString& sValue)
            //    virtual void SetAltInflectionComment(const CEString& sValue)
            /*
                                case "AltMainSymbol":
                                    {
                                        if (null == m_sVerbStemAlternation || m_sVerbStemAlternation.Length < 1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            m_Lexeme.SetVerbStemAlternation(m_sVerbStemAlternation);
                                        }
                                        break;
                                    }

                                case "Index":
                                    {
                                        int iType = 0;
                                        if (Int32.TryParse(m_sIndex, out iType))
                                        {
                                            m_Lexeme.SetType(iType);
                                        }
                                        break;
                                    }
            */
            /*
            case "PartPastPassZhd":
            {
                bRet = bSetYesNoValue("PartPastPassZhd", m_sPartPastPassZhd, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetPartPastPassZhd(bValue);
                }
                break;
            }
            */

            /*
            case "Section":
            {
                if (m_iSection < 1 || m_iSection> 18)
                {
                    MessageBox.Show("Unexpected value for m_iSection property" + m_iSection, "Error");
                    continue;
                }

                    m_Lexeme.SetSection(m_iSection);
                break;
            }
            */
            /*
        case "NoComparative":
        {
            bRet = bSetYesNoValue("NoComparative", m_sNoComparative, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetNoComparative(bValue);
            }
            break;
        }
            */

            /*
        case "AssumedForms":
        {
            bRet = bSetYesNoValue("AssumedForms", m_sAssumedForms, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetAssumedForms(bValue);
            }
            break;
        }
            */
            /*
        case "SecondGenitive":
        {
            bRet = bSetYesNoValue("SecondGenitive", m_sSecondGenitive, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetSecondGenitive(bValue);
            }
            break;
        }
        */
            /*
        case "SecondLocative":
        {
            bRet = bSetYesNoValue("SecondLocative", m_sSecondLocative, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetSecondLocative(bValue);
            }
            break;
        }
                */
            /*
        case "SecondLocativeOptional":
        {
            bRet = bSetYesNoValue("SecondLocativeOptional", m_sSecondLocativeOptional, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetSecondLocativeOptional(bValue);
            }
            break;
        }
                    */

            /*
            case "Loc2Preposition":
            {
                if (null == m_sLoc2Preposition || m_sLoc2Preposition.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetLoc2Preposition(m_sLoc2Preposition);
                }
                break;
            }
                */

            /*
            case "HasAspectPair":
            {
                bRet = bSetYesNoValue("HasAspectPair", m_sHasAspectPair, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasAspectPair(bValue);
                }
                break;
            }
                */
            /*
            case "AspectPairType":
            {
                if (m_iAspectPair < 1 || m_iAspectPair > 18)
                {
                    MessageBox.Show("Unexpected value for m_iAspectPair property" + m_iAspectPair, "Error");
                    continue;
                }

                m_Lexeme.SetAltAspectPairType(m_iAspectPair);
                break;
            }
            */
            /*
            case "AspectPairData":
            {
                if (null == m_sAspectPairData || m_sAspectPairData.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetAspectPairData(m_sAspectPairData);
                }
                break;
            }
            */

            /*
            case "AltAspectPairComment":
            {
                if (null == m_sAltAspectPairComment || m_sAltAspectPairComment.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetAltAspectPairComment(m_sAltAspectPairComment);
                }
                break;
            }
            */

            /*
            case "QuestionableForms":
            {
                if (null == m_sQuestionableForms || m_sQuestionableForms.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetQuestionableForms(m_sQuestionableForms);
                }
                break;
            }
                */

            /*
            case "HasIrregularVariants":
            {
                bRet = bSetYesNoValue("HasIrregularVariants", m_sHasIrregularVariants, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasIrregularVariants(bValue);
                }
                break;
            }
                */

            /*
            case "HasDeficiencies":
            {
                bRet = bSetYesNoValue("HasDeficiencies", m_sHasDeficiencies, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasDeficiencies(bValue);
                }
            }
                */

            /*
            case "RestrictedForms":
            {
                if (null == m_sRestrictedForms || m_sRestrictedForms.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetQuestionableForms(m_sRestrictedForms);
                }
                break;
            }
                */

            /*
            case "Contexts":
            {
                if (null == m_sContexts || m_sContexts.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetQuestionableForms(m_sContexts);
                }
                break;
            }
                */

            /*
            case "TrailingComment":
            {
                if (null == m_sTrailingComment || m_sTrailingComment.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetQuestionableForms(m_sTrailingComment);
                }
                break;
            }
                */

            /*
            case "TrailingComment":
            {
                if (null == m_sTrailingComment || m_sTrailingComment.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetQuestionableForms(m_sTrailingComment);
                }
                break;
            }
                */
            /*
            case "AspectPairType":
            {
                if (m_iInflectionId < 1 || m_iInflectionId > 16)
                {
                    MessageBox.Show("Unexpected value for m_iInflectionId property" + m_iInflectionId, "Error");
                    continue;
                }

                m_Lexeme.SetInflectionId(m_iInflectionId);
                break;
            }
            */
            /*
            case "PrimaryInflectionGroup":
            {
                bRet = bSetYesNoValue("PrimaryInflectionGroup", m_sPrimaryInflectionGroup, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetPrimaryInflectionGroup(bValue);
                }
            }
            */
            /*
            case "Type":
            {
                if (m_iType < 1 || m_iType > 16)
                {
                    MessageBox.Show("Unexpected value for m_iType property" + m_iType, "Error");
                    continue;
                }

                m_Lexeme.SetType(m_iType);
                break;
            }
            */
            /*
            case "StressType1":
                {
                    m_Lexeme.SetAccentType1(Helpers.eStringToAccentType(m_sStressType1));
                    break;
                }

            case "StressType2":
                {
                    m_Lexeme.SetAccentType1(Helpers.eStringToAccentType(m_sStressType2));
                    break;
                }
                */
            /*
            case "ShortFormsRestricted":
            {
                bRet = bSetYesNoValue("ShortFormsRestricted", m_sShortFormsRestricted, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetShortFormsRestricted(bValue);
                }
            }
                    */
            /*
            case "PastParticipleRestricted":
            {
                bool bValue = false;
                if ("Да" == m_sPastParticipleRestricted)
                {
                    bValue = true;
                }
                else if ("Нет" == m_sPastParticipleRestricted)
                {
                    bValue = false;
                }
                else
                {
                    MessageBox.Show("Unexpected value for m_sPastParticipleRestricted property" + m_sPastParticipleRestricted, "Error");
                    continue;
                }
                m_Lexeme.SetPastParticipleRestricted(bValue);

                break;
            }
            */
            /*
            case "NoLongForms":
            {
                bRet = bSetYesNoValue("NoLongForms", m_sNoLongForms, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoLongForms(bValue);
                }
            }
            */
            /*
        case "ShortFormsIncomplete":
        {
            bRet = bSetYesNoValue("ShortFormsIncomplete", m_sShortFormsIncomplete, ref bValue);
            if (bRet)
            {
                m_Lexeme.SetShortFormsIncomplete(bValue);
            }
        }
        */
            /*
            case "NoPastParticiple":
            {
                bRet = bSetYesNoValue("NoPastParticiple", m_sNoPastParticiple, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoPastParticiple(bValue);
                }
            }
                    */
            /*
            case "NoPastParticiple":
            {
                bRet = bSetYesNoValue("NoPastParticiple", m_sNoPastParticiple, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoPastParticiple(bValue);
                }
            }
            */
            /*
            case "StemAugment":
            {
                bRet = bSetYesNoValue("StemAugment", m_sStemAugment, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetStemAugment(bValue);
                }
            }


            case "1SgStem":
            {
                if (null == m_s1SgStem || m_s1SgStem.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.Set1SgStem(m_s1SgStem);
                }
                break;
            }

            case "3SgStem":
            {
                if (null == m_s3SgStem || m_s3SgStem.Length < 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.Set3SgStem(m_s3SgStem);
                }
                break;
            }
            case "Infinitive":
            {
                if (null == m_sInfinitive || m_sInfinitive.Length< 1)
                {
                    continue;
                }
                else
                {
                    m_Lexeme.SetInfinitive(m_sInfinitive);
                }
                break;
            }
            */
            /*
            default:
                MessageBox.Show("Internal error: unable to offload data.");
                break;
                */
            //}
        }

        public void OffloadData()
        {
            bool bRet = true;
            bool bValue = true;

            foreach (string sProperty in m_PropertiesChanged)
            {
                switch (sProperty)
                {
                    //case "GraphicStem":
                    //case "HasIrregularForms":

                    case "FleetingVowel":
                    {
                        bRet = bGetYesNoValue("FleetingVowel", m_sFleetingVowel, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasFleetingVowel(bValue);
                        }
                        break;
                    }

                    case "HasYoAlternation":
                    {
                        bRet = bGetYesNoValue("YoAlternation", m_sYoAlternation, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasYoAlternation(bValue);
                        }
                        break;
                    }

                    case "HasOAlternation":
                    {
                        bRet = bGetYesNoValue("OAlternation", m_sOAlternation, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasOAlternation(bValue);
                        }
                        break;
                    }

                    case "SourceForm":
                    {
                        if (null == m_sSourceForm || m_sSourceForm.Length < 1)
                        {
                            MessageBox.Show("Unexpected value for SourceForm property" + m_sSourceForm, "Error");
                            continue;
                        }
                        else
                        {
                            string sTmp = m_sSourceForm;
                            Helpers.RestoreStressMarks(ref sTmp);
                            m_Lexeme.SetSourceForm(sTmp);
                        }
                        break;
                    }

                    case "HeadwordComment":
                    {
                        if (null == m_sHeadwordComment || m_sHeadwordComment.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetHeadwordComment(m_sHeadwordComment);
                        }
                        break;
                    }

                    case "Variant":
                    {
                        if (null == m_sVariant || m_sVariant.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetHeadwordVariant(m_sVariant);
                        }
                        break;
                    }

                    /*
                    case "HeadwordVariantComment":
                    {
                        if (null == m_sHeadwordVariantComment || m_sHeadwordVariantComment.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetHeadwordVariantComment(m_sHeadwordVariantComment);
                        }
                        break;
                    }
                    */

                    /*
                    case "SetPluralOf":
                    {
                        if (null == m_sPluralOf || m_sm_sPluralOf.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetPluralOf(m_sPluralOf);
                        }
                        break;
                    }
                    */

                    /*
                    case "SetPluralOf":
                    {
                        if (null == m_sPluralOf || m_sPluralOf.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetPluralOf(m_sPluralOf);
                        }
                        break;
                    }
                    */

                    case "Usage":
                    {
                        if (null == m_sUsage || m_sUsage.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetUsage(m_sUsage);
                        }
                        break;
                    }

                    /*
                case "SeeRef":
                {
                    if (null == m_sSeeRef || m_sSeeRef.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_Lexeme.SetSeeRef(m_sSeeRef);
                    }
                    break;
                }

                case "BackRef":
                {
                    if (null == m_sBackRef || m_sBackRef.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_Lexeme.SetBackRef(m_sSeeRef);
                    }
                    break;
                }

                case "IsUnstressed":
                {
                    bRet = bGetYesNoValue("IsUnstressed", m_sIsUnstressed, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetIsUnstressed(bValue);
                    }                    
                    break;
                }

                case "IsVariant":
                {
                    bRet = bGetYesNoValue("IsVariant", m_sIsVariant, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetIsVariant(bValue);
                    }
                    break;
                }
                      */

                case "MainSymbol":
                {
                    if (null == m_sMainSymbol || m_sMainSymbol.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_Lexeme.SetMainSymbol(m_sMainSymbol);
                    }
                    break;
                }

                    /*
                case "IsPluralOf":
                {
                    bRet = bGetYesNoValue("IsPluralOf", m_sIsPluralOf, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetIsPluralOf(bValue);
                    }
                    break;
                }
                    */
                    /*
                case "Transitive":
                {
                    bRet = bGetYesNoValue("Transitive", m_sTransitive, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetTransitive(bValue);
                    }
                    break;
                }
                */

                    //    virtual void SetIsReflexive(ET_Reflexive eValue)
                    //    virtual void SetMainSymbolPluralOf(const CEString& sValue)

                            /*
                case "AltMainSymbol":
                {
                    if (null == m_sAltMainSymbol || m_sAltMainSymbol.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_Lexeme.SetAltMainSymbol(m_sAltMainSymbol);
                    }
                    break;
                }
                            */

                            //                virtual void SetAspect(ET_Aspect eValue)
                            //    virtual void SetInflectionType(const CEString& sValue)
                            //    virtual void SetPartOfSpeech(ET_PartOfSpeech eValue)
                            //    virtual void SetComment(const CEString& sValue)
                            //    virtual void SetAltMainSymbolComment(const CEString& sValue)
                            //    virtual void SetAltInflectionComment(const CEString& sValue)

                case "AltMainSymbol":
                {
                    if (null == m_sVerbStemAlternation || m_sVerbStemAlternation.Length < 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_Lexeme.SetVerbStemAlternation(m_sVerbStemAlternation);
                    }
                    break;
                }

                case "Index":
                {
                    int iType = 0;
                    if (Int32.TryParse(m_sIndex, out iType))
                    {
                        m_Lexeme.SetType(iType);
                    }
                    break;
                }
                    /*
                    case "PartPastPassZhd":
                    {
                        bRet = bGetYesNoValue("PartPastPassZhd", m_sPartPastPassZhd, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetPartPastPassZhd(bValue);
                        }
                        break;
                    }
                    */

                    /*
                    case "Section":
                    {
                        if (m_iSection < 1 || m_iSection> 18)
                        {
                            MessageBox.Show("Unexpected value for m_iSection property" + m_iSection, "Error");
                            continue;
                        }

                            m_Lexeme.SetSection(m_iSection);
                        break;
                    }
                    */
                    /*
                case "NoComparative":
                {
                    bRet = bGetYesNoValue("NoComparative", m_sNoComparative, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetNoComparative(bValue);
                    }
                    break;
                }
                    */

                    /*
                case "AssumedForms":
                {
                    bRet = bGetYesNoValue("AssumedForms", m_sAssumedForms, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetAssumedForms(bValue);
                    }
                    break;
                }
                    */
                    /*
                case "SecondGenitive":
                {
                    bRet = bGetYesNoValue("SecondGenitive", m_sSecondGenitive, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetSecondGenitive(bValue);
                    }
                    break;
                }
                */
                    /*
                case "SecondLocative":
                {
                    bRet = bGetYesNoValue("SecondLocative", m_sSecondLocative, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetSecondLocative(bValue);
                    }
                    break;
                }
                        */
                    /*
                case "SecondLocativeOptional":
                {
                    bRet = bGetYesNoValue("SecondLocativeOptional", m_sSecondLocativeOptional, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetSecondLocativeOptional(bValue);
                    }
                    break;
                }
                            */

                    /*
                    case "Loc2Preposition":
                    {
                        if (null == m_sLoc2Preposition || m_sLoc2Preposition.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetLoc2Preposition(m_sLoc2Preposition);
                        }
                        break;
                    }
                        */

                    /*
                    case "HasAspectPair":
                    {
                        bRet = bGetYesNoValue("HasAspectPair", m_sHasAspectPair, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasAspectPair(bValue);
                        }
                        break;
                    }
                        */
                    /*
                    case "AspectPairType":
                    {
                        if (m_iAspectPair < 1 || m_iAspectPair > 18)
                        {
                            MessageBox.Show("Unexpected value for m_iAspectPair property" + m_iAspectPair, "Error");
                            continue;
                        }

                        m_Lexeme.SetAltAspectPairType(m_iAspectPair);
                        break;
                    }
                    */
                    /*
                    case "AspectPairData":
                    {
                        if (null == m_sAspectPairData || m_sAspectPairData.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetAspectPairData(m_sAspectPairData);
                        }
                        break;
                    }
                    */

                    /*
                    case "AltAspectPairComment":
                    {
                        if (null == m_sAltAspectPairComment || m_sAltAspectPairComment.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetAltAspectPairComment(m_sAltAspectPairComment);
                        }
                        break;
                    }
                    */

                    /*
                    case "QuestionableForms":
                    {
                        if (null == m_sQuestionableForms || m_sQuestionableForms.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetQuestionableForms(m_sQuestionableForms);
                        }
                        break;
                    }
                        */

                    /*
                    case "HasIrregularVariants":
                    {
                        bRet = bGetYesNoValue("HasIrregularVariants", m_sHasIrregularVariants, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasIrregularVariants(bValue);
                        }
                        break;
                    }
                        */

                    /*
                    case "HasDeficiencies":
                    {
                        bRet = bGetYesNoValue("HasDeficiencies", m_sHasDeficiencies, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetHasDeficiencies(bValue);
                        }
                    }
                        */

                    /*
                    case "RestrictedForms":
                    {
                        if (null == m_sRestrictedForms || m_sRestrictedForms.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetQuestionableForms(m_sRestrictedForms);
                        }
                        break;
                    }
                        */

                    /*
                    case "Contexts":
                    {
                        if (null == m_sContexts || m_sContexts.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetQuestionableForms(m_sContexts);
                        }
                        break;
                    }
                        */

                    /*
                    case "TrailingComment":
                    {
                        if (null == m_sTrailingComment || m_sTrailingComment.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetQuestionableForms(m_sTrailingComment);
                        }
                        break;
                    }
                        */

                    /*
                    case "TrailingComment":
                    {
                        if (null == m_sTrailingComment || m_sTrailingComment.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetQuestionableForms(m_sTrailingComment);
                        }
                        break;
                    }
                        */
                    /*
                    case "AspectPairType":
                    {
                        if (m_iInflectionId < 1 || m_iInflectionId > 16)
                        {
                            MessageBox.Show("Unexpected value for m_iInflectionId property" + m_iInflectionId, "Error");
                            continue;
                        }

                        m_Lexeme.SetInflectionId(m_iInflectionId);
                        break;
                    }
                    */
                    /*
                    case "PrimaryInflectionGroup":
                    {
                        bRet = bGetYesNoValue("PrimaryInflectionGroup", m_sPrimaryInflectionGroup, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetPrimaryInflectionGroup(bValue);
                        }
                    }
                    */
                    /*
                    case "Type":
                    {
                        if (m_iType < 1 || m_iType > 16)
                        {
                            MessageBox.Show("Unexpected value for m_iType property" + m_iType, "Error");
                            continue;
                        }

                        m_Lexeme.SetType(m_iType);
                        break;
                    }
                    */
                    
                    case "StressType1":
                    {
                        m_Lexeme.SetAccentType1(Helpers.eStringToAccentType(m_sStressType1));
                        break;
                    }

                    case "StressType2":
                    {
                        m_Lexeme.SetAccentType1(Helpers.eStringToAccentType(m_sStressType2));
                        break;
                    }

                    /*
                    case "ShortFormsRestricted":
                    {
                        bRet = bGetYesNoValue("ShortFormsRestricted", m_sShortFormsRestricted, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetShortFormsRestricted(bValue);
                        }
                    }
                            */
                    /*
                    case "PastParticipleRestricted":
                    {
                        bool bValue = false;
                        if ("Да" == m_sPastParticipleRestricted)
                        {
                            bValue = true;
                        }
                        else if ("Нет" == m_sPastParticipleRestricted)
                        {
                            bValue = false;
                        }
                        else
                        {
                            MessageBox.Show("Unexpected value for m_sPastParticipleRestricted property" + m_sPastParticipleRestricted, "Error");
                            continue;
                        }
                        m_Lexeme.SetPastParticipleRestricted(bValue);

                        break;
                    }
                    */
                    /*
                    case "NoLongForms":
                    {
                        bRet = bGetYesNoValue("NoLongForms", m_sNoLongForms, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetNoLongForms(bValue);
                        }
                    }
                    */
                    /*
                case "ShortFormsIncomplete":
                {
                    bRet = bGetYesNoValue("ShortFormsIncomplete", m_sShortFormsIncomplete, ref bValue);
                    if (bRet)
                    {
                        m_Lexeme.SetShortFormsIncomplete(bValue);
                    }
                }
                */
                    /*
                    case "NoPastParticiple":
                    {
                        bRet = bGetYesNoValue("NoPastParticiple", m_sNoPastParticiple, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetNoPastParticiple(bValue);
                        }
                    }
                            */
                    /*
                    case "NoPastParticiple":
                    {
                        bRet = bGetYesNoValue("NoPastParticiple", m_sNoPastParticiple, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetNoPastParticiple(bValue);
                        }
                    }
                    */
                    /*
                    case "StemAugment":
                    {
                        bRet = bGetYesNoValue("StemAugment", m_sStemAugment, ref bValue);
                        if (bRet)
                        {
                            m_Lexeme.SetStemAugment(bValue);
                        }
                    }
                    

                    case "1SgStem":
                    {
                        if (null == m_s1SgStem || m_s1SgStem.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.Set1SgStem(m_s1SgStem);
                        }
                        break;
                    }

                    case "3SgStem":
                    {
                        if (null == m_s3SgStem || m_s3SgStem.Length < 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.Set3SgStem(m_s3SgStem);
                        }
                        break;
                    }
                    case "Infinitive":
                    {
                        if (null == m_sInfinitive || m_sInfinitive.Length< 1)
                        {
                            continue;
                        }
                        else
                        {
                            m_Lexeme.SetInfinitive(m_sInfinitive);
                        }
                        break;
                    }
                    */
                    default:
                        MessageBox.Show("Internal error: unable to offload data.");
                        break;
                }
            }
        }

        #region Event_Handlers

        public void lexeme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {        
            m_PropertiesChanged.Add(e.PropertyName);
        }

        #endregion

    }
}
