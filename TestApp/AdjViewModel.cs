using System;
using System.Windows.Input;

using MainLibManaged;

namespace ZalTestApp
{
    public class AdjViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        AdjModel m_AdjModel = null;

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

        #region Bindings_Long_Masculine_Sg

        private string m_sAdjL_M_Sg_N;
        public string AdjL_M_Sg_N
        {
            get
            {
                return m_sAdjL_M_Sg_N;
            }
            set
            {
                m_sAdjL_M_Sg_N = value;
                OnPropertyChanged("AdjL_M_Sg_N");
            }
        }

        private string m_sAdjL_M_Sg_A_Anim;
        public string AdjL_M_Sg_A_Anim
        {
            get
            {
                return m_sAdjL_M_Sg_A_Anim;
            }
            set
            {
                m_sAdjL_M_Sg_A_Anim = value;
                OnPropertyChanged("AdjL_M_Sg_A_Anim");
            }
        }

        private string m_sAdjL_M_Sg_A_Inanim;
        public string AdjL_M_Sg_A_Inanim
        {
            get
            {
                return m_sAdjL_M_Sg_A_Inanim;
            }
            set
            {
                m_sAdjL_M_Sg_A_Inanim = value;
                OnPropertyChanged("AdjL_M_Sg_A_Inanim");
            }
        }

        private string m_sAdjL_M_Sg_G;
        public string AdjL_M_Sg_G
        {
            get
            {
                return m_sAdjL_M_Sg_G;
            }
            set
            {
                m_sAdjL_M_Sg_G = value;
                OnPropertyChanged("AdjL_M_Sg_G");
            }
        }

        private string m_sAdjL_M_Sg_P;
        public string AdjL_M_Sg_P
        {
            get
            {
                return m_sAdjL_M_Sg_P;
            }
            set
            {
                m_sAdjL_M_Sg_P = value;
                OnPropertyChanged("AdjL_M_Sg_P");
            }
        }

        private string m_sAdjL_M_Sg_D;
        public string AdjL_M_Sg_D
        {
            get
            {
                return m_sAdjL_M_Sg_D;
            }
            set
            {
                m_sAdjL_M_Sg_D = value;
                OnPropertyChanged("AdjL_M_Sg_D");
            }
        }

        private string m_sAdjL_M_Sg_I;
        public string AdjL_M_Sg_I
        {
            get
            {
                return m_sAdjL_M_Sg_I;
            }
            set
            {
                m_sAdjL_M_Sg_I = value;
                OnPropertyChanged("AdjL_M_Sg_I");
            }
        }

        #endregion

        #region Bindings_Long_Feminine_Sg

        private string m_sAdjL_F_Sg_N;
        public string AdjL_F_Sg_N
        {
            get
            {
                return m_sAdjL_F_Sg_N;
            }
            set
            {
                m_sAdjL_F_Sg_N = value;
                OnPropertyChanged("AdjL_F_Sg_N");
            }
        }

        private string m_sAdjL_F_Sg_A;
        public string AdjL_F_Sg_A
        {
            get
            {
                return m_sAdjL_F_Sg_A;
            }
            set
            {
                m_sAdjL_F_Sg_A = value;
                OnPropertyChanged("AdjL_F_Sg_A");
            }
        }

        private string m_sAdjL_F_Sg_G;
        public string AdjL_F_Sg_G
        {
            get
            {
                return m_sAdjL_F_Sg_G;
            }
            set
            {
                m_sAdjL_F_Sg_G = value;
                OnPropertyChanged("AdjL_F_Sg_G");
            }
        }

        private string m_sAdjL_F_Sg_P;
        public string AdjL_F_Sg_P
        {
            get
            {
                return m_sAdjL_F_Sg_P;
            }
            set
            {
                m_sAdjL_F_Sg_P = value;
                OnPropertyChanged("AdjL_F_Sg_P");
            }
        }

        private string m_sAdjL_F_Sg_D;
        public string AdjL_F_Sg_D
        {
            get
            {
                return m_sAdjL_F_Sg_D;
            }
            set
            {
                m_sAdjL_F_Sg_D = value;
                OnPropertyChanged("AdjL_F_Sg_D");
            }
        }

        private string m_sAdjL_F_Sg_I;
        public string AdjL_F_Sg_I
        {
            get
            {
                return m_sAdjL_F_Sg_I;
            }
            set
            {
                m_sAdjL_F_Sg_I = value;
                OnPropertyChanged("AdjL_F_Sg_I");
            }
        }

        #endregion

        #region Bindings_Long_Neuter_Sg

        private string m_sAdjL_N_Sg_N;
        public string AdjL_N_Sg_N
        {
            get
            {
                return m_sAdjL_N_Sg_N;
            }
            set
            {
                m_sAdjL_N_Sg_N = value;
                OnPropertyChanged("AdjL_N_Sg_N");
            }
        }

        private string m_sAdjL_N_Sg_A;
        public string AdjL_N_Sg_A
        {
            get
            {
                return m_sAdjL_N_Sg_A;
            }
            set
            {
                m_sAdjL_N_Sg_A = value;
                OnPropertyChanged("AdjL_N_Sg_A");
            }
        }

        private string m_sAdjL_N_Sg_G;
        public string AdjL_N_Sg_G
        {
            get
            {
                return m_sAdjL_N_Sg_G;
            }
            set
            {
                m_sAdjL_N_Sg_G = value;
                OnPropertyChanged("AdjL_N_Sg_G");
            }
        }

        private string m_sAdjL_N_Sg_P;
        public string AdjL_N_Sg_P
        {
            get
            {
                return m_sAdjL_N_Sg_P;
            }
            set
            {
                m_sAdjL_N_Sg_P = value;
                OnPropertyChanged("AdjL_N_Sg_P");
            }
        }

        private string m_sAdjL_N_Sg_D;
        public string AdjL_N_Sg_D
        {
            get
            {
                return m_sAdjL_N_Sg_D;
            }
            set
            {
                m_sAdjL_N_Sg_D = value;
                OnPropertyChanged("AdjL_N_Sg_D");
            }
        }

        private string m_sAdjL_N_Sg_I;
        public string AdjL_N_Sg_I
        {
            get
            {
                return m_sAdjL_N_Sg_I;
            }
            set
            {
                m_sAdjL_N_Sg_I = value;
                OnPropertyChanged("AdjL_N_Sg_I");
            }
        }

        #endregion

        #region Bindings_Long_Plural

        private string m_sAdjL_Pl_N;
        public string AdjL_Pl_N
        {
            get
            {
                return m_sAdjL_Pl_N;
            }
            set
            {
                m_sAdjL_Pl_N = value;
                OnPropertyChanged("AdjL_Pl_N");
            }
        }

        private string m_sAdjL_Pl_A_Anim;
        public string AdjL_Pl_A_Anim
        {
            get
            {
                return m_sAdjL_Pl_A_Anim;
            }
            set
            {
                m_sAdjL_Pl_A_Anim = value;
                OnPropertyChanged("AdjL_Pl_A_Anim");
            }
        }

        private string m_sAdjL_Pl_A_Inanim;
        public string AdjL_Pl_A_Inanim
        {
            get
            {
                return m_sAdjL_M_Sg_A_Inanim;
            }
            set
            {
                m_sAdjL_Pl_A_Inanim = value;
                OnPropertyChanged("AdjL_Pl_A_Inanim");
            }
        }

        private string m_sAdjL_Pl_G;
        public string AdjL_Pl_G
        {
            get
            {
                return m_sAdjL_Pl_G;
            }
            set
            {
                m_sAdjL_Pl_G = value;
                OnPropertyChanged("AdjL_Pl_G");
            }
        }

        private string m_sAdjL_Pl_P;
        public string AdjL_Pl_P
        {
            get
            {
                return m_sAdjL_Pl_P;
            }
            set
            {
                m_sAdjL_Pl_P = value;
                OnPropertyChanged("AdjL_Pl_P");
            }
        }

        private string m_sAdjL_Pl_D;
        public string AdjL_Pl_D
        {
            get
            {
                return m_sAdjL_Pl_D;
            }
            set
            {
                m_sAdjL_Pl_D = value;
                OnPropertyChanged("AdjL_Pl_D");
            }
        }

        private string m_sAdjL_Pl_I;
        public string AdjL_Pl_I
        {
            get
            {
                return m_sAdjL_Pl_I;
            }
            set
            {
                m_sAdjL_Pl_I = value;
                OnPropertyChanged("AdjL_Pl_I");
            }
        }

        #endregion

        public AdjViewModel(CLexemeManaged l)
        {
            BackCommand = new RelayCommand(new Action<object>(GoBack));
            m_AdjModel = new AdjModel();
            m_AdjModel.bGenerateAdjForms(l);

            AdjL_M_Sg_N = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_N");
            AdjL_M_Sg_A_Anim = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_A_Anim");
            AdjL_M_Sg_A_Inanim = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_A_Inanim");
            AdjL_M_Sg_G = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_G");
            AdjL_M_Sg_P = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_P");
            AdjL_M_Sg_D = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_D");
            AdjL_M_Sg_I = m_AdjModel.sGetFormByGramHash("AdjL_M_Sg_I");

            AdjL_F_Sg_N = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_N");
            AdjL_F_Sg_A = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_A");
            AdjL_F_Sg_G = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_G");
            AdjL_F_Sg_P = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_P");
            AdjL_F_Sg_D = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_D");
            AdjL_F_Sg_I = m_AdjModel.sGetFormByGramHash("AdjL_F_Sg_I");

            AdjL_N_Sg_N = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_N");
            AdjL_N_Sg_A = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_A");
            AdjL_N_Sg_G = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_G");
            AdjL_N_Sg_P = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_P");
            AdjL_N_Sg_D = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_D");
            AdjL_N_Sg_I = m_AdjModel.sGetFormByGramHash("AdjL_N_Sg_I");

            AdjL_Pl_N = m_AdjModel.sGetFormByGramHash("AdjL_Pl_N");
            AdjL_Pl_A_Anim = m_AdjModel.sGetFormByGramHash("AdjL_Pl_A_Anim");
            AdjL_Pl_A_Inanim = m_AdjModel.sGetFormByGramHash("AdjL_PL_A_Inanim");
            AdjL_Pl_G = m_AdjModel.sGetFormByGramHash("AdjL_Pl_G");
            AdjL_Pl_P = m_AdjModel.sGetFormByGramHash("AdjL_Pl_P");
            AdjL_Pl_D = m_AdjModel.sGetFormByGramHash("AdjL_Pl_D");
            AdjL_Pl_I = m_AdjModel.sGetFormByGramHash("AdjL_Pl_I");
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

    }

}       //  namespace ZalTestApp
