using MainLibManaged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace ZalTestApp
{
    #region Lexeme
    public class Lexemes
    {
        private List<CLexemeManaged> m_Lexemes;
        public int NLexemes
        {
            get { return m_Lexemes.Count; }
        }

        public void Add(CLexemeManaged l)
        {
            m_Lexemes.Add(l);
        }

        public Lexemes()
        {
            m_Lexemes = new List<CLexemeManaged>();
        }

        public CLexemeManaged this[int index]
        {
            get
            {
                return m_Lexemes[index];
            }

            set
            {
                m_Lexemes[index] = value;
            }
        }
    }
    #endregion

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

        public Lexemes m_Lexemes = new Lexemes();
        public int NLexemes
        {
            get
            {
                return m_Lexemes.NLexemes;
            }
        }

        public CLexemeManaged GetLexemeAt(int iAt)
        {
            if (iAt < 0 || iAt >= m_Lexemes.NLexemes)
            {
                return null;
            }
            return m_Lexemes[iAt];
        }

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
                var eRet = m_Dictionary.eSetDbPath(Path);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    Path = "";
                    System.Windows.MessageBox.Show("Unable to open dictionary.");
                }
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
            eRet = m_Dictionary.eGetFirstLexeme(ref l);
            if (EM_ReturnCode.H_NO_ERROR != eRet)
            {
                System.Windows.MessageBox.Show("Lexeme not found.");
                return;
            }

            do
            {
                m_Lexemes.Add(l);
                eRet = m_Dictionary.eGetNextLexeme(ref l);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
                System.Windows.MessageBox.Show("Error accessing lexeme collection.");
                return;
            }
        }

    }
}       //  namespace ZalTestApp
