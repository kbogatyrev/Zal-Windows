using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Text;

namespace ZalTestApp
{
    public class TextParserViewModel : ViewModelBase
    {
        MainModel m_MainModel = null;
        string m_sTextName;
        string m_sSourceText;
        string m_sSourceMetadata;

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

        string m_sDisplayTextName = "Введите рабочее название текста (слово или сокращение без пробелов), напр. \"Некрасов_дедушка_1-2\".";
        public string DisplayTextName
        {
            get { return m_sDisplayTextName; }
            set { m_sDisplayTextName = value; }
        }
        
        string m_sDisplayMetaData = "Введите краткое описание текста, напр.: Барков И.С. Девичья игрушка. Детгиз, М. 1949.";
        public string DisplayMetaData
        {
            get { return m_sDisplayMetaData; }
            set { m_sDisplayMetaData = value; }
        }

        string m_sDisplayText = "Введите, скопируйте, или откройте текст для разбора.";
        public string DisplayText
        {
            get { return m_sDisplayText; }
            set { m_sDisplayText = value; OnPropertyChanged("DisplayText");  }
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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;
            if (true == openFileDialog.ShowDialog())
            {
                var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    DisplayText = streamReader.ReadToEnd();
                }
            }

            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
//                DbOpen = true;
            }
        }

        public void ParseText(Object obj)
        {
            m_sSourceText = DisplayText;
            m_sSourceMetadata = DisplayMetaData;
            m_MainModel.ParseText(m_sDisplayTextName, m_sSourceMetadata, m_sSourceText);
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
