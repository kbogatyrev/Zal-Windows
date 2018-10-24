using MainLibManaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

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

        public static EM_Case eStringToCase(string sCase)
        {
            switch (sCase)
            {
                case "N":
                    return EM_Case.CASE_NOM;
                case "A":
                    return EM_Case.CASE_ACC;
                case "G":
                    return EM_Case.CASE_GEN;
                case "L":
                    return EM_Case.CASE_LOC;
                case "D":
                    return EM_Case.CASE_DAT;
                case "I":
                    return EM_Case.CASE_INST;
                case "P":
                    return EM_Case.CASE_PREP;
                case "Part":
                    return EM_Case.CASE_PART;
                case "Num":
                    return EM_Case.CASE_NUM;
                default:
                    return EM_Case.CASE_UNDEFINED;
            }
        }

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

        public static EM_Number eStringToNumber(string sNumber)
        {
            switch (sNumber)
            {
                case "Sg":
                    return EM_Number.NUM_SG;
                case "Pl":
                    return EM_Number.NUM_PL;
                default:
                    return EM_Number.NUM_UNDEFINED;
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

        public static EM_Gender eStringToGender(string sGender)
        {
            switch (sGender)
            {
                case "M":
                    return EM_Gender.GENDER_M;
                case "F":
                    return EM_Gender.GENDER_F;
                case "N":
                    return EM_Gender.GENDER_N;
                default:
                    return EM_Gender.GENDER_UNDEFINED;
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

        public static EM_Person eStringToPerson(string sPerson)
        {
            switch (sPerson)
            {
                case "1":
                    return EM_Person.PERSON_1;
                case "2":
                    return EM_Person.PERSON_2;
                case "3":
                    return EM_Person.PERSON_3;
                default:
                    return EM_Person.PERSON_UNDEFINED;
            }
        }

        public static EM_ReturnCode eGramHashToSubparadigm(string sHash, ref EM_PartOfSpeech ePartOfSpeech, ref EM_Subparadigm eSubParadigm)
        {
            ePartOfSpeech = EM_PartOfSpeech.POS_UNDEFINED;
            eSubParadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;

            string[] arrTokens = sHash.Split('_');
            if (arrTokens.Length < 1)
            {
                return EM_ReturnCode.H_ERROR_UNEXPECTED;
            }

            string sSubparadigm = arrTokens[0];
            switch (sSubparadigm)
            {
                case "Noun":
                    ePartOfSpeech = EM_PartOfSpeech.POS_NOUN;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_NOUN;
                    break;

                case "Pronoun":
                    ePartOfSpeech = EM_PartOfSpeech.POS_PRONOUN;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRONOUN;
                    break;

                case "AdjL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_LONG_ADJ;
                    break;

                case "AdjComp":
                    ePartOfSpeech = EM_PartOfSpeech.POS_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_COMPARATIVE;
                    break;

                case "AdjS":
                    ePartOfSpeech = EM_PartOfSpeech.POS_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_SHORT_ADJ;
                    break;

                case "PronAdj":
                    ePartOfSpeech = EM_PartOfSpeech.POS_PRONOUN_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
                    break;

                case "PPresA":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT;
                    break;

                case "PPastA":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT;
                    break;

                case "PPresPL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG;
                    break;

                case "PPastPL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG;
                    break;

                case "Infinitive":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_INFINITIVE;
                    break;

                case "Pres":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE;
                    break;

                case "Past":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PAST_TENSE;
                    break;

                case "Impv":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_IMPERATIVE;
                    break;

                case "VAdvPres":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT;
                    break;

                case "VAdvPast":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST;
                    break;

                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;

            }       //  switch (sSubparadigm)

            return EM_ReturnCode.H_NO_ERROR;

        }       //  eGramHashToSubparadigm()

        public static EM_ReturnCode eGramHashToGender(string sHash, ref EM_Gender eGender)
        {
            EM_PartOfSpeech ePartOfSpeech = EM_PartOfSpeech.POS_UNDEFINED;
            EM_Subparadigm eSubParadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            eGender = EM_Gender.GENDER_UNDEFINED;

            EM_ReturnCode eRet = Helpers.eGramHashToSubparadigm(sHash, ref ePartOfSpeech, ref eSubParadigm);

            string sGender = "";

            string[] arrTokens = sHash.Split('_');

            if (EM_Subparadigm.SUBPARADIGM_LONG_ADJ == eSubParadigm &&
                EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ == eSubParadigm &&
                EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT == eSubParadigm &&
                EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT == eSubParadigm &&
                EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG == eSubParadigm &&
                EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG == eSubParadigm)
            {
                if (arrTokens.Length < 4)
                {
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }

                sGender = arrTokens[1];
            }

            if (EM_Subparadigm.SUBPARADIGM_SHORT_ADJ == eSubParadigm)
            {
                if (arrTokens.Length < 2)
                {
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
                sGender = arrTokens[1];
            }

            switch (sGender)
            {
                case "M":
                    eGender = EM_Gender.GENDER_M;
                    break;

                case "N":
                    eGender = EM_Gender.GENDER_N;
                    break;

                case "F":
                    eGender = EM_Gender.GENDER_F;
                    break;

                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
            }

            return EM_ReturnCode.H_NO_ERROR;

        }       //  eGramHashToGender()

        public static EM_ReturnCode eGramHashToNumber(string sHash, ref EM_Number eNumber)
        {
            EM_PartOfSpeech ePartOfSpeech = EM_PartOfSpeech.POS_UNDEFINED;
            EM_Subparadigm eSubParadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            eNumber = EM_Number.NUM_UNDEFINED;

            EM_ReturnCode eRet = Helpers.eGramHashToSubparadigm(sHash, ref ePartOfSpeech, ref eSubParadigm);

            string[] arrTokens = sHash.Split('_');

            string sNumber = "";

            switch (eSubParadigm)
            {
                case EM_Subparadigm.SUBPARADIGM_NOUN:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN:
                    if (arrTokens.Length < 2)
                    {
                        return EM_ReturnCode.H_NO_ERROR;
                    }
                    sNumber = arrTokens[1];
                    break;

                case EM_Subparadigm.SUBPARADIGM_LONG_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    if (arrTokens.Length < 3)
                    {
                        return EM_ReturnCode.H_NO_ERROR;
                    }
                    sNumber = arrTokens[2];
                    break;

                case EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE:
                    if (arrTokens.Length < 2)
                    {
                        return EM_ReturnCode.H_NO_ERROR;
                    }
                    sNumber = arrTokens[1];
                    break;

                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;

            }       //  switch (eSubparadigm)

            if (sNumber.Length > 0)
            {
                switch (sNumber)
                {
                    case "Sg":
                        eNumber = EM_Number.NUM_SG;
                        break;

                    case "Pl":
                        eNumber = EM_Number.NUM_PL;
                        break;

                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
            }

            return EM_ReturnCode.H_ERROR_UNEXPECTED;

        }   //  eGramHashToNumber()

        public static EM_ReturnCode eGramHashToCase(string sHash, ref EM_Case eCase, ref EM_Animacy eAnimacy)
        {
            EM_PartOfSpeech ePartOfSpeech = EM_PartOfSpeech.POS_UNDEFINED;
            EM_Subparadigm eSubParadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;

            eCase = EM_Case.CASE_UNDEFINED;
            eAnimacy = EM_Animacy.ANIM_UNDEFINED;

            EM_ReturnCode eRet = Helpers.eGramHashToSubparadigm(sHash, ref ePartOfSpeech, ref eSubParadigm);

            string[] arrTokens = sHash.Split('_');

            string sCase = "";

            bool bAdjParadigm = false;

            switch (eSubParadigm)
            {
                case EM_Subparadigm.SUBPARADIGM_NOUN:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN:
                    if (arrTokens.Length < 3)
                    {
                        return EM_ReturnCode.H_NO_ERROR;
                    }
                    sCase = arrTokens[2];
                    break;

                case EM_Subparadigm.SUBPARADIGM_LONG_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT:
                case EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG:
                case EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG:
                    bAdjParadigm = true;

                    if (arrTokens.Length < 4)
                    {
                        return EM_ReturnCode.H_NO_ERROR;
                    }
                    sCase = arrTokens[3];
                    break;

                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;

            }       //  switch (eSubparadigm)

            if (sCase.Length <= 0)
            {
                return EM_ReturnCode.H_ERROR_UNEXPECTED;
            }

            switch (sCase)
            {
                case "N":
                    eCase = EM_Case.CASE_NOM;
                    break;
                case "A":
                    eCase = EM_Case.CASE_ACC;
                    if (bAdjParadigm)
                    {
                        if (arrTokens.Length >= 5)
                        {
                            string sAnimacy = arrTokens[4];
                            if ("Anim" == sAnimacy)
                            {
                                eAnimacy = EM_Animacy.ANIM_YES;
                            }
                            if ("Inanim" == sAnimacy)
                            {
                                eAnimacy = EM_Animacy.ANIM_YES;
                            }
                            else
                            {
                                return EM_ReturnCode.H_ERROR_UNEXPECTED;
                            }
                        }
                    }
                    break;
                case "G":
                    eCase = EM_Case.CASE_GEN;
                    break;
                case "L":
                    eCase = EM_Case.CASE_LOC;
                    break;
                case "D":
                    eCase = EM_Case.CASE_DAT;
                    break;
                case "I":
                    eCase = EM_Case.CASE_INST;
                    break;
                case "P":
                    eCase = EM_Case.CASE_PREP;
                    break;
                case "Part":
                    eCase = EM_Case.CASE_PART;
                    break;
                case "Num":
                    eCase = EM_Case.CASE_NUM;
                    break;
                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;

            }       //  switch (sCase)

            return EM_ReturnCode.H_NO_ERROR;

        }   //  eGramHashToCase()

        public static EM_ReturnCode eParseGramHash(string sHash, ref EM_PartOfSpeech ePartOfSpeech, ref EM_Subparadigm eSubParadigm,
            ref EM_Number eNumber, ref EM_Gender eGender, ref EM_Case eCase, ref EM_Person ePerson, ref EM_Animacy eAnimacy)
        {
            ePartOfSpeech = EM_PartOfSpeech.POS_UNDEFINED;
            eSubParadigm = EM_Subparadigm.SUBPARADIGM_UNDEFINED;
            eNumber = EM_Number.NUM_UNDEFINED;
            eGender = EM_Gender.GENDER_UNDEFINED;
            eCase = EM_Case.CASE_UNDEFINED;
            ePerson = EM_Person.PERSON_UNDEFINED;

            string[] arrTokens = sHash.Split('_');
            if (arrTokens.Length < 1)
            {
                return EM_ReturnCode.H_ERROR_UNEXPECTED;
            }

            bool bAdjParadigm = false;

            string sSubparadigm = arrTokens[0];
            switch (sSubparadigm)
            {
                case "Noun":
                    ePartOfSpeech = EM_PartOfSpeech.POS_NOUN;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_NOUN;
                    break;

                case "Pronoun":
                    ePartOfSpeech = EM_PartOfSpeech.POS_PRONOUN;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRONOUN;
                    break;

                case "AdjL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_LONG_ADJ;
                    break;

                case "PronAdj":
                    ePartOfSpeech = EM_PartOfSpeech.POS_PRONOUN_ADJ;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ;
                    break;

                case "PPresA":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT;
                    break;

                case "PPastA":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT;
                    break;

                case "PPresPL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG;
                    break;

                case "PPastPL":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG;
                    break;

                case "Infinitive":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_INFINITIVE;
                    break;

                case "Pres":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE;
                    break;

                case "Past":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_PAST_TENSE;
                    break;

                case "Impv":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_IMPERATIVE;
                    break;

                case "VAdvPres":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PRESENT;
                    break;

                case "VAdvPast":
                    ePartOfSpeech = EM_PartOfSpeech.POS_VERB;
                    eSubParadigm = EM_Subparadigm.SUBPARADIGM_ADVERBIAL_PAST;
                    break;

                default:
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;

            }       //  switch (sSubparadigm)

            if (arrTokens.Length < 2)
            {
                return EM_ReturnCode.H_NO_ERROR;
            }

            if (EM_Subparadigm.SUBPARADIGM_LONG_ADJ == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PRONOUN_ADJ == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PART_PRES_ACT == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PART_PAST_ACT == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PART_PRES_PASS_LONG == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PART_PAST_PASS_LONG == eSubParadigm)
            {
                bAdjParadigm = true;
            }

            //
            // Gender
            //
            if (bAdjParadigm)
            {
                if (arrTokens.Length < 2)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }

                string sGender = arrTokens[1];
                switch (sGender)
                {
                    case "M":
                        eGender = EM_Gender.GENDER_M;
                        break;

                    case "N":
                        eGender = EM_Gender.GENDER_N;
                        break;

                    case "F":
                        eGender = EM_Gender.GENDER_F;
                        break;

                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }

                if (arrTokens.Length < 3)
                {
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
            }

            //
            // Number
            //
            string sNumber = "";
            if (EM_Subparadigm.SUBPARADIGM_NOUN == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PRONOUN == eSubParadigm)
            {
                if (arrTokens.Length < 2)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }
                sNumber = arrTokens[1];
            }
            else if (bAdjParadigm)
            {
                if (arrTokens.Length < 3)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }
                sNumber = arrTokens[2];
            }
            else if (EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE == eSubParadigm ||
                     EM_Subparadigm.SUBPARADIGM_IMPERATIVE == eSubParadigm)
            {
                if (arrTokens.Length < 2)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }
                sNumber = arrTokens[1];
            }

            if (sNumber.Length > 0)
            {
                switch (sNumber)
                {
                    case "Sg":
                        eNumber = EM_Number.NUM_SG;
                        break;

                    case "Pl":
                        eNumber = EM_Number.NUM_PL;
                        break;

                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
            }

            //
            // Case
            //
            string sCase = "";
            if (EM_Subparadigm.SUBPARADIGM_NOUN == eSubParadigm ||
                EM_Subparadigm.SUBPARADIGM_PRONOUN == eSubParadigm)
            {
                if (arrTokens.Length < 3)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }
                sCase = arrTokens[2];
            }
            else if (bAdjParadigm)
            {
                if (arrTokens.Length < 4)
                {
                    return EM_ReturnCode.H_NO_ERROR;
                }
                sCase = arrTokens[3];
            }

            if (sCase.Length > 0)
            {
                switch (sCase)
                {

                    case "N":
                        eCase = EM_Case.CASE_NOM;
                        break;
                    case "A":
                        eCase = EM_Case.CASE_ACC;
                        break;
                    case "G":
                        eCase = EM_Case.CASE_GEN;
                        break;
                    case "L":
                        eCase = EM_Case.CASE_LOC;
                        break;
                    case "D":
                        eCase = EM_Case.CASE_DAT;
                        break;
                    case "I":
                        eCase = EM_Case.CASE_INST;
                        break;
                    case "P":
                        eCase = EM_Case.CASE_PREP;
                        break;
                    case "Part":
                        eCase = EM_Case.CASE_PART;
                        break;
                    case "Num":
                        eCase = EM_Case.CASE_NUM;
                        break;
                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;

                }
            }       //  if (sCase.Length > 0)

            //
            // Animacy
            //
            eAnimacy = EM_Animacy.ANIM_UNDEFINED;
            if (bAdjParadigm)
            {
                if (EM_Case.CASE_ACC == eCase)
                {
                    if (arrTokens.Length >= 5)
                    {
                        string sAnimacy = arrTokens[4];
                        if ("Anim" == sAnimacy)
                        {
                            eAnimacy = EM_Animacy.ANIM_YES;
                        }
                        if ("Inanim" == sAnimacy)
                        {
                            eAnimacy = EM_Animacy.ANIM_YES;
                        }
                        else
                        {
                            return EM_ReturnCode.H_ERROR_UNEXPECTED;
                        }
                    }
                }
            }

            ePerson = EM_Person.PERSON_UNDEFINED;
            if (EM_Subparadigm.SUBPARADIGM_PRESENT_TENSE == eSubParadigm || 
                EM_Subparadigm.SUBPARADIGM_IMPERATIVE == eSubParadigm)
            {
                if (arrTokens.Length < 3)
                {
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }

                string sPerson = arrTokens[2];
                switch (sPerson)
                {
                    case "1":
                        ePerson = EM_Person.PERSON_1;
                        break;
                    case "2":
                        ePerson = EM_Person.PERSON_2;
                        break;
                    case "3":
                        ePerson = EM_Person.PERSON_3;
                        break;
                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
            }

            if (EM_Subparadigm.SUBPARADIGM_PAST_TENSE == eSubParadigm)
            {
                if (arrTokens.Length < 2)
                {
                    return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }

                eNumber = EM_Number.NUM_SG;
                switch (arrTokens[1])
                {
                    case "M":
                        eGender = EM_Gender.GENDER_M;
                        break;
                    case "F":
                        eGender = EM_Gender.GENDER_F;
                        break;
                    case "N":
                        eGender = EM_Gender.GENDER_N;
                        break;
                    case "Pl":
                        eNumber = EM_Number.NUM_PL;
                        break;
                    default:
                        return EM_ReturnCode.H_ERROR_UNEXPECTED;
                }
            }

            return EM_ReturnCode.H_NO_ERROR;

        }       //  eParseGramHash()

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

        public static string sListToCommaSeparatedString(List<string> forms, List<Tuple<string, string>> comments)
        {
            string sOut = "";
            if (null == forms)
            {
                return sOut;
            }

            for (int iAt = 0; iAt < forms.Count; ++iAt)
            {
                if (sOut.Length > 0)
                {
                    sOut += ", ";
                }

                if (comments[iAt].Item1.Length > 0)
                {
                    sOut += String.Format("({0}) ", comments[iAt].Item1);
                }
                sOut += forms[iAt];
                if (comments[iAt].Item2.Length > 0)
                {
                    sOut += String.Format(" ({0})", comments[iAt].Item2);
                }
            }
            return sOut;
        }

        public static List<string> CommaSeparatedStringToList(string str)
        {
            return str.Split(',').Select(sValue => sValue.Trim()).ToList<string>();
        }

        public static bool CommaSeparatedStringToList(string str, out List<string> listForms, out List<Tuple<string, string>> listComments)
        {
            listForms = new List<string>();
            listComments = new List<Tuple<string, string>>();

            Regex regex = new Regex(@"^(?:\(([^\)]+)\))?([^\(]+)(?:\(([^\)]+)?\))?");

            List<string> listSegments = str.Split(',').Select(sValue => sValue.Trim()).ToList<string>();
            foreach (var sSegment in listSegments)
            {
                Match m = regex.Match(sSegment);
                if (m.Groups.Count != 4)
                {
                    return false;
                }

                string sForm = m.Groups[2].ToString().Trim();
                if (sForm.Length < 1)
                {
                    return false;
                }

                string sLeftComment = m.Groups[1].ToString().Trim();
                string sRightComment = m.Groups[3].ToString().Trim();
                listForms.Add(sForm);
                listComments.Add(new Tuple<string, string>(sLeftComment, sRightComment));
            }

            return true;
        }


    }       //  public static class HelpersC:\git-repos\Zal-Windows\TestApp\packages.config
}       //  namespace ZalTestApp
