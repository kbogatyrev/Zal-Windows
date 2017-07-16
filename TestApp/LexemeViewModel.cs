﻿using MainLibManaged;
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
                m_NounViewModel = new NounViewModel(m_Lexeme);
            }
            ShowNounFormsEvent?.Invoke(m_Lexeme);
        }
    }
}
