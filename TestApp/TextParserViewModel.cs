using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class TextParserViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        MainModel m_MainModel = null;
        string m_sSourceText;

        private ICommand m_OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                return m_OpenFileCommand;
            }
            set
            {
                m_OpenFileCommand = value;
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

        private ICommand m_ParseTextCommand;
        public ICommand ParseTextCommand
        {
            get
            {
                return m_ParseTextCommand;
            }
            set
            {
                m_ParseTextCommand = value;
            }
        }

        string m_sMetaData;
        public string MetaData
        {
            get { return m_sMetaData; }
            set { m_sMetaData = value; }
        }

        string m_sDisplayText;
        public string DisplayText
        {
            get { return m_sDisplayText; }
            set { m_sDisplayText = value; }
        }

        string m_sParsedText;
        public string ParsedText
        {
            get { return m_sParsedText; }
            set { m_sParsedText = value; }
        }

        bool m_bReadyToParse = false;
        public bool ReadyToParse
        {
            get { return m_bReadyToParse;  }
            set { m_bReadyToParse = value; }
        }

        bool m_bEnableOpenFileBtn = true;
        public bool EnableOpenFileBtn
        {
            get { return m_bEnableOpenFileBtn; }
            set { m_bEnableOpenFileBtn = value; }
        }

        public TextParserViewModel(MainModel m)
        {
            m_MainModel = m;
            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            ParseTextCommand = new RelayCommand(new Action<object>(ParseText));
            CancelCommand = new RelayCommand(new Action<object>(Cancel));
            m_sParsedText = "";
            m_bReadyToParse = true;
        }

        public void OpenFile(Object obj)
        {
        }

        public void ParseText(Object obj)
        {
            m_sSourceText = DisplayText;
            m_MainModel.ParseText(m_sSourceText);
            IEnumerator<int> parseEnumerator = (IEnumerator<int>)m_MainModel.GetTextParseEnumerator();
            while (parseEnumerator.MoveNext())
            {
                int iWord = (int)parseEnumerator.Current;
                List<CWordFormManaged> listWf = m_MainModel.WordFormsFromWordPos(iWord);
                foreach (CWordFormManaged wf in listWf)
                {
                    m_sParsedText += GetWordFormWithStress(wf);
                    m_sParsedText += " ";
                }
            }

            DisplayText = m_sParsedText;

            m_bReadyToParse = false;

            OnPropertyChanged("DisplayText");
        }

        public void Cancel(Object obj)
        {
        }

        private string GetWordFormWithStress(CWordFormManaged wf)
        {
            if (null == wf)
            {
                return "";
            }

            string sWordForm = wf.sWordForm();
            string sWordFormWithStress = "";

            int iStressPos = -1;
            EM_StressType eStressType = EM_StressType.STRESS_TYPE_UNDEFINED;

            EM_ReturnCode eRet = wf.eGetFirstStressPos(ref iStressPos, ref eStressType);
            if (EM_ReturnCode.H_NO_ERROR == eRet && iStressPos < sWordForm.Length)
            {
                sWordFormWithStress = sWordForm.Substring(0, iStressPos);
                sWordFormWithStress += eStressType == EM_StressType.STRESS_PRIMARY ? "/" : @"\";
                sWordFormWithStress += sWordForm.Substring(iStressPos);
                Helpers.AssignDiacritics(sWordFormWithStress, ref sWordForm);
            }
            else
            {
                string sMsg = "Ошибка при считывании места ударения: ";
                sMsg += eRet.ToString();
//                MessageBox.Show(sMsg, "Zal Error");
            }

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                iStressPos = -1;
                eStressType = EM_StressType.STRESS_TYPE_UNDEFINED;
                eRet = wf.eGetNextStressPos(ref iStressPos, ref eStressType);
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
//                        MessageBox.Show(sMsg, "Zal Error");
                    }
                }
            }

            return sWordFormWithStress;

        }       //  GetWordFormWithStress()



    }       //  public class TextParserViewModel ...
}
