using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class NounViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void EditButtonHandler();
//        public event EditButtonHandler EditButtonEvent;

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

        private ICommand m_EditCommand;
        public ICommand EditCommand
        {
            get
            {
                return m_EditCommand;
            }
            set
            {
                m_EditCommand = value;
            }
        }

        #endregion

        #region Bindings

        private bool m_bReadOnly;
        public bool ReadOnly
        {
            get
            {
                return m_bReadOnly;
            }
            set
            {
                m_bReadOnly = value;
                OnPropertyChanged("ReadOnly");
            }
        }

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

        private bool m_bNoun_Sg_N_IsReadOnly = true;
        public bool Noun_Sg_N_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_N_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_N_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_N_IsReadOnly");
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

        private bool m_bNoun_Sg_A_IsReadOnly = true;
        public bool Noun_Sg_A_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_A_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_A_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_A_IsReadOnly");
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

        private bool m_bNoun_Sg_G_IsReadOnly = true;
        public bool Noun_Sg_G_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_G_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_G_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_G_IsReadOnly");
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

        private bool m_bNoun_Sg_P_IsReadOnly = true;
        public bool Noun_Sg_P_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_P_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_P_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_P_IsReadOnly");
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

        private bool m_bNoun_Sg_D_IsReadOnly = true;
        public bool Noun_Sg_D_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_D_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_D_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_D_IsReadOnly");
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
                OnPropertyChanged("Noun_Sg_I");
            }
        }

        private bool m_bNoun_Sg_I_IsReadOnly = true;
        public bool Noun_Sg_I_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_I_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_I_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_I_IsReadOnly");
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

        private bool m_bNoun_Sg_Part_IsReadOnly = true;
        public bool Noun_Sg_Part_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_Part_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_Part_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_Part_IsReadOnly");
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

        private bool m_bNoun_Sg_L_IsReadOnly = true;
        public bool Noun_Sg_L_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_L_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_L_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_L_IsReadOnly");
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

        private bool m_bIs_L2_optional_IsReadOnly = true;
        public bool IsL2_optional_IsReadOnly
        {
            get
            {
                return m_bIs_L2_optional_IsReadOnly;
            }
            set
            {
                m_bIs_L2_optional_IsReadOnly = value;
                OnPropertyChanged("Is_L2_optional_IsReadOnly");
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

        private bool m_bNoun_Sg_L2_Prepositions_IsReadOnly = true;
        public bool Noun_Sg_L2_Prepositions_IsReadOnly
        {
            get
            {
                return m_bNoun_Sg_L2_Prepositions_IsReadOnly;
            }
            set
            {
                m_bNoun_Sg_L2_Prepositions_IsReadOnly = value;
                OnPropertyChanged("Noun_Sg_L2_Prepositions_IsReadOnly");
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

        private bool m_bNoun_Pl_N_IsReadOnly = true;
        public bool Noun_Pl_N_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_N_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_N_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_N_IsReadOnly");
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

        private bool m_bNoun_Pl_A_IsReadOnly = true;
        public bool Noun_Pl_A_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_A_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_A_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_A_IsReadOnly");
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

        private bool m_bNoun_Pl_G_IsReadOnly = true;
        public bool Noun_Pl_G_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_G_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_G_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_G_IsReadOnly");
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

        private bool m_bNoun_Pl_P_IsReadOnly = true;
        public bool Noun_Pl_P_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_P_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_P_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_P_IsReadOnly");
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

        private bool m_bNoun_Pl_D_IsReadOnly = true;
        public bool Noun_Pl_D_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_D_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_D_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_D_IsReadOnly");
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

        private bool m_bNoun_Pl_I_IsReadOnly = true;
        public bool Noun_Pl_I_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_I_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_I_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_I_IsReadOnly");
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

        private bool m_bNoun_Pl_L_IsReadOnly = true;
        public bool Noun_Pl_L_IsReadOnly
        {
            get
            {
                return m_bNoun_Pl_L_IsReadOnly;
            }
            set
            {
                m_bNoun_Pl_L_IsReadOnly = value;
                OnPropertyChanged("Noun_Pl_L_IsReadOnly");
            }
        }

        private bool m_bEditEnabled = false;
        public bool EditEnabled
        {
            get
            {
                return m_bEditEnabled;
            }
            set
            {
                m_bEditEnabled = value;
                OnPropertyChanged("EditEnabled");
            }
        }

        #endregion

        public NounViewModel(CLexemeManaged lexeme, MainModel m)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            EditCommand = new RelayCommand(new Action<object>(EditForm));

//            ReadOnly = true;
            EditEnabled = true;

            m_MainModel = m;
            string sLexemeHash = lexeme.sHash();

            List<string> m_Forms = null;

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_N", out m_Forms);
            Noun_Sg_N = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_A", out m_Forms);
            Noun_Sg_A = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_G", out m_Forms);
            Noun_Sg_G = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_P", out m_Forms);
            Noun_Sg_P = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_D", out m_Forms);
            Noun_Sg_D = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_I", out m_Forms);
            Noun_Sg_I = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_Part", out m_Forms);
            Noun_Sg_Part = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Sg_L", out m_Forms);
            Noun_Sg_L = Helpers.sListToCommaSeparatedString(m_Forms);

            Noun_Sg_L2_Prepositions = m_MainModel.sGetL2Prepositions(lexeme);

            Is_L2_optional = m_MainModel.IsL2Optonal(lexeme);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_N", out m_Forms);
            Noun_Pl_N = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_A", out m_Forms);
            Noun_Pl_A = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_G", out m_Forms);
            Noun_Pl_G = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_P", out m_Forms);
            Noun_Pl_P = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_D", out m_Forms);
            Noun_Pl_D = Helpers.sListToCommaSeparatedString(m_Forms);

            m_MainModel.GetFormsByGramHash(sLexemeHash, "Noun_Pl_I", out m_Forms);
            Noun_Pl_I = Helpers.sListToCommaSeparatedString(m_Forms);
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void EditForm(Object obj)
        {
//            Noun_Sg_N_IsReadOnly = false;

            var sPropName = obj as string;
            switch (obj as string)
            {
                case "Noun_Sg_N":
                    Noun_Sg_N_IsReadOnly = !Noun_Sg_N_IsReadOnly;
                    break;
                case "Noun_Sg_A":
                    Noun_Sg_A_IsReadOnly = !Noun_Sg_A_IsReadOnly;
                    break;
                case "Noun_Sg_G":
                    Noun_Sg_G_IsReadOnly = !Noun_Sg_G_IsReadOnly;
                    break;
                case "Noun_Sg_P":
                    Noun_Sg_P_IsReadOnly = !Noun_Sg_P_IsReadOnly;
                    break;
                case "Noun_Sg_D":
                    Noun_Sg_D_IsReadOnly = !Noun_Sg_D_IsReadOnly;
                    break;
                case "Noun_Sg_I":
                    Noun_Sg_I_IsReadOnly = !Noun_Sg_I_IsReadOnly;
                    break;
                case "Noun_Pl_N":
                    Noun_Pl_N_IsReadOnly = !Noun_Pl_N_IsReadOnly;
                    break;
                case "Noun_Pl_A":
                    Noun_Pl_A_IsReadOnly = !Noun_Pl_A_IsReadOnly;
                    break;
                case "Noun_Pl_G":
                    Noun_Pl_G_IsReadOnly = !Noun_Pl_G_IsReadOnly;
                    break;
                case "Noun_Pl_P":
                    Noun_Pl_P_IsReadOnly = !Noun_Pl_P_IsReadOnly;
                    break;
                case "Noun_Pl_D":
                    Noun_Pl_D_IsReadOnly = !Noun_Pl_D_IsReadOnly;
                    break;
                case "Noun_Pl_I":
                    Noun_Pl_I_IsReadOnly = !Noun_Pl_I_IsReadOnly;
                    break;
                default:
                    MessageBox.Show("Internal error: unknown word form hash.");
                    break;
            }       // switch
        }
    }
}
