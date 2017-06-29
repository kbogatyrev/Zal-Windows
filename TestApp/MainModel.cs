using MainLibManaged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace ZalTestApp
{
    public class MainModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        string m_sPath;
        public string Path
        {
            get
            {
                return m_sPath;
            }
            set
            {
                m_sPath = value;
                OnPropertyChanged("Path");
            }
        }

        public CDictionaryManaged m_Dictionary = new CDictionaryManaged();
        public CDictionaryManaged Engine
        {
            get
            {
                return m_Dictionary;
            }
        }

        private List<CLexemeManaged> m_Lexemes;

        public void OpenDictionary(string str)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|SQLite 3 files (*.db3)|*.db3";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialog1.FileName;
            }
        }

        public void SearchByInitalForm(string str)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return;
            }

            var eRet = m_Dictionary.eGetLexemesByInitialForm(str);
            CLexemeManaged l = null;
            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                eRet = m_Dictionary.eGetFirstLexeme(ref l);
            }
            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                m_Lexemes.Add(l);
                eRet = m_Dictionary.eGetFirstLexeme(ref l);
            }

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
                System.Windows.MessageBox.Show("Error accessing lexeme collection.");
            }
        }

    }
}