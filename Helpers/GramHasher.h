#pragma once

#include "stdafx.h"
//#include "ExtString.h"

//static void operator++ (ET_MainSymbol& eo_ms) 
//{
//    eo_ms = (ET_MainSymbol)(eo_ms + 1);
//}

static void operator++ (ET_Case& eC) 
{
    eC = (ET_Case)(eC + 1);
}

static void operator++ (ET_Number& eN) 
{
    eN = (ET_Number)(eN + 1);
}

static void operator++ (ET_Gender& eG) 
{
    eG = (ET_Gender)(eG + 1);
}

static void operator++ (ET_Animacy& eo_a) 
{
    eo_a = (ET_Animacy)(eo_a + 1);
}

static void operator++ (ET_Person& eo_p) 
{
    eo_p = (ET_Person)(eo_p + 1);
}

class CGramInfo
{
public:
    CEString sLemma; // For reflexivity -- this is currently disabled

    ET_PartOfSpeech m_ePos;
    ET_Subparadigm m_eSubparadigm;
    ET_Case m_eCase;
    ET_Number m_eNumber;
    ET_Gender m_eGender;
    ET_Person m_ePerson;
    ET_Animacy m_eAnimacy;
    ET_Reflexive m_eReflexive;
    ET_Aspect m_eAspect;

public:
    CGramInfo (ET_PartOfSpeech ePos, 
               ET_Subparadigm eSubparadigm, 
               ET_Case eCase, 
               ET_Number eNumber, 
               ET_Gender eGender,
               ET_Person ePerson,
               ET_Animacy eAnimacy,
               ET_Aspect eAspect,
               ET_Reflexive eReflexive) :         
        m_ePos (ePos),
        m_eSubparadigm (eSubparadigm),
        m_eCase (eCase),
        m_eNumber (eNumber),
        m_eGender (eGender),
        m_ePerson (ePerson),
        m_eAnimacy (eAnimacy),
        m_eAspect (eAspect),
        m_eReflexive (eReflexive)
        {}

    void GramClear()
    {
        m_ePos = POS_UNDEFINED;
        m_eSubparadigm = SUBPARADIGM_UNDEFINED;
        m_eCase = CASE_UNDEFINED;
        m_eNumber = NUM_UNDEFINED;
        m_eGender = GENDER_UNDEFINED;
        m_ePerson = PERSON_UNDEFINED;
        m_eAnimacy = ANIM_UNDEFINED;
        m_eReflexive = REFL_UNDEFINED;
        m_eAspect = ASPECT_UNDEFINED;
    }

    CGramInfo()
    {
        GramClear();
    }
};

class CGramHasher : public CGramInfo
{
    static const long MIN_NOUN = 0;
    static const long MIN_ADJ = 10000;
    static const long MIN_VERB = 60000;
    static const long MIN_UNCHANGEABLE = 100000;

    static enum etSubtype
    {
        ecSubtypeUndefined,
        ecParticiple,
        ecAdverbial
    };

    static enum etTense
    {
        ecTenseUndefined,
        ecTensePresent,
        ecTensePast,
        ecTenseFuture
    };

public:
    CGramHasher()
    {
        GramClear();
    }

    CGramHasher (int iHash)
    {
        ET_ReturnCode rc = H_NO_ERROR;
        GramClear();
        rc = hDecodeHash (iHash);
            if (H_NO_ERROR != rc)
            {
                throw CException (hr, L"CGramHasher::hDecode() failed.");
            }
        }
    }

    CGramHasher (ET_PartOfSpeech ePos, 
                 ET_Subparadigm eSubparadigm, 
                 ET_Case eCase, 
                 ET_Number eNumber, 
                 ET_Gender eGender,
                 ET_Person ePerson,
                 ET_Animacy eAnimacy,
                 ET_Aspect eAspect,
                 ET_Reflexive eReflexive) :         
        CGramInfo (ePos, eSubparadigm, eCase, eNumber, eGender, ePerson, eAnimacy, eAspect, eReflexive)
    {}

    CGramHasher (ET_Subparadigm eSubparadigm,   // For verbs
                 ET_Number eNumber, 
                 ET_Gender eGender, 
                 ET_Person ePerson,
                 ET_Animacy eAnimacy,
                 ET_Aspect eAspect,
                 ET_Case eCase,
                 ET_Reflexive eReflexive) :
        CGramInfo (POS_VERB, eSubparadigm, eCase, eNumber, eGender, ePerson, eAnimacy, eAspect, eReflexive)
    {}

    CGramHasher (ET_Gender eGender,         // For nouns
                 ET_Animacy eAnimacy,
                 ET_Case eCase,
                 ET_Number eNumber) :
        CGramInfo (POS_NOUN, 
                   SUBPARADIGM_NOUN, 
                   eCase, 
                   eNumber, 
                   eGender, 
                   PERSON_UNDEFINED, 
                   eAnimacy, 
                   ASPECT_UNDEFINED, 
                   REFL_UNDEFINED)
        {}

    CGramHasher (ET_Subparadigm eSubparadigm, // For adjectives
                 ET_Number eNumber, 
                 ET_Gender eGender, 
                 ET_Animacy eAnimacy,
                 ET_Case eCase) :
        CGramInfo (POS_ADJ, 
                   eSubparadigm, 
                   eCase, 
                   eNumber, 
                   eGender, 
                   PERSON_UNDEFINED, 
                   eAnimacy, 
                   ASPECT_UNDEFINED, 
                   REFL_UNDEFINED)
        {}

    int iGramHash()
    {
        // Ranges:
        //          0- 9'999: Nouns
        //     10'000-59'999: Adjectives (incl. pronouns and numerals)
        //     60'000-99'999: Verbs
        //    100'000-infinity: all the rest (unchangeable Parts of Speech)

        int iHash = 0;

        if ((POS_NOUN == m_ePos &&
            (m_eSubparadigm != SUBPARADIGM_NOUN && m_eSubparadigm != SUBPARADIGM_UNDEFINED))
            || ((POS_ADJ == m_ePos || POS_NUM == m_ePos || POS_PRONOUN == m_ePos ||
                 POS_NUM_ADJ == m_ePos || POS_PRONOUN_ADJ == m_ePos) && 
               (m_eSubparadigm != SUBPARADIGM_SHORT_ADJ &&
                m_eSubparadigm != SUBPARADIGM_LONG_ADJ &&
                m_eSubparadigm != SUBPARADIGM_COMPARATIVE))
            || (POS_VERB  == m_ePos && (m_eSubparadigm != SUBPARADIGM_UNDEFINED
                                && m_eSubparadigm != SUBPARADIGM_INFINITIVE
                                && m_eSubparadigm != SUBPARADIGM_PAST_TENSE 
                                && m_eSubparadigm != SUBPARADIGM_PRESENT_TENSE 
                                && m_eSubparadigm != SUBPARADIGM_IMPERATIVE 
                                && m_eSubparadigm != SUBPARADIGM_ADVERBIAL_PRESENT 
                                && m_eSubparadigm != SUBPARADIGM_ADVERBIAL_PAST 
                                && m_eSubparadigm != SUBPARADIGM_PART_PRES_ACT 
                                && m_eSubparadigm != SUBPARADIGM_PART_PRES_PASS_LONG
                                && m_eSubparadigm != SUBPARADIGM_PART_PRES_PASS_SHORT
                                && m_eSubparadigm != SUBPARADIGM_PART_PAST_ACT 
                                && m_eSubparadigm != SUBPARADIGM_PART_PAST_PASS_LONG
                                && m_eSubparadigm != SUBPARADIGM_PART_PAST_PASS_SHORT)))
        {
            return -1;
        }

        if (POS_UNDEFINED == m_ePos || POS_ADV == m_ePos || POS_PREP == m_ePos ||
            POS_CONJ == m_ePos || POS_PARTICLE == m_ePos || POS_COMPAR == m_ePos ||
            POS_INTERJ == m_ePos || POS_PARENTH == m_ePos)
        {
            iHash = MIN_UNCHANGEABLE + (int)m_ePos;
            return iHash;
        }
        int iCase;
        if (m_eSubparadigm == SUBPARADIGM_NOUN) // Nouns
        {
            iHash = MIN_NOUN + m_ePos * ANIM_COUNT * GENDER_COUNT * CASE_COUNT * NUM_COUNT +
                     m_eAnimacy * GENDER_COUNT * CASE_COUNT * NUM_COUNT + 
                     m_eGender * CASE_COUNT * NUM_COUNT + 
                     m_eCase * NUM_COUNT + 
                     m_eNumber;
            return iHash;
        }
        if (POS_ADJ == m_ePos || 
            ((POS_NUM == m_ePos || POS_PRONOUN == m_ePos || POS_NUM_ADJ == m_ePos || POS_PRONOUN_ADJ == m_ePos) && 
             (SUBPARADIGM_LONG_ADJ == m_eSubparadigm || SUBPARADIGM_SHORT_ADJ == m_eSubparadigm))) // 264 possibilities
        {
            if (m_eCase != CASE_ACC || m_eAnimacy != ANIM_YES)
            {
                iCase = (int)m_eCase;
            }
            else
            {
                iCase = CASE_COUNT; // In fact, it means Accusative case of an animate adjective
            }
            iHash = MIN_ADJ +
                     m_ePos * SUBPARADIGM_COUNT * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT +
                     m_eSubparadigm * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT +
                     iCase * NUM_COUNT * GENDER_COUNT +
                     m_eNumber * GENDER_COUNT +
                     m_eGender;
            return iHash;
        }
        if (m_ePos == POS_VERB)
        {
            if (SUBPARADIGM_PART_PRES_ACT == m_eSubparadigm 
                || SUBPARADIGM_PART_PRES_PASS_LONG == m_eSubparadigm
                || SUBPARADIGM_PART_PAST_ACT == m_eSubparadigm
                || SUBPARADIGM_PART_PAST_PASS_LONG == m_eSubparadigm
                || SUBPARADIGM_PAST_TENSE == m_eSubparadigm)
            {
                if (m_eCase != CASE_ACC || m_eAnimacy != ANIM_YES)
                {
                    iCase = (int)m_eCase;
                }
                else           
                {
                    iCase = CASE_COUNT; // In fact, it means Accusative case of an animate participle
                }
                iHash = MIN_VERB +
                    m_eSubparadigm * ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eAspect * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    iCase * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eNumber * GENDER_COUNT * REFL_COUNT + 
                    m_eGender * REFL_COUNT +  
                    m_eReflexive;
                return iHash;
            }
            else if (SUBPARADIGM_PART_PRES_PASS_SHORT == m_eSubparadigm ||
                     SUBPARADIGM_PART_PAST_PASS_SHORT == m_eSubparadigm)
            {
                iCase = CASE_UNDEFINED;
                if (NUM_PL == m_eNumber)
                {
                    m_eGender = GENDER_UNDEFINED;
                }
                iHash = MIN_VERB +
                    m_eSubparadigm * ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eAspect * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    iCase * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eNumber * GENDER_COUNT * REFL_COUNT + 
                    m_eGender * REFL_COUNT +  
                    m_eReflexive;
                return iHash;
            }
            else if (SUBPARADIGM_PRESENT_TENSE == m_eSubparadigm || SUBPARADIGM_IMPERATIVE == m_eSubparadigm)
            {
                iHash = MIN_VERB +
                    m_eSubparadigm * ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eAspect * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    0 * NUM_COUNT * PERSON_COUNT * REFL_COUNT + 
                    m_eNumber * PERSON_COUNT * REFL_COUNT + 
                    m_ePerson * REFL_COUNT +  
                    m_eReflexive;
            }
            else if (SUBPARADIGM_ADVERBIAL_PRESENT == m_eSubparadigm 
                || SUBPARADIGM_ADVERBIAL_PAST == m_eSubparadigm 
                || SUBPARADIGM_INFINITIVE == m_eSubparadigm 
                || SUBPARADIGM_UNDEFINED == m_eSubparadigm)
            {
                iHash = MIN_VERB +
                    m_eSubparadigm * ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    m_eAspect * (CASE_COUNT + 1) * NUM_COUNT * GENDER_COUNT * REFL_COUNT + 
                    0 * NUM_COUNT * PERSON_COUNT * REFL_COUNT + 
                    0 * PERSON_COUNT * REFL_COUNT + 
                    0 * REFL_COUNT +  
                    m_eReflexive;
            }
            return iHash;
        }
        
        if (SUBPARADIGM_COMPARATIVE == m_eSubparadigm) // 1 possibility
        {
            iHash = MIN_VERB + SUBPARADIGM_COUNT * ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * PERSON_COUNT * REFL_COUNT; // = MIN_VERB + 20196
            return iHash;
        }
        return -1;
    } // get_GramHash

    ET_ReturnCode eDecodeHash (int iHash)
    {
        // Ranges:
        //          0-9999: Nouns (incl. numerals)
        //     10'000-49'999: Adjectives (incl. pronouns and numerals)
        //     60'000-99'999: Verbs
        //    100'000-infinity: all the rest (unchangeable Parts of Speech)

        int i_shift = 0;
        eGramClear();
        if (iHash >= MIN_NOUN && iHash < MIN_ADJ)
        {
            iHash -= MIN_NOUN;
            m_eSubparadigm = SUBPARADIGM_NOUN;
            m_eNumber = (ET_Number)(iHash % NUM_COUNT);
            iHash = (iHash - m_eNumber) / NUM_COUNT;
            m_eCase = (ET_Case)(iHash % CASE_COUNT);
            iHash = (iHash - m_eCase) / CASE_COUNT;
            m_eGender = (ET_Gender)(iHash % GENDER_COUNT);
            iHash = (iHash - m_eGender) / GENDER_COUNT;
            m_eAnimacy = (ET_Animacy)(iHash % ANIM_COUNT);
            iHash = (iHash - m_eAnimacy) / ANIM_COUNT;
            m_ePos = (ET_PartOfSpeech)(iHash % POS_COUNT);
            return H_NO_ERROR;
        }
        else if (iHash >= MIN_ADJ && iHash < MIN_VERB)
        {
            iHash -= MIN_ADJ;
            m_eGender = (ET_Gender)(iHash % GENDER_COUNT);
            iHash = (iHash - m_eGender) / GENDER_COUNT;
            m_eNumber = (ET_Number)(iHash % NUM_COUNT);
            iHash = (iHash - m_eNumber) / NUM_COUNT;
            int i_tmp_case = iHash % (CASE_COUNT + 1);
            if (i_tmp_case == CASE_COUNT)
            {
                m_eCase = CASE_ACC;
                m_eAnimacy = ANIM_YES;
            }
            else
            {
                m_eCase = (ET_Case)i_tmp_case;
                if (m_eCase == CASE_ACC)
                {
                    m_eAnimacy = ANIM_NO;
                }           
            }
            iHash = (iHash - i_tmp_case) / (CASE_COUNT + 1);
            m_eSubparadigm = (ET_Subparadigm)(iHash % SUBPARADIGM_COUNT);
            iHash = (iHash - m_eSubparadigm) / SUBPARADIGM_COUNT;
            m_ePos = (ET_PartOfSpeech)(iHash % POS_COUNT);
        }
        else if (iHash >= MIN_VERB && iHash < MIN_UNCHANGEABLE)
        {
            m_ePos = POS_VERB;
            iHash -= MIN_VERB;
            m_eSubparadigm = (ET_Subparadigm)(iHash / (ASPECT_COUNT * (CASE_COUNT + 1) * NUM_COUNT * PERSON_COUNT * REFL_COUNT));
            if (SUBPARADIGM_PART_PRES_ACT == m_eSubparadigm
                || SUBPARADIGM_PART_PRES_PASS_LONG  == m_eSubparadigm 
                || SUBPARADIGM_PART_PRES_PASS_SHORT  == m_eSubparadigm 
                || SUBPARADIGM_PART_PAST_ACT  == m_eSubparadigm 
                || SUBPARADIGM_PART_PAST_PASS_LONG == m_eSubparadigm 
                || SUBPARADIGM_PART_PAST_PASS_SHORT == m_eSubparadigm 
                || SUBPARADIGM_PAST_TENSE == m_eSubparadigm)
            {
                m_eReflexive = (ET_Reflexive)(iHash % REFL_COUNT);
                iHash = (iHash - m_eReflexive) / REFL_COUNT;
                m_eGender = (ET_Gender)(iHash % GENDER_COUNT);
                iHash = (iHash - m_eGender) / GENDER_COUNT;
                m_eNumber = (ET_Number)(iHash % NUM_COUNT);
                iHash = (iHash - m_eNumber) / NUM_COUNT;
                int i_tmp_case = iHash % (CASE_COUNT + 1);
                iHash = (iHash - i_tmp_case) / (CASE_COUNT + 1);
                if (i_tmp_case == CASE_COUNT)
                {
                    m_eCase = CASE_ACC;
                    m_eAnimacy = ANIM_YES;
                }
                else
                {
                    m_eCase = (ET_Case)i_tmp_case;
                    if (m_eCase != CASE_UNDEFINED)
                    {
                        m_eAnimacy = ANIM_NO;
                    }           
                }
                m_eAspect = (ET_Aspect)(iHash % ASPECT_COUNT);
                return H_NO_ERROR;
            }
            else if (SUBPARADIGM_PRESENT_TENSE == m_eSubparadigm || SUBPARADIGM_IMPERATIVE == m_eSubparadigm)
            {
                m_eReflexive = (ET_Reflexive)(iHash % REFL_COUNT);
                iHash = (iHash - m_eReflexive) / REFL_COUNT;
                m_ePerson = (ET_Person)(iHash % PERSON_COUNT);
                iHash = (iHash - m_ePerson) / PERSON_COUNT;
                m_eNumber = (ET_Number)(iHash % NUM_COUNT);
                iHash = (iHash - m_eNumber) / NUM_COUNT;
                iHash /= (CASE_COUNT + 1);
                m_eAspect = (ET_Aspect)(iHash % ASPECT_COUNT);
                return H_NO_ERROR;
            }
            else if (SUBPARADIGM_ADVERBIAL_PRESENT == m_eSubparadigm
                || SUBPARADIGM_ADVERBIAL_PAST == m_eSubparadigm
                || SUBPARADIGM_INFINITIVE == m_eSubparadigm 
                || SUBPARADIGM_UNDEFINED == m_eSubparadigm)
            {
                m_eReflexive = (ET_Reflexive)(iHash % REFL_COUNT);
                iHash = (iHash - m_eReflexive) / REFL_COUNT;
                iHash /= PERSON_COUNT;
                iHash /= NUM_COUNT;
                iHash /= (CASE_COUNT + 1);
                m_eAspect = (ET_Aspect)(iHash % ASPECT_COUNT);
                return H_NO_ERROR;
            }
        }
        else if (iHash >= MIN_UNCHANGEABLE && iHash < MIN_UNCHANGEABLE + POS_COUNT)
        {
            m_ePos = (ET_PartOfSpeech)(iHash - MIN_UNCHANGEABLE);
            return H_NO_ERROR;
        }

        return H_ERROR_GENERAL;
    
}   // hDecodeHash (long iHash)

    int iDecodeString (CEString& sGram)
    {
        // Partially depends on POS.
       
        //                           **** Warning! ****
        // 1. The list may be incomplete.
        // 2. Verb reflexivity is recognized only if str_lemma is present.
        // 3. If the description is deficient, subparadigm may rely on 
        //    aspect information.
        // 4. The user must make sure all unnecessary grammar information
        //    has been erased.

        if (sGram.uiLength() <= 0)
        {
            return E_INVALIDARG;
        }
        if (POS_UNDEFINED == m_ePos)
        {
            return H_ERROR_GENERAL;
        }

//        ET_PartOfSpeech ePOS_tmp = m_ePos;
        //hGramClear();
//        m_ePos = ePOS_tmp;

        // Text parsing
        CEString sParameter;
//        CEString sGram (sGram);
//        bool bMatch;
        bool bShort = false;
        etSubtype eSubtype = ecSubtypeUndefined;
        etTense eTense = ecTenseUndefined;
        ET_Voice eVoice = VOICE_UNDEFINED;

        while (sGram.uiLength() > 0)
        {
            sGram.RegexMatch (L"\\s*([^\\s\\.]*\\.?)\\s*(.*)");
            sParameter = sGram.sGetRegexMatch (0);
            sGram = sGram.sGetRegexMatch (1);
            if (sParameter == L"1")
            {
                m_ePerson = PERSON_1;
            }
            if (sParameter == L"2")
            {
                m_ePerson = PERSON_2;
            }
            if (sParameter == L"3")
            {
                m_ePerson = PERSON_3;
            }
            if (sParameter == L"ед.")
            {
                m_eNumber = NUM_SG;
            }
            if (sParameter == L"мн.")
            {
                m_eNumber = NUM_PL;
            }
            if (sParameter == L"И.")
            {
                m_eCase = CASE_NOM;
            }
            if (sParameter == L"Р.")
            {
                m_eCase = CASE_GEN;
            }
            if (sParameter == L"Д.")
            {
                m_eCase = CASE_DAT;
            }
            if (sParameter == L"В.")
            {
                m_eCase = CASE_ACC;
            }
            if (sParameter == L"Т.")
            {
                m_eCase = CASE_INST;
            }
            if (sParameter == L"П.")
            {
                m_eCase = CASE_PREP;
            }
            if (sParameter == L"Р2")       
            {
                m_eCase = CASE_PART;
            }
            if (sParameter == L"П2")
            {
                m_eCase = CASE_LOC;          
            }
            if (sParameter == L"кф" && m_ePos == POS_ADJ)
            {
                m_eSubparadigm = SUBPARADIGM_SHORT_ADJ;
            }
            if (sParameter == L"пф" && m_ePos == POS_ADJ)
            {
                m_eSubparadigm = SUBPARADIGM_LONG_ADJ;
            }
            if (sParameter == L"кф" && m_ePos == POS_VERB)
            {
                bShort = true;
            }
            if (sParameter == L"буд.")
            {
                eTense = ecTenseFuture;
            }
            if (sParameter == L"наст.")
            {
                eTense = ecTensePresent;
            }
            if (sParameter == L"прош.")
            {
                eTense = ecTensePast;
            }
            if (sParameter == L"прич.")
            {
                eSubtype = ecParticiple;
            }
            if (sParameter == L"деепр.")
            {
                eSubtype = ecAdverbial;
            }
            if (sParameter == L"повел.")
            {
                m_eSubparadigm = SUBPARADIGM_IMPERATIVE;
            }
            if (sParameter == L"страд.")
            {
                eVoice = VOICE_PASSIVE;
            }
            if (sParameter == L"м")
            {
                m_eGender = GENDER_M;
            }
            if (sParameter == L"ж")
            {
                m_eGender = GENDER_F;
            }
            if (sParameter == L"с")
            {
                m_eGender = GENDER_N;
            }
            if (sParameter == L"одуш.")
            {
                m_eAnimacy = ANIM_YES;
            }
            if (sParameter == L"неод.")
            {
                m_eAnimacy = ANIM_NO;
            }
            if (sParameter == L"сравн.")
            {
                m_eSubparadigm = SUBPARADIGM_COMPARATIVE;
            }
            if (sParameter == L"инф.")
            {
                m_eSubparadigm = SUBPARADIGM_INFINITIVE;
            }
            if (sParameter == L"св")
            {
                m_eAspect = ASPECT_PERFECTIVE;
            }
            if (sParameter == L"нсв")
            {
                m_eAspect = ASPECT_IMPERFECTIVE;
            }
        }

        // Implications
        if (m_ePos == POS_NOUN)
        {
            m_eSubparadigm = SUBPARADIGM_NOUN;
        }
        if ((m_ePos == POS_ADJ || m_ePos == POS_PRONOUN_ADJ ||
             m_ePos == POS_PRONOUN || m_ePos == POS_NUM || m_ePos == POS_NUM_ADJ) &&
            m_eSubparadigm == SUBPARADIGM_UNDEFINED)
        {
            m_eSubparadigm = SUBPARADIGM_LONG_ADJ;
        }
        if (m_ePos == POS_ADJ || m_ePos == POS_PRONOUN_ADJ || m_ePos == POS_NUM_ADJ)
        {
            if (m_eCase == CASE_ACC && (m_eGender == GENDER_M || m_eGender == GENDER_F) &&
                m_eAnimacy == ANIM_UNDEFINED)
            {
                m_eAnimacy = ANIM_NO;
            }
            if ((GENDER_M == m_eGender || 
                 GENDER_F == m_eGender || 
                 GENDER_N == m_eGender) && 
                NUM_UNDEFINED == m_eNumber)
            {
                m_eNumber = NUM_SG;
            }
            if (NUM_PL == m_eNumber)
            {
                m_eGender = GENDER_UNDEFINED;
            }
        }
        if (m_ePos == POS_VERB)
        {
            // Convert several parameters to Subparadigm
            if (ecSubtypeUndefined == eSubtype &&
                (eTense == ecTensePresent || eTense == ecTenseFuture))
            {
                m_eSubparadigm = SUBPARADIGM_PRESENT_TENSE;
            }
            if (ecSubtypeUndefined == eSubtype && eTense == ecTensePast)
            {
                m_eSubparadigm = SUBPARADIGM_PAST_TENSE;
            }
            if (ecParticiple == eSubtype &&
                eVoice == VOICE_PASSIVE &&
                eTense == ecTenseUndefined)
            {
                eTense = ecTensePast;
            }
            if (ecParticiple == eSubtype &&
                eVoice == VOICE_UNDEFINED)
            {
                eVoice = VOICE_ACTIVE;
            }
            if (ecParticiple == eSubtype &&
                (eTense == ecTensePresent || eTense == ecTenseFuture) &&
                eVoice == VOICE_ACTIVE)
            {
                m_eSubparadigm = SUBPARADIGM_PART_PRES_ACT;
            }
            if (ecParticiple == eSubtype && 
                (eTense == ecTensePresent || eTense == ecTenseFuture) &&
                eVoice == VOICE_PASSIVE)
            {
                if (bShort)
                {
                    m_eSubparadigm = SUBPARADIGM_PART_PRES_PASS_SHORT;
                }
                else
                {
                    m_eSubparadigm = SUBPARADIGM_PART_PRES_PASS_LONG;
                }
            }
            if (ecParticiple == eSubtype && eTense == ecTensePast && eVoice == VOICE_ACTIVE)
            {
                m_eSubparadigm = SUBPARADIGM_PART_PAST_ACT;
            }
            if (ecParticiple == eSubtype && eTense == ecTensePast && eVoice == VOICE_PASSIVE)
            {
                if (bShort)
                {
                    m_eSubparadigm = SUBPARADIGM_PART_PAST_PASS_SHORT;
                }
                else
                {
                    m_eSubparadigm = SUBPARADIGM_PART_PAST_PASS_LONG;
                }
            }
            if (ecAdverbial == eSubtype && 
                (eTense == ecTensePresent || eTense == ecTenseFuture))
            {
                m_eSubparadigm = SUBPARADIGM_ADVERBIAL_PRESENT;
            }
            if (ecAdverbial == eSubtype && eTense == ecTensePast)
            {
                m_eSubparadigm = SUBPARADIGM_ADVERBIAL_PAST;
            }

            // Other specifications
            if (SUBPARADIGM_IMPERATIVE == eSubtype && m_ePerson == PERSON_UNDEFINED && m_eNumber != NUM_PL)
            {
                m_ePerson = PERSON_2;
                m_eNumber = NUM_SG;
            }

            if (ecParticiple == eSubtype && m_eCase == CASE_ACC && m_eAnimacy == ANIM_UNDEFINED)
            {
                m_eAnimacy = ANIM_NO;
            }

            if (ecParticiple == eSubtype && !bShort && m_eCase == CASE_UNDEFINED && m_eNumber == NUM_UNDEFINED)
            {
                m_eAnimacy = ANIM_UNDEFINED;
                m_eCase = CASE_NOM;
                m_eNumber = NUM_SG;
            }

            if (eTense == ecTenseFuture && m_eAspect == ASPECT_UNDEFINED)
            {
                m_eAspect = ASPECT_PERFECTIVE;
            }

            if (eTense == ecTensePresent && m_eAspect == ASPECT_UNDEFINED)
            {
                m_eAspect = ASPECT_IMPERFECTIVE;
            }

            /* // Temporarily blocked
            bMatch = regex_match(str_Lemma, result, (const wregex)L".*(ся|сь)");
            if (bMatch == true)
            {
                m_eReflexive = REFL_YES;
            }
            else
            {
                m_eReflexive = REFL_NO;
            }
            */

            if (m_eSubparadigm == SUBPARADIGM_PAST_TENSE || ecParticiple == eSubtype)
            {
                if ((GENDER_M == m_eGender || GENDER_F == m_eGender || GENDER_N == m_eGender) && NUM_UNDEFINED == m_eNumber)
                {
                    m_eNumber = NUM_SG;
                }
                if (NUM_PL == m_eNumber)
                {
                    m_eGender = GENDER_UNDEFINED;
                }
            }

            if (ecParticiple == eSubtype && GENDER_UNDEFINED == m_eGender)
            {
                m_eGender = GENDER_M;
            }
        }
       
        return H_NO_ERROR;

    } // DecodeString (BSTR bstr_gram)

    void Initialize (ET_Gender eo_init_gender, ET_Animacy eo_init_animacy)    // Nouns
    {
        hGramClear();
        m_ePos = POS_NOUN;
        m_eGender = eo_init_gender;
        m_eAnimacy = eo_init_animacy;

        m_eCase = CASE_NOM;
        m_eNumber = NUM_SG;
        m_eSubparadigm = SUBPARADIGM_NOUN;
    }

    void Initialize (ET_Aspect eInitAspect, ET_Reflexive eInitReflexive)    // Verbs
    {
        hGramClear();
        m_ePos = POS_VERB;
        m_eAspect = eInitAspect;
        m_eReflexive = eInitReflexive;

        m_eSubparadigm = SUBPARADIGM_PRESENT_TENSE;
        m_ePerson = PERSON_1;
        m_eNumber = NUM_SG;
    }

    void Initialize (ET_PartOfSpeech eInitPos)    // Adjectives and adjectival pronouns
    {
        hGramClear();
        m_ePos = eInitPos;   // POS_ADJ or POS_PRONOUN_ADJ

        m_eSubparadigm = SUBPARADIGM_LONG_ADJ;
        m_eGender = GENDER_M;
        m_eCase = CASE_NOM;
        m_eNumber = NUM_SG;
    }

    void SetParadigm (ET_Subparadigm eInitSubparadigm)
    {
        if (m_ePos == POS_UNDEFINED)
        {
            return;  // The POS should be specified first
        }
        m_eSubparadigm = eInitSubparadigm;
        if (m_ePos == POS_ADJ || m_ePos == POS_PRONOUN_ADJ)
        {
            m_eGender = GENDER_M;
            m_eCase = CASE_NOM;
            m_eNumber = NUM_SG;
        }
        else if (m_ePos == POS_VERB)
        {
            if (eInitSubparadigm == SUBPARADIGM_PRESENT_TENSE)
            {
                m_ePerson = PERSON_1;
                m_eNumber = NUM_SG;
                m_eGender = GENDER_UNDEFINED;
                m_eCase = CASE_UNDEFINED;
            }
            else if (eInitSubparadigm == SUBPARADIGM_PAST_TENSE)
            {
                m_ePerson = PERSON_UNDEFINED;
                m_eNumber = NUM_SG;
                m_eGender = GENDER_M;
                m_eCase = CASE_UNDEFINED;
            }
            else if (eInitSubparadigm == SUBPARADIGM_PART_PRES_ACT ||
                     eInitSubparadigm == SUBPARADIGM_PART_PRES_PASS_LONG ||
                     eInitSubparadigm == SUBPARADIGM_PART_PRES_PASS_SHORT ||
                     eInitSubparadigm == SUBPARADIGM_PART_PAST_ACT ||
                     eInitSubparadigm == SUBPARADIGM_PART_PAST_PASS_LONG ||
                     eInitSubparadigm == SUBPARADIGM_PART_PAST_PASS_SHORT)
            {
                m_ePerson = PERSON_UNDEFINED;
                m_eNumber = NUM_SG;
                m_eGender = GENDER_M;
                m_eCase = CASE_NOM;
            }
            else if (eInitSubparadigm == SUBPARADIGM_INFINITIVE ||
                     eInitSubparadigm == SUBPARADIGM_ADVERBIAL_PRESENT ||
                     eInitSubparadigm == SUBPARADIGM_ADVERBIAL_PAST)
            {
                m_ePerson = PERSON_UNDEFINED;
                m_eNumber = NUM_UNDEFINED;
                m_eGender = GENDER_UNDEFINED;
                m_eCase = CASE_UNDEFINED;
            }
            else if (eInitSubparadigm == SUBPARADIGM_IMPERATIVE)
            {
                m_ePerson = PERSON_2;
                m_eNumber = NUM_SG;
                m_eGender = GENDER_UNDEFINED;
                m_eCase = CASE_UNDEFINED;
            }
        }
    }

    bool bIncrement()
    {
        if (m_ePos == POS_NOUN)
        {
            ++m_eCase;
            if (m_eCase != CASE_COUNT)
            {
                if (m_eCase == CASE_PART || m_eCase == CASE_LOC || m_eCase == CASE_NUM)
                {
                    bool b_ = bIncrement();
                    return b_;
                }
                return true;
            }
            else
            {
                m_eCase = CASE_NOM;
                ++m_eNumber;
                if (m_eNumber != NUM_COUNT)
                {
                    return true;
                }
                return false;
            }
        }
        if (m_ePos == POS_ADJ || m_ePos == POS_PRONOUN_ADJ)
        {
            if (m_eSubparadigm == SUBPARADIGM_LONG_ADJ)
            {
                if (m_eCase == CASE_ACC &&
                    (m_eGender == GENDER_M || m_eGender == GENDER_F || m_eNumber == NUM_PL))
                {
                    if (m_eAnimacy == ANIM_UNDEFINED)
                    {
                        m_eAnimacy = ANIM_NO;
                        return true;
                    }
                    else if (m_eAnimacy == ANIM_NO)
                    {
                        m_eAnimacy = ANIM_YES;
                        return true;
                    }
                    else if (m_eAnimacy == ANIM_YES)
                    {
                        m_eAnimacy = ANIM_UNDEFINED;
                    }
                }
                ++m_eCase;
                if (m_eCase != CASE_COUNT)
                {
                    if (m_eCase == CASE_PART || m_eCase == CASE_LOC || m_eCase == CASE_NUM)
                    {
                        bool b_ = bIncrement();
                        return b_;
                    }
                    if (m_eCase == CASE_ACC &&
                        (m_eGender == GENDER_M || m_eGender == GENDER_F || m_eNumber == NUM_PL))
                    {
                        m_eAnimacy = ANIM_NO;
                    }
                    return true;
                }
                else
                {
                    m_eCase = CASE_NOM;
                    if (m_eNumber == NUM_SG)
                    {
                        ++m_eGender;
                        if (m_eGender != GENDER_COUNT)
                        {
                            return true;
                        }
                        else
                        {
                            m_eGender = GENDER_UNDEFINED;
                            m_eNumber = NUM_PL;
                            return true;
                        }
                    }
                    else
                    {
                        return false;   // The user should proceed to the Short subparadigm manually
                    }
                }
            }
            else if (m_eSubparadigm == SUBPARADIGM_SHORT_ADJ)
            {
                if (m_eNumber == NUM_SG)
                {
                    ++m_eGender;
                    if (m_eGender != GENDER_COUNT)
                    {
                        return true;
                    }
                    else
                    {
                        m_eGender = GENDER_UNDEFINED;
                        m_eNumber = NUM_PL;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        if (m_ePos == POS_VERB)
        {
            if (m_eSubparadigm == SUBPARADIGM_PRESENT_TENSE)
            {
                ++m_ePerson;
                if (m_ePerson != PERSON_COUNT)
                {
                    return true;
                }
                else
                {
                    m_ePerson = PERSON_1;
                    ++m_eNumber;
                    if (m_eNumber != NUM_COUNT)
                    {
                        return true;
                    }
                    return false;
                }
            }
            if (m_eSubparadigm == SUBPARADIGM_PART_PRES_ACT ||
                m_eSubparadigm == SUBPARADIGM_PART_PAST_ACT ||
                m_eSubparadigm == SUBPARADIGM_PART_PRES_PASS_LONG ||
                m_eSubparadigm == SUBPARADIGM_PART_PAST_PASS_LONG)
            {
                if (m_eCase == CASE_ACC &&
                    (m_eGender == GENDER_M || m_eGender == GENDER_F || m_eNumber == NUM_PL))
                {
                    if (m_eAnimacy == ANIM_UNDEFINED)
                    {
                        m_eAnimacy = ANIM_NO;
                        return true;
                    }
                    else if (m_eAnimacy == ANIM_NO)
                    {
                        m_eAnimacy = ANIM_YES;
                        return true;
                    }
                    else if (m_eAnimacy == ANIM_YES)
                    {
                        m_eAnimacy = ANIM_UNDEFINED;
                    }
                }
                ++m_eCase;
                if (m_eCase != CASE_COUNT)
                {
                    if (m_eCase == CASE_PART || m_eCase == CASE_LOC || m_eCase == CASE_NUM)
                    {
                        bool b_ = bIncrement();
                        return b_;
                    }
                    if (m_eCase == CASE_ACC &&
                        (m_eGender == GENDER_M || m_eGender == GENDER_F || m_eNumber == NUM_PL))
                    {
                        m_eAnimacy = ANIM_NO;
                    }
                    return true;
                }
                else
                {
                    m_eCase = CASE_NOM;
                    if (m_eNumber == NUM_SG)
                    {
                        ++m_eGender;
                        if (m_eGender != GENDER_COUNT)
                        {
                            return true;
                        }
                        else
                        {
                            m_eGender = GENDER_UNDEFINED;
                            m_eNumber = NUM_PL;
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (m_eSubparadigm == SUBPARADIGM_PART_PAST_PASS_SHORT ||
                     m_eSubparadigm == SUBPARADIGM_PART_PRES_PASS_SHORT ||
                     m_eSubparadigm == SUBPARADIGM_PAST_TENSE)
            {
                if (m_eNumber == NUM_SG)
                {
                    ++m_eGender;
                    if (m_eGender != GENDER_COUNT)
                    {
                        return true;
                    }
                    else
                    {
                        m_eGender = GENDER_UNDEFINED;
                        m_eNumber = NUM_PL;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (m_eSubparadigm == SUBPARADIGM_IMPERATIVE)
            {
                ++m_eNumber;
                if (m_eNumber != NUM_COUNT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (m_eSubparadigm == SUBPARADIGM_ADVERBIAL_PRESENT ||
                     m_eSubparadigm == SUBPARADIGM_ADVERBIAL_PAST ||
                     m_eSubparadigm == SUBPARADIGM_INFINITIVE)
            {
                return false;
            }
        }

        return false;
    }   // b_Increment() ...

};
