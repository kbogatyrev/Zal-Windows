using System;
using System.Windows;
using System.Windows.Input;

using MainLibManaged;

namespace ZalTestApp
{
    public class LexemeViewModel : ViewModelBase
    {
        private CLexemeManaged m_Lexeme = null;
        public CLexemeManaged Lexeme
        {
            get
            {
                return m_Lexeme;
            }
        }

        private NounViewModel m_NounViewModel = null;
        private AdjViewModel m_AdjViewModel = null;

        public delegate void ShowNounFormsHandler(CLexemeManaged l);
        public event ShowNounFormsHandler ShowNounFormsEvent;

        public delegate void ShowAdjFormsHandler(CLexemeManaged l);
        public event ShowAdjFormsHandler ShowAdjFormsEvent;

        public delegate void RemoveLexemeHandler(LexemeViewModel lvm);
        public event RemoveLexemeHandler RemoveLexemeEvent;

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

        private ICommand m_RemoveLexemeCommand;
        public ICommand RemoveLexemeCommand
        {
            get
            {
                return m_RemoveLexemeCommand;
            }
            set
            {
                m_RemoveLexemeCommand = value;
            }
        }

        #endregion

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
                string sRet = Helpers.sAccenTypeToStressSchema(eAp1);
                var eAp2 = m_Lexeme.eAccentType2();
                if (eAp2 != EM_AccentType.AT_UNDEFINED)
                {
                    sRet += "/" + Helpers.sAccenTypeToStressSchema(eAp2);
                }
                return sRet;
            }
        }
        #endregion

        public LexemeViewModel(CLexemeManaged l)
        {
            m_Lexeme = l;
            ShowParadigmCommand = new RelayCommand(new Action<object>(ShowParadigm));
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));
        }

        private void ShowParadigm(object arg)
        {
            if (null == m_Lexeme)
            {
                MessageBox.Show("Internal error: lexeme object not initialized.");
                return;
            }

            switch (m_Lexeme.ePartOfSpeech())
            {
                case EM_PartOfSpeech.POS_NOUN:
                    if (null == m_NounViewModel)
                    {
                        m_NounViewModel = new NounViewModel(m_Lexeme);
                    }
                    ShowNounFormsEvent?.Invoke(m_Lexeme);
                    break;

                case EM_PartOfSpeech.POS_ADJ:
                    if (null == m_AdjViewModel)
                    {
                        m_AdjViewModel = new AdjViewModel(m_Lexeme);
                    }
                    ShowAdjFormsEvent?.Invoke(m_Lexeme);
                    break;

                default:
                    MessageBox.Show("Internal error: illegal part of speech value.");
                    return;

            }   //  switch...

        }   // ShowParadigm()

        private void RemoveLexeme(object arg)
        {
            RemoveLexemeEvent?.Invoke(this);
        }

    }   //  public class LexemeViewModel 

}   //  namespace ZalTestApp

