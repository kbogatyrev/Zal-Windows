using MainLibManaged;
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
        }

        public LexemeGridViewModel()
        {
            m_Lexemes = new ObservableCollection<LexemeViewModel>();
        }

        public void Add (LexemeViewModel lvm)
        {
            m_Lexemes.Add(lvm);
        }
    }
}
