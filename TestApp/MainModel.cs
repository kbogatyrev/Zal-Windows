using MainLibManaged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace ZalTestApp
{
    public class MainModel : INotifyPropertyChanged
    {
        private CDictionaryManaged m_Dictionary = null;

        private CVerifierManaged m_Verifier = null; 
        public CVerifierManaged Verifier()
        {
            return m_Verifier;
        }

        private Dictionary<string, Dictionary<string, List<string>>> m_Lexemes;  // lexeme hash to (gram hash --> forms)
        public IEnumerator GetEnumerator()
        {
            return m_Lexemes.Keys.GetEnumerator();
        }

        private Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> m_FormComments;
        // ^-- lex. hash       ^-- gram hash     ^-- leading comment, trailing comment))

        public IEnumerator GetFormCommentsEnumerator()
        {
            return m_FormComments.Keys.GetEnumerator();
        }

        private Dictionary<string, CLexemeManaged> m_LexemeHashToLexeme;
        public CLexemeManaged LexemeFromHash(string sHash)
        {
            CLexemeManaged lexeme = null;
            m_LexemeHashToLexeme.TryGetValue(sHash, out lexeme);

            return lexeme;
        }

        private Dictionary<string, string> m_StoredLexemes; // hash --> source form
        public IEnumerator GetStoredLexemesEnumerator()
        {
            return m_StoredLexemes.GetEnumerator();
        }

        //public Dictionary<string, List<string>> m_ChangedForms = new Dictionary<string, List<string>>();       // gram hash --> current form

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
                return m_Lexemes.Count;
            }
        }
        #endregion

        public MainModel()
        {
            m_Dictionary = new CDictionaryManaged();
            string sPath = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData) + @"\Zal\ZalData.db3";
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

            m_Dictionary.eGetVerifier(ref m_Verifier);

            m_Lexemes = new Dictionary<string, Dictionary<string, List<string>>>();
            m_FormComments = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
            m_LexemeHashToLexeme = new Dictionary<string, CLexemeManaged>();
            m_StoredLexemes = new Dictionary<string, string>();

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

        public bool bEditLexeme(CLexemeManaged source, ref CLexemeManaged copy)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            copy = m_Dictionary.CopyLexemeForEdit(source);
            if (copy != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool bSaveLexeme(CLexemeManaged l)
        {
            if (null == m_Dictionary)
            {
                System.Windows.MessageBox.Show("Dictionary was not initialized.");
                return false;
            }

            EM_ReturnCode eRet = (EM_ReturnCode)m_Dictionary.eSaveLexeme(l);
            return eRet == EM_ReturnCode.H_NO_ERROR ? true : false;
        }

        public bool GetFormsByGramHash(string sLexemeHash, string sGramHash, out List<string> forms)
        {
            forms = null;

            //CLexemeManaged lexeme = null;
            //if (!m_LexemeHashToLexeme.TryGetValue(lexeme.sHash(), out lexeme))
            //{
            //    System.Windows.MessageBox.Show("Lexeme not found.");
            //    return false;
            //}

            Dictionary<string, List<string>> paradigm;
            if (!m_Lexemes.TryGetValue(sLexemeHash, out paradigm))
            {
                System.Windows.MessageBox.Show("Internal error: lexeme hash not found.");
                return false;
            }
            try
            {
                forms = paradigm[sGramHash];
            }
            catch (Exception ex)
            {
//                System.Windows.MessageBox.Show("Internal error: Gram hash not recognized. " + ex.Message);
                return false;
            }

            return true;
        }

        public bool GetFormComments(string sLexemeHash, string sGramHash, out List<Tuple<string, string>> comments)
        {
            comments = null;

            Dictionary<string, List<Tuple<string, string>>> c = null;
            if (m_FormComments.TryGetValue(sLexemeHash, out c))
            {
                c.TryGetValue(sGramHash, out comments);
            }

            return true;
        }

        public bool UpdateFormsByGramHash(string sLexemeHash, string sGramHash, List<string> forms)
        {
            Dictionary<string, List<string>> paradigm;
            if (!m_Lexemes.TryGetValue(sLexemeHash, out paradigm))
            {
                System.Windows.MessageBox.Show("Internal error: Lexeme hash not found.");
                return false;
            }
            try
            {
                forms = paradigm[sGramHash];
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Internal error: Gram hash not recognized. " + ex.Message);
                return false;
            }
            return true;
        }

        public bool IsL2Optonal(CLexemeManaged lexeme)
        {
            return lexeme.bSecondLocativeOptional();
        }

        public string sGetL2Prepositions(CLexemeManaged lexeme)
        {
            return lexeme.sLoc2Preposition();
        }

        public void RemoveLexeme(CLexemeManaged l)
        {
            string sLexemeHash = l.sHash();
            m_Dictionary.Clear(l);
            m_Lexemes.Remove(sLexemeHash);
            m_LexemeHashToLexeme.Remove(sLexemeHash);
        }

        public void Clear()
        {
            m_Dictionary.Clear();
            m_Lexemes.Clear();
        }

/*
        public EM_ReturnCode OpenDictionary(string sPath)
        {
            Path = sPath;
            var eRet = m_Dictionary.eSetDbPath(Path);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                Path = "";
                System.Windows.MessageBox.Show("Unable to open dictionary.");
            }

            return eRet;
        }

        public EM_ReturnCode OpenEditDictionary(string sPath)
        {
            Path = sPath;
            var eRet = m_EditDictionary.eSetDbPath(Path);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                Path = "";
                System.Windows.MessageBox.Show("Unable to open edit dictionary.");
            }

            return eRet;
        }
*/

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

            m_Lexemes.Clear();

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
            eRet = m_Dictionary.eGetFirstLexeme(ref lexeme);
            if (EM_ReturnCode.H_NO_ERROR != eRet)
            {
                System.Windows.MessageBox.Show("Error: unable to retrieve lexeme");
                return;
            }

            do
            {
                if (!m_Lexemes.ContainsKey(lexeme.sHash()))
                {
                    eRet = lexeme.eGenerateParadigm();
                    if (eRet != EM_ReturnCode.H_NO_ERROR)
                    {
//                        System.Windows.MessageBox.Show("Error generating paradigm.");
//                        return;
                    }

                    if (lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NOUN || lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN ||
                        lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_NUM || lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_ADJ ||
                        lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_PRONOUN_ADJ || lexeme.ePartOfSpeech() == EM_PartOfSpeech.POS_VERB)
                    {
                        if (!bArrangeParadigm(lexeme))
                        {
                            System.Windows.MessageBox.Show("Unable to generate forms.");
                        }
                    }
                }

                eRet = m_Dictionary.eGetNextLexeme(ref lexeme);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
                System.Windows.MessageBox.Show("Error accessing lexeme collection.");
                return;
            }
        }       //  SearchByInitialForm()

        #region Regression
        public bool GetStoredLexemeData()
        {
            EM_ReturnCode eRet = m_Dictionary.eGetVerifier(ref m_Verifier);
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                System.Windows.MessageBox.Show("Unable to load Verifier.");
                return false;
            }

            eRet = m_Verifier.eLoadStoredLexemes();
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
                m_StoredLexemes[sLexemeHash] = sHeadword;
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

            var eRet = m_Dictionary.eImportTestData(sPath, callback);

            return eRet;
        }

        public EM_ReturnCode ExportRegressionData(string sPath, MainLibManaged.DelegateProgress callback)
        {
            if (null == m_Dictionary)
            {
                return EM_ReturnCode.H_UNAVAILABLE;
            }

            MainLibManaged.DelegateProgress DelegateProgress = new MainLibManaged.DelegateProgress(callback);

            var eRet = m_Dictionary.eExportTestData(sPath, callback);

            return eRet;
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
        private bool bArrangeParadigm(CLexemeManaged lexeme)
        {
            switch (lexeme.ePartOfSpeech())
            {
                case EM_PartOfSpeech.POS_NOUN:
                case EM_PartOfSpeech.POS_PRONOUN:
                case EM_PartOfSpeech.POS_NUM:
                    return bGenerateNominalForms(lexeme);

                case EM_PartOfSpeech.POS_ADJ:
                case EM_PartOfSpeech.POS_PRONOUN_ADJ:
                    return bGenerateAdjForms(lexeme);

                case EM_PartOfSpeech.POS_VERB:
                    return bGenerateVerbForms(lexeme);

                default:
                    System.Windows.MessageBox.Show("Part of speech not handled.");
                    return false;
            }

            //return true;
        }
 
        private bool bGenerateNominalForms(CLexemeManaged lexeme)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>();
            Dictionary<string, List<Tuple<string, string>>> comments = null;

            CWordFormManaged wf = null;
//            EM_ReturnCode eRet = (EM_ReturnCode)lexeme.eGenerateParadigm();
//            if (eRet != EM_ReturnCode.H_NO_ERROR)
//            {
//                System.Windows.MessageBox.Show("Error: unable to genetrate paradigm.");
//                return false;
//            }

            eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);

            while (EM_ReturnCode.H_NO_ERROR == eRet || EM_ReturnCode.H_FALSE == eRet)
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

                    if (!paradigm.ContainsKey(sKey))
                    {
                        paradigm[sKey] = new List<string>();
                    }

                    paradigm[sKey].Add(sWordForm);
                }
                catch(Exception ex)
                {
//                    return false;
                }

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            }   //  while...

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
//                System.Windows.MessageBox.Show("Error generating noun forms.");
                return true;        // OK to show empty paradigm
            }

            string sHash = lexeme.sHash();
            m_Lexemes[sHash] = paradigm;
            m_LexemeHashToLexeme[sHash] = lexeme;
            m_FormComments[sHash] = comments;

            return true;

        }   //  bGenerateNominalForms()

        private bool bGenerateAdjForms(CLexemeManaged lexeme)
        {
            EM_ReturnCode eRet = EM_ReturnCode.H_NO_ERROR;

            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>(); // hash -> form
            Dictionary<string, List<Tuple<string, string>>> comments = null;    // hash -> left comment, right comment

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                if (null == wf)
                {
                    continue;
                }

                string sKey = "";
                if (wf.eSubparadigm() == EM_Subparadigm.SUBPARADIGM_LONG_ADJ)
                {
                    sKey = "AdjL_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_"
                        + Helpers.sCaseToString(wf.eCase());
                }
                else if (wf.eSubparadigm() == EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ)
                {
                    sKey = "PronAdj_";
                    if (wf.eNumber() == EM_Number.NUM_SG)
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_"
                        + Helpers.sCaseToString(wf.eCase());
                }
                else if (wf.eSubparadigm() == EM_Subparadigm.SUBPARADIGM_SHORT_ADJ)
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
                else if (wf.eSubparadigm() == EM_Subparadigm.SUBPARADIGM_COMPARATIVE)
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
                    string sWordForm = wf.sWordForm();
                    Helpers.MarkStress(ref sWordForm, wf);

                    if (!paradigm.ContainsKey(sKey))
                    {
                        paradigm[sKey] = new List<string>();
                    }
                    paradigm[sKey].Add(sWordForm);
                }

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            }   //  while... 

            string sHash = lexeme.sHash();
            m_Lexemes[sHash] = paradigm;
            m_LexemeHashToLexeme[sHash] = lexeme;
            m_FormComments[sHash] = comments;

            EM_Subparadigm eSp = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
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
                eSp = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
            }

            HandleAccusatives(lexeme, eSp);

            return true;

        }   //  GenerateAdjForms()

        private bool bGenerateVerbForms(CLexemeManaged lexeme)
        {
            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>();
            Dictionary<string, List<Tuple<string, string>>> comments = null;

            CWordFormManaged wf = null;
            var eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);
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
                        sKey = "Infinitive";
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
                        sKey = "VAdvPres";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                        sKey = "VAdvPast";
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
                    if (!paradigm.ContainsKey(sKey))
                    {
                        paradigm[sKey] = new List<string>();
                    }
                    paradigm[sKey].Add(sWordForm);
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

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);
                if ((eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_NO_MORE) || null == wf)
                {
                    System.Windows.MessageBox.Show("Unable to load a word form.");
                    return false;
                }

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            string sHash = lexeme.sHash();
            m_Lexemes[sHash] = paradigm;
            m_LexemeHashToLexeme[sHash] = lexeme;
            m_FormComments[sHash] = comments;

            if (paradigm.ContainsKey("PPresA_M_Sg_N"))
            {
                HandleAccusatives(lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT);
            }
            if (paradigm.ContainsKey("PPastA_M_Sg_N"))
            {
                HandleAccusatives(lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT);
            }
            if (paradigm.ContainsKey("PPresPL_M_Sg_N"))
            {
                HandleAccusatives(lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG);
            }
            if (paradigm.ContainsKey("PPastPL_M_Sg_N"))
            {
                HandleAccusatives(lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG);
            }

            return true;

        }   //  GenerateVerbForms()

        public bool bLeadComment(string sLexemeHash, string sFormHash, ref string sComment)
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

            sComment = wf.sLeadComment();

            return true;
        }

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

        public bool bIsIrregular(string sLexemeHash, string sFormHash)
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
//                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIrregular();
        }

        public bool bIsEdited(string sLexemeHash, string sFormHash)
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
//                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIsEdited();
        }

        public bool bIsVariant(string sLexemeHash, string sFormHash)
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
                //                System.Windows.MessageBox.Show("Unable to check word form edit status.");
                return false;
            }

            return wf.bIsVariant();
        }

        private void HandleAccusatives(CLexemeManaged lexeme, EM_Subparadigm eSubparadigm)
        {
            Dictionary<string, List<string>> paradigm;
            if (!m_Lexemes.TryGetValue(lexeme.sHash(), out paradigm))
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
                List<string> values = paradigm[sPrefix + "M_Sg_N"];
                foreach (var sForm in values)
                {
                    if (!paradigm.ContainsKey(sPrefix + "M_Sg_A_Inanim"))
                    {
                        paradigm[sPrefix + "M_Sg_A_Inanim"] = new List<string>();
                    }
                    paradigm[sPrefix + "M_Sg_A_Inanim"].Add(sForm);
                }

                values = paradigm[sPrefix + "M_Sg_G"];
                foreach (var sForm in values)
                {
                    if (!paradigm.ContainsKey(sPrefix + "M_Sg_A_Anim"))
                    {
                        paradigm[sPrefix + "M_Sg_A_Anim"] = new List<string>();
                    }
                    paradigm[sPrefix + "M_Sg_A_Anim"].Add(sForm);
                }

                values = paradigm[sPrefix + "Pl_N"];
                foreach (var sForm in values)
                {
                    if (!paradigm.ContainsKey(sPrefix + "Pl_A_Inanim"))
                    {
                        paradigm[sPrefix + "Pl_A_Inanim"] = new List<string>();
                    }
                    paradigm[sPrefix + "Pl_A_Inanim"].Add(sForm);
                }

                values = paradigm[sPrefix + "Pl_G"];
                foreach (var sForm in values)
                {
                    if (!paradigm.ContainsKey(sPrefix + "Pl_A_Anim"))
                    {
                        paradigm[sPrefix + "Pl_A_Anim"] = new List<string>();
                    }
                    paradigm[sPrefix + "Pl_A_Anim"].Add(sForm);
                }
            }
            catch (KeyNotFoundException exKey)
            {
                System.Windows.MessageBox.Show("Error: unable to find form:  " + exKey.Message);
            }
            catch (Exception exOther)
            {
                System.Windows.MessageBox.Show("Exception: " + exOther.Message);
            }
        }       //  HandleAccusatives()

        #endregion

        public bool bGetFormByGramHash(CLexemeManaged l, string sHash, out List<string> forms)
        {
            forms = null;
            Dictionary<string, List<string>> paradigm;
            if (m_Lexemes.TryGetValue(l.sHash(), out paradigm))
            {
                if (paradigm.TryGetValue(sHash, out forms))
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
