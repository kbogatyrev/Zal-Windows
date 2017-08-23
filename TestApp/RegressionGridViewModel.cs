using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Collections;

using MainLibManaged;
using System.Collections.Generic;

namespace ZalTestApp
{
    public class RegressionGridViewModel : ViewModelBase
    {
        MainModel m_MainModel = null;

        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        private DataTable m_RegressionData;
        public DataTable RegressionData
        {
            get
            {
                return m_RegressionData;
            }

            set
            {
                m_RegressionData = value;
            }
        }

        #region ICommand
        private ICommand m_BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return m_BackCommand;
            }
            set
            {
                m_BackCommand = value;
            }
        }
        private ICommand m_RunTestCommand;
        public ICommand RunTestCommand
        {
            get
            {
                return m_RunTestCommand;
            }
            set
            {
                m_RunTestCommand = value;
            }
        }
        private ICommand m_SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return m_SaveCommand;
            }
            set
            {
                m_SaveCommand = value;
            }
        }
        private ICommand m_DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return m_DeleteCommand;
            }
            set
            {
                m_DeleteCommand = value;
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
        #endregion

        public RegressionGridViewModel(MainModel mm)
        {
            m_MainModel = mm;
 
            RunTestCommand = new RelayCommand(new Action<object>(RunTest));
            SaveCommand = new RelayCommand(new Action<object>(Save));
            DeleteCommand = new RelayCommand(new Action<object>(Delete));
            CancelCommand = new RelayCommand(new Action<object>(Cancel));
            BackCommand = new RelayCommand(new Action<object>(GoBack));

            bool bRet = m_MainModel.GetStoredLexemeData();
            if (!bRet)
            {
                MessageBox.Show("Unable to load regression data.");
                return;
            }

            m_RegressionData = new DataTable("RegressionTable");

            DataColumn colCheckbox = new DataColumn();
            colCheckbox.DataType = Type.GetType("System.Boolean");
            colCheckbox.ColumnName = "Selected";
            m_RegressionData.Columns.Add(colCheckbox);

            DataColumn colHeadWord = new DataColumn();
            colHeadWord.DataType = Type.GetType("System.String");
            colHeadWord.ColumnName = "SourceForm";
            m_RegressionData.Columns.Add(colHeadWord);

            DataColumn colLexHash = new DataColumn();
            colLexHash.DataType = Type.GetType("System.String");
            colLexHash.ColumnName = "LexemeHash";
            m_RegressionData.Columns.Add(colLexHash);

            DataColumn colTestResult = new DataColumn();
            colTestResult.DataType = Type.GetType("System.String");
            colTestResult.ColumnName = "TestResult";
            m_RegressionData.Columns.Add(colTestResult);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = m_RegressionData.Columns["LexemeHash"];
            m_RegressionData.PrimaryKey = PrimaryKeyColumns;

            IEnumerator storedLexEnumerator = m_MainModel.GetStoredLexemesEnumerator();
            while (storedLexEnumerator.MoveNext())
            {
                var headWordToHash = (KeyValuePair<string, string>)storedLexEnumerator.Current;
                DataRow row = m_RegressionData.NewRow();
                row["SourceForm"] = headWordToHash.Key;
                row["LexemeHash"] = headWordToHash.Value;
                row["Selected"] = false;
                m_RegressionData.Rows.Add(row);
            }
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void RunTest(Object obj)
        {
            MessageBox.Show("RunTest");
            var row = m_RegressionData.Rows[1];
            row["TestResult"] = "Pass";
        }

        public void Save(Object obj)
        {
            MessageBox.Show("Save");
        }

        public void Delete(Object obj)
        {
            MessageBox.Show("Delete");
        }

        public void Cancel(Object obj)
        {
            MessageBox.Show("Cancel");
        }

    }
}
