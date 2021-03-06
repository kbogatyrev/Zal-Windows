#include "stdafx.h"
#include "Analyzer.h"
#include "Dictionary.h"
#include "Lexeme.h"

void CAnalyzer::Init()
{
    // Fill map_MainSymbol
    static wchar_t * arrMainSymbol[] = { L"м", L"мо", L"ж", L"жо", L"с", L"со", L"мо-жо", L"мн.",
            L"мн. неод.", L"мн. одуш.", L"мн. _от_", L"п", L"мс", L"мс-п", L"мс-предик.", L"числ.", L"числ.-п", 
            L"св", L"нсв", L"св-нсв", L"н", L"предл.", L"союз", L"предик.", L"вводн.", L"сравн.", 
            L"част.", L"межд.", L"NULL"  };
    for (ET_MainSymbol eoMs = MS_START; eoMs < MS_END; ++eoMs)
    {
        m_mapMainSymbol[arrMainSymbol[eoMs]] = eoMs; 
    }
}

HRESULT CAnalyzer::LoadDb(BSTR bstrPath)
{
    USES_CONVERSION;

int iDebugFlag =  _CrtSetDbgFlag( _CRTDBG_REPORT_FLAG );
iDebugFlag |= _CRTDBG_LEAK_CHECK_DF;

    if (m_pDb)
    {
        delete m_pDb;
    }

    m_sDbPath = OLE2W(bstrPath);
    m_pDb = new CSqlite(m_sDbPath);
    if (m_pDb == NULL)
    {
        ERROR_LOG (L"Failed to instantiate CSqlite.");
        return E_FAIL;
    }

    // Fill umap_freq_stems
    umap_freq_stems.clear();
    CEString str_query = L"Select * from stems_for_analysis"; // TODO: frequencies
    m_pDb->PrepareForSelect(str_query);
    stStemLinks s_links;
    CEString str_stem = L"", str_stem_without_yo = L"";

    while (m_pDb->bGetRow())
    {
        m_pDb->GetData(0, s_links.iStemId);
        m_pDb->GetData(1, s_links.llLexemeId);
        m_pDb->GetData(2, str_stem);
        m_pDb->GetData(3, s_links.iStressPos);
        m_pDb->GetData(4, s_links.iEndingsLink);
        m_pDb->GetData(5, s_links.sLemma);
        if (str_stem.bIsEmpty())
        {
            continue;
        }
        umap_freq_stems.insert(pair<CEString, stStemLinks>(str_stem, s_links));
        str_stem_without_yo = str_stem;
        str_stem_without_yo.Replace(0, str_stem_without_yo.uiLength(), L'ё', L'е');
        if (str_stem_without_yo != str_stem)
        {
            umap_freq_stems.insert (pair<CEString, stStemLinks> (str_stem_without_yo, s_links));
        }
    }

    m_pDb->Finalize();

    // Fill arr_freq_endings
    int i_subtable_id, i_cur_subtable_id = -1, i_hash, iStressPos;
    CEString str_ending = L"", str_ending_without_yo = L"";
    if (arr_freq_endings != NULL)
    {
        delete[] arr_freq_endings;
    }
    int i_last_subtable = m_pDb->iLastID(L"endings_for_analysis");
    if (i_last_subtable < -1)
    {
        return E_FAIL;
    }
    arr_freq_endings = new CEndingsTable[i_last_subtable + 1];
    str_query = L"Select * from endings_for_analysis order by subtable_id asc";
    m_pDb->PrepareForSelect(str_query);
    CEndingsTable co_et;
    while (m_pDb->bGetRow())
    {
        m_pDb->GetData(1, i_subtable_id);
        m_pDb->GetData(2, str_ending);
        m_pDb->GetData(3, i_hash);
        m_pDb->GetData(4, iStressPos);
        if (i_subtable_id != i_cur_subtable_id)
        {
            // Push the current subtable to the array
            if (i_cur_subtable_id >= 0)
            {
                CEndingsTable coETCopy(co_et);
                arr_freq_endings[i_cur_subtable_id] = coETCopy;
            }
            i_cur_subtable_id = i_subtable_id;
            co_et.Clear();
        }
        co_et.iAddEnding(i_hash, str_ending, iStressPos);
        str_ending_without_yo = str_ending;
        str_ending_without_yo.Replace(0, str_ending_without_yo.uiLength(), L'ё', L'е');
        if (str_ending_without_yo != str_ending)
        {
            co_et.iAddEnding(i_hash, str_ending_without_yo, iStressPos);
        }
    }
    m_pDb->Finalize();

    // Fill umap_endings2subtbl
    umap_endings2subtbl.clear();
    str_query = L"Select * from subtables_for_heuristics";
    m_pDb->PrepareForSelect(str_query);
    int iLexemeCount = 0, iCutRight = 0;
    CEString sStemFinal = L"", sLemmaFinal = L"";
    while (m_pDb->bGetRow())
    {
        m_pDb->GetData (1, i_subtable_id);
        m_pDb->GetData (2, iLexemeCount);
        m_pDb->GetData (3, sStemFinal);
        m_pDb->GetData (4, iCutRight);
        m_pDb->GetData (5, sLemmaFinal);
        arr_freq_endings[i_subtable_id].m_iLexemeCount = iLexemeCount;
        arr_freq_endings[i_subtable_id].m_iCutRight = iCutRight;
        arr_freq_endings[i_subtable_id].m_sStemFinal = sStemFinal;
        arr_freq_endings[i_subtable_id].m_sLemmaFinal = sLemmaFinal;
        for (vector<stInflexion>::iterator iterEnding = arr_freq_endings[i_subtable_id].m_vecEndings.begin();
             iterEnding != arr_freq_endings[i_subtable_id].m_vecEndings.end();
             ++iterEnding)
        {
//            umap_endings2subtbl.insert(unordered_multimap<CEString, int>::value_type(*iter_ending, i_subtable_id));
            umap_endings2subtbl.insert(pair<CEString, int>(iterEnding->sEnding, i_subtable_id));
        }
    }
    m_pDb->Finalize();
    return S_OK;
}

HRESULT CAnalyzer::Analyze(BSTR bstr_Wordform)
{
    USES_CONVERSION;

    if (m_pDb == NULL)
    {
        ERROR_LOG (L"DB pointer is NULL.");
        return E_FAIL;
    }
    
    m_coll.clear();
    vector<CHasher> vec_possible_analyses;
    int i_wf_count = iAnalyze(OLE2W(bstr_Wordform), vec_possible_analyses, TRUE);
    if (i_wf_count < 0)
    {
        ERROR_LOG (L"Negative number returned from i_Analyze.");
        return E_FAIL;
    }
    if (i_wf_count == 0)
    {
        return S_FALSE;
    }
    // Otherwise, i_wf_count > 0
    // Then, for every wordform found, put the data to the collection
    for (vector<CHasher>::iterator itWf = vec_possible_analyses.begin();
        itWf != vec_possible_analyses.end();
        itWf++)
    {
        CComObject<CWordForm> *wF = NULL;
        hHasher2Wordform(OLE2W(bstr_Wordform), *itWf, wF);
        m_coll.push_back(CComVariant(wF));
    }
    vec_possible_analyses.clear();

    return S_OK;
} // Analyze(BSTR bstr_Wordform)

int CAnalyzer::iAnalyze(CEString sWordform,
                        vector<CHasher>& vecPossibleWordforms,
                        BOOL bGuess)
{
    // Be careful: it changes the input
    if (sWordform.uiLength() <= 0 || m_pDb == NULL) // || vecPossibleWordforms == NULL)
    {
        return -1;
    }

// Multiple stress marks??

    CEString sWordformOriginal(sWordform);
    int iStressPosStem = -1, iStressPosEnding = -1;
    int iStressPos = sWordform.uiFindOneOf(0, L"<\u0301");
    if (ecNotFound == iStressPos)
    {
        iStressPos = -1;
    }
    else
    {
        sWordform.sErase(iStressPos, 1);
    }

/*
    wsmatch result;
    bool b_match = regex_match(wstring(sWordform), result, (const wregex)L"^([^<\u0301]*)([<\u0301])(.*)$");
    if (b_match == true)
    {
        CEString sLeft = (CEString)result[1];
        CEString str_delimiter = (CEString)result[2];
        CEString sRight = (CEString)result[3];
        sWordform = sLeft + sRight;
        if (str_delimiter[0] == L'<')       // кор<ова
        {
            iStressPos = sLeft.length();
        }
        else                                // коро\u0301ва
        {
            iStressPos = sLeft.length() - 1;
        }
    }
    else
    {
        iStressPos = -1;
    }
*/

    wsmatch result;
    bool bMatch = sWordform.bRegexMatch(L"^([^<\u0301]*)([<\u0301])(.*)$");
    if (bMatch == true)
    {
        CEString sLeft = sWordform.sGetRegexMatch(0);
        CEString sDelimiter = sWordform.sGetRegexMatch(1);
        CEString sRight = sWordform.sGetRegexMatch(2);
        sWordform = sLeft + sRight;
        if (sDelimiter[0] == L'<')       // кор<ова
        {
            iStressPos = sLeft.uiLength();
        }
        else                                // коро\u0301ва
        {
            iStressPos = sLeft.uiLength() - 1;
        }
    }
    else
    {
        iStressPos = -1;
    }

    CEString sLeft, sRight;
    vector<stStemLinks> vecStems;
    vecPossibleWordforms.clear();
    for (int iLeft = sWordform.uiLength(); iLeft >= 0; --iLeft)
    {
        sLeft = sWordform.sSubstr(0, iLeft);
        sRight = sWordform.sSubstr(iLeft, sWordform.uiLength() - iLeft);

        // Stress positions for the stem and the ending
        if (iStressPos == -1)
        {
            iStressPosStem = iStressPosEnding = -2;
        }
        else if (iStressPos >= sLeft.uiLength())
        {
            iStressPosStem = -1;
            iStressPosEnding = iStressPos - sLeft.uiLength();
        }
        else
        {
            iStressPosStem = iStressPos;
            iStressPosEnding = -1;
        }
        
        vecStems.clear();
        iLookUpStems(vecStems, sLeft, iStressPosStem);
        if (vecStems.empty())
        {
            continue;
        }
        iCheckEndings(vecPossibleWordforms, vecStems, sLeft, sRight, iStressPosEnding);
    }

    // If we have no result, try cutting of possible prefixes
    if (vecPossibleWordforms.empty())
    {
        for (int iLeft = min(sWordformOriginal.uiLength(), 4); iLeft >= 1; --iLeft)
        {
            sLeft = sWordformOriginal.sSubstr(0, iLeft);
            sRight = sWordformOriginal.sSubstr(iLeft, sWordformOriginal.uiLength() - iLeft);
            if (iLeft == 4)
            {
                if (sLeft == L"пол-")
                {
                    int iResult = iAnalyze(sRight, vecPossibleWordforms, bGuess);
                    if (iResult > 0)
                    {
                        for (int iWf = iResult - 1; iWf >= 0; --iWf)
                        {
                            if (vecPossibleWordforms[iWf].m_ePos != POS_NOUN ||
                                vecPossibleWordforms[iWf].m_eNumber != NUM_SG ||
                                vecPossibleWordforms[iWf].m_eCase != CASE_GEN)
                            {
                                vecPossibleWordforms.erase(vecPossibleWordforms.begin() + iWf);
                            }
                        }
                        if (vecPossibleWordforms.size() > 0)
                        {
                            return vecPossibleWordforms.size();
                        }
                    }
                }
            }
            else if (iLeft == 3)
            {
                if (sLeft == L"пол")
                {
                    int iResult = iAnalyze(sRight, vecPossibleWordforms, bGuess);
                    if (iResult > 0)
                    {
                        for (int iWf = iResult - 1; iWf >= 0; --iWf)
                        {
                            if (vecPossibleWordforms[iWf].m_ePos != POS_NOUN ||
                                vecPossibleWordforms[iWf].m_eNumber != NUM_SG ||
                                vecPossibleWordforms[iWf].m_eCase != CASE_GEN)
                            {
                                vecPossibleWordforms.erase(vecPossibleWordforms.begin() + iWf);
                            }
                        }
                        if (vecPossibleWordforms.size() > 0)
                        {
                            return vecPossibleWordforms.size();
                        }
                    }
                }
            }
            else if (iLeft == 2)
            {
                if (sLeft == L"не")
                {
                    int iResult = iAnalyze(sRight, vecPossibleWordforms, bGuess);
                    if (iResult > 0)
                    {
                        for (int iWf = iResult - 1; iWf >= 0; --iWf)
                        {
                            if (vecPossibleWordforms[iWf].m_ePos != POS_NOUN ||
                                vecPossibleWordforms[iWf].m_ePos != POS_ADJ ||
                                vecPossibleWordforms[iWf].m_ePos != POS_VERB ||
                                (vecPossibleWordforms[iWf].m_ePos == POS_VERB &&
                                 (vecPossibleWordforms[iWf].m_eSubparadigm == SUBPARADIGM_PRESENT_TENSE ||
                                  vecPossibleWordforms[iWf].m_eSubparadigm == SUBPARADIGM_PAST_TENSE ||
                                  vecPossibleWordforms[iWf].m_eSubparadigm == SUBPARADIGM_INFINITIVE)))
                            {
                                vecPossibleWordforms.erase(vecPossibleWordforms.begin() + iWf);
                            }
                        }
                        if (vecPossibleWordforms.size() > 0)
                        {
                            return vecPossibleWordforms.size();
                        }
                    }
                }
            }
        }
    }
    // Now, if we haven't found anything, we may guess the lexeme
    if (vecPossibleWordforms.empty() && bGuess == TRUE)
    {
        for (int iLeft = 0; iLeft <= sWordform.uiLength(); ++iLeft)
        {
            sLeft = sWordform.sSubstr (0, iLeft);
            sRight = sWordform.sSubstr (iLeft, sWordform.uiLength() - iLeft);

            // Stress positions for the stem and the ending
            if (iStressPos == -1)
            {
                iStressPosStem = iStressPosEnding = -2;
            }
            else if (iStressPos >= sLeft.uiLength())
            {
                iStressPosStem = -1;
                iStressPosEnding = iStressPos - sLeft.uiLength();
            }
            else
            {
                iStressPosStem = iStressPos;
                iStressPosEnding = -1;
            }
            vecStems.clear();
            iCheckEndings (vecPossibleWordforms, vecStems, sLeft, sRight, iStressPosEnding);
            if ((bContainsPlausibleVariants (vecPossibleWordforms) && sRight.uiLength() <= 3) ||
                vecPossibleWordforms.size() >= 4)
            {
                break;
            }
        }
        if (vecPossibleWordforms.size() > 4)
        {
            LeaveMostPlausible (vecPossibleWordforms);
        }
    }
    return vecPossibleWordforms.size();
}

int CAnalyzer::iLookUpStems(vector<stStemLinks>& vecStems, CEString sLeft, int iStressPosStem)
{
    // Search the DB for graphic stems equal to sLeft;
    // write their IDs to vec_stems_id
    if (m_pDb == NULL) // || vecStems == NULL)
    {
        return -1;
    }
    // First, try to find it among the frequent stems:
//    pair<unordered_multimap<wstring, stStemLinks>::iterator,
//         unordered_multimap<wstring, stStemLinks>::iterator> pair_search_result = umap_freq_stems.equal_range((wstring)sLeft);
    pair<multimap<CEString, stStemLinks>::iterator,
         multimap<CEString, stStemLinks>::iterator> pair_search_result = umap_freq_stems.equal_range(sLeft);
    for (; pair_search_result.first != pair_search_result.second; ++pair_search_result.first)
    {
        stStemLinks s_links = pair_search_result.first->second;
        if (iStressPosStem >= -1 && s_links.iStressPos != iStressPosStem)
        {
            continue;
        }
        vecStems.push_back(s_links);
    }
    //if (!pvec_stems_id->empty())
    //{
        return vecStems.size();
    //}

    // Then, if we haven't got anything, look it up in the DB.
        /*
    CEString str_query = L"Select * from stems_for_analysis where stem = \"" + sLeft + L"\"";
    if (iStressPosStem >= -1)
    {
        str_query += L" and stress_pos = " + str_ToString<int>(iStressPosStem);
    }
    int iStemId;
    m_pDb->v_PrepareForSelect(str_query);
    while (m_pDb->b_GetRow())
    {
        m_pDb->GetData(0, iStemId);
        pvec_stems_id->push_back(iStemId);
    }
    return pvec_stems_id->size();
    */
}

int CAnalyzer::iCheckEndings(vector<CHasher>& vecPossibleWordforms,
                             vector<stStemLinks>& vecStems,
                             CEString sLeft,
                             CEString sRight,
                             int iStressPosEnding)
// If pvec_stems_id IS NOT empty:
// For every stem in pvec_stems_id, take the corresponding endings table
// and look whether it contains an ending equal to sRight;
// for every such ending, add a wordform to vecPossibleWordforms.
//
// If pvec_stems_id IS empty:
// Look for an ending equal to sRight; for every such ending,
// build a wordform and store it in vecPossibleWordforms.
// (Identical wordforms are stored as one wordform.)
{
    if (m_pDb == NULL) // || vecStems == NULL)
    {
        return -1;
    }
    static vector<int> vecGram;
    CEString str_query, sLemma;
    vector<CEString> vecLemma;

    for (vector<stStemLinks>::iterator itStems = vecStems.begin();
        itStems != vecStems.end(); itStems++)
    {
        // For each *itStems look up the endings table ID in DB, then in this table try to find
        // endings which are equal to sRight. For each ending found, write the parameters
        // to tmpWf and then push_back tmpWf to vecPossibleWordforms:
        vecGram.clear();
        vecGram = arr_freq_endings[(*itStems).iEndingsLink].m_vecFind (sRight, iStressPosEnding);
        if (vecGram.empty())
        {
            continue;
        }
        for (vector<int>::iterator iter_endings = vecGram.begin();
            iter_endings != vecGram.end(); iter_endings++)
        {
            CHasher tmpWf;
            tmpWf.hDecodeHash(*iter_endings);
            tmpWf.m_llLexemeId = (*itStems).llLexemeId;
            tmpWf.m_sLemma = (*itStems).sLemma;
            //tmpWf.str_WordForm = sLeft + sRight;
            //h_AddClassifyingCategories(&tmpWf);
            vecPossibleWordforms.push_back (tmpWf);
        }
        vecLemma.clear(); // that vector is different for every stem found
    }

    if (vecStems.empty())
    // Try to guess the lexeme
    {
        if (sLeft.uiLength() <= 2)
        {
            return 0;
        }
        vector<int> vec_i_possible_ETs;
//        pair<unordered_multimap<wstring, int>::iterator,
//             unordered_multimap<wstring, int>::iterator> pair_search_result = umap_endings2subtbl.equal_range((wstring)sRight);
        pair<multimap<CEString, int>::iterator,
             multimap<CEString, int>::iterator> pair_search_result = umap_endings2subtbl.equal_range(sRight);
        for (; pair_search_result.first != pair_search_result.second; ++pair_search_result.first)
        {
            vec_i_possible_ETs.push_back(pair_search_result.first->second);
        }
        for (vector<int>::iterator iter_ET = vec_i_possible_ETs.begin();
             iter_ET != vec_i_possible_ETs.end();
             ++iter_ET)
        {
            if (arr_freq_endings[*iter_ET].m_sStemFinal.uiLength() > 0 &&
//                !regex_match(sLeft, (const wregex)(L"^.*(" + arr_freq_endings[*iter_ET].m_sStemFinale + L")$")))
                !sLeft.bRegexMatch (L"^.*(" + arr_freq_endings[*iter_ET].m_sStemFinal + L")$"))
            {
                continue;
            }
            if (sLeft.uiLength() <= arr_freq_endings[*iter_ET].m_iCutRight)
            {
                continue;
            }
            vecGram.clear();
            vecGram = arr_freq_endings[*iter_ET].m_vecFind(sRight, -2);
            if (vecGram.empty())
            {
                continue;
            }
            for (vector<int>::iterator itHash = vecGram.begin();
                 itHash != vecGram.end(); ++itHash)
            {
                CHasher tmpWf;
                tmpWf.m_sLemma = sLeft.sSubstr(0, sLeft.uiLength() - arr_freq_endings[*iter_ET].m_iCutRight) + arr_freq_endings[*iter_ET].m_sLemmaFinal;
                if (!bIsValidLemma (tmpWf.m_sLemma))
                {
                    continue;
                }
                // Check if what we've found is a new wordform
                bool bExists = false;
                for (vector<CHasher>::iterator itWf = vecPossibleWordforms.begin();
                     itWf != vecPossibleWordforms.end();
                     ++itWf)
                {
                    if ((*itWf).m_sLemma == tmpWf.m_sLemma && (*itWf).iGramHash() == *itHash)
                    {
                        bExists = true;
                    }
                }
                if (!bExists)
                {
                    tmpWf.hDecodeHash(*itHash);
                    tmpWf.m_llLexemeId = 0;
                    vecPossibleWordforms.push_back(tmpWf);
                }
            }
        }
    }
    vecGram.clear();
    return 0;
}

HRESULT CAnalyzer::hAddClassifyingCategories(CHasher * pco_wf)
{
    if (pco_wf == NULL || m_pDb == NULL)
    {
        return E_FAIL;
    }
    CEString str_query (L"select * from descriptor where id = " + CEString::sToString<__int64>(pco_wf->m_llLexemeId));
    CEString str_MainSymbol = L"";
    m_pDb->PrepareForSelect(str_query);
    m_pDb->bGetRow();
    m_pDb->GetData(4, str_MainSymbol);

    ET_MainSymbol eoMs = m_mapMainSymbol[str_MainSymbol];
    switch (eoMs)
    {
        case MS_M:
            pco_wf->m_eGender = GENDER_M;
            pco_wf->m_eAnimacy = ANIM_NO;
            break;
        case MS_MO:
            pco_wf->m_eGender = GENDER_M;
            pco_wf->m_eAnimacy = ANIM_YES;
            break;
        case MS_ZH:
            pco_wf->m_eGender = GENDER_F;
            pco_wf->m_eAnimacy = ANIM_NO;
            break;
        case MS_ZHO:
            pco_wf->m_eGender = GENDER_F;
            pco_wf->m_eAnimacy = ANIM_YES;
            break;
        case MS_S:
            pco_wf->m_eGender = GENDER_N;
            pco_wf->m_eAnimacy = ANIM_NO;
            break;
        case MS_SO:
            pco_wf->m_eGender = GENDER_N;
            pco_wf->m_eAnimacy = ANIM_YES;
            break;
        case MS_MO_ZHO:
            pco_wf->m_eAnimacy = ANIM_YES;
            break;
        case MS_MN_NEOD:
            pco_wf->m_eAnimacy = ANIM_NO;
            break;
        case MS_MN_ODUSH:
            pco_wf->m_eAnimacy = ANIM_YES;
            break;

        case MS_SV:
            pco_wf->m_eAspect = ASPECT_PERFECTIVE;
            break;
        case MS_NSV:
            pco_wf->m_eAspect = ASPECT_IMPERFECTIVE;
            break;
    }    // switch
    return S_OK;
}

void CAnalyzer::DeleteRepeats(vector<CEString>& vec_strings)
{
    if (vec_strings.empty())
    {
        return;
    }
    CEString str_previous = L"";
    sort(vec_strings.begin(), vec_strings.end());
    for (vector<CEString>::iterator iter = vec_strings.begin();
        iter != vec_strings.end();
        ++iter)
    {
        if (*iter == str_previous)
        {
            vector<CEString>::iterator iter_erase = iter;
            --iter;
            if (iter == vec_strings.begin())
            {
                str_previous = L"";
            }
            else
            {
                --iter;
                str_previous = *iter;
                ++iter;
            }
            vec_strings.erase(iter_erase);
        }
        else
        {
            str_previous = *iter;
        }
    }
}

HRESULT CAnalyzer::hHasher2Wordform(const CEString& sWordform,
                                    CHasher co_from,
                                    CComObject<CWordForm> *& pco_to)
{
    HRESULT h_r = S_OK;

    h_r = CComObject<CWordForm>::CreateInstance (&pco_to);
    if (S_OK != h_r)
    {
        ATLASSERT(0);
        CString cs_msg;
        cs_msg.Format (L"CreateInstance() failed, error %x.", h_r);
        ERROR_LOG ((LPCTSTR)cs_msg);
        throw CException (E_INVALIDARG, (LPCTSTR)cs_msg);
    }

    pco_to->m_ePos = co_from.m_ePos;
    pco_to->m_eSubparadigm = co_from.m_eSubparadigm;
    pco_to->m_sLemma = co_from.m_sLemma;
    pco_to->m_eAnimacy = co_from.m_eAnimacy;
    pco_to->m_eAspect = co_from.m_eAspect;
    pco_to->m_eCase = co_from.m_eCase;
    pco_to->m_eGender = co_from.m_eGender;
    pco_to->m_eNumber = co_from.m_eNumber;
    pco_to->m_ePerson = co_from.m_ePerson;
    pco_to->m_eReflexive = co_from.m_eReflexive;
    pco_to->m_llLexemeId = co_from.m_llLexemeId;
    pco_to->m_sWordForm = sWordform;
    // TODO: stress position
    return S_OK;
}

bool CAnalyzer::bIsValidLemma(CEString sWf)
{
//    if (!regex_search(sWf, (const wregex)(L"[аеёиоуыэюяАЕЁИОУЫЭЮЯ]")))
    if (!sWf.bRegexSearch (L"[аеёиоуыэюяАЕЁИОУЫЭЮЯ]"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[аеёиоуыэюяъь][ьъ]")))
    if (sWf.bRegexSearch (L"[аеёиоуыэюяъь][ьъ]"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"ъ[аоуыэи]")))
    if (sWf.bRegexSearch (L"ъ[аоуыэи]"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[аоэуе][аоэуы]ть$")))
    if (sWf.bRegexSearch (L"[аоэуе][аоэуы]ть$"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[кгхц]ь$")))
    if (sWf.bRegexSearch (L"[кгхц]ь$"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[кгх]ый$")))
    if (sWf.bRegexSearch (L"[кгх]ый$"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[жчшщ]ы")))
    if (sWf.bRegexSearch (L"[жчшщ]ы"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"ы$")))
    if (sWf.bRegexSearch (L"ы$"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[бвгджзклмнпрстфхцчшщ](й|ъ$)")))
    if (sWf.bRegexSearch (L"[бвгджзклмнпрстфхцчшщ](й|ъ$)"))
    {
        return false;
    }
//    if (regex_search(sWf, (const wregex)(L"[бвгджзклмнпрстфхцчшщ]{4}$")))
    if (sWf.bRegexSearch (L"[бвгджзклмнпрстфхцчшщ]{4}$"))
    {
        return false;
    }
    return true;
}

bool CAnalyzer::bContainsPlausibleVariants(vector<CHasher>& vecPossibleWordforms)
{
    //if (vecPossibleWordforms == NULL)
    //{
    //    return false;
    //}
    for (vector<CHasher>::iterator itWf = vecPossibleWordforms.begin();
         itWf != vecPossibleWordforms.end();
         ++itWf)
    {
        if (iPlausibility (*itWf) >= 4)
        {
            return true;
        }
    }
    return false;
}

int CAnalyzer::iPlausibility(CHasher wF)
{
    if (wF.m_ePos == POS_PRONOUN ||
        wF.m_ePos == POS_PRONOUN_ADJ ||
        wF.m_ePos == POS_PRONOUN_PREDIC ||
        wF.m_ePos == POS_NUM ||
        wF.m_ePos == POS_NUM_ADJ ||
        wF.m_ePos == POS_PREDIC ||
        wF.m_ePos == POS_PREP ||
        wF.m_ePos == POS_CONJ ||
        wF.m_ePos == POS_PARTICLE ||
        wF.m_ePos == POS_INTERJ ||
        wF.m_ePos == POS_PARENTH ||
        wF.m_ePos == POS_INTERJ ||
        wF.m_ePos == POS_NULL ||
        wF.m_ePos == POS_UNDEFINED)
    {
        return 0;
    }
    else if (wF.m_eSubparadigm == SUBPARADIGM_COMPARATIVE ||
             wF.m_ePos == POS_COMPAR)
    {
        return 1;
    }
    else if (wF.m_eSubparadigm == SUBPARADIGM_PART_PRES_PASS_SHORT ||
             wF.m_eSubparadigm == SUBPARADIGM_PART_PAST_PASS_SHORT ||
             wF.m_eSubparadigm == SUBPARADIGM_SHORT_ADJ)
    {
        return 2;
    }
    else if (wF.m_ePos == POS_ADV ||
             wF.m_eSubparadigm == SUBPARADIGM_IMPERATIVE ||
             wF.m_eSubparadigm == SUBPARADIGM_ADVERBIAL_PAST ||
             wF.m_eSubparadigm == SUBPARADIGM_ADVERBIAL_PRESENT ||
             (wF.m_ePos == POS_NOUN && wF.m_eAnimacy == ANIM_YES))
    {
        return 3;
    }
    else if (wF.m_eSubparadigm == SUBPARADIGM_PRESENT_TENSE ||
             wF.m_eSubparadigm == SUBPARADIGM_PAST_TENSE ||
             wF.m_eSubparadigm == SUBPARADIGM_PART_PAST_PASS_LONG)
    {
        return 4;
    }
    else if (wF.m_ePos == POS_NOUN && wF.m_eAnimacy == ANIM_NO && wF.m_eGender == GENDER_N)
    {
        return 5;
    }
    else if (wF.m_ePos == POS_NOUN && wF.m_eAnimacy == ANIM_NO && wF.m_eGender == GENDER_M)
    {
        return 6;
    }
    else if (wF.m_ePos == POS_NOUN && wF.m_eAnimacy == ANIM_NO && wF.m_eGender == GENDER_F)
    {
        return 7;
    }
    return 8;
}

void CAnalyzer::LeaveMostPlausible(vector<CHasher>& vecPossibleWordforms)
{
//    if (vecPossibleWordforms == NULL || vecPossibleWordforms->size() <= 4)
    if (vecPossibleWordforms.size() <= 4)
    {
        return;
    }
    vector<CHasher> vec_plausible_wordforms;
    int i_plausibility_level = 8;
    while (vec_plausible_wordforms.size() < 4 && i_plausibility_level > 0)
    {
        for (int i_wf = vecPossibleWordforms.size() - 1; i_wf >= 0; --i_wf)
        {
            if (iPlausibility((vecPossibleWordforms)[i_wf]) == i_plausibility_level)
            {
                vec_plausible_wordforms.push_back((vecPossibleWordforms)[i_wf]);
            }
        }
        --i_plausibility_level;
    }
    vecPossibleWordforms.clear();
    vecPossibleWordforms = vec_plausible_wordforms;
}
