﻿using MainLibManaged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ZalTestApp
{
    public class MainModel : INotifyPropertyChanged
    {
        private CDictionaryManaged m_Dictionary = null;
        private CParserManaged m_Parser = null;
        private CAnalyticsManaged m_Analytics = null;

        private CVerifierManaged m_Verifier = null;
        public CVerifierManaged Verifier()
        {
            return m_Verifier;
        }

        private Dictionary<string, Dictionary<string, List<CWordFormManaged>>> m_dctHashToWordform;  // hash to (gram hash --> forms)
        public IEnumerator GetEnumerator()
        {
            return m_dctHashToWordform.Keys.GetEnumerator();
        }

        private Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> m_dctFormComments;
        //                           ^-- lex. hash       ^-- gram hash     ^-- leading comment, trailing comment))

        private Dictionary<string, CInflectionManaged> m_dctHashToEntry;
        public CInflectionManaged EntryFromHash(string sHash)
        {
            CInflectionManaged entry;
            m_dctHashToEntry.TryGetValue(sHash, out entry);

            return entry;
        }

        private Dictionary<string, string> m_dctStoredLexemes; // hash --> source form
        public IEnumerator GetStoredLexemesEnumerator()
        {
            return m_dctStoredLexemes.GetEnumerator();
        }

        private Dictionary<string, List<CWordFormManaged>> m_dctParses;
        public IEnumerator GetParseEnumerator()
        {
            return m_dctParses.Keys.GetEnumerator();
        }

        public List<CWordFormManaged> WordFormsFromHash(string sHash)
        {
            List<CWordFormManaged> listWf = null;
            m_dctParses.TryGetValue(sHash, out listWf);

            return listWf;
        }

        private Dictionary<CWordFormManaged, CLexemeManaged> m_dctWordformToLexeme;
        public CLexemeManaged GetLexemeFromWordform(CWordFormManaged wordform)
        {
            CLexemeManaged l = null;
            m_dctWordformToLexeme.TryGetValue(wordform, out l);

            return l;
        }

        private Dictionary<int, List<CWordFormManaged>> m_dctWordPosToParses;
        public IEnumerator GetTextParseEnumerator()
        {
            return m_dctWordPosToParses.Keys.GetEnumerator();
        }

        public List<CWordFormManaged> WordFormsFromWordPos(int iPos)
        {
            List<CWordFormManaged> lstWf = null;
            m_dctWordPosToParses.TryGetValue(iPos, out lstWf);

            return lstWf;
        }

        public class TactGroup 
        {
            public int iFirstWordPosition;
            public string sSource;     // TODO: add members
        }

        //  Must be sortable so derived from IComparer
        class TactGroupComparer : IComparer<TactGroup>
        {
            int IComparer<TactGroup>.Compare(TactGroup left, TactGroup right)
            {
                int iLeft = left.iFirstWordPosition;
                int iRight = right.iFirstWordPosition;

                if (iLeft == iRight)
                {
                    return 0;
                }
                if (iLeft < iRight)
                {
                    return -1;
                }
                return 1;
            }

        }

        Dictionary<int, List<TactGroup>> m_LineToTactGroup = new Dictionary<int, List<TactGroup>>();

        // TODO: turn this into property
        public string ParsedText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Properties
        bool m_bInitialized;
        public bool Initialized
        {
            get
            {
                return m_bInitialized;
            }
            set
            {
                m_bInitialized = value;
            }
        }

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

        string m_sImportPath;
        public string ImportPath
        {
            get
            {
                return m_sImportPath;
            }
            set
            {
                m_sImportPath = value;
                OnPropertyChanged("ImportPath");
            }
        }

        string m_sExportPath;
        public string ExportPath
        {
            get
            {
                return m_sExportPath;
            }
            set
            {
                m_sExportPath = value;
                OnPropertyChanged("ExportPath");
            }
        }

        //        public CDictionaryManaged m_Dictionary = new CDictionaryManaged();
        //        public CDictionaryManaged Engine
        //        {
        //            get
        //            {
        //                return m_Dictionary;
        //            }
        //        }

        public int NLexemes
        {
            get
            {
                return m_dctHashToWordform.Count;
            }
        }

        public int NParses
        {
            get
            {
                return m_dctParses.Count;
            }
        }
        #endregion

        public MainModel()
        {
            m_Dictionary = new CDictionaryManaged();
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Zal\ZalData.db3";
            var eRet = m_Dictionary.eSetDbPath(sPath);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                System.Windows.MessageBox.Show("Error: Main dictionary was not initialized.");
                m_Dictionary = null;
                return;
            }

            //m_EditDictionary = new CDictionaryManaged();
            //eRet = m_EditDictionary.eSetDbPath("ZalEdit.db3");
            //if (eRet != EM_ReturnCode.H_NO_ERROR)
            //{
            //    System.Windows.MessageBox.Show("Error: Edit dictionary was not initialized.");
            //    m_EditDictionary = null;
            //    return;
            //}

//            m_Dictionary.eGetVerifier(ref m_Verifier);
            //            m_Dictionary.eGetParser(ref m_Parser);

            m_dctHashToWordform = new Dictionary<string, Dictionary<string, List<CWordFormManaged>>>();
            m_dctFormComments = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
            m_dctHashToEntry = new Dictionary<string, CInflectionManaged>();
            m_dctStoredLexemes = new Dictionary<string, string>();
            m_dctParses = new Dictionary<string, List<CWordFormManaged>>();
            m_dctWordformToLexeme = new Dictionary<CWordFormManaged, CLexemeManaged>();
            m_dctWordPosToParses = new Dictionary<int, List<CWordFormManaged>>();

            m_bInitialized = true;
        }

        public bool bCreateLexeme(ref CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            l = m_Dictionary.CreateLexemeForEdit();
            if (l != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool bEditLexeme(CLexemeManaged sourceL, ref CLexemeManaged copyL)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            m_Dictionary.eCopyEntryForEdit(sourceL, copyL);

            return true;
        }

        public bool bSaveNewHeadword(CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            //            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveNewHeadword(l);
            //            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
            return true;        // TODO restore code above
        }

        public bool bUpdateHeadword(CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

//            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eUpdateHeadword(l);
//            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                return false;
            }
/*
            eRet = (EM_ReturnCode)m_Dictionary.eSaveHeadwordStress(l);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                return false;
            }

            eRet = (EM_ReturnCode)m_Dictionary.eSaveHomonyms(l);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                return false;
            }
            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
*/
        }

        public bool bSaveAspectPairInfo(CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }
            return true;            // TODO REMOVE
//            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveAspectPairInfo(l);
//            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
        }

        public bool bSaveP2Info(CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            //            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveP2Info(l);
            //            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
            return true;
        }
/*
        public bool bSaveCommonDeviation(CInflectionManaged i)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveCommonDeviation(i);
            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
        }
*/
/*
        public bool bSaveInflectionInfo(CInflectionManaged i)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveInflectionInfo(i);
            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
        }
*/
        public bool bSaveDescriptorInfo(CInflectionManaged inflection, bool bNewEntry = false)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            CLexemeManaged lexeme = null;
            var rc = inflection.eGetLexeme(ref lexeme);
            if (rc != EM_ReturnCode.H_NO_ERROR || null == lexeme)
            {
                MessageBox.Show("Не удалось получить доступ к лесеме.", "Zal");
                return false;
            }

            if (lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB && lexeme.eIsReflexive() == EM_Reflexive.REFL_UNDEFINED)
            {
                if (lexeme.sSourceForm().EndsWith("ся") || lexeme.sSourceForm().EndsWith("сь"))
                {
                    lexeme.SetIsReflexive(EM_Reflexive.REFL_YES);
                }
                else
                {
                    lexeme.SetIsReflexive(EM_Reflexive.REFL_NO);
                }
            }

//            var eRet = l.eMakeGraphicStem();
//            if (eRet != EM_ReturnCode.H_NO_ERROR)
//            {
//                System.Windows.MessageBox.Show("Unable to create graphic stem.");
//                return false;
//            }

            var sOldLexHash = inflection.sStoredHash();

            var eRet = EM_ReturnCode.H_NO_ERROR;
// TODO RESTORE
return true;
/*
            if (bNewEntry)
            {
                eRet = (EM_ReturnCode)m_Dictionary.eSaveDescriptorInfo(lexeme);
            }
            else
            {
                eRet = (EM_ReturnCode)m_Dictionary.eUpdateDescriptorInfo(lexeme);
            }
*/
            var sNewLexHash = inflection.sHash();

            var sDbPath = m_Dictionary.sGetDbPath();
            var dbConnection = new SQLiteConnection("Data Source = " + sDbPath + ";Version=3;");
            dbConnection.Open();
            if (dbConnection.State != System.Data.ConnectionState.Open)
            {
                System.Windows.MessageBox.Show("Unable to access database.");
                return false;
            }

            SQLiteCommand insertCmd = new SQLiteCommand("INSERT INTO lexeme_hashes_changed (old_hash, new_hash) VALUES (@old_hash, @new_hash)", dbConnection);
            insertCmd.Parameters.AddWithValue("@old_hash", sOldLexHash);
            insertCmd.Parameters.AddWithValue("@new_hash", sNewLexHash);

            try
            {
                insertCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sMsg = String.Format("Exception saving descriptor data: {0}", ex.Message);
                return false;
            }

            dbConnection.Close();

            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
        }

        public bool GetFormsByGramHash(string sLexemeHash, string sGramHash, out List<FormDescriptor> forms)
        {
            forms = new List<FormDescriptor>();
            Dictionary<string, List<CWordFormManaged>> dctParadigm;
            if (!m_dctHashToWordform.TryGetValue(sLexemeHash, out dctParadigm))
            {
                System.Windows.MessageBox.Show("Internal error: lexeme hash not found.");
                return false;
            }
            try
            {
                foreach (var wfObj in dctParadigm[sGramHash])
                {
                    FormDescriptor fd = new FormDescriptor();
                    fd.WordFormManaged = wfObj;
//                    string sSwf = wfObj.sWordForm();
//                    Helpers.MarkStress(ref sSwf, wfObj);
//                    fd.StressedWordform = sSwf;
                    forms.Add(fd);
                }
            }
            catch
            {
                //                System.Windows.MessageBox.Show("Internal error: Gram hash not recognized. " + ex.Message);
                return false;
            }

            return true;
        }

        public bool UpdateFormsByGramHash(string sLexemeHash, string sGramHash, List<CWordFormManaged> lstForms)
        {
            Dictionary<string, List<CWordFormManaged>> dctParadigm;
            if (!m_dctHashToWordform.TryGetValue(sLexemeHash, out dctParadigm))
            {
                System.Windows.MessageBox.Show("Internal error: Lexeme hash not found.");
                return false;
            }
            try
            {
                lstForms = dctParadigm[sGramHash];
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Internal error: Gram hash not recognized. " + ex.Message);
                return false;
            }
            return true;
        }

        public bool IsP2Optonal(CLexemeManaged lexeme)
        {
            return lexeme.bSecondPrepositionalOptional();
        }

        public string sGetP2Prepositions(CLexemeManaged lexeme)
        {
            return lexeme.sP2Preposition();
        }

        // Remove from the database
        public EM_ReturnCode eDeleteLexeme(CLexemeManaged l)
        {
            //            var eRet = m_Dictionary.eDeleteLexeme(l);
            //            return eRet;
            return EM_ReturnCode.H_NO_ERROR;
        }

        // Remove from internal data structures
        public void RemoveLexeme(CLexemeManaged lexeme)
        {
            CInflectionEnumeratorManaged ie = null;
            CInflectionManaged inflection = null;
            lexeme.eCreateInflectionEnumerator(ref ie);
            var eRet = ie.eGetFirstInflection(ref inflection);
            if (EM_ReturnCode.H_NO_ERROR != eRet)
            {
                System.Windows.MessageBox.Show("Error: unable to retrieve inflection");
                return;
            }
            do
            {
                if (!m_dctHashToWordform.ContainsKey(inflection.sParadigmHash()))
                {
                    eRet = inflection.eGenerateParadigm();
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
                        //                        System.Windows.MessageBox.Show("Error generating paradigm.");
                        //                        return;
                    }

                    if (!bArrangeParadigm(inflection))
                    {
                        System.Windows.MessageBox.Show("Unable to generate forms.");
                    }
                }

                m_Dictionary.Clear();
                m_dctHashToWordform.Remove(inflection.sHash());
                m_dctHashToEntry.Remove(inflection.sHash());

                eRet = ie.eGetNextInflection(ref inflection);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);
        }

        public void Clear()
        {
            m_Dictionary.Clear();
            m_dctHashToWordform.Clear();
        }

        public EM_ReturnCode OpenDictionary(string sPath)
        {
            Path = sPath;
            var eRet = m_Dictionary.eSetDbPath(Path);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                Path = "";
                System.Windows.MessageBox.Show("Unable to open dictionary.");
            }


            // TODO:refactor & cleanup
            if (m_Parser != null)       // need to renew to avoid crash
            {
                m_Parser = null;
                m_Dictionary.eGetParser(ref m_Parser);
                if (null == m_Parser)
                {
                    System.Windows.MessageBox.Show("Parser was not initialized.");
                }
            }

            m_Dictionary.eGetAnalytics(ref m_Analytics);
            if (null == m_Analytics)
            {
                System.Windows.MessageBox.Show("Analytics module was not initialized.");
            }

            m_Dictionary.eGetVerifier(ref m_Verifier);
            if (null == m_Verifier)
            {
                System.Windows.MessageBox.Show("Verifier module was not initialized.");
            }

            return eRet;
        }

        public void SearchByInitialForm(string str)
        {
            if (null == str)
            {
                System.Windows.MessageBox.Show("Empty search string.");
                return;
            }

            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return;
            }

            m_dctHashToWordform.Clear();

            var eRet = m_Dictionary.eGetLexemesByInitialForm(str);
            if (eRet != EM_ReturnCode.H_NO_MORE && eRet != EM_ReturnCode.H_FALSE)
            {
                System.Windows.MessageBox.Show("Lexeme not found -- error in DB lookup.");
                return;
            }

            if (EM_ReturnCode.H_FALSE == eRet)
            {
                System.Windows.MessageBox.Show("Лексема не найдена.");
                return;
            }

            CLexemeManaged lexeme = null;
            CLexemeEnumeratorManaged le = null;
            m_Dictionary.eCreateLexemeEnumerator(ref le);
            var eRetLex = le.eGetFirstLexeme(ref lexeme);
            if (EM_ReturnCode.H_NO_ERROR != eRetLex)
            {
                System.Windows.MessageBox.Show("Error: unable to retrieve lexeme");
                return;
            }

            do
            {
                CInflectionManaged inflection = null;
                CInflectionEnumeratorManaged ie = null;
                lexeme.eCreateInflectionEnumerator(ref ie);
                var eRetInfl = ie.eGetFirstInflection(ref inflection);
                if (EM_ReturnCode.H_NO_ERROR != eRetInfl && EM_ReturnCode.H_FALSE != eRetInfl)
                {
                    System.Windows.MessageBox.Show("Error: unable to retrieve inflection");
                    return;
                }
                if (EM_ReturnCode.H_NO_ERROR == eRetInfl)
                {
                    do
                    {
                        if (!m_dctHashToWordform.ContainsKey(inflection.sParadigmHash()))
                        {
                            eRet = inflection.eGenerateParadigm();
                            if (eRet != EM_ReturnCode.H_NO_ERROR)
                            {
                                //                        System.Windows.MessageBox.Show("Error generating paradigm.");
                                //                        return;
                            }

                            if (!bArrangeParadigm(inflection))
                            {
                                System.Windows.MessageBox.Show("Unable to generate forms.");
                            }
                        }

                        eRetInfl = ie.eGetNextInflection(ref inflection);

                    } while (EM_ReturnCode.H_NO_ERROR == eRetInfl);
                }

                eRetLex = le.eGetNextLexeme(ref lexeme);

            } while (EM_ReturnCode.H_NO_ERROR == eRetLex);

            if (eRetLex != EM_ReturnCode.H_NO_MORE)
            {
                System.Windows.MessageBox.Show("Error accessing lexeme collection.");
                return;
            }
        }       //  SearchByInitialForm()

        public bool GenerateAllForms()
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            var eRet = m_Dictionary.eGenerateAllForms();
            if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_NO_MORE && eRet != EM_ReturnCode.H_FALSE)
            {
                System.Windows.MessageBox.Show("Form generation failed.");
                return false;
            }

            return true;

        }       //  GenerateAllForms()

        public bool ParseWord(string sForm)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }
            m_Dictionary.eGetParser(ref m_Parser);
            if (null == m_Parser)
            {
                System.Windows.MessageBox.Show("Parser was not initialized.");
                return false;
            }

            m_dctParses.Clear();

            var eRet = m_Parser.eParseWord(sForm);
            if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_NO_MORE && eRet != EM_ReturnCode.H_FALSE)
            {
                System.Windows.MessageBox.Show("Form analysis failed.");
                return false;
            }

            CWordFormManaged wordFormData = null;
            eRet = m_Parser.eGetFirstWordForm(ref wordFormData);
            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                string sKey = wordFormData.sGramHash();
                if (sKey.Length > 0)
                {
                    List<CWordFormManaged> listWf;
                    if (!m_dctParses.TryGetValue(sKey, out listWf))
                    {
                        listWf = new List<CWordFormManaged>();
                        m_dctParses.Add(sKey, listWf);
                    }

                    listWf.Add(wordFormData);
                }
                else
                {
                    System.Windows.MessageBox.Show("Empty gram hash.");
//                    return false;
                }

                CLexemeManaged lexeme = null;
//                m_Dictionary.Clear();
                eRet = m_Dictionary.eGetLexemeById(wordFormData.llLexemeId(), ref lexeme);
                if (EM_ReturnCode.H_NO_ERROR == eRet || EM_ReturnCode.H_NO_MORE == eRet)
                {
                    m_dctWordformToLexeme.Add(wordFormData, lexeme);
                }
                else
                {
                    System.Windows.MessageBox.Show("Unable to find lexeme.");
                }

                eRet = m_Parser.eGetNextWordForm(ref wordFormData);
            }

            return true;

        }       //  ParseWord()

        #region Regression
        public bool GetStoredLexemeData()
        {
//            EM_ReturnCode eRet = m_Dictionary.eGetVerifier(ref m_Verifier);
//            if (eRet != EM_ReturnCode.H_NO_ERROR)
//            {
//                System.Windows.MessageBox.Show("Unable to load Verifier.");
//                return false;
//            }

            if (null == m_Verifier)
            {
                System.Windows.MessageBox.Show("Unable to acquire verifier.", "Zal Test");
                return false;
            }

            var eRet = m_Verifier.eLoadStoredLexemes();
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                System.Windows.MessageBox.Show("Unable to load stored data.");
                return false;
            }

            if (m_Verifier.iCount() <= 0)
            {
                System.Windows.MessageBox.Show("No test forms in the database", "Zal Test");
                return false;
            }

            string sLexemeHash = null, sHeadword = null;
            eRet = m_Verifier.eGetFirstLexemeData(ref sLexemeHash, ref sHeadword);
            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                m_dctStoredLexemes[sLexemeHash] = sHeadword;
                eRet = m_Verifier.eGetNextLexemeData(ref sLexemeHash, ref sHeadword);
            }

            return true;
        }

        public EM_ReturnCode VerifyLexeme(string sLexemeHash, ref EM_TestResult eTestResult)
        {
            eTestResult = EM_TestResult.TEST_RESULT_UNDEFINED;

            if (null == m_Verifier)
            {
                return EM_ReturnCode.H_UNAVAILABLE;
            }

            var eRet = m_Verifier.eVerify(sLexemeHash);
            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                eTestResult = m_Verifier.eResult();
            }

            return eRet;
        }

        public EM_ReturnCode ImportRegressionData(string sPath, MainLibManaged.DelegateProgress callback)
        {
            if (null == m_Dictionary)
            {
                return EM_ReturnCode.H_UNAVAILABLE;
            }

            MainLibManaged.DelegateProgress DelegateProgress = new MainLibManaged.DelegateProgress(callback);

            //            var eRet = m_Dictionary.eImportTestData(sPath, callback);
            //            return eRet;
            return EM_ReturnCode.H_UNAVAILABLE;
        }

        public EM_ReturnCode ExportRegressionData(string sPath, MainLibManaged.DelegateProgress callback)
        {
            if (null == m_Dictionary)
            {
                return EM_ReturnCode.H_UNAVAILABLE;
            }

            MainLibManaged.DelegateProgress DelegateProgress = new MainLibManaged.DelegateProgress(callback);

//            var eRet = m_Dictionary.eExportTestData(sPath, callback);
//            return eRet;
            return EM_ReturnCode.H_UNAVAILABLE;
        }

        public EM_ReturnCode DeleteSavedLexeme(string sLexemeHash, ref EM_TestResult eTestResult)
        {
            eTestResult = EM_TestResult.TEST_RESULT_UNDEFINED;

            if (null == m_Verifier)
            {
                return EM_ReturnCode.H_UNAVAILABLE;
            }

            var eRet = m_Verifier.eDeleteStoredLexeme(sLexemeHash);
            if (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                eTestResult = m_Verifier.eResult();
            }

            return eRet;
        }

        #endregion

        #region FormGeneration
        private bool bArrangeParadigm(CInflectionManaged inflection)
        {
            CLexemeManaged lexeme = null;
            var eRc = inflection.eGetLexeme(ref lexeme);
            if (eRc != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to retrieve lexeme data.");
                return false;
            }

            EM_PartOfSpeech ePos = lexeme.ePartOfSpeech();
            switch (ePos)
            {
                case EM_PartOfSpeech.POS_NOUN:
                case EM_PartOfSpeech.POS_PRONOUN:
                    return bGenerateNominalForms(inflection);

                case EM_PartOfSpeech.POS_NUM:
                    return bGenerateNumeralForms(inflection);

                case EM_PartOfSpeech.POS_ADJ:
                case EM_PartOfSpeech.POS_PRONOUN_ADJ:
                case EM_PartOfSpeech.POS_NUM_ADJ:
                    return bGenerateAdjForms(inflection);

                case EM_PartOfSpeech.POS_VERB:
                    return bGenerateVerbForms(inflection);

                case EM_PartOfSpeech.POS_ADV:
                case EM_PartOfSpeech.POS_CONJUNCTION:
                case EM_PartOfSpeech.POS_COMPAR:
                case EM_PartOfSpeech.POS_INTERJ:
                case EM_PartOfSpeech.POS_PARTICLE:
                case EM_PartOfSpeech.POS_PARENTH:
                case EM_PartOfSpeech.POS_PREDIC:
                case EM_PartOfSpeech.POS_PREPOSITION:
                case EM_PartOfSpeech.POS_PRONOUN_PREDIC:
                case EM_PartOfSpeech.POS_NULL:
                    return bGetUninflectedForm(inflection);

                default:
                    System.Windows.MessageBox.Show("Part of speech not handled.");
                    return false;
            }

            //return true;
        }
 
        private bool bGenerateNominalForms(CInflectionManaged inflection)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;
            CLexemeManaged lexeme = null;
            var eRc = inflection.eGetLexeme(ref lexeme);
            if (eRc != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to retrieve lexeme data.");
                return false;
            }

            Dictionary<string, List<CWordFormManaged>> dctParadigm = new Dictionary<string, List<CWordFormManaged>>();

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)inflection.eGetFirstWordForm(ref wf);
            while (wf != null && (EM_ReturnCode.H_NO_ERROR == eRet || EM_ReturnCode.H_FALSE == eRet))
            {
                try
                {
                    string sKey = "";
                    switch (lexeme.ePartOfSpeech())
                    {
                        case EM_PartOfSpeech.POS_NOUN:
                            sKey = "Noun";
                            break;

                        case EM_PartOfSpeech.POS_PRONOUN:
                            sKey = "Pronoun";
                            break;

                        case EM_PartOfSpeech.POS_NUM:
                            sKey = "Numeral";
                            break;

                        default:
                            System.Windows.MessageBox.Show("Part of speech not handled.");
                            return false;
                    }

                    sKey += "_";
                    sKey += Helpers.sNumberToString(wf.eNumber());
                    sKey += "_";
                    sKey += Helpers.sCaseToString(wf.eCase());

                    string sWordForm = wf.sWordForm();
                    Helpers.MarkStress(ref sWordForm, wf);

                    if (lexeme.bAssumedForms())
                    {
                        if (lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NOUN && wf.eNumber() == EM_Number.NUM_PL)
                        {
                            sWordForm = "*" + sWordForm;
                        }
                    }

                    if (!dctParadigm.ContainsKey(sKey))
                    {
                        dctParadigm[sKey] = new List<CWordFormManaged>();
                    }

                    dctParadigm[sKey].Add(wf);
                }
                catch
                {
//                    return false;
                }

                eRet = (EM_ReturnCode)inflection.eGetNextWordForm(ref wf);

            }   //  while...

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
//                System.Windows.MessageBox.Show("Error generating noun forms.");
//                return true;        // OK to show empty paradigm
            }

            string sHash = inflection.sParadigmHash();
            m_dctHashToWordform[sHash] = dctParadigm;
            m_dctHashToEntry[sHash] = inflection;

            return true;

        }   //  bGenerateNominalForms()

        private bool bGenerateNumeralForms(CInflectionManaged inflection)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            Dictionary<string, List<CWordFormManaged>> dctParadigm = new Dictionary<string, List<CWordFormManaged>>();

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)inflection.eGetFirstWordForm(ref wf);
            while (wf != null && (EM_ReturnCode.H_NO_ERROR == eRet || EM_ReturnCode.H_FALSE == eRet))
            {
                try
                {
                    string sKey = "Numeral";
                    if (EM_Subparadigm.SUBPARADIGM_NUM_2TO4 == wf.eSubparadigm())
                    {
                        sKey += "24";
                    }
                    sKey += "_";
                    if (wf.eGender() != EM_Gender.GENDER_UNDEFINED)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender());
                        sKey += "_";
                    }
                    sKey += Helpers.sCaseToString(wf.eCase());
                    if (wf.eAnimacy() != EM_Animacy.ANIM_UNDEFINED)
                    {
                        sKey += "_";
                        if (wf.eAnimacy() == EM_Animacy.ANIM_NO)
                        {
                            sKey += "Inanim";
                        }
                        else if (wf.eAnimacy() == EM_Animacy.ANIM_YES)
                        {
                            sKey += "Anim";
                        }
                        else
                        {
                            return false;
                        }
                    }

                    string sWordForm = wf.sWordForm();
                    Helpers.MarkStress(ref sWordForm, wf);

                    if (!dctParadigm.ContainsKey(sKey))
                    {
                        dctParadigm[sKey] = new List<CWordFormManaged>();
                    }

                    dctParadigm[sKey].Add(wf);
                }
                catch
                {
                    //                    return false;
                }

                eRet = (EM_ReturnCode)inflection.eGetNextWordForm(ref wf);

            }   //  while...

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
                //                System.Windows.MessageBox.Show("Error generating noun forms.");
                //                return true;        // OK to show empty paradigm
            }

            string sHash = inflection.sParadigmHash();
            m_dctHashToWordform[sHash] = dctParadigm;
            m_dctHashToEntry[sHash] = inflection;

            return true;
        }

        private bool bGenerateAdjForms(CInflectionManaged inflection)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            CLexemeManaged lexeme = null;
            var eRc = inflection.eGetLexeme(ref lexeme);
            if (eRc != EM_ReturnCode.H_NO_ERROR)
            {
                MessageBox.Show("Unable to retrieve lexeme data.");
                return false;
            }

            Dictionary<string, List<CWordFormManaged>> dctParadigm = new Dictionary<string, List<CWordFormManaged>>(); // hash -> form
            Dictionary<string, List<Tuple<string, string>>> comments = null;    // hash -> left comment, right comment

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)inflection.eGetFirstWordForm(ref wf);

            EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                if (null == wf)
                {
                    continue;
                }

                eSp = wf.eSubparadigm();

                string sKey = "";
                if (eSp == EM_Subparadigm.SUBPARADIGM_LONG_ADJ)
                {
                    sKey = "AdjL_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_"
                        + Helpers.sCaseToString(wf.eCase());
                }
                else if (eSp == EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ)
                {
                    sKey = "PronAdj_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_"
                        + Helpers.sCaseToString(wf.eCase());
                }
                else if (eSp == EM_Subparadigm.SUBPARADIGM_NUM_ADJ)
                {
                    sKey = "NumAdj_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_"
                        + Helpers.sCaseToString(wf.eCase());
                }
                else if (eSp == EM_Subparadigm.SUBPARADIGM_SHORT_ADJ)
                {
                    sKey = "AdjS_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender());
                    }
                    else if (wf.eNumber() == EM_Number.NUM_PL)
                    {
                        sKey += Helpers.sNumberToString(wf.eNumber());
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Error: illegal number value.");
                    }
                }
                else if (eSp == EM_Subparadigm.SUBPARADIGM_COMPARATIVE)
                {
                    sKey = "AdjComp";
                }

                if (wf.bIrregular())
                {
                    if (null == comments)
                    {
                        comments = new Dictionary<string, List<Tuple<string, string>>>();
                    }
                    if (!comments.ContainsKey(sKey))
                    {
                        comments[sKey] = new List<Tuple<string, string>>();
                    }
                    comments[sKey].Add(new Tuple<string, string>(wf.sLeadComment(), wf.sTrailingComment()));
                }

                if (sKey != null)
                {
                    if (!dctParadigm.ContainsKey(sKey))
                    {
                        dctParadigm[sKey] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sKey].Add(wf);
                }

                eRet = (EM_ReturnCode)inflection.eGetNextWordForm(ref wf);

            }   //  while... 

            string sHash = inflection.sParadigmHash();
            m_dctHashToWordform[sHash] = dctParadigm;
            m_dctHashToEntry[sHash] = inflection;
            m_dctFormComments[sHash] = comments;

            eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            if (EM_PartOfSpeech.POS_ADJ == lexeme.ePartOfSpeech())
            {
                if ("мс" == lexeme.sInflectionType())
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
                }
                else
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_LONG_ADJ;
                }
            }
            else if (EM_PartOfSpeech.POS_PRONOUN_ADJ == lexeme.ePartOfSpeech())
            {
                if ("мс-п" == lexeme.sInflectionType())
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
                }
                else
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_LONG_ADJ;
                }
            }
            else if (EM_PartOfSpeech.POS_NUM_ADJ == lexeme.ePartOfSpeech())
            {
                if ("числ.-п" == lexeme.sInflectionType())
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_NUM_ADJ;
                }
                else if ("мс" == lexeme.sInflectionType())
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
                }
                else
                {
                    eSp = EM_Subparadigm.SUBPARADIGM_LONG_ADJ;
                }
            }

            HandleAccusatives(inflection, eSp);

            return true;

        }   //  GenerateAdjForms()

        private bool bGenerateVerbForms(CInflectionManaged inflection)
        {
            Dictionary<string, List<CWordFormManaged>> dctParadigm = new Dictionary<string, List<CWordFormManaged>>();
            Dictionary<string, List<Tuple<string, string>>> comments = null;

            CWordFormManaged wf = null;
            var eRet = (EM_ReturnCode)inflection.eGetFirstWordForm(ref wf);
            if (eRet != EM_ReturnCode.H_NO_ERROR || null == wf)
            {
                System.Windows.MessageBox.Show("Unable to load a word form.");
                return false;
            }

            do
            {
                string sKey = "";
                switch (wf.eSubparadigm())
                {
                    case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                        sKey = "Pres_" + Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sPersonToString(wf.ePerson());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PAST_TENSE:
                        sKey = "Past_";
                        if (EM_Number.NUM_SG == wf.eNumber())
                        {
                            sKey += Helpers.sGenderToString(wf.eGender());
                        }
                        else if (EM_Number.NUM_PL == wf.eNumber())
                        {
                            sKey += Helpers.sNumberToString(wf.eNumber());
                        }
                        else
                        {
                            // TODO: error handling
                        }
                        break;

                    case EM_Subparadigm.SUBPARADIGM_INFINITIVE:
                        sKey = "Inf";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_IMPERATIVE:
                        sKey = "Impv_";
                        sKey += Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sPersonToString(wf.ePerson());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                        sKey = "PPresA_";
                        if (EM_Number.NUM_SG == wf.eNumber())
                        {
                            sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                        }
                        sKey += Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sCaseToString(wf.eCase());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                        sKey = "PPastA_";
                        if (EM_Number.NUM_SG == wf.eNumber())
                        {
                            sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                        }
                        sKey += Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sCaseToString(wf.eCase());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT:
                        sKey = "VAdv_Pres";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                        sKey = "VAdv_Past";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                        sKey = "PPresPL_";
                        if (EM_Number.NUM_SG == wf.eNumber())
                        {
                            sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                        }
                        sKey += Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sCaseToString(wf.eCase());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT:
                        sKey = "PPresPS_";
                        if (wf.eNumber() == EM_Number.NUM_SG)
                        {
                            sKey += Helpers.sGenderToString(wf.eGender());
                        }
                        else if (wf.eNumber() == EM_Number.NUM_PL)
                        {
                            sKey += Helpers.sNumberToString(wf.eNumber());
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Error: illegal number value.");
                        }
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                        sKey = "PPastPL_";
                        if (EM_Number.NUM_SG == wf.eNumber())
                        {
                            sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                        }
                        sKey += Helpers.sNumberToString(wf.eNumber()) + "_" +
                            Helpers.sCaseToString(wf.eCase());
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT:
                        sKey = "PPastPS_";
                        if (wf.eNumber() == EM_Number.NUM_SG)
                        {
                            sKey += Helpers.sGenderToString(wf.eGender());
                        }
                        else if (wf.eNumber() == EM_Number.NUM_PL)
                        {
                            sKey += Helpers.sNumberToString(wf.eNumber());
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Error: illegal number value.");
                        }
                        break;

                }   // switch...

                if (sKey.Length > 0)
                {
                    var sWordForm = wf.sWordForm();
                    Helpers.MarkStress(ref sWordForm, wf);
                    if (!dctParadigm.ContainsKey(sKey))
                    {
                        dctParadigm[sKey] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sKey].Add(wf);
                }

                if (wf.bIrregular())
                {
                    if (null == comments)
                    {
                        comments = new Dictionary<string, List<Tuple<string, string>>>();
                    }
                    if (!comments.ContainsKey(sKey))
                    {
                        comments[sKey] = new List<Tuple<string, string>>();
                    }
                    comments[sKey].Add(new Tuple<string, string>(wf.sLeadComment(), wf.sTrailingComment()));
                }

                eRet = (EM_ReturnCode)inflection.eGetNextWordForm(ref wf);
                if ((eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_NO_MORE) || null == wf)
                {
                    System.Windows.MessageBox.Show("Unable to load a word form.");
                    return false;
                }

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            string sHash = inflection.sParadigmHash();
            m_dctHashToWordform[sHash] = dctParadigm;
            m_dctHashToEntry[sHash] = inflection;
//            m_FormComments[sHash] = comments;

            if (dctParadigm.ContainsKey("PPresA_M_Sg_N"))
            {
                HandleAccusatives(inflection, EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT);
            }
            if (dctParadigm.ContainsKey("PPastA_M_Sg_N"))
            {
                HandleAccusatives(inflection, EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT);
            }
            if (dctParadigm.ContainsKey("PPresPL_M_Sg_N"))
            {
                HandleAccusatives(inflection, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG);
            }
            if (dctParadigm.ContainsKey("PPastPL_M_Sg_N"))
            {
                HandleAccusatives(inflection, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG);
            }

            return true;

        }   //  GenerateVerbForms()

        public bool bGetUninflectedForm(CInflectionManaged inflection)
        {
            CWordFormManaged wf = null;
            var eRet = (EM_ReturnCode)inflection.eGetFirstWordForm(ref wf);
            if (eRet != EM_ReturnCode.H_NO_ERROR || null == wf)
            {
                System.Windows.MessageBox.Show("Unable to load a word form.");
                return false;
            }

            Dictionary<string, List<CWordFormManaged>> dctParadigm = new Dictionary<string, List<CWordFormManaged>>();
            var sKey = wf.sGramHash();
            dctParadigm[sKey] = new List<CWordFormManaged>();
            dctParadigm[sKey].Add(wf);
            string sLexHash = inflection.sParadigmHash();
            m_dctHashToWordform[sLexHash] = dctParadigm;
            m_dctHashToEntry[sLexHash] = inflection;

            return true;

        }       //  bGetUninflectedForm()

        public bool bLeadComment(string sEntryHash, string sFormHash, ref string sComment)
        {
            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = inflection.eWordFormFromHash(sFormHash, 0, ref wf);    // no need to check for all
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            sComment = wf.sLeadComment();

            return true;
        }

        /*
        public bool bTrailingComment(string sLexemeHash, string sFormHash, ref string sComment)
        {
            CLexemeManaged lexeme;
            if (!m_LexemeHashToLexeme.TryGetValue(sLexemeHash, out lexeme))
            {
                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = lexeme.eWordFormFromHash(sFormHash, 0, ref wf);    // no need to check for all
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            sComment = wf.sTrailingComment();

            return true;
        }
        */

        public bool bIsIrregular(string sEntryHash, string sFormHash)
        {
            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
//                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = inflection.eWordFormFromHash(sFormHash, 0, ref wf);    // no need to check for all
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
//                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIrregular();
        }

        public bool bIsEdited(string sEntryHash, string sFormHash)
        {
            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
//                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = inflection.eWordFormFromHash(sFormHash, 0, ref wf);    // no need to check for all
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
//                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIsEdited();
        }

        public bool bIsVariant(string sEntryHash, string sFormHash)
        {
            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = inflection.eWordFormFromHash(sFormHash, 0, ref wf);    // no need to check for all
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                //                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIsVariant();
        }

        public bool bIsMissing(string sEntryHash, string sFormHash)
        {
            if (null == sFormHash)
            {
                return false;
            }

            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
                //                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            EM_ReturnCode eRet = inflection.eFormExists(sFormHash);
            if (eRet != EM_ReturnCode.H_TRUE)
            {
                //                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return true;
            }

            return false;
        }

        public bool bIsDifficult(string sEntryHash, string sFormHash)
        {
            if (null == sFormHash)
            {
                return false;
            }

            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
                //                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            EM_ReturnCode eRet = inflection.eIsFormDifficult(sFormHash);
            if (EM_ReturnCode.H_TRUE == eRet)
            {
                //                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return true;
            }

            return false;
        }

        public bool bIsAssumed(string sEntryHash, string sFormHash)
        {
            if (null == sFormHash)
            {
                return false;
            }

            CInflectionManaged inflection;
            if (!m_dctHashToEntry.TryGetValue(sEntryHash, out inflection))
            {
                //                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return false;
            }

            EM_ReturnCode eRet = inflection.eIsFormAssumed(sFormHash);
            if (EM_ReturnCode.H_TRUE == eRet)
            {
                //                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return true;
            }

            return false;
        }

        private void HandleAccusatives(CInflectionManaged inflection, EM_Subparadigm eSubparadigm)
        {
            Dictionary<string, List<CWordFormManaged>> dctParadigm;
            if (!m_dctHashToWordform.TryGetValue(inflection.sParadigmHash(), out dctParadigm))
            {
                System.Windows.MessageBox.Show("Unable to find lexeme.");
                return;
            }

            string sPrefix = null;
            switch (eSubparadigm)
            {
                case EM_Subparadigm.SUBPARADIGM_LONG_ADJ:
                    sPrefix = "AdjL_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ:
                    sPrefix = "PronAdj_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_NUM_ADJ:
                    sPrefix = "NumAdj_";
//                    sPrefix = "PronAdj_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                    sPrefix = "PPresA_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                    sPrefix = "PPastA_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                    sPrefix = "PPresPL_";
                    break;

                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    sPrefix = "PPastPL_";
                    break;
                    
                case EM_Subparadigm.SUBPARADIGM_UNDEFINED:
                default:
                    System.Windows.MessageBox.Show("Illegal subparadigm");
                    return;
            }

            try
            {
                var values = dctParadigm[sPrefix + "M_Sg_N"];
                foreach (var sForm in values)
                {
                    if (!dctParadigm.ContainsKey(sPrefix + "M_Sg_A_Inanim"))
                    {
                        dctParadigm[sPrefix + "M_Sg_A_Inanim"] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sPrefix + "M_Sg_A_Inanim"].Add(sForm);
                }

                values = dctParadigm[sPrefix + "M_Sg_G"];
                foreach (var sForm in values)
                {
                    if (!dctParadigm.ContainsKey(sPrefix + "M_Sg_A_Anim"))
                    {
                        dctParadigm[sPrefix + "M_Sg_A_Anim"] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sPrefix + "M_Sg_A_Anim"].Add(sForm);
                }

                values = dctParadigm[sPrefix + "Pl_N"];
                foreach (var sForm in values)
                {
                    if (!dctParadigm.ContainsKey(sPrefix + "Pl_A_Inanim"))
                    {
                        dctParadigm[sPrefix + "Pl_A_Inanim"] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sPrefix + "Pl_A_Inanim"].Add(sForm);
                }

                values = dctParadigm[sPrefix + "Pl_G"];
                foreach (var sForm in values)
                {
                    if (!dctParadigm.ContainsKey(sPrefix + "Pl_A_Anim"))
                    {
                        dctParadigm[sPrefix + "Pl_A_Anim"] = new List<CWordFormManaged>();
                    }
                    dctParadigm[sPrefix + "Pl_A_Anim"].Add(sForm);
                }
            }
//            catch (KeyNotFoundException exKey)
            catch
            {
                //               System.Windows.MessageBox.Show("Error: unable to find form:  " + exKey.Message);
            }
//            catch (Exception exOther)
//            {
//                System.Windows.MessageBox.Show("Exception: " + exOther.Message);
//            }
        }       //  HandleAccusatives()

        #endregion

        public bool bGetFormByGramHash(CInflectionManaged inflection, string sHash, out List<CWordFormManaged> lstForms)
        {
            lstForms = null;
            Dictionary<string, List<CWordFormManaged>> dctParadigm;
            if (m_dctHashToWordform.TryGetValue(inflection.sHash(), out dctParadigm))
            {
                if (dctParadigm.TryGetValue(sHash, out lstForms))
                {
                    return true;
                }
                else
                {
                    System.Windows.MessageBox.Show("Error accessing lexeme form by gram hash.");
                    return false;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error accessing paradigm collection.");
                return false;
            }
        }
    }       //  public class MainModel
}       //  namespace ZalTestApp
