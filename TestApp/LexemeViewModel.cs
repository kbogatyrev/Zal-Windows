using MainLibManaged;

namespace ZalTestApp
{
    public class LexemeViewModel : ViewModelBase
    {
        private CDictionaryManaged m_Engine;
        public CDictionaryManaged Engine
        {
            set
            {
                m_Engine = value;
            }
        }

        private string m_sSourceForm = "псевдосинхрофазотрон";
        public string SourceForm
        {
            get
            {
                return m_sSourceForm;
            }
            set
            {
                m_sSourceForm = value;
                OnPropertyChanged("SourceForm");
            }
        }
        private string m_sGraphicStem = "псевдосинхрофазотрон";
        public string GraphicStem
        {
            get
            {
                return m_sGraphicStem;
            }
            set
            {
                m_sGraphicStem = value;
                OnPropertyChanged("GraphicStem");
            }
        }
    }
}
