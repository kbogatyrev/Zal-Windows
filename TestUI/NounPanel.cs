using System.Collections.Generic;

namespace TestUI
{
    public partial class NounPanel : WordFormsPanel
    {
        public string sLexName
        {
            get
            {
                return textBoxLexName.Text;
            }

            set
            {
                textBoxLexName.Text = value;
            }
        }

        public NounPanel()
        {
            InitializeComponent();

            m_dictForms = new Dictionary<string, StCell>();
            m_dictForms.Add ("NSg", new StCell (textBoxNSg));
            m_dictForms.Add ("ASg", new StCell (textBoxASg));
            m_dictForms.Add ("GSg", new StCell (textBoxGSg));
            m_dictForms.Add ("PSg", new StCell (textBoxLSg));
            m_dictForms.Add ("DSg", new StCell (textBoxDSg));
            m_dictForms.Add ("ISg", new StCell (textBoxISg));

            m_dictForms.Add ("NPl", new StCell (textBoxNPl));
            m_dictForms.Add ("APl", new StCell (textBoxAPl));
            m_dictForms.Add ("GPl", new StCell (textBoxGPl));
            m_dictForms.Add ("PPl", new StCell (textBoxLPl));
            m_dictForms.Add ("DPl", new StCell (textBoxDPl));
            m_dictForms.Add ("IPl", new StCell (textBoxIPl));

            m_dictForms.Add("G2Sg", new StCell(textBoxG2));
            m_dictForms.Add("P2Sg", new StCell(textBoxL2));

        }   // NounPanel

    }   //  public partial class NounPanel 

}   //  namespace TestUI
