#include "StdAfx.h"
#include "ZalEntryParser.h"
#include "GramHasher.h"

using namespace Hlib;

bool bExtractStress (CEString& sSource, CEString& sOut, map<int, ET_StressType>& mapStress)
{
    CEString sIn (sSource);
    sIn.SetBreakChars (L"<>");

    sOut.Erase();

    for (int iTextField = 0; iTextField < (int)sIn.uiGetNumOfFields(); ++iTextField)
    {
        sOut += sIn.sGetField (iTextField);
    }

    int iMarksFound = 0;
    for (int iBreakField = 0; 
         iBreakField < (int)sIn.uiGetNumOfFields (ecTokenBreakChars); 
         ++iBreakField)
    {
        CEString sStressMark = sIn.sGetField (iBreakField, ecTokenBreakChars);
        if (sStressMark.uiLength() != 1)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error processing stress marks: ");
            sMsg += sIn;
            ERROR_LOG (sMsg);
            return false;
        }

        int iPos = sIn.uiGetFieldOffset (iBreakField, ecTokenBreakChars);
        iPos -= iMarksFound;
        ++iMarksFound;

        if (L"<" == sStressMark)
        {
            mapStress[iPos] = STRESS_PRIMARY;
        }
        else if (L">" == sStressMark)
        {
            mapStress[iPos] = STRESS_SECONDARY;
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Error extracting stress marks: ");
            sMsg += sIn;
            ERROR_LOG (sMsg);
            return false;
        }

    }   //  for (int iBreakField = 0; ...)

    if (mapStress.empty())
    {
        sOut.SetVowels (g_szRusVowels);
        int iYoPos = sOut.uiFind (L"ё");
        if (ecNotFound != iYoPos)
        {
            mapStress[iYoPos] = STRESS_PRIMARY;
        }
        else
        {
            if (sOut.uiGetNumOfSyllables() != 1)
            {
                ATLASSERT(0);
                ERROR_LOG (L"No stress position given for a polysyllabic irregular form.");
                return false;
            }
            int iStressPos = sOut.uiFindFirstOf (g_szRusVowels);
            if (ecNotFound == iStressPos)
            {
                ATLASSERT(0);
                ERROR_LOG (L"Unable to find vowel.");
                return false;
            }
            mapStress[iStressPos] = STRESS_PRIMARY;
        }
    }

    return true;

}   //  bExtractStress (...)

bool StHeadword::bSaveToDb (CSqlite * pDbHandle, __int64& llHeadwordId)
{
    try
    {
        pDbHandle->PrepareForInsert (L"headword", 8);
        pDbHandle->Bind (1, sSource);  // 0-based
        pDbHandle->Bind (2, sPluralOf);
        pDbHandle->Bind (3, sComment);
        pDbHandle->Bind (4, sUsage);
        pDbHandle->Bind (5, sVariant);
        pDbHandle->Bind (6, sVariantComment);
//        pDbHandle->Bind (7, sSeeRef);
//        pDbHandle->Bind (8, sOrigination);
        pDbHandle->InsertRow();
        pDbHandle->Finalize();
        
        llHeadwordId = pDbHandle->llGetLastKey();

        //
        // Homonyms
        //
        for (unsigned int ui_homonym = 0; ui_homonym < vecIHomonyms.size(); ++ui_homonym)
        {
            pDbHandle->PrepareForInsert (L"homonyms", 2);
            pDbHandle->Bind (1, llHeadwordId);  // 0-based
            pDbHandle->Bind (2, vecIHomonyms[ui_homonym]);
            pDbHandle->InsertRow();
            pDbHandle->Finalize();
        }

        //
        // Stress
        //
        map<int, ET_StressType>::iterator itStress = mapStress.begin();
        for (; itStress != mapStress.end(); ++itStress)
        {
            bSaveStressData (pDbHandle, llHeadwordId, (*itStress).first, (*itStress).second);
        }
    }
    catch (...)
    {
        CEString sMsg;
        CEString sError;
        try
        {
            pDbHandle->GetLastError (sError);
            sMsg += L", error %d: ";
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, pDbHandle->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return false;
    }

    return true;

}   //  bool StHeadword::bSaveToDb (CSqlite * pDbHandle)

bool StHeadword::bSaveStressData (CSqlite * pDbHandle, 
                                  __int64 llHeadwordId, 
                                  int iPosition, 
                                  ET_StressType eType)
{
    try
    {
        pDbHandle->PrepareForInsert (L"stress", 3);
        pDbHandle->Bind (1, llHeadwordId);  // 0-based
        pDbHandle->Bind (2, iPosition);
        pDbHandle->Bind (3, (STRESS_PRIMARY == eType) ? true : false);    // is_primary
        pDbHandle->InsertRow();
        pDbHandle->Finalize();
    }
    catch (CException& ex)
    {
        CEString sMsg (ex.szGetDescription());
        CEString sError;
        try
        {
            pDbHandle->GetLastError (sError);
            sMsg += CEString (L", error %d: ");
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, pDbHandle->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return false;
    }

    return true;

}   //  bSaveStressData()

bool StDescriptor::bSaveToDb (CSqlite * pDbHandle, 
                              __int64 llWordId, 
                              __int64& llDescriptorId, 
                              __int64& llInflectionId, 
                              __int64& llAltInflectionId)
{
    llDescriptorId = -1;

    static CGramHasher cGram;
    try
    {
        pDbHandle->PrepareForInsert (L"descriptor", 32);
        pDbHandle->Bind (1, llWordId);
        pDbHandle->Bind (2, sGraphicStem);
        pDbHandle->Bind (3, bVariant);
        pDbHandle->Bind (4, sMainSymbol);
        pDbHandle->Bind (5, ePartOfSpeech);
        pDbHandle->Bind (6, bPluralOf);
        pDbHandle->Bind (7, bIntransitive);
        pDbHandle->Bind (8, bReflexive);
        pDbHandle->Bind (9, sMainSymbolPluralOf);
        pDbHandle->Bind (10, sAltMainSymbol);
        pDbHandle->Bind (11, sInflectionSymbol);
        pDbHandle->Bind (12, sComment);
        pDbHandle->Bind (13, sAltMainSymbolComment);
        pDbHandle->Bind (14, sAltInflectionComment);
        pDbHandle->Bind (15, sVerbAlternation);
        pDbHandle->Bind (16, bPastPartPassZhD);
        pDbHandle->Bind (17, iSection);
        pDbHandle->Bind (18, bNoComparative);
        pDbHandle->Bind (19, bAssumedForms);
        pDbHandle->Bind (20, bYo);
        pDbHandle->Bind (21, bO);
        pDbHandle->Bind (22, bGen2);
        pDbHandle->Bind (23, bIsImpersonal);
        pDbHandle->Bind (24, bIsIterative);
        pDbHandle->Bind (25, bHasAspectPair);
        pDbHandle->Bind (26, bHasDifficulties);
        pDbHandle->Bind (27, bHasIrregularForms);
        pDbHandle->Bind (28, bHasDeficiencies);
        pDbHandle->Bind (29, sRestrictedForms);
        pDbHandle->Bind (30, sContexts);
        pDbHandle->Bind (31, sCognate);
        pDbHandle->Bind (32, sTrailingComment);
        pDbHandle->InsertRow();
        pDbHandle->Finalize();

        llDescriptorId = pDbHandle->llGetLastKey();

        if (bLoc2)
        {
            pDbHandle->PrepareForInsert (L"second_locative", 3);
            pDbHandle->Bind (1, llDescriptorId);
            pDbHandle->Bind (2, bLoc2Optional);
            pDbHandle->Bind (3, sLoc2Preposition);
            pDbHandle->InsertRow();
            pDbHandle->Finalize();
        }

        if (bHasAspectPair)
        {
            vector<CEString>::iterator itAspectComment = vecAspectPairComment.begin();
            for (vector<int>::iterator itAspectType = vecAspectPairType.begin();
                itAspectType != vecAspectPairType.end();
                ++itAspectType, ++itAspectComment)
            {
                pDbHandle->PrepareForInsert (L"aspect_pair", 3);
                pDbHandle->Bind (1, llDescriptorId);
                pDbHandle->Bind (2, *itAspectType);
                pDbHandle->Bind (3, *itAspectComment);
                pDbHandle->InsertRow();
                pDbHandle->Finalize();
            }
        }

        bool bRet = bSaveIrregularForms (pDbHandle, llDescriptorId, false);
        if (!bRet)
        {
            //return false;
        }

        bRet = bSaveIrregularForms (pDbHandle, llDescriptorId, true);
        if (!bRet)
        {
            //return false;
        }

        bRet = bSaveDifficultForms (pDbHandle, llDescriptorId, 1); // difficulties
        if (!bRet)
        {
            //return false;
        }

        bRet = bSaveDifficultForms (pDbHandle, llDescriptorId, 2); // obsolete
        if (!bRet)
        {
            //return false;
        }

        bRet = bSaveDifficultForms (pDbHandle, llDescriptorId, 3); // colloquial
        if (!bRet)
        {
            //return false;
        }

        bRet = bSaveNonexistentForms (pDbHandle, llDescriptorId);
        if (!bRet)
        {
            //return false;
        }

        if (stInflectionGroup.bHasData)
        {
            bool b_ = bSaveInflectionGroup (pDbHandle, 
                                            llDescriptorId, 
                                            stInflectionGroup,
                                            true,
                                            llInflectionId);
            if (!b_)
            {
                ERROR_LOG (L"Error saving inflection group.");
            }

            if (stAltInflectionGroup.bHasData)
            {
                b_ = bSaveInflectionGroup (pDbHandle, 
                                           llDescriptorId, 
                                           stAltInflectionGroup,
                                           false,
                                           llAltInflectionId);
            }
            if (!b_)
            {
                ERROR_LOG (L"Error saving alt. inflection group.");
            }
        }
    }
    catch (CException& ex)
    {
        ATLASSERT(0);
        CEString sMsg (ex.szGetDescription());
        CEString sError;
        try
        {
            pDbHandle->GetLastError (sError);
            sMsg += CEString (L", error %d: ") + sError;
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, pDbHandle->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return false;
    }

    return true;

}   // StDescriptor::bSaveToDb()

bool StDescriptor::bSaveInflectionGroup (CSqlite * pDbHandle, 
                                         __int64 llDescriptorId, 
                                         const StInflectionGroup& stData, 
                                         bool bIsPrimary,
                                         __int64& llInflectionId)
{
    try
    {
        bool bDummy = false;

        pDbHandle->PrepareForInsert (L"inflection", 12);
        pDbHandle->Bind (1, llDescriptorId);
        pDbHandle->Bind (2, bIsPrimary);
        pDbHandle->Bind (3, stData.iType);
        pDbHandle->Bind (4, stData.eAccentType1);
        pDbHandle->Bind (5, stData.eAccentType2);

        if (POS_VERB == ePartOfSpeech)
        {
            pDbHandle->Bind (6, bDummy);          // short_form_restrictions N/A
            pDbHandle->Bind (7, stData.bX);       // past_part_restrictions
            pDbHandle->Bind (8, bDummy);          // no_short_form N/A
            pDbHandle->Bind (9, stData.bBoxedX);  // no_past_part
        }
        else
        {
            pDbHandle->Bind (6, stData.bX);       // short_form_restrictions 
            pDbHandle->Bind (7, bDummy);          // past_part_restrictions N/A
            pDbHandle->Bind (8, stData.bBoxedX);  // no_short_form
            pDbHandle->Bind (9, bDummy);          // no_past_part N/A
        }

        pDbHandle->Bind (10, stData.bFleetingVowel);
        pDbHandle->Bind (11, stData.iStemAugmentType);
        pDbHandle->Bind (12, stData.iInflectedParts);
        pDbHandle->InsertRow();
        pDbHandle->Finalize();

        llInflectionId = pDbHandle->llGetLastKey();

        for (unsigned int uiCd = 0; uiCd < stData.vecCommonDeviations.size(); ++uiCd)
        {
            pDbHandle->PrepareForInsert (L"common_deviation", 3);
            pDbHandle->Bind (1, llInflectionId);
            pDbHandle->Bind (2, stData.vecCommonDeviations[uiCd].iType);
            pDbHandle->Bind (3, stData.vecCommonDeviations[uiCd].bOptional);
            pDbHandle->InsertRow();
            pDbHandle->Finalize();
        }
    }
    catch (CException& ex)
    {
        CEString sMsg (ex.szGetDescription());
        CEString sError;
        try
        {
            pDbHandle->GetLastError (sError);
            sMsg += CEString (L", error %d: ");
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, pDbHandle->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return false;
    }

    return true;

}   //  bSaveInflectionGroup (...)

bool StDescriptor::bSaveIrregularForms(CSqlite *pDbHandle, __int64 llDescriptorId, bool bIsAlternative)
{
    CGramHasher cGram;
    CEString sFormsList = bIsAlternative ? sAltIrregularForms : sIrregularForms;
    while (sFormsList.uiLength()) // it deletes sFormsList
    {
        CEString sRegex = L"\\s*_([^_]+)_\\s*([^\\s,;]+),?;?(.*)";
        bool bMatch = sFormsList.bRegexMatch (sRegex);
        if (bMatch == true)
        {
            CEString sGram = sFormsList.sGetRegexMatch (0);
            CEString sForm = sFormsList.sGetRegexMatch(1);
            sFormsList = sFormsList.sGetRegexMatch(2);
            map<int, ET_StressType> mapStress;
            bool bRet = bExtractStress (sForm, sForm, mapStress);

            cGram.GramClear();
            cGram.m_ePos = ePartOfSpeech;
            // Aspect, gender, and animacy are also stored; aspect is needed for correct
            // calculation of other grammatical parameters in case of incomplete information
//                if (m_sMainSymbol == L"св")
            ET_MainSymbol eMs = this->eInflectionSymbol; //map_MainSymbol[m_sMainSymbol];
            switch (eMs)
            {
                case MS_M:
                    cGram.m_eGender = GENDER_M;
                    cGram.m_eAnimacy = ANIM_NO;
                    break;
                case MS_MO:
                    cGram.m_eGender = GENDER_M;
                    cGram.m_eAnimacy = ANIM_YES;
                    break;
                case MS_ZH:
                    cGram.m_eGender = GENDER_F;
                    cGram.m_eAnimacy = ANIM_NO;
                    break;
                case MS_ZHO:
                    cGram.m_eGender = GENDER_F;
                    cGram.m_eAnimacy = ANIM_YES;
                    break;
                case MS_S:
                    cGram.m_eGender = GENDER_N;
                    cGram.m_eAnimacy = ANIM_NO;
                    break;
                case MS_SO:
                    cGram.m_eGender = GENDER_N;
                    cGram.m_eAnimacy = ANIM_YES;
                    break;
                case MS_MO_ZHO:
                    cGram.m_eAnimacy = ANIM_YES;
                    break;
                case MS_MN_NEOD:
                    cGram.m_eAnimacy = ANIM_NO;
                    break;
                case MS_MN_ODUSH:
                    cGram.m_eAnimacy = ANIM_YES;
                    break;

                case MS_SV:
                    cGram.m_eAspect = ASPECT_PERFECTIVE;
                    break;
                case MS_NSV:
                    cGram.m_eAspect = ASPECT_IMPERFECTIVE;
                    break;
            }    // switch
            cGram.eDecodeString (sGram);
// TODO can DecodeString handle this?
            if (POS_VERB == ePartOfSpeech)
            {
                cGram.m_eReflexive = bReflexive ? REFL_YES : REFL_NO;
            }

/*
            // Temporary hack to align hash in dictionary with hash created by form builder
            if (SUBPARADIGM_PART_PAST_PASS_LONG == cGram.m_eSubparadigm)
            {
                cGram.m_eNumber = NUM_UNDEFINED;      // intentionally omitted per GDRL
                cGram.m_eGender = GENDER_UNDEFINED;   // intentionally omitted per GDRL
                cGram.m_ePerson = PERSON_UNDEFINED;
                cGram.m_eAnimacy = ANIM_UNDEFINED;
                cGram.m_eCase = CASE_UNDEFINED;       // intentionally omitted per GDRL
            }               
*/

            if (SUBPARADIGM_PART_PRES_ACT == cGram.m_eSubparadigm || 
                SUBPARADIGM_PART_PRES_PASS_LONG == cGram.m_eSubparadigm || 
                SUBPARADIGM_PART_PAST_ACT == cGram.m_eSubparadigm || 
                SUBPARADIGM_PART_PAST_PASS_LONG == cGram.m_eSubparadigm)
            {
                if (NUM_UNDEFINED == cGram.m_eNumber)
                {
                    cGram.m_eNumber = NUM_SG;
                }
                if (GENDER_UNDEFINED == cGram.m_eGender)
                {
                    cGram.m_eGender = GENDER_M;
                }
                if (CASE_UNDEFINED == cGram.m_eCase)
                {
                    cGram.m_eCase = CASE_NOM;
                }
                cGram.m_ePerson = PERSON_UNDEFINED;
                cGram.m_eAnimacy = ANIM_UNDEFINED;
            }

            if (SUBPARADIGM_IMPERATIVE == cGram.m_eSubparadigm)
            {
                cGram.m_eNumber = NUM_SG;         // undefined in the source
                cGram.m_ePerson = PERSON_2;       // undefined in the source
            }               

            pDbHandle->PrepareForInsert (L"irregular_forms", 4);
            pDbHandle->Bind (1, llDescriptorId);
            pDbHandle->Bind (2, cGram.iGramHash());     // Morphosyntactic values code
            pDbHandle->Bind (3, sForm);                 // Wordform
            pDbHandle->Bind (4, bIsAlternative);          // is alternative to the regular form?
            pDbHandle->InsertRow();
            pDbHandle->Finalize();

            bSaveIrregularStressData (pDbHandle, 
                                      pDbHandle->llGetLastKey(), 
                                      mapStress);
        }
        else
        {
            if (sFormsList.uiLength() > 0)
            {
                if (bIsAlternative)
                {
                    ERROR_LOG(L"INSERT>  sAltIrregularForms is non-empty after extraction: " + sFormsList);
                }
                else
                {
                    ERROR_LOG(L"INSERT>  sIrregularForms is non-empty after extraction: " + sFormsList);
                }
                return false;
            }
        }
    } // while (sFormsList.length())...
    return true;
}

bool StDescriptor::bSaveDifficultForms(CSqlite *pDbHandle, __int64 llDescriptorId, int iType)
{
    CGramHasher cGram;
    CEString sFormsList;
    switch (iType)
    {
    case 1:
        sFormsList = sDifficulties;
        break;
    case 2:
        sFormsList = sObsolete;
        break;
    case 3:
        sFormsList = sColloquial;
        break;
    default:
        ERROR_LOG(L"Wrong type number in b_SaveDifficultForms");
        return false;
    }

    while (sFormsList.uiLength()) // it deletes sFormsList
        {
            CEString sRegex = L"\\s*([^,;]+)[;,]?\\s*(.*)";
            bool bMatch = sFormsList.bRegexMatch (sRegex);
            if (bMatch == true)
            {
                CEString sGram = sFormsList.sGetRegexMatch(0);
                sFormsList = sFormsList.sGetRegexMatch(1);
                CEString sForm = L"";
                bMatch = sGram.bRegexMatch(L"(.*?)\\s*\\(([^\\)]+)\\)\\s*");
                if (bMatch)
                {
                    sForm = sGram.sGetRegexMatch(1);
                    sGram = sGram.sGetRegexMatch(0);
                }

                cGram.GramClear();
                cGram.m_ePos = ePartOfSpeech;
                if (sMainSymbol == L"св")
                {
                    cGram.m_eAspect = ASPECT_PERFECTIVE;
                }
                else if (sMainSymbol == L"нсв")
                {
                    cGram.m_eAspect = ASPECT_IMPERFECTIVE;
                }
                if (POS_VERB == ePartOfSpeech)
                {
                    cGram.m_eReflexive = bReflexive ? REFL_YES : REFL_NO;
                }
                cGram.eDecodeString(sGram);
                pDbHandle->PrepareForInsert (L"difficulties", 4);
                pDbHandle->Bind (1, llDescriptorId);
                pDbHandle->Bind (2, cGram.iGramHash());   // Morphosyntactic values code
                pDbHandle->Bind (3, sForm);               // The "difficult" wordform itself
                pDbHandle->Bind (4, iType);                 // 1 = difficult, 2 = obsolete, 3 = informal
                pDbHandle->InsertRow();
                pDbHandle->Finalize();
            }
            else
            {
                if (sFormsList.uiLength() > 0)
                {
                    if (iType == 1)
                    {
                        ERROR_LOG(L"INSERT>  xstr_Difficult is non-empty after extraction: " + sFormsList);
                    }
                    else if (iType == 2)
                    {
                        ERROR_LOG(L"INSERT>  xstr_Obsolete is non-empty after extraction: " + sFormsList);
                    }
                    else if (iType == 3)
                    {
                        ERROR_LOG(L"INSERT>  xstr_Colloquial is non-empty after extraction: " + sFormsList);
                    }
                    return false;
                }
            }
        } // while (sFormsList.length())...
    return true;
}

bool StDescriptor::bSaveNonexistentForms (CSqlite *pDbHandle, __int64 llDescriptorId)
{
    CGramHasher cGram;
    CEString sFormsList (sDeficient);

    while (sFormsList.uiLength()) // it deletes sFormsList
        {
            CEString sRegex = L"\\s*([^,;]+)[;,]?\\s*(.*)";
            bool bMatch = sFormsList.bRegexMatch (sRegex);
            if (bMatch == true)
            {
                CEString sGram = sFormsList.sGetRegexMatch(0);
                sFormsList = sFormsList.sGetRegexMatch(1);

                cGram.GramClear();
                cGram.m_ePos = ePartOfSpeech;
                if (sMainSymbol == L"св")
                {
                    cGram.m_eAspect = ASPECT_PERFECTIVE;
                }
                else if (sMainSymbol == L"нсв")
                {
                    cGram.m_eAspect = ASPECT_IMPERFECTIVE;
                }
                if (POS_VERB == ePartOfSpeech)
                {
                    cGram.m_eReflexive = bReflexive ? REFL_YES : REFL_NO;
                }
                cGram.eDecodeString(sGram);
                pDbHandle->PrepareForInsert (L"nonexistent_forms", 2);
                pDbHandle->Bind (1, llDescriptorId);
                pDbHandle->Bind (2, cGram.iGramHash());   // Morphosyntactic values code
                pDbHandle->InsertRow();
                pDbHandle->Finalize();
            }
            else
            {
                if (sFormsList.uiLength() > 0)
                {
                    ERROR_LOG(L"INSERT>  sDeficient is non-empty after extraction: " + sFormsList);
                    return false;
                }
            }
        } // while (sFormsList.length())...
    return true;
}

bool StDescriptor::bSaveIrregularStressData (CSqlite * pDbHandle, 
                                             __int64 llFormId, 
                                             const map<int, ET_StressType>& mapStress) 
{
    try
    {
        map<int, ET_StressType>::const_iterator itAt = mapStress.begin();
        for (; itAt != mapStress.end(); ++itAt)
        {
            pDbHandle->PrepareForInsert (L"irregular_stress", 3);
            pDbHandle->Bind (1, llFormId);
            pDbHandle->Bind (2, (*itAt).first);
            pDbHandle->Bind (3, (STRESS_PRIMARY == (*itAt).second) ? true : false);
            pDbHandle->InsertRow();
            pDbHandle->Finalize();
        }
    }
    catch (CException& ex)
    {
        CEString sMsg (ex.szGetDescription());
        CEString sError;
        try
        {
            pDbHandle->GetLastError (sError);
            sMsg += CEString (L", error %d: ");
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, pDbHandle->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return false;
    }

    return true;

}   //  bSaveIrregularStressData (...)
