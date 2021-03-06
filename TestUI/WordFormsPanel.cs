using MainLibManaged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestUI
{
    public partial class WordFormsPanel : UserControl
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WordFormsPanel
            // 
            this.Name = "WordFormsPanel";
            this.Size = new System.Drawing.Size(282, 154);
            this.ResumeLayout(false);

        }

        public struct StCell
        {
            public TextBox m_TextboxTarget;
            public HashSet<string> m_SetContents;

            public StCell(TextBox textBoxWordForms)
            {
                m_TextboxTarget = textBoxWordForms;
                m_SetContents = new HashSet<string>();
            }
        }

        protected Dictionary<string, StCell> m_dictForms;

        protected void Clear()
        {
            foreach (KeyValuePair<string, StCell> entry in m_dictForms)
            {
                entry.Value.m_TextboxTarget.Text.Remove(0);
            }
        }

        public void SetForm(string sKey, string sText, EM_Status eStatus)
        {
            try
            {
                switch (eStatus)
                {
                    case EM_Status.STATUS_COMMON:
                        {
                            break;
                        }
                    case EM_Status.STATUS_INCORRECT:
                        {
                            sText = "(Incorrect) " + sText;
                            break;
                        }
                    case EM_Status.STATUS_OBSOLETE:
                        {
                            sText += " (Устар.)";
                            break;
                        }
                    case EM_Status.STATUS_ASSUMED:
                        {
                            sText = "(Предпол.) *" + sText;
                            break;
                        }
                    case EM_Status.STATUS_QUESTIONABLE:
                        {
                            sText = "(Затрудн.) ?" + sText;
                            break;
                        }
                    case EM_Status.STATUS_RARE:
                        {
                            sText += " (Rare)";
                            break;
                        }
                }

                if (m_dictForms[sKey].m_SetContents.Contains(sText))
                {
                    return;
                }

                m_dictForms[sKey].m_SetContents.Add(sText);

                if (m_dictForms[sKey].m_TextboxTarget.Text.Length > 0)
                {
                    m_dictForms[sKey].m_TextboxTarget.Text += "\r\n";
                }
                m_dictForms[sKey].m_TextboxTarget.Text += sText;

                if (m_dictForms[sKey].m_TextboxTarget.Lines.Length > 1)
                {
                    m_dictForms[sKey].m_TextboxTarget.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    m_dictForms[sKey].m_TextboxTarget.ScrollBars = ScrollBars.None;
                }

            }
            catch (Exception ex)
            {
                //                MainLib.ZalError err = new MainLib.ZalError();
                string sMsg = "SetForm: ";
                //                sMsg += err.LastError;
                sMsg += ex.Message;
                MessageBox.Show(sMsg, "Zal Error", MessageBoxButtons.OK);
                return;
            }
        }   //  SetForm (...)
    }
}
