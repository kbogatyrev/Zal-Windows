using System;
using System.Windows.Input;

using MainLibManaged;
using System.Collections.Generic;

namespace ZalTestApp
{
    public class VerbViewModel : ViewModelBase
    {
        public delegate void BackButtonHandler();
        public event BackButtonHandler BackButtonEvent;

        public delegate void ShowParticipleForms(CLexemeManaged l, EM_Subparadigm sp);
        public event ShowParticipleForms ShowParticipleFormsEvent;

        //        VerbModel m_VerbModel = null;
        MainModel m_MainModel = null;
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

        private ICommand m_ShowPastActFormsCommand;
        public ICommand ShowPastActFormsCommand
        {
            get
            {
                return m_ShowPastActFormsCommand;
            }
            set
            {
                m_ShowPastActFormsCommand = value;
            }
        }

        private ICommand m_ShowPresPassFormsCommand;
        public ICommand ShowPresPassFormsCommand
        {
            get
            {
                return m_ShowPresPassFormsCommand;
            }
            set
            {
                m_ShowPresPassFormsCommand = value;
            }
        }

        private ICommand m_ShowPastPassFormsCommand;
        public ICommand ShowPastPassFormsCommand
        {
            get
            {
                return m_ShowPastPassFormsCommand;
            }
            set
            {
                m_ShowPastPassFormsCommand = value;
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

        private bool m_bPPresAExists;
        public bool PPresAExists
        {
            get
            {
                return m_bPPresAExists;
            }
            set
            {
                m_bPPresAExists = value;
                OnPropertyChanged("PPresAExists");
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

        private bool m_bPPastAExists;
        public bool PPastAExists
        {
            get
            {
                return m_bPPastAExists;
            }
            set
            {
                m_bPPastAExists = value;
                OnPropertyChanged("PPastAExists");
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

        private bool m_bPPresPExists;
        public bool PPresPExists
        {
            get
            {
                return m_bPPresPExists;
            }
            set
            {
                m_bPPresPExists = value;
                OnPropertyChanged("PPresPExists");
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

        private bool m_bPPastPExists;
        public bool PPastPExists
        {
            get
            {
                return m_bPPastPExists;
            }
            set
            {
                m_bPPastPExists = value;
                OnPropertyChanged("PPastPExists");
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

        public VerbViewModel(CLexemeManaged lexeme, MainModel m)
        {
            m_MainModel = m;
            m_Lexeme = lexeme;

            BackCommand = new RelayCommand(new Action<object>(GoBack));
            ShowPresActFormsCommand = new RelayCommand(new Action<object>(ShowPresActForms));
            ShowPastActFormsCommand = new RelayCommand(new Action<object>(ShowPastActForms));
            ShowPresPassFormsCommand = new RelayCommand(new Action<object>(ShowPresPassForms));
            ShowPastPassFormsCommand = new RelayCommand(new Action<object>(ShowPastPassForms));

            List<string> forms = null;
            m_MainModel.GetFormsByGramHash(lexeme, "Infinitive", out forms);
            Infinitive = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Sg_1", out forms);
            Pres_Sg_1 = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Sg_2", out forms);
            Pres_Sg_2 = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Sg_3", out forms);
            Pres_Sg_3 = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Pl_1", out forms);
            Pres_Pl_1 = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Pl_2", out forms);
            Pres_Pl_2 = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Pres_Pl_3", out forms);
            Pres_Pl_3 = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "Past_M", out forms);
            Past_M = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Past_F", out forms);
            Past_F = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Past_N", out forms);
            Past_N = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Past_Pl", out forms);
            Past_Pl = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "Impv_Sg_2", out forms);
            Impv_Sg_2 = Helpers.sListToCommaSeparatedString(forms);
            m_MainModel.GetFormsByGramHash(lexeme, "Impv_Pl_2", out forms);
            Impv_Pl_2 = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "PPresA_M_Sg_N", out forms);
            PPresA_M_Sg_N = Helpers.sListToCommaSeparatedString(forms);
            PPresAExists = m_sPPresA_M_Sg_N.Length > 0;

            m_MainModel.GetFormsByGramHash(lexeme, "VAdvPres", out forms);
            VAdvPres = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "PPastA_M_Sg_N", out forms);
            PPastA_M_Sg_N = Helpers.sListToCommaSeparatedString(forms);
            PPastAExists = m_sPPastA_M_Sg_N.Length > 0;

            m_MainModel.GetFormsByGramHash(lexeme, "VAdvPast", out forms);
            VAdvPast = Helpers.sListToCommaSeparatedString(forms);

            m_MainModel.GetFormsByGramHash(lexeme, "PPresPL_M_Sg_N", out forms);
            PPresPL_M_Sg_N = Helpers.sListToCommaSeparatedString(forms);
            PPresPExists = m_sPPresPL_M_Sg_N.Length > 0;

            m_MainModel.GetFormsByGramHash(lexeme, "PPastPL_M_Sg_N", out forms);
            PPastPL_M_Sg_N = Helpers.sListToCommaSeparatedString(forms);
            PPastPExists = m_sPPastPL_M_Sg_N.Length > 0;
        }

        public void GoBack(Object obj)
        {
            BackButtonEvent?.Invoke();
        }

        public void ShowPresActForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT);
        }

        public void ShowPastActForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT);
        }

        public void ShowPresPassForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG);
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_SHORT);
        }

        public void ShowPastPassForms(Object obj)
        {
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG);
            ShowParticipleFormsEvent?.Invoke(m_Lexeme, EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_SHORT);
        }

    }       //  AdjViewModel 

}       //  namespace ZalTestApp
