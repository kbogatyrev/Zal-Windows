#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING

#include "Endings.h"

#include "WordForm.h"
#include "Lexeme.h"
#include "FormBuilderPronounAdj.h"

using namespace Hlib;

CEString CFormBuilderPronounAdj::sGramHash (ET_PartOfSpeech ePos, 
                                            ET_Case eCase, 
                                            ET_Number eNumber, 
                                            ET_Gender eGender, 
                                            ET_Animacy eAnimacy)
{
    CGramHasher hasher (ePos, 
//                        SUBPARADIGM_PRONOUN_ADJ, 
                        (ePos == POS_PRONOUN_ADJ || ePos == POS_NOUN) ? SUBPARADIGM_PRONOUN_ADJ : SUBPARADIGM_NUM_ADJ,
                        eCase, 
                        eNumber, 
                        eGender,
                        PERSON_UNDEFINED,                             
                        eAnimacy, 
                        ASPECT_UNDEFINED, 
                        REFL_UNDEFINED); 

    CEString sHash = hasher.sGramHash();
    return sHash;
}

ET_ReturnCode CFormBuilderPronounAdj::eGetStressPositions (CEString& sStem,
                                                           const CEString& sEnding,
                                                           ET_StressLocation eStressType,
                                                           vector<int>& vecStressPos)
{
    ET_ReturnCode rc = H_NO_ERROR;

    sStem.SetVowels (g_szRusVowels);

    int iStressPos = -1;

    if (STRESS_LOCATION_STEM == eStressType)
    {
        rc = eGetStemStressPositions (sStem, m_eSubparadigm, NUM_UNDEFINED, vecStressPos);
                                                                    // Number not needed for this subparadigm
        if (rc != H_NO_ERROR)
        {
            return rc;
        }
    }
    else if (STRESS_LOCATION_ENDING == eStressType)
    {
        if (sEnding.uiNSyllables() < 1)
        {
            iStressPos = sStem.uiNSyllables() - 1;
        }
        else
        {
            if (L"мс-п" == m_pLexeme->sInflectionType() &&
                (L"его" == sEnding || L"ему" == sEnding ||
                 L"ого" == sEnding || L"ому" == sEnding))
            {
                iStressPos = sStem.uiNSyllables() + 1;    // одног<о, твоем<у
            }
            else
            {
                iStressPos = sStem.uiNSyllables();
            }
        }

//        hGetEndingStressPosition (str_Stem, sEnding, i_stressPos);
        vecStressPos.push_back (iStressPos);
    }
    else
    {
        ASSERT(0);
        ERROR_LOG (L"Illegal stress type.");
        return H_ERROR_INVALID_ARG;
    }

    return rc;

}   //  eGetStressPositions (...)

ET_ReturnCode CFormBuilderPronounAdj::eCreateFormTemplate (ET_Gender eGender,
                                                           ET_Number eNumber, 
                                                           ET_Case eCase, 
                                                           ET_Animacy eAnimacy,
                                                           const CEString& sStem,
                                                           const CEString& sEnding,
                                                           __int64 llEndingKey,
                                                           CWordForm *& pWordForm)
{
    ET_ReturnCode rc = H_NO_ERROR;
    
    pWordForm = new CWordForm;
    if (NULL == pWordForm)
    {
		ASSERT(0);
        ERROR_LOG (L"Unable to instantiate CWordForm.");
        return H_ERROR_POINTER;
    }

    pWordForm->m_pLexeme = m_pLexeme;
    pWordForm->m_ePos = m_pLexeme->ePartOfSpeech();
    pWordForm->m_eSubparadigm = SUBPARADIGM_PRONOUN_ADJ;
    pWordForm->m_sStem = sStem;
    pWordForm->m_sEnding = sEnding;
    pWordForm->m_llEndingDataId = llEndingKey;
    pWordForm->m_eGender = eGender;
    pWordForm->m_eCase = eCase;
    pWordForm->m_eNumber = eNumber;
    pWordForm->m_eAnimacy = eAnimacy;
    pWordForm->m_sWordForm = sStem + sEnding;
    pWordForm->m_eStatus = m_eStatus;
    pWordForm->m_llLexemeId = m_pLexeme->llLexemeId();

//    rc = eAssignSecondaryStress (pWordForm);

    return rc;

}   //  eCreateFormTemplate (...)

ET_ReturnCode CFormBuilderPronounAdj::eHandleCommonDeviations (CWordForm * pWordForm)
{
    return H_NO_ERROR;

}   //  eHandleCommonDeviations (...)

ET_ReturnCode CFormBuilderPronounAdj::eBuild()
{
	ASSERT(m_pLexeme);   // we assume base class ctor took care of this

    ET_ReturnCode rc = H_NO_ERROR;

    m_pEndings = new CAdjPronounEndings(m_pLexeme);
    if (NULL == m_pEndings)
    {
        return H_ERROR_POINTER;
    }

    CHasher gramTmp;
//    gramTmp.Initialize (POS_PRONOUN_ADJ);
//    gramTmp.Initialize(m_pLexeme->ePartOfSpeech());
//    gramTmp.SetParadigm(SUBPARADIGM_PRONOUN_ADJ);
  
    ET_Subparadigm eSubParadigm = SUBPARADIGM_PRONOUN_ADJ;
    if (m_pLexeme->ePartOfSpeech() == POS_NOUN)
    {
        gramTmp.Initialize(m_pLexeme->eMainSymbolToGender(), ET_Animacy::ANIM_UNDEFINED);
//        gramTmp.Initialize(m_pLexeme->eMainSymbolToGender(), m_pLexeme->eMainSymbolToAnimacy());
        eSubParadigm = SUBPARADIGM_PRONOUN_ADJ;
    }
    else
    {
        gramTmp.Initialize(POS_PRONOUN_ADJ);
        eSubParadigm = SUBPARADIGM_PRONOUN_ADJ;
    }
    gramTmp.SetParadigm(eSubParadigm);

    do
    {
        bool bSkipRegular = false;
        if (m_pLexeme->bHasIrregularForms())
        {
            CEString sPronAdj = sGramHash (m_pLexeme->ePartOfSpeech(),
                                           gramTmp.m_eCase, gramTmp.m_eNumber,
                                           gramTmp.m_eGender, gramTmp.m_eAnimacy);

            if (m_pLexeme->bHasMissingForms() && H_TRUE != m_pLexeme->eFormExists(sPronAdj))
            {
                continue;
            }

            if (m_pLexeme->bHasIrregularForm(sPronAdj))
            {
                map<CWordForm *, bool> mapPronAdj;
                rc = m_pLexeme->eGetIrregularForms(sPronAdj, mapPronAdj);
                if (rc != H_NO_ERROR)
                {
                    CEString sMsg(L"Unable to acquire irregular ");
                    sMsg += sPronAdj;
                    ERROR_LOG(sMsg + L" for " + m_pLexeme->sSourceForm());
                    continue;
                }

                map<CWordForm *, bool>::iterator it = mapPronAdj.begin();
                for (; it != mapPronAdj.end(); ++it)
                {
/*
                    CWordForm * pWordForm = NULL;
                    rc = eCreateFormTemplate(gramTmp.m_eGender,
                                             gramTmp.m_eNumber,
                                             gramTmp.m_eCase,
                                             gramTmp.m_eAnimacy,
                                             L"",
                                             L"",
                                             -1,
                                             pWordForm);
                    if (rc != H_NO_ERROR)
                    {
                        CEString sMsg(L"Unable to create irregular form template for  ");
                        sMsg += sPronAdj;
                        ERROR_LOG(sMsg + L"; source form = " + m_pLexeme->sSourceForm());
                        continue;
                    }

                    pWordForm->m_sWordForm = (*it).first->m_sWordForm;
                    pWordForm->m_mapStress = (*it).first->m_mapStress;
*/
                    m_pLexeme->AddWordForm(it->first);

                    bSkipRegular = !(*it).second;  // i.e. no regular variant TODO: this DOES NOT indicate that no regular variants exist
//                    bSkipRegular = true;
                }
            }
        }       //  if (m_pLexeme->m_bHasIrregularForms)

        if (bSkipRegular)
        {
            continue;
        }

        ET_StressLocation eStressType = STRESS_LOCATION_UNDEFINED;
        switch (m_pLexeme->eAccentType1())
        {
            case AT_A:
            {
                eStressType = STRESS_LOCATION_STEM;
                break;
            }
            case AT_B:
            {
                eStressType = STRESS_LOCATION_ENDING;
                break;
            }
            case AT_F:
            {
                if (NUM_SG == gramTmp.m_eNumber)
                {
                    eStressType = STRESS_LOCATION_ENDING;
                }
                else if (CASE_NOM == gramTmp.m_eCase)
                {
                    eStressType = STRESS_LOCATION_STEM;
                }
                else if (CASE_ACC == gramTmp.m_eCase && ANIM_NO == gramTmp.m_eAnimacy)
                {
                    eStressType = STRESS_LOCATION_STEM;
                }
                else
                {
                    eStressType = STRESS_LOCATION_ENDING;
                }
                break;
            }
            default:
            {
//                ASSERT(0);
                CEString sMsg(L"Illegal accent type; lexeme = ");
                ERROR_LOG(sMsg + m_pLexeme->sSourceForm());
//                return H_ERROR_UNEXPECTED;
                continue;
            }

        }   //  switch (m_pLexeme->eAccentType1())

        ET_Animacy eAnimacy = ANIM_UNDEFINED;
        if (CASE_ACC == gramTmp.m_eCase)
        {
            if ((GENDER_M == gramTmp.m_eGender && NUM_SG == gramTmp.m_eNumber) || (NUM_PL == gramTmp.m_eNumber))
            {
                eAnimacy = m_pLexeme->ePartOfSpeech() == POS_PRONOUN_ADJ ? gramTmp.m_eAnimacy : m_pLexeme->eMainSymbolToAnimacy();
            }
        }

        ET_Gender eGender = gramTmp.m_eGender;
        if (m_pLexeme->ePartOfSpeech() == POS_NOUN)
        {
            if (NUM_SG == gramTmp.m_eNumber)
            {
                eGender = m_pLexeme->eMainSymbolToGender();
            }
            else
            {
                eGender = GENDER_UNDEFINED;
            }
        }

//        rc = ((CAdjPronounEndings *)m_pEndings)->eSelect(gramTmp.m_eGender, gramTmp.m_eNumber, gramTmp.m_eCase, gramTmp.m_eAnimacy);
        rc = ((CAdjPronounEndings*)m_pEndings)->eSelect(eGender, gramTmp.m_eNumber, gramTmp.m_eCase, eAnimacy);
        int64_t iNumEndings = m_pEndings->iCount();
        if (iNumEndings < 1)
        {
            ERROR_LOG (L"No endings");
            continue;
        }

        for (int iEnding = 0; iEnding < iNumEndings; ++iEnding)
        {
            CEString sEnding;
            int64_t llEndingKey = -1;
            rc = m_pEndings->eGetEnding(iEnding, sEnding, llEndingKey);
            if (rc != H_NO_ERROR)
            {
                CEString sMsg(L"Unable to acquire ");
                sMsg += gramTmp.sGramHash();
                sMsg += L" ending of ";
                ERROR_LOG(sMsg + m_pLexeme->sSourceForm());
                continue;
            }

            auto sStem(m_sStem);
            if (m_pLexeme->bFleetingVowel() )//&&
//                (gramTmp.m_eCase != CASE_NOM || gramTmp.m_eGender != GENDER_M || gramTmp.m_eNumber != NUM_SG) &&
 //               (gramTmp.m_eCase != CASE_ACC || gramTmp.m_eGender != GENDER_M || gramTmp.m_eAnimacy != ANIM_NO))
            {
                rc = eFleetingVowelCheck (gramTmp.m_eNumber, gramTmp.m_eCase,
                                          gramTmp.m_eGender, eStressType,
                                          SUBPARADIGM_PRONOUN_ADJ, sEnding, sStem);
                if (rc != H_NO_ERROR)
                {
                    CEString sMsg(L"Fleeting vowel check failed for ");
                    sMsg += gramTmp.sGramHash();
                    sMsg += L" of ";
                    sMsg += m_pLexeme->sSourceForm();
                    ERROR_LOG(sMsg);
                    continue;
                }
            }

            CWordForm * pWordForm = NULL;
            rc = eCreateFormTemplate(gramTmp.m_eGender, 
                                     gramTmp.m_eNumber, 
                                     gramTmp.m_eCase, 
                                     gramTmp.m_eAnimacy, 
                                     sStem,
                                     sEnding, 
                                     llEndingKey,
                                     pWordForm);
            if (rc != H_NO_ERROR)
            {
                CEString sMsg(L"CreateFormTemplate failed for  ");
                sMsg += gramTmp.sGramHash();
                sMsg += L" of ";
                ERROR_LOG(sMsg + m_pLexeme->sSourceForm());
                continue;
            }

            vector<int> vecStress;
            rc = eGetStressPositions (sStem, sEnding, eStressType, vecStress);
            if (rc != H_NO_ERROR)
            {
                CEString sMsg(L"Unable GetStressPositions() failed for ");
                sMsg += gramTmp.sGramHash();
                sMsg += L" of ";
                ERROR_LOG(sMsg + m_pLexeme->sSourceForm());
                continue;
            }

            if (1 == vecStress.size() || m_pLexeme->bIsMultistressedCompound())
            {
                vector<int>::iterator itStressPos = vecStress.begin();
                for (; itStressPos != vecStress.end(); ++itStressPos)
                {
                    pWordForm->m_mapStress[*itStressPos] = STRESS_PRIMARY;
                }
                m_pLexeme->AddWordForm (pWordForm);
            }
            else
            {
                vector<int>::iterator itStressPos = vecStress.begin();
                for (; itStressPos != vecStress.end(); ++itStressPos)
                {
                    if (itStressPos != vecStress.begin())
                    {
                        CWordForm * pWfVariant = NULL;
                        CloneWordForm (pWordForm, pWfVariant);
                        pWordForm = pWfVariant;
                    }
                    pWordForm->m_mapStress[*itStressPos] = STRESS_PRIMARY;
                    m_pLexeme->AddWordForm (pWordForm);
                }
            }
        }
    } while (gramTmp.bIncrement());

    return H_NO_ERROR;

}   //  eBuild (...)
