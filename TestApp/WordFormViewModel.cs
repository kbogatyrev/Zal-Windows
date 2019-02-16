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

            EM_ReturnCode eRet = m_WordForm.eGetFirstStressPos(ref iStressPos, ref eStressType);
            if (EM_ReturnCode.H_NO_ERROR == eRet && iStressPos < sWordForm.Length)
            {
                string sWfWithAccents = sWordForm.Substring(0, iStressPos);
                sWfWithAccents += eStressType == EM_StressType.STRESS_PRIMARY ? "/" : @"\";
                sWfWithAccents += sWordForm.Substring(iStressPos);
                Helpers.AssignDiacritics(sWfWithAccents, ref sWordForm);
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
                    string sWfWithAccents = sWordForm.Substring(0, iStressPos);
                    sWfWithAccents += eStressType == EM_StressType.STRESS_PRIMARY ? "/" : @"\";
                    sWfWithAccents += sWordForm.Substring(iStressPos);
                    Helpers.AssignDiacritics(sWfWithAccents, ref sWordForm);
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

            AddProperty("Словоформа", sWordForm);
            AddProperty("Дескриптор", m_WordForm.sGramHash());

        }       //  CollectLexemeProperties()

        private void RemoveWordForm(object arg)
        {
            RemoveWordFormEvent?.Invoke(this);
        }

    }   //  public class WordFormViewModel 

}   //  namespace ZalTestApp
