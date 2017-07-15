using MainLibManaged;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ZalTestApp
{
    public class LexemeViewModel : ViewModelBase
    {
        private CLexemeManaged m_Lexeme = null;
        private NounViewModel m_NounViewModel = null;

        public delegate void ShowNounFormsHandler(CLexemeManaged l);
        public event ShowNounFormsHandler ShowNounFormsEvent;

        #region Bindings
        public string SourceForm
        {
            get
            {
                if (null == m_Lexeme)
                {
                    MessageBox.Show("Internal error: lexeme not loaded.");
                    return "";
                }
                return m_Lexeme.sSourceForm();
            }
        }
        public string GraphicStem
        {
            get
            {
                if (null == m_Lexeme)
                {
                    MessageBox.Show("Internal error: lexeme not loaded.");
                    return "";
                }
                return m_Lexeme.sGraphicStem();
            }
        }

        public string MainSymbol
        {
            get
            {
                if (null == m_Lexeme)
                {
                    MessageBox.Show("Internal error: lexeme not loaded.");
                    return "";
                }
                return m_Lexeme.sMainSymbol();
            }
        }

        public string Index
        {
            get
            {
                if (null == m_Lexeme)
                {
                    MessageBox.Show("Internal error: lexeme not loaded.");
                    return "";
                }
                return m_Lexeme.iType().ToString();
            }
        }

        public string StressSchema
        {
            get
            {
                if (null == m_Lexeme)
                {
                    MessageBox.Show("Internal error: lexeme not loaded.");
                    return "";
                }

                var eAp1 = m_Lexeme.eAccentType1();
                string sRet = sAccenTypeToStressSchema(eAp1);
                var eAp2 = m_Lexeme.eAccentType2();
                if (eAp2 != EM_AccentType.AT_UNDEFINED)
                {
                    sRet += "/" + sAccenTypeToStressSchema(eAp2);
                }
                return sRet;
            }
        }
        #endregion

        public LexemeViewModel(CLexemeManaged l)
        {
            m_Lexeme = l;
            ShowParadigmCommand = new RelayCommand(new System.Action<object>(ShowParadigm));
        }

        private void ShowParadigm(object arg)
        {
            if (null == m_Lexeme)
            {
                MessageBox.Show("Internal error: lexeme object not initialized.");
                return;
            }

            if (null == m_NounViewModel)
            {
                m_NounViewModel = new NounViewModel();
                if (ShowNounFormsEvent != null)
                {
                    ShowNounFormsEvent(m_Lexeme);
                }
            }
        }

        #region ICommand
        private ICommand m_ShowParadigmCommand;
        public ICommand ShowParadigmCommand
        {
            get
            {
                return m_ShowParadigmCommand;
            }
            set
            {
                m_ShowParadigmCommand = value;
            }
        }
        #endregion

        #region Helpers
        private string sAccenTypeToStressSchema(EM_AccentType eType)
        {
            switch (eType)
            { 
                case EM_AccentType.AT_A:
                    return "a";
                case EM_AccentType.AT_A1:
                    return "a'";
                case EM_AccentType.AT_B:
                    return "b";
                case EM_AccentType.AT_B1:
                    return "b'";
                case EM_AccentType.AT_C:
                    return "c";
                case EM_AccentType.AT_C1:
                    return "c'";
                case EM_AccentType.AT_C2:
                    return "c''";
                case EM_AccentType.AT_D:
                    return "d";
                case EM_AccentType.AT_D1:
                    return "d'";
                case EM_AccentType.AT_E:
                    return "e";
                case EM_AccentType.AT_F:
                    return "f";
                case EM_AccentType.AT_F1:
                    return "f'";
                case EM_AccentType.AT_F2:
                    return "f''";
                case EM_AccentType.AT_UNDEFINED:
                default:
                    return "Undefined";
            };

        }
        #endregion
    }
}
