using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

using MainLibManaged;
using System.Collections.Generic;

namespace ZalTestApp
{
    public class LexemeProperty : ViewModelBase
    {
        private string m_sName;
        private string m_sValue;

        public string LexemePropertyName
        {
            get { return m_sName; }
            set
            {
                m_sName = value;
                OnPropertyChanged("LexemePropertyName");
            }
        }

        public string LexemePropertyValue
        {
            get { return m_sValue; }
            set
            {
                m_sValue = value;
                OnPropertyChanged("LexemePropertyValue");
            }
        }
    }

    public class LexemeViewModel : ViewModelBase
    {
        public ObservableCollection<LexemeProperty> LexemeDetails { get; private set; }

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
        private VerbViewModel m_VerbViewModel = null;

        public delegate void ShowNounFormsHandler(CLexemeManaged l);
        public event ShowNounFormsHandler ShowNounFormsEvent;

        public delegate void ShowAdjFormsHandler(CLexemeManaged l);
        public event ShowAdjFormsHandler ShowAdjFormsEvent;

        public delegate void ShowVerbFormsHandler(CLexemeManaged l);
        public event ShowVerbFormsHandler ShowVerbFormsEvent;

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

        private ICommand m_SaveParadigmCommand;
        public ICommand SaveParadigmCommand
        {
            get
            {
                return m_SaveParadigmCommand;
            }
            set
            {
                m_SaveParadigmCommand = value;
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

        public LexemeViewModel()
        {
            m_Lexeme = null;
            ShowParadigmCommand = new RelayCommand(new Action<object>(ShowParadigm));
            SaveParadigmCommand = new RelayCommand(new Action<object>(SaveParadigm));
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));
        }

        public LexemeViewModel(CLexemeManaged l)
        {
            m_Lexeme = l;
            ShowParadigmCommand = new RelayCommand(new Action<object>(ShowParadigm));
            SaveParadigmCommand = new RelayCommand(new Action<object>(SaveParadigm));
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));

            LexemeDetails = new ObservableCollection<LexemeProperty>();

            CollectLexemeProperties();
        }

        private void AddProperty(string sName, string sValue)
        {
            LexemeProperty lp = new LexemeProperty();
            lp.LexemePropertyName = sName;
            lp.LexemePropertyValue = sValue;
            LexemeDetails.Add(lp);
        }

        private void CollectLexemeProperties()
        {
            if (null == m_Lexeme)
            {
                return;
            }

            AddProperty("Исходная форма", m_Lexeme.sSourceForm());

            List<int> arrHomonyms = m_Lexeme.arrHomonyms();
            if (arrHomonyms.Count > 0)
            {
                string sHomonyms = "";
                foreach (int iHomonym in arrHomonyms)
                {
                    if (sHomonyms.Length > 0)
                    {
                        sHomonyms += ", ";
                    }
                    sHomonyms += iHomonym.ToString();
                }
                AddProperty("Омонимы", sHomonyms);
            }

            if (m_Lexeme.sHeadwordComment().Length > 0)
            {
                AddProperty("Доп. помета (1)", m_Lexeme.sHeadwordComment());
            }

            if (m_Lexeme.sPluralOf().Length > 0)
            {
                AddProperty("Мн. от", m_Lexeme.sPluralOf());
            }

            if (m_Lexeme.sUsage().Length > 0)
            {
                AddProperty("Доп. помета (2)", m_Lexeme.sUsage());
            }

            //            if (m_Lexeme.sHeadwordVariantComment().Length > 0)
            //            {
            //                AddProperty("Доп. помета", m_Lexeme.sHeadwordVariantComment());
            //            }

            if (m_Lexeme.sSeeRef().Length > 0)
            {
                AddProperty("См. также", m_Lexeme.sSeeRef());
            }

            if (m_Lexeme.sTrailingComment().Length > 0)
            {
                AddProperty("Доп. помета (3)", m_Lexeme.sTrailingComment());
            }

            if (m_Lexeme.sRestrictedForms().Length > 0)
            {
                AddProperty("Фразеологизмы", m_Lexeme.sRestrictedForms());
            }

            if (m_Lexeme.bNoComparative())
            {
                AddProperty("Нет сравнительной степени", "");
            }

            if (m_Lexeme.bAssumedForms())
            {
                AddProperty("Есть гипотетические формы", "");
            }

            if (m_Lexeme.bHasIrregularForms())
            {
                AddProperty("Есть нерегулярные формы", "");
            }

            if (m_Lexeme.bHasIrregularVariants())
            {
                AddProperty("Есть нерегулярные варианты", "");
            }

            if (m_Lexeme.bHasDeficiencies())
            {
                AddProperty("Неполная парадигма", "");
            }

            if (m_Lexeme.bShortFormsRestricted())
            {
                AddProperty("Краткие формы затруднительны", "");
            }

            if (m_Lexeme.bShortFormsIncomplete())
            {
                AddProperty("Краткие формы ограничены", "");
            }

            if (m_Lexeme.bNoLongForms())
            {
                AddProperty("Нет полных форм", "");
            }

            if (m_Lexeme.bPastParticipleRestricted())
            {
                AddProperty("Прич. прош. страд. затрудн.", "");
            }

            if (m_Lexeme.bNoLongForms())
            {
                AddProperty("Нет прич. прошю страд.", "");
            }

            AddProperty("Графическая основа", m_Lexeme.sGraphicStem());

            string sMainSymbol = m_Lexeme.sMainSymbol();
            if (m_Lexeme.sAltMainSymbol().Length > 0)
            {
                sMainSymbol += "//";
                sMainSymbol += m_Lexeme.sAltMainSymbol();
                if (m_Lexeme.sAltMainSymbolComment().Length > 0)
                {
                    sMainSymbol += " " + m_Lexeme.sAltMainSymbolComment();
                }
            }
            AddProperty("Основной символ", sMainSymbol);

            AddProperty("Индекс", m_Lexeme.iType().ToString());

            var eAp1 = m_Lexeme.eAccentType1();
            string sRet = Helpers.sAccenTypeToStressSchema(eAp1);
            var eAp2 = m_Lexeme.eAccentType2();
            if (eAp2 != EM_AccentType.AT_UNDEFINED)
            {
                sRet += "/" + Helpers.sAccenTypeToStressSchema(eAp2);
            }
            AddProperty("Схема ударения", sRet);

            if (Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
            {
                if (m_Lexeme.bHasAspectPair())
                {
                    string sAspectPair = "";
                    string sAltAspectPair = "";
                    int iStressPos = -1;
                    m_Lexeme.eGetAspectPair(ref sAspectPair, ref iStressPos);
                    if (sAspectPair[iStressPos] != 'ё')
                    {
                        char chrMark = '\x301';
                        sAspectPair = sAspectPair.Insert(iStressPos + 1, chrMark.ToString());
                    }

                    if (m_Lexeme.bHasAltAspectPair())
                    {
                        int iAltStressPos = -1;
                        m_Lexeme.eGetAltAspectPair(ref sAltAspectPair, ref iAltStressPos);
                        if (sAltAspectPair[iAltStressPos] != 'ё')
                        {
                            char chrMark = '\x301';
                            sAltAspectPair = sAltAspectPair.Insert(iAltStressPos + 1, chrMark.ToString());
                        }
                    }

                    sAspectPair += ", ";
                    sAspectPair += sAltAspectPair;

                    AddProperty("Видовая пара", sAspectPair);
                }
            }


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
                    ShowNounFormsEvent?.Invoke(m_Lexeme);
                    break;

                case EM_PartOfSpeech.POS_ADJ:
                    ShowAdjFormsEvent?.Invoke(m_Lexeme);
                    break;

                case EM_PartOfSpeech.POS_VERB:
                    ShowVerbFormsEvent?.Invoke(m_Lexeme);
                    break;

                default:
                    MessageBox.Show("Internal error: illegal part of speech value.");
                    return;

            }   //  switch...

        }   // ShowParadigm()

        private void SaveParadigm(object arg)
        {
            try
            {
                EM_ReturnCode eRet = m_Lexeme.eGenerateParadigm();
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show("Не удалось породить парадигму.", "Zal");
                    return;
                }
                eRet = m_Lexeme.eSaveTestData();
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    MessageBox.Show("Ошибка при попытке сохранить парадигму.", "Zal");
                    return;
                }
                MessageBox.Show("Парадигма сохранена.", "Zal");
            }
            catch (Exception ex)
            {
                //                MainLib.ZalError err = new MainLib.ZalError();
                string sMsg = "LexemeDataPanel_Save: ";
                sMsg += ex.Message;
                MessageBox.Show(sMsg, "Zal Error");
                return;
            }
        }   // SaveParadigm()

        private void RemoveLexeme(object arg)
        {
            RemoveLexemeEvent?.Invoke(this);
        }

    }   //  public class LexemeViewModel 

}   //  namespace ZalTestApp
