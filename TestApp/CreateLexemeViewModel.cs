using MainLibManaged;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZalTestApp
{
    public class CreateLexemeViewModel : ViewModelBase
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

#region Accessors_and_Mutators
        private string m_sPOS;
        public string sPOS
        {
            get
            {
                return m_sPOS;
            }
            set
            {
                if (value != m_sPOS)
                {
                    m_sPOS = value;
                }
                OnPropertyChanged("sPOS");
            }
        }

        public E_POS POS
        {
            get
            {
                return m_StringToPos[m_sPOS];
            }
        }

        private string m_sDlgTitle = "Часть речи";
        public string DlgTitle
        {
            get
            {
                return m_sDlgTitle;
            }
            set
            {
                if (value != m_sDlgTitle)
                {
                    m_sDlgTitle = value;
                }
                OnPropertyChanged("m_sDlgTitle");
            }
        }
        #endregion

        public CreateLexemeViewModel()
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
