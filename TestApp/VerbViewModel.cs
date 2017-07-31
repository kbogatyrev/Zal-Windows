using System;
using System.Windows.Input;

using MainLibManaged;

namespace ZalTestApp
{
    public class VerbViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void ShowParticipleForms(CLexemeManaged l, EM_Subparadigm sp);
        public event ShowParticipleForms ShowParticipleFormsEvent;

        VerbModel m_VerbModel = null;
        CLexemeManaged m_Lexeme = null;

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

        private ICommand m_ShowPresActFormsCommand;
        public ICommand ShowPresActFormsCommand
        {
            get
            {
                return m_ShowPresActFormsCommand;
            }
            set
            {
                m_ShowPresActFormsCommand = value;
            }
        }

        private ICommand m_ShowPastActCommand;
        public ICommand ShowPastActCommand
        {
            get
            {
                return m_ShowPastActCommand;
            }
            set
            {
                m_ShowPastActCommand = value;
            }
        }

        private ICommand m_ShowPresPassCommand;
        public ICommand ShowPresPassCommand
        {
            get
            {
                return m_ShowPresPassCommand;
            }
            set
            {
                m_ShowPresPassCommand = value;
            }
        }

        private ICommand m_ShowPastPassCommand;
        public ICommand ShowPastPassCommand
        {
            get
            {
                return m_ShowPastPassCommand;
            }
            set
            {
                m_ShowPastPassCommand = value;
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

        private string m_sInfinitive;
        public string Infinitive
        {
            get
            {
                return m_sInfinitive;
            }
            set
            {
                m_sInfinitive = value;
                OnPropertyChanged("Infinitive");
            }
        }

        #region Bindings_PresentTense

        private string m_sPres_Sg_1;
        public string Pres_Sg_1
        {
            get
            {
                return m_sPres_Sg_1;
            }
            set
            {
                m_sPres_Sg_1 = value;
                OnPropertyChanged("Pres_Sg_1");
            }
        }

        private string m_sPres_Sg_2;
        public string Pres_Sg_2
        {
            get
            {
                return m_sPres_Sg_2;
            }
            set
            {
                m_sPres_Sg_2 = value;
                OnPropertyChanged("Pres_Sg_2");
            }
        }

        private string m_sPres_Sg_3;
        public string Pres_Sg_3
        {
            get
            {
                return m_sPres_Sg_3;
            }
            set
            {
                m_sPres_Sg_3 = value;
                OnPropertyChanged("Pres_Sg_3");
            }
        }

        private string m_sPres_Pl_1;
        public string Pres_Pl_1
        {
            get
            {
                return m_sPres_Pl_1;
            }
            set
            {
                m_sPres_Pl_1 = value;
                OnPropertyChanged("Pres_Pl_1");
            }
        }

        private string m_sPres_Pl_2;
        public string Pres_Pl_2
        {
            get
            {
                return m_sPres_Pl_2;
            }
            set
            {
                m_sPres_Pl_2 = value;
                OnPropertyChanged("Pres_Pl_2");
            }
        }

        private string m_sPres_Pl_3;
        public string Pres_Pl_3
        {
            get
            {
                return m_sPres_Pl_3;
            }
            set
            {
                m_sPres_Pl_3 = value;
                OnPropertyChanged("Pres_Pl_3");
            }
        }
        #endregion

        #region Bindings_PastTense
        private string m_sPast_M;
        public string Past_M
        {
            get
            {
                return m_sPast_M;
            }
            set
            {
                m_sPast_M = value;
                OnPropertyChanged("Past_M");
            }
        }

        private string m_sPast_F;
        public string Past_F
        {
            get
            {
                return m_sPast_F;
            }
            set
            {
                m_sPast_F = value;
                OnPropertyChanged("Past_F");
            }
        }

        private string m_sPast_N;
        public string Past_N
        {
            get
            {
                return m_sPast_N;
            }
            set
            {
                m_sPast_N = value;
                OnPropertyChanged("Past_N");
            }
        }

        private string m_sPast_Pl;
        public string Past_Pl
        {
            get
            {
                return m_sPast_Pl;
            }
            set
            {
                m_sPast_Pl = value;
                OnPropertyChanged("Past_Pl");
            }
        }
        #endregion

        #region Bindings_Imperative
        private string m_sImpv_Sg_2;
        public string Impv_Sg_2
        {
            get
            {
                return m_sImpv_Sg_2;
            }
            set
            {
                m_sImpv_Sg_2 = value;
                OnPropertyChanged("Impv_Sg_2");
            }
        }

        private string m_sImpv_Pl_2;
        public string Impv_Pl_2
        {
            get
            {
                return m_sImpv_Pl_2;
            }
            set
            {
                m_sImpv_Pl_2 = value;
                OnPropertyChanged("Impv_Pl_2");
            }
        }
        #endregion

        #region Bindings_Non-Finite
        private string m_sPPresA_M_Sg_N;
        public string PPresA_M_Sg_N
        {
            get
            {
                return m_sPPresA_M_Sg_N;
            }
            set
            {
                m_sPPresA_M_Sg_N = value;
                OnPropertyChanged("PPresA_M_Sg_N");
            }
        }

        private string m_sPPastA_M_Sg_N;
        public string PPastA_M_Sg_N
        {
            get
            {
                return m_sPPastA_M_Sg_N;
            }
            set
            {
                m_sPPastA_M_Sg_N = value;
                OnPropertyChanged("PPastA_M_Sg_N");
            }
        }

        private string m_sPPresPL_M_Sg_N;
        public string PPresPL_M_Sg_N
        {
            get
            {
                return m_sPPresPL_M_Sg_N;
            }
            set
            {
                m_sPPresPL_M_Sg_N = value;
                OnPropertyChanged("PPresPL_M_Sg_N");
            }
        }

        private string m_sPPastPL_M_Sg_N;
        public string PPastPL_M_Sg_N
        {
            get
            {
                return m_sPPastPL_M_Sg_N;
            }
            set
            {
                m_sPPastPL_M_Sg_N = value;
                OnPropertyChanged("PPastPL_M_Sg_N");
            }
        }

        private string m_sVAdvPres;
        public string VAdvPres
        {
            get
            {
                return m_sVAdvPres;
            }
            set
            {
                m_sVAdvPres = value;
                OnPropertyChanged("VAdvPres");
            }
        }

        private string m_sVAdvPast;
        public string VAdvPast
        {
            get
            {
                return m_sVAdvPast;
            }
            set
            {
                m_sVAdvPast = value;
                OnPropertyChanged("VAdvPast");
            }
        }
        #endregion

        public VerbViewModel(CLexemeManaged lexeme)
        {
            m_Lexeme = lexeme;

            BackCommand = new RelayCommand(new Action<object>(GoBack));
            ShowPresActFormsCommand = new RelayCommand(new Action<object>(ShowPresActForms));


            m_VerbModel = new VerbModel();

            m_VerbModel.bGenerateVerbForms(lexeme);

            Infinitive = m_VerbModel.sGetFormByGramHash("Infinitive");

            Pres_Sg_1 = m_VerbModel.sGetFormByGramHash("Pres_Sg_1");
            Pres_Sg_2 = m_VerbModel.sGetFormByGramHash("Pres_Sg_2");
            Pres_Sg_3 = m_VerbModel.sGetFormByGramHash("Pres_Sg_3");

            Pres_Pl_1 = m_VerbModel.sGetFormByGramHash("Pres_Pl_1");
            Pres_Pl_2 = m_VerbModel.sGetFormByGramHash("Pres_Pl_2");
            Pres_Pl_3 = m_VerbModel.sGetFormByGramHash("Pres_Pl_3");

            Past_M = m_VerbModel.sGetFormByGramHash("Past_M");
            Past_F = m_VerbModel.sGetFormByGramHash("Past_F");
            Past_N = m_VerbModel.sGetFormByGramHash("Past_N");
            Past_Pl = m_VerbModel.sGetFormByGramHash("Past_Pl");

            Impv_Sg_2 = m_VerbModel.sGetFormByGramHash("Impv_Sg_2");
            Impv_Pl_2 = m_VerbModel.sGetFormByGramHash("Impv_Pl_2");

            PPresA_M_Sg_N = m_VerbModel.sGetFormByGramHash("PPresA_M_Sg_N");
            VAdvPres = m_VerbModel.sGetFormByGramHash("VAdvPres");
            PPastA_M_Sg_N = m_VerbModel.sGetFormByGramHash("PPastA_M_Sg_N");
            VAdvPast = m_VerbModel.sGetFormByGramHash("VAdvPast");
            PPresPL_M_Sg_N = m_VerbModel.sGetFormByGramHash("PPresPL_M_Sg_N");
            PPastPL_M_Sg_N = m_VerbModel.sGetFormByGramHash("PPastPL_M_Sg_N");
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void ShowPresActForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT);
        }

    }       //  AdjViewModel 

}       //  namespace ZalTestApp
