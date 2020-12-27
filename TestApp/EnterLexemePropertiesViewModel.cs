using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using MainLibManaged;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace ZalTestApp
{
    public class EnterLexemePropertiesViewModel : ViewModelBase
    {
        public delegate void ShowParadigmForEditHandler(CLexemeManaged l);
        public event ShowParadigmForEditHandler ShowParadigmForEditEvent;

        HashSet<string> m_PropertiesChanged;
        string[] HeadwordProperties = { "SourceFormWithAccents", "Variant", "HeadwordComment", "IsPluralOf", "PluralOf", "SeeRef", "BackRef",
                                        "IsUnstressed", "IsVariant", "Usage" };
        string[] DescriptorProperties = { "SourceFormIsIrregular", "GraphicStem", "AltMainSymbol", "MainSymbol", "InflectionType", "Triangle", "FleetingVowel", "YoAlternation",
                                          "OAlternation", "TildeSymbol", "SecondGenitive", "HasAspectPair", "HasIrregularForms", "HasDeficiencies", "RestrictedContexts", "Contexts",
                                          "TrailingComment", "VerbStemAlternation", "PartPastPassZhd", "Section", "NoComparative", "AssumedForms" };
        string[] InflectionProperties = { "Index", "StressType1", "StressType2", "SmallCircle", "FleetingVowel", "XSymbol", "FramedXSymbol", "IsPrimaryInflectionGroup" };
        string[] CommonDeviationProperties = { "NumberInCircle" };
        string[] AspectPairProperties = { "SvToNsv", "NsvToSv", "AltAspectPairComment" };
        string[] Gen2Properties = { "Gen2", "SecondGenitive" };
        string[] P2Properties = { "P2", "P2Preposition" };

        HashSet<char> m_CyrillicAlphabet;
        char[] m_Vowels = { 'а', 'е', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
        char[] m_sStressMarks = { '/', '\\' };

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
            POS_PREPOSITION,
            POS_CONJUNCTION,
            POS_PARTICLE,
            POS_INTERJ,     // вводн.
            POS_PARENTH,    // межд.
            POS_NULL,
            POS_COUNT
        };
        Dictionary<string, E_POS> m_StringToPos;

        #endregion

        #region Bindings

        private bool m_bPropertiesChanged;
        public bool PropertiesChanged
        {
            get
            {
                return m_bPropertiesChanged;
            }
            set
            {
                if (value != m_bPropertiesChanged)
                {
                    m_bPropertiesChanged = value;
                }
                OnPropertyChanged("PropertiesChanged");
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
                if (value != m_sSourceForm)
                {
                    m_sSourceForm = value;
                    if (!SourceFormIsIrregular)
                    {
                        m_Lexeme.eMakeGraphicStem();
                        GraphicStem = m_Lexeme.sGraphicStem();
                    }
                }
                OnPropertyChanged("SourceForm");
                OnPropertyChanged("GraphicStem");
            }
        }

        public string SourceFormWithAccents
        {
            get
            {
                string sSourceFormWithAccents = "";
                Helpers.AssignDiacritics(m_sSourceForm, ref sSourceFormWithAccents);
                return sSourceFormWithAccents;
            }
            set
            {
                string sNewSourceForm = value;
                Helpers.RestoreInlineStressMarks(ref sNewSourceForm);
                if (sNewSourceForm != m_sSourceForm)
                {
                    SourceForm = sNewSourceForm;
                    var sTmp = sNewSourceForm.Replace(@"\", "");
                    sTmp = sTmp.Replace("/", "");
                    string sGs = "";
                    if (!SourceFormIsIrregular)
                    {
                        m_Lexeme.eMakeGraphicStem(sTmp, ref sGs);
                        GraphicStem = sGs;
                    }
                }
                OnPropertyChanged("SourceFormWithAccents");
            }
        }

        private bool m_bSourceFormIsIrregular = false;
        public bool SourceFormIsIrregular
        {
            get { return m_bSourceFormIsIrregular; }
            set
            {
                if (value != m_bSourceFormIsIrregular)
                {
                    m_bSourceFormIsIrregular = value;
                    GraphicStem = "";
                }
                OnPropertyChanged("SourceFormIsIrregular");
                OnPropertyChanged("GraphicStemGenerated");
                OnPropertyChanged("SourceFormWithAccents");
            }
        }
        public bool GraphicStemGenerated
        {
            get 
            { 
                return !m_bSourceFormIsIrregular; 
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

        private string m_sIsPluralOf;
        public string IsPluralOf
        {
            get
            {
                return m_sIsPluralOf;
            }
            set
            {
                if (value != m_sIsPluralOf)
                {
                    m_sIsPluralOf = value;
                }
                OnPropertyChanged("IsPluralOf");
            }
        }

        private string m_sPluralOf;
        public string PluralOf
        {
            get
            {
                return m_sPluralOf;
            }
            set
            {
                if (value != m_sPluralOf)
                {
                    m_sPluralOf = value;
                }
                OnPropertyChanged("PluralOf");
                GraphicStem = m_Lexeme.sGraphicStem();
            }
        }

        private string m_sSeeRef;
        public string SeeRef
        {
            get
            {
                return m_sSeeRef;
            }
            set
            {
                if (value != m_sSeeRef)
                {
                    m_sSeeRef = value;
                }
                OnPropertyChanged("SeeRef");
            }
        }
        private string m_sBackRef;
        public string BackRef
        {
            get
            {
                return m_sBackRef;
            }
            set
            {
                if (value != m_sBackRef)
                {
                    m_sBackRef = value;
                }
                OnPropertyChanged("BackRef");
            }
        }

        private string m_sIsUnstressed;
        public string IsUnstressed
        {
            get
            {
                return m_sIsUnstressed;
            }
            set
            {
                if (value != m_sIsUnstressed)
                {
                    m_sIsUnstressed = value;
                }
                OnPropertyChanged("IsUnstressed");
            }
        }

        private string m_sIsVariant;
        public string IsVariant
        {
            get
            {
                return m_sIsVariant;
            }

            set
            {
                if (value != m_sIsVariant)
                {
                    m_sIsVariant = value;
                }
                OnPropertyChanged("IsVariant");
            }
        }

        private string m_sGraphicStem;
        public string GraphicStem
        {
            get
            {
                return m_sGraphicStem;
            }

            set
            {
                if (value != m_sGraphicStem)
                {
                    m_sGraphicStem = value;
                }
                OnPropertyChanged("GraphicStem");
            }
        }

        private string m_sAltMainSymbol;
        public string AltMainSymbol
        {
            get
            {
                return m_sAltMainSymbol;
            }

            set
            {
                if (value != m_sAltMainSymbol)
                {
                    m_sAltMainSymbol = value;
                }
                OnPropertyChanged("AltMainSymbol");
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
                    if (GraphicStemGenerated)
                    {
                        m_Lexeme.SetMainSymbol(m_sMainSymbol);
                        m_Lexeme.eMakeGraphicStem();
                        GraphicStem = m_Lexeme.sGraphicStem();
                    }
                }
                OnPropertyChanged("MainSymbol");
                OnPropertyChanged("GraphicStem");
            }
        }

        private string m_sInflectionType;
        public string InflectionType
        {
            get
            {
                return m_sInflectionType;
            }
            set
            {
                if (value != m_sInflectionType)
                {
                    m_sInflectionType = value;
                    m_Lexeme.SetInflectionType(m_sInflectionType);
                    if (GraphicStemGenerated)
                    {
                        m_Lexeme.eMakeGraphicStem();
                        GraphicStem = m_Lexeme.sGraphicStem();
                    }
                }
                OnPropertyChanged("InflectionType");
                OnPropertyChanged("GraphicStem");
            }
        }

        private string m_sIsTransitive;
        public string IsTransitive
        {
            get
            {
                return m_sIsTransitive;
            }

            set
            {
                if (value != m_sIsTransitive)
                {
                    m_sIsTransitive = value;
                }
                OnPropertyChanged("IsTransitive");
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
                    GraphicStem = m_Lexeme.sGraphicStem();
                }
                OnPropertyChanged("Index");
                OnPropertyChanged("GraphicStem");
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

        private string m_sSmallCircle;
        public string SmallCircle
        {
            get
            {
                return m_sSmallCircle;
            }
            set
            {
                if (value != m_sSmallCircle)
                {
                    m_sSmallCircle = value;
                }
                OnPropertyChanged("SmallCircle");
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
                OnPropertyChanged("SecondGenitive");
            }
        }

        private string m_sP2;
        public string P2
        {
            get
            {
                return m_sP2;
            }
            set
            {
                if (value != m_sP2)
                {
                    m_sP2 = value;
                }
                OnPropertyChanged("SecondPrepositional");
            }
        }

        private string m_sP2Preposition;
        public string P2Preposition
        {
            get
            {
                return m_sP2Preposition;
            }
            set
            {
                if (value != m_sP2Preposition)
                {
                    m_sP2Preposition = value;
                }
                OnPropertyChanged("P2Preposition");
            }
        }

        private string m_sHasAspectPair;
        public string HasAspectPair
        {
            get
            {
                return m_sHasAspectPair;
            }
            set
            {
                if (value != m_sHasAspectPair)
                {
                    m_sHasAspectPair = value;
                }
                OnPropertyChanged("HasAspectPair");
            }
        }

        private string m_sSvToNsv;
        public string SvToNsv
        {
            get
            {
                return m_sSvToNsv;
            }
            set
            {

                if (value != m_sSvToNsv)
                {
                    m_sSvToNsv = value;
                }
                OnPropertyChanged("SvToNsv");
            }
        }

        private string m_sNsvToSv;
        public string NsvToSv
        {
            get
            {
                return m_sNsvToSv;
            }
            set
            {

                if (value != m_sNsvToSv)
                {
                    m_sNsvToSv = value;
                }
                OnPropertyChanged("NsvToSv");
            }
        }

/*
        private string m_sAspectPairData;
        public string AspectPairData
        {
            get
            {
                return m_sAspectPairData;
            }
            set
            {
                if (value != m_sAspectPairData)
                {
                    m_sAspectPairData = value;
                }
                OnPropertyChanged("AspectPairData");
            }
        }
*/

        private string m_sAltAspectPairComment;
        public string AltAspectPairComment
        {
            get
            {
                return m_sAltAspectPairComment;
            }
            set
            {
                if (value != m_sAltAspectPairComment)
                {
                    m_sAltAspectPairComment = value;
                }
                OnPropertyChanged("AltAspectPairComment");
            }
        }

        //private string m_sQuestionableForms;
        //public string QuestionableForms
        //{
        //    get
        //    {
        //        return m_sQuestionableForms;
        //    }
        //    set
        //    {
        //        if (value != m_sQuestionableForms)
        //        {
        //            m_sQuestionableForms = value;
        //        }
        //        OnPropertyChanged("QuestionableForms");
        //    }
        //}

        private string m_sHasIrregularForms;
        public string HasIrregularForms
        {
            get
            {
                return m_sHasIrregularForms;
            }
            set
            {
                if (value != m_sHasIrregularForms)
                {
                    m_sHasIrregularForms = value;
                }
                OnPropertyChanged("HasIrregularForms");
            }
        }

        private string m_sHasIrregularVariants;
        public string HasIrregularVariants
        {
            get
            {
                return m_sHasIrregularVariants;
            }
            set
            {
                if (value != m_sHasIrregularVariants)
                {
                    m_sHasIrregularVariants = value;
                }
                OnPropertyChanged("HasIrregularVariants");
            }
        }

        //private string m_sHasDeficiencies;
        //public string HasDeficiencies
        //{
        //    get
        //    {
        //        return m_sHasDeficiencies;
        //    }
        //    set
        //    {
        //        if (value != m_sHasDeficiencies)
        //        {
        //            m_sHasDeficiencies = value;
        //        }
        //        OnPropertyChanged("HasDeficiencies");
        //    }
        //}

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

        private string m_sContexts;
        public string Contexts
        {
            get
            {
                return m_sContexts;
            }
            set
            {
                if (value != m_sContexts)
                {
                    m_sContexts = value;
                }
                OnPropertyChanged("Contexts");
            }
        }

        private string m_sTrailingComment;
        public string TrailingComment
        {
            get
            {
                return m_sTrailingComment;
            }
            set
            {
                if (value != m_sTrailingComment)
                {
                    m_sTrailingComment = value;
                }
                OnPropertyChanged("TrailingComment");
            }
        }

        private string m_sIsPrimaryInflectionGroup;
        public string IsPrimaryInflectionGroup
        {
            get
            {
                return m_sIsPrimaryInflectionGroup;
            }
            set
            {
                if (value != m_sIsPrimaryInflectionGroup)
                {
                    m_sIsPrimaryInflectionGroup = value;
                }
                OnPropertyChanged("IsPrimaryInflectionGroup");
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

        private string m_sPartPastPassZhd;
        public string PartPastPassZhd
        {
            get
            {
                return m_sPartPastPassZhd;
            }
            set
            {
                if (value != m_sPartPastPassZhd)
                {
                    m_sPartPastPassZhd = value;
                }
                OnPropertyChanged("PartPastPassZhd");
            }
        }

        private int m_iSection;
        public int Section
        {
            get
            {
                return m_iSection;
            }
            set
            {
                if (value != m_iSection)
                {
                    m_iSection = value;
                }
                OnPropertyChanged("Section");
            }
        }

        private string m_sNoComparative;
        public string NoComparative
        {
            get
            {
                return m_sNoComparative;
            }
            set
            {
                if (value != m_sNoComparative)
                {
                    m_sNoComparative = value;
                }
                OnPropertyChanged("NoComparative");
            }
        }

        private string m_sAssumedForms;
        public string AssumedForms
        {
            get
            {
                return m_sAssumedForms;
            }
            set
            {
                if (value != m_sAssumedForms)
                {
                    m_sAssumedForms = value;
                }
                OnPropertyChanged("AssumedForms");
            }
        }

        private string m_sSecondGenitive;
        public string SecondGenitive
        {
            get
            {
                return m_sSecondGenitive;
            }
            set
            {
                if (value != m_sSecondGenitive)
                {
                    m_sSecondGenitive = value;
                }
                OnPropertyChanged("SecondGenitive");
            }
        }

        private string m_sSecondPrepositional;
        public string SecondPrepositional
        {
            get
            {
                return m_sSecondPrepositional;
            }
            set
            {
                if (value != m_sSecondPrepositional)
                {
                    m_sSecondPrepositional = value;
                }
                OnPropertyChanged("SecondPrepositional");
            }
        }

        private string m_sSecondPrepositionalOptional;
        public string SecondPrepositionalOptional
        {
            get
            {
                return m_sSecondPrepositionalOptional;
            }
            set
            {
                if (value != m_sSecondPrepositionalOptional)
                {
                    m_sSecondPrepositionalOptional = value;
                }
                OnPropertyChanged("SecondPrepositionalOptional");
            }
        }

        private List<string> m_YesNoValues;
        public List<string> YesNoValues
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

        private ICommand m_EditFormsCommand;
        public ICommand EditFormsCommand
        {
            get
            {
                return m_EditFormsCommand;
            }
            set
            {
                m_EditFormsCommand = value;
            }
        }

        // Other...
        public bool m_bHeadwordChanged = false;
        public bool HeadwordChanged
        {
            get
            {
                return m_bHeadwordChanged;
            }
            set
            {
                m_bHeadwordChanged = value;
            }
        }

        public bool m_bDescriptorChanged = false;
        public bool DescriptorChanged
        {
            get
            {
                return m_bDescriptorChanged;
            }
            set
            {
                m_bDescriptorChanged = value;
            }
        }

        public bool m_bInflectionGroupChanged = false;
        public bool InflectionGroupChanged
        {
            get
            {
                return m_bInflectionGroupChanged;
            }
            set
            {
                m_bInflectionGroupChanged = value;
            }
        }

        public bool m_bCommonDeviationChanged = false;
        public bool CommonDeviationChanged
        {
            get
            {
                return m_bCommonDeviationChanged;
            }
            set
            {
                m_bCommonDeviationChanged = value;
            }
        }

        public bool m_bGen2Changed = false;
        public bool Gen2Changed
        {
            get
            {
                return m_bGen2Changed;
            }
            set
            {
                m_bGen2Changed = value;
            }
        }

        public bool m_bP2Changed = false;
        public bool P2Changed
        {
            get
            {
                return m_bP2Changed;
            }
            set
            {
                m_bP2Changed = value;
            }
        }

        public bool m_bAspectPairChanged = false;
        public bool AspectPairChanged
        {
            get
            {
                return m_bAspectPairChanged;
            }
            set
            {
                m_bAspectPairChanged = value;
            }
        }

        #endregion

        #region Property_Delegates

        private delegate bool ChangedPropertyHandler();

        Dictionary<string, ChangedPropertyHandler> m_ChangedPropertiesHandlers = new Dictionary<string, ChangedPropertyHandler>();

        private void InitChangedPropertyHandlers()
        {
            m_ChangedPropertiesHandlers.Add("FleetingVowel", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("FleetingVowel", m_sFleetingVowel, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasFleetingVowel(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("YoAlternation", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("YoAlternation", m_sYoAlternation, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasYoAlternation(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("OAlternation", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("OAlternation", m_sOAlternation, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasOAlternation(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SourceForm", () =>
            {
                if (null == m_sSourceForm || m_sSourceForm.Length < 1)
                {
                    MessageBox.Show("Unexpected value for SourceForm property" + m_sSourceForm, "Error");
                    return false;
                }
                else
                {
                    string sTmp = m_sSourceForm;
                    Helpers.RestoreInlineStressMarks(ref sTmp);
                    m_Lexeme.SetSourceForm(sTmp);
                    return true;
                }
            });

            m_ChangedPropertiesHandlers.Add("SourceFormWithAccents", () =>
            {
                if (null == m_sSourceForm || m_sSourceForm.Length < 1)
                {
                    MessageBox.Show("Unexpected value for SourceForm property" + m_sSourceForm, "Error");
                    return false;
                }
                else
                {
                    string sTmp = m_sSourceForm;
                    Helpers.RestoreInlineStressMarks(ref sTmp);
                    m_Lexeme.SetSourceForm(sTmp);
                    return true;
                }
            });

            m_ChangedPropertiesHandlers.Add("HeadwordComment", () =>
            {
                if (null == m_sHeadwordComment)
                {
                    return true;
                }
                m_Lexeme.SetHeadwordComment(m_sHeadwordComment);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("Variant", () =>
            {
                if (null == m_sVariant)
                {
                    return true;
                }
                m_Lexeme.SetHeadwordVariant(m_sVariant);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("HeadwordVariantComment", () =>
            {
                //                if (null == m_sHeadwordVariantComment || m_sHeadwordVariantComment.Length < 1)
                //                {
                //                    return true;
                //                }
                //                m_Lexeme.SetHeadwordVariantComment(m_sHeadwordVariantComment);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("IsPluralOf", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("IsPluralOf", m_sIsPluralOf, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetIsPluralOf(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SetPluralOf", () =>
            {
                if (null == m_sPluralOf)
                {
                    return true;
                }
                m_Lexeme.SetPluralOf(m_sPluralOf);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("Usage", () =>
            {
                if (null == m_sUsage)
                {
                    return true;
                }
                m_Lexeme.SetUsage(m_sUsage);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("SeeRef", () =>
            {
                if (null == m_sSeeRef)
                {
                    return true;
                }
                m_Lexeme.SetSeeRef(m_sSeeRef);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("BackRef", () =>
            {
                if (null == m_sBackRef)
                {
                    return true;
                }
                m_Lexeme.SetBackRef(m_sBackRef);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("IsUnstressed", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("IsUnstressed", m_sIsUnstressed, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetIsUnstressed(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("IsVariant", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("IsVariant", m_sIsVariant, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetIsVariant(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("GraphicStem", () =>
            {
                if (null == m_sGraphicStem || m_sGraphicStem.Length < 1)
                {
                    MessageBox.Show("Unexpected value for GraphicStem property" + m_sGraphicStem, "Error");
                    return false;
                }
                else
                {
                    m_Lexeme.SetGraphicStem(m_sGraphicStem);
                    return true;
                }
            });

            m_ChangedPropertiesHandlers.Add("MainSymbol", () =>
            {
                if (null == m_sMainSymbol)
                {
                    return true;
                }
                m_Lexeme.SetMainSymbol(m_sMainSymbol);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("IsTransitive", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("Transitive", m_sIsTransitive, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetTransitive(bValue);
                }
                return bRet;
            });


            //    virtual void SetIsReflexive(ET_Reflexive eValue)
            //    virtual void SetMainSymbolPluralOf(const CEString& sValue)

            m_ChangedPropertiesHandlers.Add("AltMainSymbol", () =>
            {
                if (null == m_sAltMainSymbol)
                {
                    return true;
                }
                m_Lexeme.SetAltMainSymbol(m_sAltMainSymbol);
                return true;
            });


            //                virtual void SetAspect(ET_Aspect eValue)
            //    virtual void SetPartOfSpeech(ET_PartOfSpeech eValue)
            //    virtual void SetComment(const CEString& sValue)
            //    virtual void SetAltMainSymbolComment(const CEString& sValue)
            //    virtual void SetAltInflectionComment(const CEString& sValue)

            m_ChangedPropertiesHandlers.Add("Index", () =>
            {
                int iType = 0;
                if (Int32.TryParse(m_sIndex, out iType))
                {
                    m_Lexeme.SetType(iType);
                }
                return true;
            });

            m_ChangedPropertiesHandlers.Add("PartPastPassZhd", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("PartPastPassZhd", m_sPartPastPassZhd, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetPartPastPassZhd(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("Section", () =>
            {
                if (m_iSection < 1 || m_iSection> 18)
                {
                    MessageBox.Show("Unexpected value for m_iSection property" + m_iSection, "Error");
                    return false;
                }
                m_Lexeme.SetSection(m_iSection);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("NoComparative", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("NoComparative", m_sNoComparative, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoComparative(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("AssumedForms", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("AssumedForms", m_sAssumedForms, ref bValue);
                if (bRet)
                {
                m_Lexeme.SetAssumedForms(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SecondGenitive", () =>
            {
                bool bValue = true;
                bool bRet = bGetYesNoValue("SecondGenitive", m_sSecondGenitive, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetSecondGenitive(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SecondLocative", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("SecondLocative", m_sSecondPrepositional, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetSecondPrepositional(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SecondPrepositionalOptional", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("SecondPrepositionalOptional", m_sSecondPrepositionalOptional, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetSecondPrepositionalOptional(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("P2Preposition", () =>
            {
                if (null == m_sP2Preposition)
                {
                    return true;
                }
                m_Lexeme.SetP2Preposition(m_sP2Preposition);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("HasAspectPair", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("HasAspectPair", m_sHasAspectPair, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasAspectPair(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SvToNsv", () =>
            {
                int iType = -999;
                string sSvToNsvData;
                var eRet = Helpers.eSvToNsvSymbolToNumber(m_sSvToNsv, out iType, out sSvToNsvData);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show("Unable to parse aspect pair value: " + sSvToNsvData, "Error");
                    return false;
                }

                m_Lexeme.SetAspectPairType(iType);
                if (sSvToNsvData.Length > 0)
                {
                    m_Lexeme.SetAspectPairData(sSvToNsvData);
                }
                return true;
            });

            m_ChangedPropertiesHandlers.Add("NsvToSv", () =>
            {
                int iNsvToSv = -1;
                string sNsvToSvData;
                var eRet = Helpers.eNsvToSvSymbolToNumber(m_sNsvToSv, out iNsvToSv, out sNsvToSvData);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                { 
                    MessageBox.Show("Unexpected value for NsvToSv " + m_sNsvToSv, "Error");
                    return false;
                }

                m_Lexeme.SetAspectPairType(iNsvToSv);
                if (sNsvToSvData.Length > 0)
                {
                    m_Lexeme.SetAspectPairData(sNsvToSvData);
                }
                return true;
            });

            
/*

                                    m_ChangedPropertiesHandlers.Add("AltAspectPairComment", () =>
                                    {
                                        if (null == m_sAltAspectPairComment)
                                        {
                                            return true;
                                        }
                                        m_Lexeme.SetAltAspectPairComment(m_sAltAspectPairComment);
                                        return true;
                                    });
                        */
            m_ChangedPropertiesHandlers.Add("HasIrregularForms", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("HasIrregularForms", m_sHasIrregularForms, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasIrregularForms(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("HasIrregularVariants", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("HasIrregularVariants", m_sHasIrregularVariants, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetHasIrregularVariants(bValue);
                }
                return bRet;
            });

            //m_ChangedPropertiesHandlers.Add("HasDeficiencies", () =>
            //{
            //    bool bValue = false;
            //    bool bRet = bGetYesNoValue("HasDeficiencies", m_sHasDeficiencies, ref bValue);
            //    if (bRet)
            //    {
            //        m_Lexeme.SetHasDeficiencies(bValue);
            //    }
            //    return bRet;
            //});

            m_ChangedPropertiesHandlers.Add("RestrictedContexts", () =>
            {
                if (null == m_sRestrictedContexts)
                {
                    return true;
                }
                m_Lexeme.SetRestrictedContexts(m_sRestrictedContexts);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("Contexts", () =>
            {
                if (null == m_sContexts)
                {
                    return true;
                }
                m_Lexeme.SetContexts(m_sContexts);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("TrailingComment", () =>
            {
            if (null == m_sTrailingComment)
            {
                return true;
            }
            m_Lexeme.SetTrailingComment(m_sTrailingComment);
            return true;
            });

            m_ChangedPropertiesHandlers.Add("PrimaryInflectionGroup", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("PrimaryInflectionGroup", m_sIsPrimaryInflectionGroup, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetPrimaryInflectionGroup(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("VerbStemAlternation", () =>
            {
                if (null == m_sVerbStemAlternation)
                {
                    return true;
                }
                m_Lexeme.SetVerbStemAlternation(m_sVerbStemAlternation);
                return true;
            });

            m_ChangedPropertiesHandlers.Add("StressType1", () =>
            {
                m_Lexeme.SetAccentType1(Helpers.eStringToAccentType(m_sStressType1));
                return true;
            });

            m_ChangedPropertiesHandlers.Add("StressType2", () =>
            {
                m_Lexeme.SetAccentType2(Helpers.eStringToAccentType(m_sStressType2));
                return true;
            });

            m_ChangedPropertiesHandlers.Add("NumberInCircle", () =>
            {
                if (m_sNumberInCircle.Length < 1)
                {
                    MessageBox.Show("Warning: expected number in circle.");
                    return false;
                }
                var sSource = m_sNumberInCircle.Replace(" ", "");
                var arrNumbersInCircle = m_sNumberInCircle.Split(new char[] { ',' });
                string sDigits = "123456789";
                Regex r = new Regex("\\[?[1-9]\\]?");
                foreach (string sNic in arrNumbersInCircle)
                {
                    Match m = r.Match(sNic);
                    if (!m.Success)
                    {
                        MessageBox.Show("Цифра в кружке: ошибка в формате.");
                        return false;
                    }

                    if (sNic.Length == 3)
                    {
                        if ('[' == sNic['0'] && ']' == sNic[2] && sDigits.IndexOf(sNic[1]) >= 0)
                        {
                            int iType = -1;
                            try
                            {
                                Convert.ToInt32(sNic[2]);
                            }
                            catch(OverflowException)
                            {
                                MessageBox.Show("Цифра в кружке: ошибка в формате (цифра).");
                            }
                            m_Lexeme.eAddCommonDeviation(iType, true);
                        }
                        else
                        {
                            MessageBox.Show("Цифра в кружке: ошибка в формате.");
                            return false;
                        }
                    }
                    else
                    {
                        if (1 == sNic.Length)
                        {
                            int iType = -1;
                            try
                            {
                                Convert.ToInt32(sNic[0]);
                            }
                            catch (OverflowException)
                            {
                                MessageBox.Show("Цифра в кружке: ошибка в формате (цифра).");
                            }
                            m_Lexeme.eAddCommonDeviation(iType, false);
                        }
                    }
                }
                return true;
            });                 //  m_ChangedPropertiesHandlers.Add("NumberInCircle" ...

            m_ChangedPropertiesHandlers.Add("ShortFormsRestricted", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("ShortFormsRestricted", m_sXSymbol, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetShortFormsRestricted(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("PastParticipleRestricted", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("PastParticipleRestricted", m_sXSymbol, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetPastParticipleRestricted(bValue);
                }
                return bRet;
            });

//            m_ChangedPropertiesHandlers.Add("NoLongForms", () =>
//            {
                //bool bValue = false;
                //bool bRet = bGetYesNoValue("NoLongForms", m_sNoLongForms, ref bValue);
                //if (bRet)
                //{
                //    m_Lexeme.SetNoLongForms(bValue);
                //}
                //return bRet;
//                return true;
//            });

            m_ChangedPropertiesHandlers.Add("ShortFormsIncomplete", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("ShortFormsIncomplete", m_sFramedXSymbol, ref bValue);
                if (bRet)
                {
                m_Lexeme.SetShortFormsIncomplete(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("NoPassivePastParticiple", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("NoPassivePastParticiple", m_sFramedXSymbol, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoPassivePastParticiple(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("TildeSymbol", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("NoComparative", m_sTildeSymbol, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetNoComparative(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("Gen2", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("Gen2", m_sGen2, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetSecondGenitive(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("P2", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("P2", m_sP2, ref bValue);
                if (bRet)
                {
                    m_Lexeme.SetSecondPrepositional(bValue);
                }
                return bRet;
            });

            m_ChangedPropertiesHandlers.Add("SmallCircle", () =>
            {
                bool bValue = false;
                bool bRet = bGetYesNoValue("SmallCircle", m_sSmallCircle, ref bValue);
                if (!bRet)
                {
                    return false;
                }

                if (!bValue)
                {
                    m_Lexeme.SetStemAugment(-1);
                }

                if (EM_PartOfSpeech.POS_NOUN == m_Lexeme.ePartOfSpeech())
                {
                    if (1 == m_Lexeme.llInflectionId())
                    {
                        m_Lexeme.SetStemAugment(1);
                    }
                    else if (3 == m_Lexeme.llInflectionId())
                    {
                        if (m_Lexeme.sSourceForm().EndsWith("онок") || m_Lexeme.sSourceForm().EndsWith("ёнок"))
                        {
                            m_Lexeme.SetStemAugment(1);
                        }
                        else if (m_Lexeme.sSourceForm().EndsWith("оночек") || m_Lexeme.sSourceForm().EndsWith("ёночек"))
                        {
                            m_Lexeme.SetStemAugment(2);
                        }
                        else
                        {
                            MessageBox.Show("Недопустимая исходная форма: ожидается -оно(че)к/-ёно(че)к");
                            return false;
                        }
                    }
                    else if (8 == m_Lexeme.llInflectionId())
                    {
                        if (m_Lexeme.sSourceForm().EndsWith("мя"))
                        {
                            m_Lexeme.SetStemAugment(3);
                        }
                    }
                }
                else if (EM_PartOfSpeech.POS_VERB == m_Lexeme.ePartOfSpeech())
                {
                    m_Lexeme.SetStemAugment(1);
                }
                return true;
            });

        }   //  InitChangedPropertyHandlers()
        #endregion

    public EnterLexemePropertiesViewModel(CLexemeManaged lexeme, bool bIsNew)
    {
        m_Lexeme = lexeme;
        m_PropertiesChanged = new HashSet<string>();
        m_sSourceForm = "";
        m_bPropertiesChanged = false;

//////
        m_bHeadwordChanged = false;        
        m_bDescriptorChanged = false;
        m_bInflectionGroupChanged = false;
        m_bCommonDeviationChanged = false;
        m_bGen2Changed = false;
        m_bP2Changed = false;
        m_bAspectPairChanged = false;
        

            //            String sAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя\u0300\u0301";
        String sAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя/\\";
        m_CyrillicAlphabet = new HashSet<char>(sAlphabet.ToCharArray());

        m_Errors = new List<string>();

        CancelCommand = new RelayCommand(new Action<object>(OnCancel));
        OKCommand = new RelayCommand(new Action<object>(OnOK));
        EditFormsCommand = new RelayCommand(new Action<object>(OnEditForms));
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
            { "Предлог", E_POS.POS_PREPOSITION },
            { "Союз", E_POS.POS_CONJUNCTION },
            { "Частица", E_POS.POS_PARTICLE },
            { "POS_INTERJ", E_POS.POS_INTERJ },
            { "POS_PARENTH", E_POS.POS_PARENTH },
            { "POS_NULL", E_POS.POS_NULL },
            { "POS_COUNT", E_POS.POS_COUNT }
        };

        YesNoValues = new List<string>() { "Да", "Нет" };

        InitChangedPropertyHandlers();

        if (!bIsNew)
        {
            LoadData();
            m_PropertiesChanged.Clear();
            PropertiesChanged = false;
            HeadwordChanged = false;
            DescriptorChanged = false;
            InflectionGroupChanged = false;
            CommonDeviationChanged = false;
            Gen2Changed = false;
            P2Changed = false;
            AspectPairChanged = false;
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
                }
                else
                {
                    ((Window)view).DialogResult = false;
                    MessageBox.Show("Ошибка в параметрах лексемы.");
                }
                ((Window)view).Close();
            }
        }

        public void OnEditForms(object view)
        {
            ShowParadigmForEditEvent?.Invoke(this.m_Lexeme);
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
                MessageBox.Show(String.Format("Unexpected value for {0} property", sPropertyName), "Error");
                return false;
            }
            return true;
        }

        public bool bVerifyData()
        {
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

            return !bErrors();

        }   //  bVerifyData()

        public void LoadData()
        {
            FleetingVowel = m_Lexeme.bHasFleetingVowel() ? m_YesNoValues[0] : m_YesNoValues[1];
            YoAlternation = m_Lexeme.bHasYoAlternation() ? m_YesNoValues[0] : m_YesNoValues[1];
            OAlternation = m_Lexeme.bHasOAlternation() ? m_YesNoValues[0] : m_YesNoValues[1];
            EM_ReturnCode eRet = m_Lexeme.eGetSourceFormWithStress(ref m_sSourceForm, false);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to acquire source form.");
                return;
            }

            SourceForm = m_sSourceForm;
            HeadwordComment = m_Lexeme.sHeadwordComment();
            Variant = m_Lexeme.sHeadwordVariant();
            //            HeadwordVariantComment = m_Lexeme.sHeadwordVariantComment();
            Usage = m_Lexeme.sUsage();
            if (m_Lexeme.sAltMainSymbol().Length > 0)
            {
                MainSymbol = m_Lexeme.sMainSymbol() + "//" + m_Lexeme.sAltMainSymbol();
//                AltMainSymbolComment = m_Lexeme.sAltMainSymbolComment();
            }
            else
            {
                MainSymbol = m_Lexeme.sMainSymbol();
            }

            InflectionType = m_Lexeme.sInflectionType();

            Index = m_Lexeme.iType().ToString();
            if (EM_PartOfSpeech.POS_VERB == m_Lexeme.ePartOfSpeech())
            {
                IsTransitive = m_Lexeme.bTransitive() ? m_YesNoValues[0] : m_YesNoValues[1];
            }
            StressType1 = Helpers.sAccenTypeToStressSchema(m_Lexeme.eAccentType1());
            if (m_Lexeme.eAccentType2() != EM_AccentType.AT_UNDEFINED)
            {
                StressType2 = Helpers.sAccenTypeToStressSchema(m_Lexeme.eAccentType2());
            }

            Triangle = m_Lexeme.bHasIrregularForms() ? m_YesNoValues[0] : m_YesNoValues[1];

            if (m_Lexeme.iStemAugment() > 0)
            {
                SmallCircle = m_YesNoValues[0];
            }
            else
            {
                SmallCircle = m_YesNoValues[1];
            }

            var ePartOfSpeech = m_Lexeme.ePartOfSpeech();

            string sNumbersInCircle = "";
            for (int iCD = 0; iCD <= 9; ++iCD)
            {
                if (m_Lexeme.bHasCommonDeviation(iCD))
                {
                    if (sNumbersInCircle.Length > 0)
                    {
                        sNumbersInCircle += ", ";
                    }
                    if (m_Lexeme.bDeviationOptional(iCD))
                    {
                        sNumbersInCircle += "[" + iCD.ToString() + "]";
                    }
                    else
                    {
                        sNumbersInCircle += iCD.ToString();
                    }
                }
            }

            if (sNumbersInCircle.Length > 0)
            {
                m_sNumberInCircle = sNumbersInCircle;
            }

            m_sPluralOf = m_Lexeme.sPluralOf();
            m_sSeeRef = m_Lexeme.sSeeRef();
            m_sBackRef = m_Lexeme.sBackRef();
            IsUnstressed = m_Lexeme.bIsUnstressed() ? m_YesNoValues[0] : m_YesNoValues[1];
            IsVariant = m_Lexeme.bIsVariant() ? m_YesNoValues[0] : m_YesNoValues[1];

            m_sGraphicStem = m_Lexeme.sGraphicStem();

            m_sAltMainSymbol = m_Lexeme.sAltMainSymbol();
            PartPastPassZhd = m_Lexeme.bPartPastPassZhd() ? m_YesNoValues[0] : m_YesNoValues[1];
            Section = m_Lexeme.iSection();
            NoComparative = m_Lexeme.bNoComparative() ? m_YesNoValues[0] : m_YesNoValues[1];
            AssumedForms = m_Lexeme.bAssumedForms() ? m_YesNoValues[0] : m_YesNoValues[1];
            SecondGenitive = m_Lexeme.bSecondGenitive() ? m_YesNoValues[0] : m_YesNoValues[1];
            SecondPrepositional = m_Lexeme.bSecondPrepositional() ? m_YesNoValues[0] : m_YesNoValues[1];
            SecondPrepositionalOptional = m_Lexeme.bSecondPrepositionalOptional() ? m_YesNoValues[0] : m_YesNoValues[1];
            P2Preposition = m_Lexeme.sP2Preposition();
            HasAspectPair = m_Lexeme.bHasAspectPair() ? m_YesNoValues[0] : m_YesNoValues[1];

            m_sSvToNsv = "";
            m_sNsvToSv = "";
            if (m_Lexeme.bHasAspectPair())
            {
                if (m_Lexeme.iAspectPairType() < 0)
                {
                    eRet = Helpers.eSvToNsvNumberToSymbol(m_Lexeme.iAspectPairType(), out m_sSvToNsv);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        MessageBox.Show(String.Format("Unable to parse aspect pair type, input: {0}", m_Lexeme.iAspectPairType()), "Error");
                    }
                }
                else
                {
                    if (m_Lexeme.iAspectPairType() > 0)
                    {
                        m_sNsvToSv = m_Lexeme.iAspectPairType().ToString();
                    }
                }

                if (m_Lexeme.sAspectPairData().Length > 0)
                {
                    bool bUseParenths = (m_Lexeme.iAspectPairType() > 0) ? m_sNsvToSv.Length > 0 : m_sSvToNsv.Length > 0;
                    ref var sAspectPairInfo = ref ((m_Lexeme.iAspectPairType() > 0) ? ref m_sNsvToSv : ref m_sSvToNsv);
                    if (bUseParenths)
                    {
                        sAspectPairInfo += " (";
                    }
                    sAspectPairInfo += m_Lexeme.sAspectPairData();
                    if (bUseParenths)
                    {
                        sAspectPairInfo += ")";
                    }
                }
            }
            AltAspectPairComment = m_Lexeme.sAltAspectPairComment();
            HasIrregularForms = m_Lexeme.bHasIrregularForms() ? m_YesNoValues[0] : m_YesNoValues[1];
            HasIrregularVariants = m_Lexeme.bHasIrregularVariants() ? m_YesNoValues[0] : m_YesNoValues[1];
            RestrictedContexts = m_Lexeme.sRestrictedContexts();
            Contexts = m_Lexeme.sContexts();
            //            Cognates 
            TrailingComment = m_Lexeme.sTrailingComment();
            IsPrimaryInflectionGroup = m_Lexeme.bPrimaryInflectionGroup() ? m_YesNoValues[0] : m_YesNoValues[1];
            if (EM_PartOfSpeech.POS_ADJ == m_Lexeme.ePartOfSpeech())
            {
                XSymbol = m_Lexeme.bShortFormsRestricted() ? m_YesNoValues[0] : m_YesNoValues[1];
                FramedXSymbol = m_Lexeme.bShortFormsIncomplete() ? m_YesNoValues[0] : m_YesNoValues[1];
            }
            if (EM_PartOfSpeech.POS_VERB == m_Lexeme.ePartOfSpeech())
            {
                XSymbol = m_Lexeme.bPastParticipleRestricted() ? m_YesNoValues[0] : m_YesNoValues[1];
                FramedXSymbol = m_Lexeme.bNoPassivePastParticiple() ? m_YesNoValues[0] : m_YesNoValues[1];
            }
        }

        public void OffloadData()
        {
            bool bRet = true;
//            bool bValue = true;

            foreach (string sProperty in m_PropertiesChanged)
            {
                ChangedPropertyHandler handler = null;
                if (m_ChangedPropertiesHandlers.TryGetValue(sProperty, out handler))
                {
                    if (null == handler)
                    {
                        MessageBox.Show("Internal error: changed property handler is null.");
                        break;
                    }
                    var ret = handler();
                }
            }
        }

        #region Event_Handlers

        public void lexeme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("YesNoValues" == e.PropertyName)        // Please refactor
            {
                return;
            }

            if (!m_bPropertiesChanged)
            {
                PropertiesChanged = true;
            }

            m_PropertiesChanged.Add(e.PropertyName);
            if (Array.Exists(HeadwordProperties, sPropertyName => sPropertyName == e.PropertyName))
            {
                HeadwordChanged = true;
            }

            if (Array.Exists(DescriptorProperties, sPropertyName => sPropertyName == e.PropertyName))
            {
                DescriptorChanged = true;
            }

            if (Array.Exists(InflectionProperties, sPropertyName => sPropertyName == e.PropertyName))
            {
                InflectionGroupChanged = true;
            }

            if (Array.Exists(CommonDeviationProperties, sPropertyName => sPropertyName == e.PropertyName))
            {
                CommonDeviationChanged = true;
            }

            if (Array.Exists(AspectPairProperties, sPropertyName => sPropertyName == e.PropertyName))
            {
                AspectPairChanged = true;
            }

            if (Array.Exists(Gen2Properties, sPropertyName => sPropertyName == e.PropertyName))
            {
                Gen2Changed = true;
            }

            if (Array.Exists(P2Properties, sPropertyName => sPropertyName == e.PropertyName))
            {
                P2Changed = true;
            }

        }

        #endregion

    }
}
