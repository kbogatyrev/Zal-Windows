using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using MainLibManaged;
using System.Collections.Generic;

namespace ZalTestApp
{
    public class LexemeProperty : ViewModelBase
    {
        private string m_sName;
        private string m_sValue;
        private string m_sSingleEntry;

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

        public string LexemePropertySingleEntry
        {
            get { return m_sSingleEntry; }
            set
            {
                m_sSingleEntry = value;
                OnPropertyChanged("LexemePropertySingleEntry");
            }
        }

    }

    public class LexemeSingleProperty : ViewModelBase
    {
        private string m_sValue;

        public string LexemeSinglePropertyValue
        {
            get { return m_sValue; }
            set
            {
                m_sValue = value;
                OnPropertyChanged("LexemeSinglePropertyValue");
            }
        }
    }

    public class LexemeViewModel : ViewModelBase
    {
        public ObservableCollection<LexemeProperty> LexemeDetails { get; private set; }
        public ObservableCollection<LexemeSingleProperty> LexemeProperties { get; private set; }

        public CLexemeManaged Lexeme
        {
            get
            {
                return m_Lexeme;
            }
        }

        private bool m_bReadOnly;
        public bool ReadOnly
        {
            get
            {
                return m_bReadOnly;
            }
        }

        public bool CanEdit
        {
            get
            {
                return !m_bReadOnly;
            }
        }

        public delegate void RemoveLexemeHandler(LexemeViewModel lvm);
        public event RemoveLexemeHandler RemoveLexemeEvent;

        public delegate void EditLexemeHandler(CLexemeManaged l);
        public event EditLexemeHandler EditLexemeEvent;

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

        private ICommand m_SaveRegressionCommand;
        public ICommand SaveRegressionCommand
        {
            get
            {
                return m_SaveRegressionCommand;
            }
            set
            {
                m_SaveRegressionCommand = value;
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

        private ICommand m_EditLexemeCommand;
        public ICommand EditLexemeCommand
        {
            get
            {
                return m_EditLexemeCommand;
            }
            set
            {
                m_EditLexemeCommand = value;
            }
        }
        #endregion

        public LexemeViewModel()
        {
            m_Lexeme = null;
            m_bReadOnly = false;
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));
            EditLexemeCommand = new RelayCommand(new Action<object>(EditLexeme));
            SaveRegressionCommand = new RelayCommand(new Action<object>(SaveRegression));

            LexemeDetails = new ObservableCollection<LexemeProperty>();
            LexemeProperties = new ObservableCollection<LexemeSingleProperty>();
        }


        public LexemeViewModel(CLexemeManaged l)
        {
            m_Lexeme = l;
            m_bReadOnly = false;
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));
            EditLexemeCommand = new RelayCommand(new Action<object>(EditLexeme));
            SaveRegressionCommand = new RelayCommand(new Action<object>(SaveRegression));

            LexemeDetails = new ObservableCollection<LexemeProperty>();
            LexemeProperties = new ObservableCollection<LexemeSingleProperty>();

            CollectLexemeProperties();
        }

        public LexemeViewModel(CLexemeManaged l, bool bReadOnly)
        {
            m_Lexeme = l;
            m_bReadOnly = bReadOnly;
            RemoveLexemeCommand = new RelayCommand(new Action<object>(RemoveLexeme));
            EditLexemeCommand = new RelayCommand(new Action<object>(EditLexeme));
            SaveRegressionCommand = new RelayCommand(new Action<object>(SaveRegression));

            LexemeDetails = new ObservableCollection<LexemeProperty>();
            LexemeProperties = new ObservableCollection<LexemeSingleProperty>();

            CollectLexemeProperties();
        }

        private void AddProperty(string sName, string sValue)
        {
            LexemeProperty lp = new LexemeProperty();
            lp.LexemePropertyName = sName;
            lp.LexemePropertyValue = sValue;
            LexemeDetails.Add(lp);
        }

        private void AddSingleProperty(string sSingleValue)
        {
            LexemeProperty lsp = new LexemeProperty();
            lsp.LexemePropertySingleEntry = sSingleValue;
            LexemeDetails.Add(lsp);
        }

        private void CollectLexemeProperties()
        {
            if (null == m_Lexeme)
            {
                return;
            }

            String sSourceForm = "";
            EM_ReturnCode eRet = m_Lexeme.eGetSourceFormWithStress(ref sSourceForm, false);
            if (eRet == EM_ReturnCode.H_NO_ERROR)
            {
                string sSourceFormWithAccents = "";
                Helpers.AssignDiacritics(sSourceForm, ref sSourceFormWithAccents);
                AddProperty("Исходная форма:", sSourceFormWithAccents);
            }

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
                AddProperty("Индекс омонима:", sHomonyms);
            }

            if (m_Lexeme.sHeadwordVariant().Length > 0)
            {
                String sVariant = "";
                eRet = m_Lexeme.eGetSourceFormWithStress(ref sVariant, true);
                string sSourceFormWithAccents = "";
                Helpers.AssignDiacritics(sVariant, ref sSourceFormWithAccents);

                AddProperty("Вариант:", sSourceFormWithAccents);
            }

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

            if (sMainSymbol != "")
            {
                AddProperty("Основной символ:", sMainSymbol);
            }

            string sInflectionSymbol = m_Lexeme.sInflectionType();
            if (sMainSymbol != sInflectionSymbol && sInflectionSymbol != "")
            {
                AddProperty("Словоизм. тип:", sInflectionSymbol);
            }

            if (m_Lexeme.iType() > 0)
            {
                AddProperty("Индекс:", m_Lexeme.iType().ToString());
            }

            if (m_Lexeme.bHasFleetingVowel())
            {
                AddSingleProperty("Есть беглая гласная");
            }

            var eAp1 = m_Lexeme.eAccentType1();
            if (eAp1 != EM_AccentType.AT_UNDEFINED)
            {
                string sRet = Helpers.sAccenTypeToStressSchema(eAp1);
                var eAp2 = m_Lexeme.eAccentType2();
                if (eAp2 != EM_AccentType.AT_UNDEFINED)
                {
                    sRet += "/" + Helpers.sAccenTypeToStressSchema(eAp2);
                }

                AddProperty("Схема ударения:", sRet);
            }

            if (Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
            {
                if (m_Lexeme.bHasAspectPair())
                {
                    try
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
                            sAspectPair += ", ";
                            sAspectPair += sAltAspectPair;
                        }
                        AddProperty("Видовая пара:", sAspectPair);
                    }
                    catch (Exception ex)
                    {
                        string sMsg = "Ошибка при порождении видовой пары: ";
                        sMsg += ex.Message;
                        MessageBox.Show(sMsg, "Zal Error");
                        return;
                    }
                }
            }

            if (m_Lexeme.sHeadwordComment().Length > 0)
            {
                AddProperty("Доп. помета (1):", m_Lexeme.sHeadwordComment());
            }

            if (m_Lexeme.sPluralOf().Length > 0)
            {
                AddProperty("Мн. от:", m_Lexeme.sPluralOf());
            }

            if (m_Lexeme.sUsage().Length > 0)
            {
                AddProperty("Доп. помета (2):", m_Lexeme.sUsage());
            }

            if (m_Lexeme.sSeeRef().Length > 0)
            {
                AddProperty("См. также:", m_Lexeme.sSeeRef());
            }

            if (m_Lexeme.sTrailingComment().Length > 0)
            {
                AddProperty("Доп. помета (3):", m_Lexeme.sTrailingComment());
            }

            if (m_Lexeme.sRestrictedForms().Length > 0)
            {
                string sPhraseo = m_Lexeme.sRestrictedForms();
                string sSourceFormWithAccents = "";
                Helpers.AssignDiacritics(sPhraseo, ref sSourceFormWithAccents);
                AddProperty("Фразеологизмы:", sSourceFormWithAccents);
            }

            if (m_Lexeme.bNoComparative())
            {
                AddSingleProperty("Нет сравнительной степени");
            }

            if (m_Lexeme.bAssumedForms())
            {
                AddSingleProperty("Есть гипотетические формы");
            }

            if (m_Lexeme.bHasIrregularForms())
            {
                AddSingleProperty("Есть нерегулярные формы");
            }

            if (m_Lexeme.bHasIrregularVariants())
            {
                AddSingleProperty("Есть нерегулярные варианты");
            }

            //if (m_Lexeme.bHasDeficiencies())
            //{
            //    AddSingleProperty("Неполная парадигма");
            //}

            if (m_Lexeme.bShortFormsRestricted())
            {
                AddSingleProperty("Краткие формы затруднительны");
            }

            if (m_Lexeme.bShortFormsIncomplete())
            {
                AddSingleProperty("Краткие формы ограничены");
            }

            if (m_Lexeme.bNoLongForms())
            {
                AddSingleProperty("Нет полных форм");
            }

            if (m_Lexeme.bPastParticipleRestricted())
            {
                AddSingleProperty("Прич. прош. страд. затрудн.");
            }

            if (m_Lexeme.bNoPassivePastParticiple())
            {
                AddSingleProperty("Нет прич. прош. страд.");
            }

            if (m_Lexeme.iStemAugment() > 0)
            {
                AddProperty("Аугмент:", m_Lexeme.iStemAugment().ToString());
            }

            List<int> lCommonDeviations = new List<int>();
            string sCommonDeviations = "";
            for (int iCD = 1; iCD <= 9; ++iCD)
            {
                if (m_Lexeme.bHasCommonDeviation(iCD))
                {
                    if (sCommonDeviations.Length > 0)
                    {
                        sCommonDeviations += ", ";
                    }
                    sCommonDeviations += iCD.ToString();
                    if (m_Lexeme.bDeviationOptional(iCD))
                    {
                        sCommonDeviations += " (вариант)";
                    }
                }
            }

            if (sCommonDeviations.Length > 0)
            {
                AddProperty("Отклонения: ", sCommonDeviations);
            }

            if (m_Lexeme.iSection() > 0)
            {
                AddProperty("Доп. особенности:", String.Format("§{0}", m_Lexeme.iSection()));
            }

//            AddProperty("Графическая основа", m_Lexeme.sGraphicStem());

            if (Lexeme.bAssumedForms())
            {
                if (Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NOUN)
                {
                    AddSingleProperty("Формы мн. ч. предположительны");
                }
                else if (Lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_ADJ)
                {
                    AddSingleProperty("Краткая форма мн. ч. предположительна");
                }
                else
                {
                    string sMsg = "Пoмета \"-\" (минус) не ожидается";
                    MessageBox.Show(sMsg, "Zal Error");
                }
            }

            //if (Lexeme.sQuestionableForms() != "")
            //{
            //    AddSingleProperty(Lexeme.sQuestionableForms());
            //}

            //if (Lexeme.bHasDeficiencies())
            //{
            //    AddSingleProperty(Lexeme.sQuestionableForms());
            //}
        }       //  CollectLexemeProperties()

        private void SaveRegression(object arg)
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
        }  

        private void RemoveLexeme(object arg)
        {
            RemoveLexemeEvent?.Invoke(this);
        }

        private void EditLexeme(object arg)
        {
            EditLexemeEvent?.Invoke(this.m_Lexeme);
        }
    }   //  public class LexemeViewModel 

}   //  namespace ZalTestApp
