using System.Collections.ObjectModel;

namespace ZalTestApp
{
    public class LexemeGridViewModel : ViewModelBase
    {
        private ObservableCollection<LexemeViewModel> m_Lexemes;
        public ObservableCollection<LexemeViewModel> LexemeCollection
        {
            get
            {
                return m_Lexemes;
            }

            set
            {
                m_Lexemes = value;
            }
        }
         
        public LexemeGridViewModel()
        {
            m_Lexemes = new ObservableCollection<LexemeViewModel>();
        }

        public void Add (LexemeViewModel lvm)
        {
            m_Lexemes.Add(lvm);
        }

        public void Remove(LexemeViewModel lvm)
        {
            m_Lexemes.Remove(lvm);
        }

        public void Clear()
        {
            m_Lexemes.Clear();
        }

    }
}
