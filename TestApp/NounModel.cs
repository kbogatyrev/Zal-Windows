using System;
using System.Collections.Generic;
using System.Windows;

using MainLibManaged;

namespace ZalTestApp
{
    public class NounModel
    {
        public Dictionary<string, string> m_GramHashToWordForm;

        public NounModel()
        {
            m_GramHashToWordForm = new Dictionary<string, string>();
        }

        public bool bGenerateNounForms(CLexemeManaged lexeme)
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
                string sKey = "Noun_" + Helpers.sNumberToString(wf.eNumber());
                sKey += "_";
                sKey += Helpers.sCaseToString(wf.eCase()); 

                string sWordForm = wf.sWordForm();
                Helpers.MarkStress(ref sWordForm, wf);

                m_GramHashToWordForm[sKey] = sWordForm;

                eRet = (EM_ReturnCode)lexeme.eGetNextWordForm(ref wf);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

            return true;

        }   //  bGenerateNounForms()

        public string sGetFormByGramHash(string sHash)
        {
            string sValue = "";
            try
            {
                sValue = m_GramHashToWordForm[sHash];
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
        }

    }       // class NounModel
}

