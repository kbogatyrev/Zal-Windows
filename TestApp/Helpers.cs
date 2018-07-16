using MainLibManaged;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZalTestApp
{
    public static class Helpers
    {
        public static EM_MainSymbol eMainSymbolToEnum(string sMainSymbol)
        {
            switch (sMainSymbol)
            {
                case "м":
                    return EM_MainSymbol.MS_M;
                case "мо":
                    return EM_MainSymbol.MS_MO;
                case "ж":
                    return EM_MainSymbol.MS_ZH;
                case "жо":
                    return EM_MainSymbol.MS_ZHO;
                case "с":
                    return EM_MainSymbol.MS_S;
                case "со":
                    return EM_MainSymbol.MS_SO;
                case "мо-жо":
                    return EM_MainSymbol.MS_MO_ZHO;
                case "мн.":
                    return EM_MainSymbol.MS_MN;
                case "мн.неод.":
                    return EM_MainSymbol.MS_MN_NEOD;
                case "мн.одуш.":
                    return EM_MainSymbol.MS_MN_ODUSH;
                case "мн. от":
                    return EM_MainSymbol.MS_MN_OT;
                case "п":
                    return EM_MainSymbol.MS_P;
                case "мс":
                    return EM_MainSymbol.MS_MS;
                case "мс-п":
                    return EM_MainSymbol.MS_MS_P;
                case "числ.":
                    return EM_MainSymbol.MS_CHISL;
                case "числ.-п":
                    return EM_MainSymbol.MS_CHISL_P;
                case "св":
                    return EM_MainSymbol.MS_SV;
                case "нсв":
                    return EM_MainSymbol.MS_NSV;
                case "св-нсв":
                    return EM_MainSymbol.MS_SV_NSV;
                case "н":
                    return EM_MainSymbol.MS_N;
                case "предл.":
                    return EM_MainSymbol.MS_PREDL;
                case "союз":
                    return EM_MainSymbol.MS_SOJUZ;
                case "предик.":
                    return EM_MainSymbol.MS_PREDIK;
                case "вводн.":
                    return EM_MainSymbol.MS_VVODN;
                case "сравн.":
                    return EM_MainSymbol.MS_SRAVN;
                case "част.":
                    return EM_MainSymbol.MS_CHAST;
                case "межд.":
                    return EM_MainSymbol.MS_MEZHD;
                default:
                    return EM_MainSymbol.MS_UNDEFINED;
            }
        }

        public static string sAccenTypeToStressSchema(EM_AccentType eType)
        {
            switch (eType)
            {
                case EM_AccentType.AT_A:
                    return "a";
                case EM_AccentType.AT_A1:
                    return "a'";
                case EM_AccentType.AT_B:
                    return "b";
                case EM_AccentType.AT_B1:
                    return "b'";
                case EM_AccentType.AT_C:
                    return "c";
                case EM_AccentType.AT_C1:
                    return "c'";
                case EM_AccentType.AT_C2:
                    return "c''";
                case EM_AccentType.AT_D:
                    return "d";
                case EM_AccentType.AT_D1:
                    return "d'";
                case EM_AccentType.AT_E:
                    return "e";
                case EM_AccentType.AT_F:
                    return "f";
                case EM_AccentType.AT_F1:
                    return "f'";
                case EM_AccentType.AT_F2:
                    return "f''";
                case EM_AccentType.AT_UNDEFINED:
                default:
                    return "Undefined";
            };
        }       //  sAccenTypeToStressSchema()

        public static string sCaseToString(EM_Case eCase)
        {
            switch (eCase)
            {
                case EM_Case.CASE_NOM:
                    return "N";
                case EM_Case.CASE_ACC:
                    return "A";
                case EM_Case.CASE_GEN:
                    return "G";
                case EM_Case.CASE_LOC:
                    return "L";
                case EM_Case.CASE_DAT:
                    return "D";
                case EM_Case.CASE_INST:
                    return "I";
                case EM_Case.CASE_PREP:
                    return "P";
                case EM_Case.CASE_PART:
                    return "Part";
                case EM_Case.CASE_NUM:
                    return "Num";
                case EM_Case.CASE_UNDEFINED:
                default:
                    return "Undefined";
            }
        }      //  CaseToString()

        public static string sNumberToString(EM_Number eNumber)
        {
            switch (eNumber)
            {
                case EM_Number.NUM_SG:
                    return "Sg";
                case EM_Number.NUM_PL:
                    return "Pl";
                case EM_Number.NUM_UNDEFINED:
                default:
                    return "Undefined";
            }
        }

        public static string sGenderToString(EM_Gender eGender)
        {
            switch (eGender)
            {
                case EM_Gender.GENDER_M:
                    return "M";
                case EM_Gender.GENDER_F:
                    return "F";
                case EM_Gender.GENDER_N:
                    return "N";
                case EM_Gender.GENDER_UNDEFINED:
                default:
                    return "Undefined";
            }
        }

        public static string sPersonToString(EM_Person ePerson)
        {
            switch (ePerson)
            {
                case EM_Person.PERSON_1:
                    return "1";
                case EM_Person.PERSON_2:
                    return "2";
                case EM_Person.PERSON_3:
                    return "3";
                case EM_Person.PERSON_UNDEFINED:
                default:
                    return "Undefined";
            }
        }

        public static EM_AccentType eStringToAccentType(string sAccentType)
        {
            switch(sAccentType)
            {
                case "a":
                    return EM_AccentType.AT_A;
                case "a'":
                    return EM_AccentType.AT_A1;
                case "b":
                    return EM_AccentType.AT_B;
                case "b'":
                    return EM_AccentType.AT_B1;
                case "c":
                    return EM_AccentType.AT_C;
                case "c'":
                    return EM_AccentType.AT_C1;
                case "c''":
                    return EM_AccentType.AT_C2;
                case "d":
                    return EM_AccentType.AT_D;
                case "d'":
                    return EM_AccentType.AT_D1;
                case "e":
                    return EM_AccentType.AT_E;
                case "f":
                    return EM_AccentType.AT_F;
                case "f'":
                    return EM_AccentType.AT_F1;
                case "f''":
                    return EM_AccentType.AT_F2;
                default:
                    return EM_AccentType.AT_UNDEFINED;
            }
        }

        public static void StressMarksToPosList(string sWord, out string sOutWord, out Dictionary<int, EM_StressType> dictPositions)
        {
            dictPositions = new Dictionary<int, EM_StressType>();
            sOutWord = "";
            int iRemoved = 0;

            for (int iPos = 0; iPos < sWord.Length; ++iPos)
            {
                char chr = sWord[iPos];
                if ('\x300' == chr)
                {
                    dictPositions[iPos-(iRemoved++)-1] = EM_StressType.STRESS_SECONDARY;
                }
                else if ('\x301' == chr)
                {
                    dictPositions[iPos-(iRemoved++)-1] = EM_StressType.STRESS_PRIMARY;
                }
                else if ('ё' == chr)
                {
                    dictPositions[iPos-iRemoved] = EM_StressType.STRESS_PRIMARY;
                    sOutWord += chr;
                }
                else
                {
                    sOutWord += chr;
                }
            }
        }

        public static void MarkStress(ref string sWordForm, CWordFormManaged wf)
        {
            int iPos = -1;
            EM_StressType eType = EM_StressType.STRESS_TYPE_UNDEFINED;
            char chrMark = ' ';
            int iStressMarksInserted = 0;

            EM_ReturnCode eRet = wf.eGetFirstStressPos(ref iPos, ref eType);
            do
            {
                if (eRet != EM_ReturnCode.H_NO_ERROR && eRet != EM_ReturnCode.H_FALSE)
                {
                    continue;
                }

                if (EM_StressType.STRESS_PRIMARY == eType)
                {
                    if (sWordForm[iPos+iStressMarksInserted] != 'ё')
                    {
                        chrMark = '\x301';
                        sWordForm = sWordForm.Insert(iPos + iStressMarksInserted++ + 1, chrMark.ToString());
                    }
                }
                else if (EM_StressType.STRESS_SECONDARY == eType)
                {
                    chrMark = '\x300';
                    sWordForm = sWordForm.Insert(iPos + iStressMarksInserted++ + 1, chrMark.ToString());
                }

                eRet = wf.eGetNextStressPos(ref iPos, ref eType);

            } while (EM_ReturnCode.H_NO_ERROR == eRet);

        }    //  MarkStress()

        // "/", "\" --> diacritics
        public static void AssignDiacritics(string sSource, ref string sTarget)
        {
            char chrStressMark = '\x301';
            char chrSecondaryStressMark = '\x300';
            int iPos = 0;
            sTarget = sSource;
            iPos = sTarget.IndexOf('/', iPos);
            while (iPos >= 0 && iPos < sTarget.Length - 1)
            {
                if (sTarget[iPos] != sTarget[iPos + 1] && (sTarget[iPos + 1] != 'ё'))
                {
                    sTarget = sTarget.Substring(0, iPos) + sTarget.Substring(iPos + 1, 1) + chrStressMark.ToString() + sTarget.Substring(iPos + 2);
                }
                else if ('ё' == sTarget[iPos + 1])
                {
                    sTarget = sTarget.Substring(0, iPos) + sTarget.Substring(iPos + 1, 1) + sTarget.Substring(iPos + 2);
                }
                else
                {
                    iPos += 2;
                }
                iPos = sTarget.IndexOf('/', iPos);
            }
            iPos = sTarget.IndexOf('\\', 0);
            while (iPos >= 0 && iPos < sTarget.Length - 1)
            {
                if (sTarget[iPos] != sTarget[iPos + 1])
                {
                    sTarget = sTarget.Substring(0, iPos) + sTarget.Substring(iPos + 1, 1) + chrSecondaryStressMark.ToString() + sTarget.Substring(iPos + 2);
                }
                else
                {
                    iPos += 2;
                }
                iPos = sTarget.IndexOf('\\', iPos);
            }
        }       //  ExtractStressMarks()

        // Diacritics --> "/", "\"
        public static void RestoreStressMarks(ref string sWord)
        {
            char chrStressMark = '\x301';
            char chrSecondaryStressMark = '\x300';
            char[] arrStressMarks = { chrStressMark, chrSecondaryStressMark };

            int iPos = 0;
            iPos = sWord.IndexOfAny(arrStressMarks, iPos);
            while (iPos > 0 && iPos < sWord.Length - 1)
            {
                StringBuilder sb = new StringBuilder(sWord);
                char chrDiacritic = (chrStressMark == sb[iPos]) ? '/' : '\\';
                sb[iPos] = sb[iPos-1];
                sb[iPos-1] = chrDiacritic;
                sWord = sb.ToString();

                iPos = sWord.IndexOfAny(arrStressMarks, iPos+1);
            }
        }

        public static string sListToCommaSeparatedString(List<string> forms)
        {
            string sOut = "";
            if (null == forms)
            {
                return sOut;
            }

            foreach (var sForm in forms)
            {
                if (sOut.Length > 0)
                {
                    sOut += ", ";
                }
                sOut += sForm;
            }
            return sOut;
        }

        public static List<string> CommaSeparatedStringToList(string str)
        {
            return str.Split(',').Select(sValue => sValue.Trim()).ToList<string>();
        }
    }       //  public static class Helpers
}       //  namespace ZalTestApp

