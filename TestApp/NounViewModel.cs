namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
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
        private string m_Noun_Sg_N = "моча\x301лка";
        public string Noun_Sg_N
        {
            get
            {
                return m_Noun_Sg_N;
            }
            set
            {
                m_sSourceForm = value;
                OnPropertyChanged("Noun_Sg_N");
            }
        }
    }
}
