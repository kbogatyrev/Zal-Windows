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
        public CVerifierManaged m_Verifier = null; 

        private Dictionary<CLexemeManaged, Dictionary<string, List<string>>> m_Lexemes;
        public IEnumerator GetEnumerator()
        {
            return m_Lexemes.Keys.GetEnumerator();
        }

        private Dictionary<string, string> m_StoredLexemes; // source form --> hash
        public IEnumerator GetStoredLexemesEnumerator()
        {
            return m_StoredLexemes.GetEnumerator();
        }

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

        #region Properties
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
            // TODO: error checking...
            m_Dictionary = new CDictionaryManaged();
//            m_Dictionary.eGetVerifier(ref m_Verifier);
            m_Lexemes = new Dictionary<CLexemeManaged, Dictionary<string, List<string>>>();
            m_StoredLexemes = new Dictionary<string, string>();
        }

        public bool GetFormsByGramHash(CLexemeManaged lexeme, string sHash, out List<string> forms)
        {
            forms = null;

            Dictionary<string, List<string>> paradigm;
            if (!m_Lexemes.TryGetValue(lexeme, out paradigm))
            {
                System.Windows.MessageBox.Show("Lexeme not found.");
                return false;
            }

            try
            {
                forms = paradigm[sHash];
            }
            catch (Exception ex)
            {
//                System.Windows.MessageBox.Show("Gram hash not recognized." + ex.Message);
                return false;
            }
            return true;
        }

        public void RemoveLexeme(CLexemeManaged l)
        {
            m_Dictionary.Clear(l);
            m_Lexemes.Remove(l);
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

            CLexemeManaged lexeme = null;
            eRet = m_Dictionary.eGetFirstLexeme(ref lexeme);
            if (EM_ReturnCode.H_NO_ERROR != eRet)
            {
                System.Windows.MessageBox.Show("Lexeme not found.");
                return;
            }

            do
            {
                if (m_Lexemes.ContainsKey(lexeme))
                {
                    m_Lexemes.Remove(lexeme);
                }

                eRet = lexeme.eGenerateParadigm();
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    System.Windows.MessageBox.Show("Error generating paradigm.");
                    return;
                }

                if (!bArrangeParadigm(lexeme))
                {
                    System.Windows.MessageBox.Show("Unable to generate forms.");
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
        public void RunRegression()
        {

        }

        public void ExportTestData(string str)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "CSV Files(*.csv)| *.csv | All Files(*.*) | *.* ";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialog1.FileName;
                var eRet = m_Dictionary.eSetDbPath(Path);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    Path = "";
                    System.Windows.MessageBox.Show("Unable to open export file.");
                }
            }
        }

        public void ImportTestData(string str)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialog1.FileName;
                var eRet = m_Dictionary.eSetDbPath(Path);
                if (eRet != EM_ReturnCode.H_NO_ERROR)
                {
                    Path = "";
                    System.Windows.MessageBox.Show("Unable to open import file.");
                }
            }
        }

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
                m_StoredLexemes[sHeadword] = sLexemeHash;
                eRet = m_Verifier.eGetNextLexemeData(ref sLexemeHash, ref sHeadword);
            }

            return true;
        }
        #endregion

        #region FormGeneration
        private bool bArrangeParadigm(CLexemeManaged lexeme)
        {
            switch (lexeme.ePartOfSpeech())
            {
                case EM_PartOfSpeech.POS_NOUN:
                    return bGenerateNounForms(lexeme);

                case EM_PartOfSpeech.POS_ADJ:
                    return bGenerateAdjForms(lexeme);

                case EM_PartOfSpeech.POS_VERB:
                    return bGenerateVerbForms(lexeme);

                default:
                    System.Windows.MessageBox.Show("Part of speech not handled.");
                    return false;
            }

            //return true;
        }
 
        private bool bGenerateNounForms(CLexemeManaged lexeme)
        {
            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>();

            CWordFormManaged wf = null;
            EM_ReturnCode eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
                string sKey = "Noun_" + Helpers.sNumberToString(wf.eNumber());
                sKey += "_";
                sKey += Helpers.sCaseToString(wf.eCase());

                string sWordForm = wf.sWordForm();
                Helpers.MarkStress(ref sWordForm, wf);

                if (!paradigm.ContainsKey(sKey))
                {
                    paradigm[sKey] = new List<string>();
                }
                paradigm[sKey].Add(sWordForm);

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            }   //  while...

            if (eRet != EM_ReturnCode.H_NO_MORE)
            {
                System.Windows.MessageBox.Show("Error generating noun forms.");
                return false;
            }

            m_Lexemes[lexeme] = paradigm;

            return true;

        }   //  bGenerateNounForms()

        private bool bGenerateAdjForms(CLexemeManaged lexeme)
        {
            EM_ReturnCode eRet = lexeme.eGenerateParadigm();
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                return false;
            }

            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>();

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);

            while (EM_ReturnCode.H_NO_ERROR == eRet)
            {
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

            m_Lexemes[lexeme] = paradigm;

            HandleAccusatives(lexeme, EM_Subparadigm.SUBPARADIGM_LONG_ADJ);

            return true;

        }   //  GenerateAdjForms()

        private void HandleAccusatives(CLexemeManaged lexeme, EM_Subparadigm eSubparadigm)
        {
            Dictionary<string, List<string>> paradigm;
            if (!m_Lexemes.TryGetValue(lexeme, out paradigm))
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

        private bool bGenerateVerbForms(CLexemeManaged lexeme)
        {
            Dictionary<string, List<string>> paradigm = new Dictionary<string, List<string>>();

            CWordFormManaged wf = null;
            var eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);
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

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            m_Lexemes[lexeme] = paradigm;

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
        #endregion

        public bool bGetFormByGramHash(CLexemeManaged l, string sHash, out List<string> forms)
        {
            forms = null;
            Dictionary<string, List<string>> paradigm;
            if (m_Lexemes.TryGetValue(l, out paradigm))
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
