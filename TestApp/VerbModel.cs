using System;
using System.Collections.Generic;
using System.Windows;

using MainLibManaged;

namespace ZalTestApp
{
    public class VerbModel
    {
        public Dictionary<string, List<string>> m_GramHashToWordForm;

        public VerbModel()
        {
            m_GramHashToWordForm = new Dictionary<string, List<string>>();
        }

        public bool bGenerateVerbForms(CLexemeManaged lexeme)
        {
            EM_ReturnCode eRet = lexeme.eGenerateParadigm();
            if (eRet != EM_ReturnCode.H_NO_ERROR)
            {
                return false;
            }

            m_GramHashToWordForm.Clear();

            CWordFormManaged wf = null;
            eRet = (EM_ReturnCode)lexeme.eGetFirstWordForm(ref wf);
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
                        // Only need N Sg m for initial display
                        if (wf.eNumber() == EM_Number.NUM_SG && wf.eGender() == EM_Gender.GENDER_M &&
                            wf.eCase() == EM_Case.CASE_NOM)
                        {
                            sKey = "PPresA_M_Sg_N";
                        }
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                        if (wf.eNumber() == EM_Number.NUM_SG && wf.eGender() == EM_Gender.GENDER_M &&
                            wf.eCase() == EM_Case.CASE_NOM)
                        {
                            sKey = "PPastA_M_Sg_N";
                        }
                        break;

                    case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT:
                        sKey = "VAdvPres";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST:
                        sKey = "VAdvPast";
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                        if (wf.eNumber() == EM_Number.NUM_SG && wf.eGender() == EM_Gender.GENDER_M &&
                            wf.eCase() == EM_Case.CASE_NOM)
                        {
                            sKey = "PPresPL_M_Sg_N";
                        }
                        break;

                    case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                        if (wf.eNumber() == EM_Number.NUM_SG && wf.eGender() == EM_Gender.GENDER_M &&
                            wf.eCase() == EM_Case.CASE_NOM)
                        {
                            sKey = "PPastPL_M_Sg_N";
                        }
                        break;

                }   // switch...

                if (sKey.Length > 0)
                {
                    var sWordForm = wf.sWordForm();
                    Helpers.MarkStress(ref sWordForm, wf);
                    if (!m_GramHashToWordForm.ContainsKey(sKey))
                    {
                        m_GramHashToWordForm[sKey] = new List<string>();
                    }
                    m_GramHashToWordForm[sKey].Add(sWordForm);
                }

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);


            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            return true;

        }   //  GenerateVerbForms()

        public string sGetFormByGramHash(string sHash)
        {
            string sValue = "";
            try
            {
                List<string> values = m_GramHashToWordForm[sHash];
                foreach (var sForm in values)
                {
                    if (sValue.Length > 0)
                    {
                        sValue += '\n';
                    }
                    sValue += sForm;
                }
            }
            catch (KeyNotFoundException)
            {
                sValue = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }

            return sValue;

        }   //  sGetFormByGramHash()

    }   // class VerbModel

}   //  namespace ZalTestApp
