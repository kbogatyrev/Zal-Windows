using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

using MainLibManaged;
using System.Collections.Generic;

namespace ZalTestApp
{
    public class WordFormProperty : ViewModelBase
    {
        private string m_sName;
        private string m_sValue;

        public string WordFormPropertyName
        {
            get { return m_sName; }
            set
            {
                m_sName = value;
                OnPropertyChanged("WordFormPropertyName");
            }
        }

        public string WordFormPropertyValue
        {
            get { return m_sValue; }
            set
            {
                m_sValue = value;
                OnPropertyChanged("WordFormPropertyValue");
            }
        }
    }

    public class WordFormViewModel : ViewModelBase
    {
        public string sPosToString(EM_PartOfSpeech ePos)
        {
            switch (ePos)
            {
                case EM_PartOfSpeech.POS_NOUN:
                    return "Существительное";
                case EM_PartOfSpeech.POS_VERB:
                    return "Глагол";
                case EM_PartOfSpeech.POS_ADJ:
                    return "Прилагательное";
                case EM_PartOfSpeech.POS_PRONOUN:
                    return "Местоимение";
                case EM_PartOfSpeech.POS_PRONOUN_ADJ:
                    return "Местоименное прилагательное";
                case EM_PartOfSpeech.POS_PRONOUN_PREDIC:
                    return "Местоимение";                   // ???????
                case EM_PartOfSpeech.POS_NUM:
                    return "Числительное";
                case EM_PartOfSpeech.POS_NUM_ADJ:
                    return "Порядковое числительное (счетное прилагательное)";
                case EM_PartOfSpeech.POS_ADV:
                    return "Наречие";
                case EM_PartOfSpeech.POS_COMPAR:
                    return "Сравнительная степень";
                case EM_PartOfSpeech.POS_PREDIC:
                    return "Предикатив";
                case EM_PartOfSpeech.POS_PREPOSITION:
                    return "Предлог";
                case EM_PartOfSpeech.POS_CONJUNCTION:
                    return "Союз";
                case EM_PartOfSpeech.POS_PARTICLE:
                    return "Частица";
                case EM_PartOfSpeech.POS_INTERJ:
                    return "Междометие";
                default:
                    return "Ошибка при считывании части речи";
            }
        }      // sPosToString()

        public static string sSubparadigmToString(EM_Subparadigm eSp)
        {
            switch (eSp)
            {
                case EM_Subparadigm.SUBPARADIGM_NOUN:
                    return "Существительное";
                case EM_Subparadigm.SUBPARADIGM_LONG_ADJ:
                    return "Прилагательное, полная форма";
                case EM_Subparadigm.SUBPARADIGM_SHORT_ADJ:
                    return "Прилагательное, краткая форма";
                case EM_Subparadigm.SUBPARADIGM_COMPARATIVE:
                    return "Прилагательное, сравнительная степень";
                case EM_Subparadigm.SUBPARADIGM_PRONOUN:
                    return "Местоимение";
                case EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ:
                    return "Местоимение-прилагательное";
                case EM_Subparadigm.SUBPARADIGM_INFINITIVE:
                    return "Инфинитив";
                case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                    return "Настоящее время";
                case EM_Subparadigm.SUBPARADIGM_PAST_TENSE:
                    return "Прошедшее время";
                case EM_Subparadigm.SUBPARADIGM_IMPERATIVE:
                    return "Повелительное наклонение";
                case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT:
                    return "Деепричастие настоящего времени";
                case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                    return "Деепричастие прошедшего времени";
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    return "Действительное причастие настоящего времени";
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    return "Страдательное причастие настоящего времени, полная форма";
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                    return "Страдательное причастие настоящего времени, краткая форма";
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    return "Действительное причастие прошедшего времени";
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    return "Страдательное причастие прошедшего времени, полная форма";
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                    return "Страдательное причастие прошедшего времени, краткая форма";
                case EM_Subparadigm.SUBPARADIGM_NUM_2TO4:
                    return "Числительное";
                case EM_Subparadigm.SUBPARADIGM_NUM:
                    return "Числительное";
                case EM_Subparadigm.SUBPARADIGM_NUM_ADJ:
                    return "Порядковое числительное";
                case EM_Subparadigm.SUBPARADIGM_ASPECT_PAIR:
                    return "Видовая пара";
                case EM_Subparadigm.SUBPARADIGM_UNDEFINED:
                default:
                    return "Undefined";
            }
        }

        public string sGenderToString(EM_Gender eGender)
        {
            switch (eGender)
            {
                case EM_Gender.GENDER_M:
                    return "Мужской";
                case EM_Gender.GENDER_F:
                    return "Женский";
                case EM_Gender.GENDER_N:
                    return "Средний";
                default:
                    return "Ошибка при считывании рода";
            }
        }      // sGenderToString()

        public string sAnimacyToString(EM_Animacy eAnimacy)
        {
            switch (eAnimacy)
            {
                case EM_Animacy.ANIM_YES:
                    return "Одуш.";
                case EM_Animacy.ANIM_NO:
                    return "Неод.";
                default:
                    return "Ошибка при считывании одушевленности";
            }
        }      // sAnimacyToString()

        public string sNumberToString(EM_Number eNumber)
        {
            switch (eNumber)
            {
                case EM_Number.NUM_SG:
                    return "Единственное";
                case EM_Number.NUM_PL:
                    return "Множественное";
                default:
                    return "Ошибка при считывании числа";
            }
        }      // sNumberToString()

        public string sCaseToString(EM_Case eCase)
        {
            switch (eCase)
            {
                case EM_Case.CASE_NOM:
                    return "Именительный";
                case EM_Case.CASE_ACC:
                    return "Винительный";
                case EM_Case.CASE_GEN:
                    return "Родительный";
                case EM_Case.CASE_PART:
                    return "Родительный (партитив)";
                case EM_Case.CASE_PREP:
                    return "Предложный";
                case EM_Case.CASE_LOC:
                    return "Предложный (локатив)";
                case EM_Case.CASE_DAT:
                    return "Дательный";
                case EM_Case.CASE_INST:
                    return "Творительный";
                default:
                    return "Ошибка при считывании числа";
            }
        }      // sNumberToString()

        public string sPersonToString(EM_Person ePerson)
        {
            switch (ePerson)
            {
                case EM_Person.PERSON_1:
                    return "Первое";
                case EM_Person.PERSON_2:
                    return "Второе";
                case EM_Person.PERSON_3:
                    return "Третье";
                default:
                    return "Ошибка при считывании лица";
            }
        }      // sNumberToString()

        public ObservableCollection<WordFormProperty> WordFormDetails { get; private set; }

        private CWordFormManaged m_WordForm = null;
        public CWordFormManaged WordForm
        {
            get
            {
                return m_WordForm;
            }
        }

        public delegate void RemoveWordFormHandler(WordFormViewModel lvm);
        public event RemoveWordFormHandler RemoveWordFormEvent;

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

        private ICommand m_RemoveWordFormCommand;
        public ICommand RemoveWordFormCommand
        {
            get
            {
                return m_RemoveWordFormCommand;
            }
            set
            {
                m_RemoveWordFormCommand = value;
            }
        }

        #endregion

        public WordFormViewModel()
        {
            m_WordForm = null;
//            RemoveWordFormCommand = new RelayCommand(new Action<object>(RemoveWordForm));
        }

        public WordFormViewModel(CWordFormManaged wf)
        {
            m_WordForm = wf;
//            RemoveWordFormCommand = new RelayCommand(new Action<object>(RemoveWordForm));

            WordFormDetails = new ObservableCollection<WordFormProperty>();

            CollectWordFormProperties();
        }

        private void AddProperty(string sName, string sValue)
        {
            WordFormProperty wfp = new WordFormProperty();
            wfp.WordFormPropertyName = sName;
            wfp.WordFormPropertyValue = sValue;
            WordFormDetails.Add(wfp);
        }

        private void CollectWordFormProperties()
        {
            if (null == m_WordForm)
            {
                return;
            }

            String sWordForm = m_WordForm.sWordForm();

            int iStressPos = -1;
            EM_StressType eStressType = EM_StressType.STRESS_TYPE_UNDEFINED;
            int iRightShift = 0;

            EM_ReturnCode eRet = m_WordForm.eGetFirstStressPos(ref iStressPos, ref eStressType);
            if (EM_ReturnCode.H_NO_ERROR == eRet && iStressPos < sWordForm.Length)
            {
                string sWfWithAccents = sWordForm.Substring(0, iStressPos);
                sWfWithAccents += eStressType == EM_StressType.STRESS_PRIMARY ? "/" : @"\";
                sWfWithAccents += sWordForm.Substring(iStressPos);
                Helpers.AssignDiacritics(sWfWithAccents, ref sWordForm);
                ++iRightShift;
            }
            else
            {
                string sMsg = "Ошибка при считывании места ударения: ";
                sMsg += eRet.ToString();
                MessageBox.Show(sMsg, "Zal Error");
            }

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                iStressPos = -1;
                eStressType = EM_StressType.STRESS_TYPE_UNDEFINED;
                eRet = m_WordForm.eGetNextStressPos(ref iStressPos, ref eStressType);
                if (EM_ReturnCode.H_NO_ERROR == eRet && iStressPos < sWordForm.Length)
                {
                    string sWfWithAccents = sWordForm.Substring(0, iStressPos+iRightShift);
                    sWfWithAccents += eStressType == EM_StressType.STRESS_PRIMARY ? "/" : @"\";
                    sWfWithAccents += sWordForm.Substring(iStressPos+iRightShift);
                    Helpers.AssignDiacritics(sWfWithAccents, ref sWordForm);
                    ++iRightShift;
                }
                else
                {
                    if (eRet != EM_ReturnCode.H_NO_MORE)
                    {
                        string sMsg = "Ошибка при считывании места ударения (2): ";
                        sMsg += eRet.ToString();
                        MessageBox.Show(sMsg, "Zal Error");
                    }
                }
            }

            AddProperty("Словоформа:", sWordForm);
//            AddProperty("Дескриптор:", m_WordForm.sGramHash());
            AddProperty("Часть речи:", sPosToString(m_WordForm.ePos()));
            if (EM_PartOfSpeech.POS_NOUN == m_WordForm.ePos())
            {
                AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                AddProperty("Падеж:", sCaseToString(m_WordForm.eCase()));
            }

            if (EM_PartOfSpeech.POS_ADJ == m_WordForm.ePos())
            {
                AddProperty("Род:", sGenderToString(m_WordForm.eGender()));
                AddProperty("Одушевленность:", sAnimacyToString(m_WordForm.eAnimacy()));
                AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                AddProperty("Падеж:", sCaseToString(m_WordForm.eCase()));
            }

            if (EM_PartOfSpeech.POS_VERB == m_WordForm.ePos())
            {
                AddProperty("Субпарадигма:", sSubparadigmToString(m_WordForm.eSubparadigm()));

                switch (m_WordForm.eSubparadigm())
                {
                    case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                        AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                        AddProperty("Лицо:", sPersonToString(m_WordForm.ePerson()));
                        break;
                    case EM_Subparadigm.SUBPARADIGM_PAST_TENSE:
                        AddProperty("Род:", sGenderToString(m_WordForm.eGender()));
                        AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                        break;
                    case EM_Subparadigm.SUBPARADIGM_IMPERATIVE:
                        AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                        AddProperty("Лицо:", sPersonToString(m_WordForm.ePerson()));
                        break;
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                        AddProperty("Падеж:", sCaseToString(m_WordForm.eCase()));
                        AddProperty("Род:", sGenderToString(m_WordForm.eGender()));
                        AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                        break;
                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                        if (EM_Number.NUM_SG == m_WordForm.eNumber())
                        {
                            AddProperty("Род:", sGenderToString(m_WordForm.eGender()));
                        }
                        AddProperty("Число:", sNumberToString(m_WordForm.eNumber()));
                        break;
                    case EM_Subparadigm.SUBPARADIGM_UNDEFINED:
                    default:
                        break;
                }
            }

        }       //  CollectLexemeProperties()

        private void RemoveWordForm(object arg)
        {
            RemoveWordFormEvent?.Invoke(this);
        }

    }   //  public class WordFormViewModel 

}   //  namespace ZalTestApp
