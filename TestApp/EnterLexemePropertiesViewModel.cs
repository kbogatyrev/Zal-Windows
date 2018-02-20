using MainLibManaged;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZalTestApp
{
    public class EnterLexemePropertiesViewModel : ViewModelBase
    {
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
            POS_INTERJ,
            POS_PARENTH,
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
        public string Minus
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
        #endregion

        public EnterLexemePropertiesViewModel()
        {
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
        }
    }
}
