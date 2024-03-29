﻿using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Collections;

using MainLibManaged;
using System.Collections.Generic;
using System.Threading;

namespace ZalTestApp
{
    public class RegressionGridViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        #region Accessors_and_Mutators
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

        private DataView m_CurrentView;
        public DataView CurrentView
        {
            get
            {
                return m_CurrentView;
            }

            set
            {
                m_CurrentView = value;
            }
        }

        private int m_iIdx;
        public int CurrentIdx
        {
            get
            {
                return m_iIdx;
            }

            set
            {
                m_iIdx = value;
                OnPropertyChanged("CurrentIdx");
            }
        }

        private string m_sCurrentLexHash;
        public string CurrentLexHash
        {
            get
            {
                return m_sCurrentLexHash;
            }
            set
            {
                m_sCurrentLexHash = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return IsRowChecked(m_sCurrentLexHash);
            }
        }

        public int NLexemes
        {
            get
            {
                //                return m_RegressionData.Rows.Count;
                return m_CurrentView.Count;
            }
        }

        private bool m_bCancelBatchVerifier;
        public bool CancelVerifier
        {
            get
            {
                return m_bCancelBatchVerifier;
            }

            set
            {
                m_bCancelBatchVerifier = value;
            }
        }

        public void SetResult(string sLexHash, string sText)
        {
            try
            {
                string sFilter = "LexemeHash='" + sLexHash + "'";
                string sSortOrder = "SourceForm ASC";

                DataRow[] arrFoundRows = m_RegressionData.Select(sFilter, sSortOrder, DataViewRowState.CurrentRows);
                if (arrFoundRows.Length > 0)
                {
                    DataRow row = arrFoundRows[0];
                    row["TestResult"] = sText;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to change result value: " + ex.Message);
                return;
            }
        }

        public bool IsRowChecked(string sLexHash)
        {
            try
            {
                string sFilter = "LexemeHash='" + sLexHash + "'";
                string sSortOrder = "SourceForm ASC";

                DataRow[] arrFoundRows = m_RegressionData.Select(sFilter, sSortOrder, DataViewRowState.CurrentRows);
                if (arrFoundRows.Length > 0)
                {
                    DataRow row = arrFoundRows[0];
                    return (bool)row["IsChecked"];
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to retrieve selection status value: " + ex.Message);
                return false;
            }
        }

        public void CheckRow(string sLexHash)
        {
            try
            {
                string sFilter = "LexemeHash='" + sLexHash + "'";
                string sSortOrder = "SourceForm ASC";

                DataRow[] arrFoundRows = m_RegressionData.Select(sFilter, sSortOrder);
                if (arrFoundRows.Length > 0)
                {
                    DataRow row = arrFoundRows[0];
                    row["IsChecked"] = true;
                    OnPropertyChanged("IsChecked");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to set row state: " + ex.Message);
            }
        }

        public void UnheckRow(string sLexHash)
        {
            try
            {
                string sFilter = "LexemeHash='" + sLexHash + "'";
                string sSortOrder = "SourceForm ASC";

                DataRow[] arrFoundRows = m_RegressionData.Select(sFilter, sSortOrder, DataViewRowState.CurrentRows);
                if (arrFoundRows.Length > 0)
                {
                    DataRow row = arrFoundRows[0];
                    row["IsChecked"] = false;
                    OnPropertyChanged("IsChecked");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to set row state: " + ex.Message);
            }
        }

        public void CheckAll()
        {
            foreach (DataRow row in m_RegressionData.Rows)
            {
                row["IsChecked"] = true;
                OnPropertyChanged("IsChecked");
            }
        }

        public void UncheckAll()
        {
            foreach (DataRow row in m_RegressionData.Rows)
            {
                row["IsChecked"] = false;
                OnPropertyChanged("IsChecked");
            }
        }

        public string LexemeHash(int iRow)
        {
            try
            {
                DataRow row = m_RegressionData.Rows[iRow];
                return (string)row["LexemeHash"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to retrieve lexeme hash value: " + ex.Message);
                return "";
            }
        }

        public string SourceForm(int iRow)
        {
            try
            {
                DataRow row = m_RegressionData.Rows[iRow];
                return (string)row["SourceForm"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: unable to retrieve source form value: " + ex.Message);
                return "";
            }
        }

        public int Rows()
        {
            return m_RegressionData.Rows.Count;
        }

        private int m_iProgress = 0;
        public int Progress
        {
            get
            {
                return m_iProgress;
            }

            set
            {
                m_iProgress = value;
            }
        }
        #endregion

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
            colCheckbox.ColumnName = "IsChecked";
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
                var lexHashToHeadWord = (KeyValuePair<string, string>)storedLexEnumerator.Current;
                DataRow row = m_RegressionData.NewRow();
                row["SourceForm"] = lexHashToHeadWord.Value;
                row["LexemeHash"] = lexHashToHeadWord.Key;
                row["IsChecked"] = true;
                m_RegressionData.Rows.Add(row);
            }

            m_CurrentView = new DataView(m_RegressionData);
            m_CurrentView.Sort = "SourceForm ASC";
        }       //  RegressionGridViewModel()

        public void GoBack(Object obj)
        {
//            m_MainModel.Clear();
            BackButtonEvent?.Invoke();
        }

        public void RunTest(Object obj)
        {
            try
            {
                VerifierThread vt = new VerifierThread(this);
                System.Threading.Thread t = new Thread(new ThreadStart(vt.ThreadProc));
                t.Name = "TestApp batch verifier thread";
                t.IsBackground = true;
                //              m_WorkerThread.Priority = ThreadPriority.Lowest;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                //                t.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return;
            }
        }

        public void Save(Object obj)
        {
            MessageBox.Show("Save");
        }

        public void Delete(Object obj)
        {
            try
            {
                string sFilter = "IsChecked='true'";
                string sSortOrder = "SourceForm ASC";
                DataRow[] arrFoundRows = m_RegressionData.Select(sFilter, sSortOrder, DataViewRowState.CurrentRows);
                if (arrFoundRows.Length > 1)
                {
                    MessageBox.Show("Удалять проверенные слова можно только по одному.");
                    return;
                }

                if (arrFoundRows.Length < 1)
                {
                    MessageBox.Show("Ошибка: слово не найдено.");
                    return;
                }

                string sLexemeHash = (string)arrFoundRows[0]["LexemeHash"];
                EM_TestResult eTestResult = EM_TestResult.TEST_RESULT_UNDEFINED;
                var eRet = m_MainModel.DeleteSavedLexeme(sLexemeHash, ref eTestResult);
                arrFoundRows[0].Delete();

            }   //  foreach (DataGridViewRow row in dataGridView.Rows)
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }
        }
 
        public void Cancel(Object obj)
        {
            MessageBox.Show("Cancel");
        }

        public void UpdateProgress(int iProgress, bool bComplete)
        {

        }

        public EM_ReturnCode Verify(string sLexHash, ref EM_TestResult eTestResult)
        {
            var eRet = m_MainModel.VerifyLexeme(sLexHash, ref eTestResult);
            return eRet;
        }

    }   //  class RegressionGridViewModel

    #region VerifierThread
    public class VerifierThread
    {
        private RegressionGridViewModel m_Caller;

        public VerifierThread(RegressionGridViewModel rvm)
        {
            m_Caller = rvm;
        }

        public void ThreadProc()
        {
            try
            {
//                for (int iLexeme = 0; iLexeme < m_Caller.NLexemes; ++iLexeme)
                foreach (DataRowView rowView in m_Caller.CurrentView)
                {
                    string sLexHash = (string)rowView.Row["LexemeHash"];
                    if (m_Caller.CancelVerifier)
                    {
                        m_Caller.SetResult(sLexHash, "Cancelled");
                        break;
                    }

                    if (!m_Caller.IsRowChecked(sLexHash))
                    {
                        m_Caller.SetResult(sLexHash, "Ignored");
                        continue;
                    }

                    EM_TestResult eTestResult = EM_TestResult.TEST_RESULT_UNDEFINED;
                    EM_ReturnCode eRet = m_Caller.Verify(sLexHash, ref eTestResult);
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        MessageBox.Show(string.Format("Unable to verify lexeme hash {0}", sLexHash));
                    }

                    switch (eTestResult)
                    {
                        case EM_TestResult.TEST_RESULT_OK:
                        {
                            m_Caller.SetResult(sLexHash, "Pass");
                            break;
                        }
                        case EM_TestResult.TEST_RESULT_FAIL:
                        {
                            m_Caller.SetResult(sLexHash, "Fail");
                            break;
                        }
                        case EM_TestResult.TEST_RESULT_INCOMPLETE:
                        {
                            m_Caller.SetResult(sLexHash, "Missing forms");
                            break;
                        }
                        default:
                        {
                            //                            MainLib.ZalError err = new MainLib.ZalError();
                            string sMsg = "Unexpected return from IVerifier; error msg: ";
                            //                            sMsg += err.LastError;
                            sMsg += eTestResult.ToString();
                            MessageBox.Show(sMsg, "Zal Error");
                            break;
//                            return;
                        }

                    }       //  switch ...

                }   //  foreach (DataGridViewRow row in dataGridView.Rows)
            }
            catch (Exception ex)
            {
                //                MainLib.ZalError err = new MainLib.ZalError();
                string sMsg = ex.Message;
                sMsg += "\n";
                //                sMsg += err.LastError;
                MessageBox.Show(sMsg, "Error");
                return;
            }

        }   //  ThreadProc()

    }   //  public class VerifierThread
    #endregion

}
