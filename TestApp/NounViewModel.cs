using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;

namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        MainModel m_MainModel = null;

        #region ICommand
        private ICommand m_BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return m_BackCommand;
            }
            set
            {
                m_BackCommand = value;
            }
        }
        #endregion

        #region Bindings

        private string m_sSourceForm;
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

        private string m_sNoun_Sg_N;
        public string Noun_Sg_N
        {
            get
            {
                return m_sNoun_Sg_N;
            }
            set
            {
                m_sNoun_Sg_N = value;
                OnPropertyChanged("Noun_Sg_N");
            }
        }

        private string m_sNoun_Sg_A;
        public string Noun_Sg_A
        {
            get
            {
                return m_sNoun_Sg_A;
            }
            set
            {
                m_sNoun_Sg_A = value;
                OnPropertyChanged("Noun_Sg_A");
            }
        }

        private string m_sNoun_Sg_G;
        public string Noun_Sg_G
        {
            get
            {
                return m_sNoun_Sg_G;
            }
            set
            {
                m_sNoun_Sg_G = value;
                OnPropertyChanged("Noun_Sg_G");
            }
        }

        private string m_sNoun_Sg_P;
        public string Noun_Sg_P
        {
            get
            {
                return m_sNoun_Sg_P;
            }
            set
            {
                m_sNoun_Sg_P = value;
                OnPropertyChanged("Noun_Sg_P");
            }
        }

        private string m_sNoun_Sg_D;
        public string Noun_Sg_D
        {
            get
            {
                return m_sNoun_Sg_D;
            }
            set
            {
                m_sNoun_Sg_D = value;
                OnPropertyChanged("Noun_Sg_D");
            }
        }

        private string m_sNoun_Sg_I;
        public string Noun_Sg_I
        {
            get
            {
                return m_sNoun_Sg_I;
            }
            set
            {
                m_sNoun_Sg_I = value;
                OnPropertyChanged("Noun_Sg_D");
            }
        }
        
        private string m_sNoun_Sg_Part;
        public string Noun_Sg_Part
        {
            get
            {
                return m_sNoun_Sg_Part;
            }
            set
            {
                m_sNoun_Sg_Part = value;
                OnPropertyChanged("Noun_Sg_Part");
            }
        }

        private string m_sNoun_Sg_L;
        public string Noun_Sg_L
        {
            get
            {
                return m_sNoun_Sg_L;
            }
            set
            {
                m_sNoun_Sg_L = value;
                OnPropertyChanged("Noun_Sg_L");
            }
        }

        private bool m_bIs_L2_optional = false;
        public bool Is_L2_optional
        {
            get
            {
                return m_bIs_L2_optional;
            }
            set
            {
                m_bIs_L2_optional = value;
                OnPropertyChanged("Is_L2_optional");
            }
        }

        private string m_sNoun_Sg_L2_Prepositions;
        public string Noun_Sg_L2_Prepositions
        {
            get
            {
                return m_sNoun_Sg_L2_Prepositions;
            }
            set
            {
                m_sNoun_Sg_L2_Prepositions = value;
                OnPropertyChanged("Noun_Sg_L2_Prepositions");
            }
        }

        private string m_sNoun_Pl_N;
        public string Noun_Pl_N
        {
            get
            {
                return m_sNoun_Pl_N;
            }
            set
            {
                m_sNoun_Pl_N = value;
                OnPropertyChanged("Noun_Pl_N");
            }
        }

        private string m_sNoun_Pl_A;
        public string Noun_Pl_A
        {
            get
            {
                return m_sNoun_Pl_A;
            }
            set
            {
                m_sNoun_Pl_A = value;
                OnPropertyChanged("Noun_Pl_A");
            }
        }

        private string m_sNoun_Pl_G;
        public string Noun_Pl_G
        {
            get
            {
                return m_sNoun_Pl_G;
            }
            set
            {
                m_sNoun_Pl_G = value;
                OnPropertyChanged("Noun_Pl_G");
            }
        }

        private string m_sNoun_Pl_P;
        public string Noun_Pl_P
        {
            get
            {
                return m_sNoun_Pl_P;
            }
            set
            {
                m_sNoun_Pl_P = value;
                OnPropertyChanged("Noun_Pl_P");
            }
        }

        private string m_sNoun_Pl_D;
        public string Noun_Pl_D
        {
            get
            {
                return m_sNoun_Pl_D;
            }
            set
            {
                m_sNoun_Pl_D = value;
                OnPropertyChanged("Noun_Pl_D");
            }
        }

        private string m_sNoun_Pl_I;
        public string Noun_Pl_I
        {
            get
            {
                return m_sNoun_Pl_I;
            }
            set
            {
                m_sNoun_Pl_I = value;
                OnPropertyChanged("Noun_Pl_D");
            }
        }

        private string m_sNoun_Pl_L;
        public string Noun_Pl_L
        {
            get
            {
                return m_sNoun_Pl_L;
            }
            set
            {
                m_sNoun_Pl_L = value;
                OnPropertyChanged("Noun_Pl_L");
            }
        }

        #endregion

        public NounViewModel(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            m_MainModel = m;
            string sLexemeHash = lexeme.sHash();

            List<string> forms = null;
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_N", out forms);
            Noun_Sg_N = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_A", out forms);
            Noun_Sg_A = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_G", out forms);
            Noun_Sg_G = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_P", out forms);
            Noun_Sg_P = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_D", out forms);
            Noun_Sg_D = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_I", out forms);
            Noun_Sg_I = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_Part", out forms);
            Noun_Sg_Part = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_L", out forms);

            Noun_Sg_L = Helpers.sListToCommaSeparatedString(forms);
            Noun_Sg_L2_Prepositions = m_MainModel.sGetL2Prepositions(lexeme);

            Is_L2_optional = m_MainModel.IsL2Optonal(lexeme);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_N", out forms);
            Noun_Pl_N = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_A", out forms);
            Noun_Pl_A = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_G", out forms);
            Noun_Pl_G = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_P", out forms);
            Noun_Pl_P = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_D", out forms);
            Noun_Pl_D = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_I", out forms);
            Noun_Pl_I = Helpers.sListToCommaSeparatedString(forms);
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }
    }
}
