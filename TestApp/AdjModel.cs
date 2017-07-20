using System;
using System.Collections.Generic;
using System.Windows;

using MainLibManaged;

namespace ZalTestApp
{
    public class AdjModel
    {
        public Dictionary<string, List<string>> m_GramHashToWordForm;

        public AdjModel()
        {
            m_GramHashToWordForm = new Dictionary<string, List<string>>();
        }

        public bool bGenerateAdjForms(CLexemeManaged lexeme)
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
                if (EM_ReturnCode.H_NO_ERROR != eRet)
                {
                    continue;
                }

                string sKey = "";
                if (wf.eSubparadigm() == EM_Subparadigm.SUBPARADIGM_LONG_ADJ)
                {
                    sKey = "AdjL_";
                    if (EM_Number.NUM_SG == wf.eNumber())
                    {
                        sKey += Helpers.sGenderToString(wf.eGender()) + "_";
                    }

                    sKey += Helpers.sNumberToString(wf.eNumber()) + "_" 
                        + Helpers.sCaseToString(wf.eCase());
                }

                string sWordForm = wf.sWordForm();
                Helpers.MarkStress(ref sWordForm, wf);

                if (!m_GramHashToWordForm.ContainsKey(sKey))
                {
                    m_GramHashToWordForm[sKey] = new List<string>();
                }
                m_GramHashToWordForm[sKey].Add(sWordForm);

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            try
            {
                List<string> values = m_GramHashToWordForm["AdjL_M_Sg_N"];
                foreach (var sForm in values)
                {
                    if (!m_GramHashToWordForm.ContainsKey("AdjL_M_Sg_A_Inanim"))
                    {
                        m_GramHashToWordForm["AdjL_M_Sg_A_Inanim"] = new List<string>();
                    }
                    m_GramHashToWordForm["AdjL_M_Sg_A_Inanim"].Add(sForm);
                }

                values = m_GramHashToWordForm["AdjL_M_Sg_G"];
                foreach (var sForm in values)
                {
                    if (!m_GramHashToWordForm.ContainsKey("AdjL_M_Sg_A_Anim"))
                    {
                        m_GramHashToWordForm["AdjL_M_Sg_A_Anim"] = new List<string>();
                    }
                    m_GramHashToWordForm["AdjL_M_Sg_A_Anim"].Add(sForm);
                }

                values = m_GramHashToWordForm["AdjL_Pl_N"];
                foreach (var sForm in values)
                {
                    if (!m_GramHashToWordForm.ContainsKey("AdjL_Pl_A_Inanim"))
                    {
                        m_GramHashToWordForm["AdjL_Pl_A_Inanim"] = new List<string>();
                    }
                    m_GramHashToWordForm["AdjL_Pl_A_Inanim"].Add(sForm);
                }

                values = m_GramHashToWordForm["AdjL_Pl_G"];
                foreach (var sForm in values)
                {
                    if (!m_GramHashToWordForm.ContainsKey("AdjL_Pl_A_Anim"))
                    {
                        m_GramHashToWordForm["AdjL_Pl_A_Anim"] = new List<string>();
                    }
                    m_GramHashToWordForm["AdjL_Pl_A_Anim"].Add(sForm);
                }
            }
            catch (KeyNotFoundException exKey)
            {
                MessageBox.Show("Error: unable to find form:  " + exKey.Message);
            }
            catch (Exception exOther)
            {
                MessageBox.Show("Exception: " + exOther.Message);
            }

            return true;

        }   //  GenerateAdjForms()

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

    }
}