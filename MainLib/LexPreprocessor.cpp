#include "stdafx.h"
#include "LexPreprocessor.h"
#include "Dictionary.h"
#include "Lexeme.h"

void CT_LexPreprocessor::vInit()
{
    // Fill map_MainSymbol
    wchar_t * arr_strMainSymbol[ ] = { L"м", L"мо", L"ж", L"жо", L"с", L"со", L"мо-жо", L"мн.",
            L"мн. неод.", L"мн. одуш.", L"мн. _от_", L"п", L"мс", L"мс-п", L"мс-предик.", L"числ.", L"числ.-п", 
            L"св", L"нсв", L"св-нсв", L"н", L"предл.", L"союз", L"предик.", L"вводн.", L"сравн.", 
            L"част.", L"межд.", L"NULL"  };
    for (ET_MainSymbol eoMs = MS_START; eoMs < MS_END; ++eoMs)
    {
        map_MainSymbol[arr_strMainSymbol[eoMs]] = eoMs; 
    }
    vecDb.clear();
    iDbSize = 0;
}

ET_ReturnCode CT_LexPreprocessor::LoadDb(BSTR bstrPath)
{
    USES_CONVERSION;

    if (m_pDb)
    {
        delete m_pDb;
    }

    sDbPath = OLE2W(bstrPath);
    m_pDb = new CSqlite(sDbPath);
    if (m_pDb == NULL)
    {
        ERROR_LOG(L"Failed to instantiate CSqlite.");
        return E_FAIL;
    }

    // Create and initialize an IDictionary object to work with
    ET_ReturnCode hr = H_NO_ERROR;
    hr = sp_dict.CoCreateInstance(__uuidof(ZalDictionary));
    if (hr != H_NO_ERROR)
    {
        ERROR_LOG(L"CreateInstance failed for ZalDictionary.");
        return hr;
    }
    sp_dict->put_DbPath(bstrPath);

    // Load several tables for faster preprocessing
    vector<CEString> vecTablesToLoad;
    
    /*
    vecTablesToLoad.push_back(L"irregular_forms");
    vecTablesToLoad.push_back(L"irregular_stress");
    vecTablesToLoad.push_back(L"nonexistent_forms");
    vecTablesToLoad.push_back(L"homonyms");
    vecTablesToLoad.push_back(L"common_deviation");
    vecTablesToLoad.push_back(L"headword");
    vecTablesToLoad.push_back(L"stress");
    */
/*
    m_pDb->bLoadTables(vecTablesToLoad);
    
    m_pDb->bCreateIndex(L"irregular_forms", L"descriptor_id");
    m_pDb->bCreateIndex(L"irregular_stress", L"form_id");
    m_pDb->bCreateIndex(L"nonexistent_forms", L"descriptor_id");
    m_pDb->bCreateIndex(L"homonyms", L"headword_id");
    m_pDb->bCreateIndex(L"common_deviation", L"inflection_id");
    m_pDb->bCreateIndex(L"headword", L"id");
    m_pDb->bCreateIndex(L"stress", L"headword_id");
    */
    
    CEString str_query = L"Select * from endings_for_analysis";
    m_pDb->PrepareForSelect(str_query);
    int iCurSubtableId = -1, iPrevSubtableId = -1;
    CEndingsTable coEndingsTable;

    while (m_pDb->bGetRow())
    {
        stInflexion soInflexionTmp;
        m_pDb->GetData(1, iCurSubtableId);
        m_pDb->GetData(2, soInflexionTmp.sEnding);
        m_pDb->GetData(3, soInflexionTmp.iGram);
        m_pDb->GetData(4, soInflexionTmp.iStress);
        if (iCurSubtableId != iPrevSubtableId && iPrevSubtableId != -1)
        {
            CEndingsTable coCopy(coEndingsTable);
            vecDb.push_back(coCopy);
            coEndingsTable.Clear();
        }
        coEndingsTable.iAddEnding(soInflexionTmp.iGram, soInflexionTmp.sEnding, soInflexionTmp.iStress);
        iPrevSubtableId = iCurSubtableId;
    }
    if (coEndingsTable.m_vecEndings.size() > 0)
    {
        vecDb.push_back(coEndingsTable);
    }
    iDbSize = vecDb.size();
    return H_NO_ERROR;
}

ET_ReturnCode CT_LexPreprocessor::PrepareLexeme(__int64 llLexemeId, BOOL bStress)
{
    if (llLexemeId < 0)
    {
        ERROR_LOG (L"Bad lexeme id.");
        return E_FAIL;
    }
    ET_ReturnCode hr = PrepareLexemes(llLexemeId, llLexemeId, bStress);
    return hr;
}

ET_ReturnCode CT_LexPreprocessor::PrepareLexemes(__int64 llStart, __int64 llEnd, BOOL bStress)
{
    USES_CONVERSION;
    
    if (llStart < 0 || llEnd < 0 || m_pDb == NULL || sp_dict == NULL)
    {
        ERROR_LOG(L"Illegal argument.");
        return H_ERROR_INVALID_ARG;
    }

    if (llStart == 0 && llEnd == 0)
    {
        // Call the i_ClassifyStems function instead of preparing lexemes
        iClassifyStems();
        return H_NO_ERROR;
    }

    // TEST
    bStress = TRUE;

    CEString sStem;
    int iMaxStems = 0;
    const int MAX_NUM_STEMS = 9;
    long lItems = 0, lId = 0, lWordforms = 0;
    __int64 llLexemeId;
    ET_ReturnCode hr;
    wsmatch result;
    CEString *arr_sParadigm, **parr_sStems, sEnding;
    parr_sStems = new CEString*;
    *parr_sStems = new CEString[1];
    bool bMatch;

    m_pDb->BeginTransaction();
    
    for (llLexemeId = llStart; 
         llLexemeId <= llEnd; 
         ++llLexemeId) // for every lexeme within the range
    {
        int *arr_iSubtableId, *arr_iHash;
        parr_sStems = new CEString*;
        *parr_sStems = new CEString[1];
        int iWf = 0;
        CComPtr<ILexeme> sp_lex;

        // Find the lexeme in the db; generate its paradigm
        // (i_th element of arr_str_Paradigm has form arr_l_Hash[i])
        try
        {
            hr = sp_dict->GetLexeme((LONG)llLexemeId, &sp_lex);
            if (hr != H_NO_ERROR)
            {
                ERROR_LOG(L"GetLexeme() failed.");
                return hr;
            }
            CComBSTR bstrLemma;
            sp_lex->get_InitialForm(&bstrLemma);
            ET_PartOfSpeech eoPOS = POS_UNDEFINED;
            hr = sp_lex->get_PartOfSpeech(&eoPOS);
            // --------- Test ----------
            //if (eoPOS != POS_NOUN && eoPOS != POS_ADJ && eoPOS != POS_VERB && eoPOS != POS_PRONOUN_ADJ)
            //if (eoPOS == POS_NUM_ADJ)
            //{
            //    continue;
            //}
            // --------- /Test ----------
            BOOL bHasIrregularForms = 0;
            sp_lex->get_HasIrregularForms(&bHasIrregularForms);
            if (eoPOS == POS_VERB && bHasIrregularForms != 0)
            {
                iMaxStems = 5;
            }
            else if ((eoPOS == POS_NOUN && bHasIrregularForms == 0) ||
                     (eoPOS == POS_ADJ && bHasIrregularForms == 0) ||
                     eoPOS == POS_PRONOUN_ADJ ||
                     eoPOS == POS_NUM || eoPOS == POS_NUM_ADJ)
            {
                iMaxStems = 2;
            }
            else
            {
                iMaxStems = 3;
            }
            hr = sp_lex->GenerateWordForms();
            if (hr == H_ERROR_UNEXPECTED) // Just skip it
            {
                continue;
            }
            if (hr != H_NO_ERROR)
            {
                ERROR_LOG (L"GenerateWordForms() failed.");
                continue;
                //return h_r;
            }
            hr = sp_lex->get_Count(&lWordforms);
            if (lWordforms <= 0)
            {
                ERROR_LOG (L"l_Wordforms = " + CEString::sToString<long>(lWordforms) + L".");
                //return E_FAIL;
                continue;
            }

            arr_sParadigm = new CEString[lWordforms];
            arr_iHash = new int[lWordforms];
            for (long lItem = 1; lItem <= lWordforms; ++lItem)
            {
                CComVariant sp_disp = NULL;
                hr = sp_lex->get_Item(lItem, &sp_disp);
                if (hr != H_NO_ERROR)
                {
                    ERROR_LOG(L"get_Item failed.");
                    continue;
                    //return h_r;
                }
                CComQIPtr<IWordForm> sp_wf = sp_disp.pdispVal;
                CComBSTR bstrWordForm;
                CEString sWordForm;
                long lHash = -1;
                sp_wf->get_Wordform(&bstrWordForm);
                if (bstrWordForm.Length() <= 0)
                {
                    ERROR_LOG(L"Empty wordform!");
                }
                sp_wf->get_Hash(&lHash);
                arr_iHash[lItem - 1] = (int)lHash; // TODO: it should be of the same type throughout the whole project
                sWordForm = OLE2W(bstrWordForm);
                if (bStress)
                {
                    long lStressPos = -1;
                    long lStressCount = 0;
                    BOOL bPrimary = FALSE;
                    sp_wf->get_StressCount(&lStressCount);
                    for (long lStress = 0; lStress < lStressCount; ++lStress)
                    {
                        // Use only the first primary stress
                        sp_wf->GetStressPos(lStress, &lStressPos, &bPrimary);
                        if (bPrimary == TRUE)
                        {
                            break;
                        }
                        else
                        {
                            lStressPos = -1;
                        }
                    }
                    CEString sWfStressed = sInsertStress(lStressPos, OLE2W(bstrWordForm));
                    arr_sParadigm[lItem - 1] = sWfStressed;
                }
                else
                {
                    arr_sParadigm[lItem - 1] = OLE2W(bstrWordForm);
                }

                sp_disp.Clear();
            }
            
            // Create several gr_stems
            int iStems = 0;
            while (iStems <= 0 && iMaxStems <= MAX_NUM_STEMS)
            {
                delete[] *parr_sStems;
                delete parr_sStems;
                parr_sStems = new CEString*;
                *parr_sStems = new CEString[1];
                iStems = iLCP(arr_sParadigm, parr_sStems, (int)lWordforms, iMaxStems);
                if (iStems == 1 && (*parr_sStems)[0].uiLength() <= 0)
                {
                    iStems = 0;
                }
                iMaxStems += 2;
            }
            
            // Split every wordform into stem and ending,
            // then for every gr_stem create a table of endings usable with it
            CEndingsTable *arr_m_Endings;
            arr_m_Endings = new CEndingsTable[iStems];
            arr_iSubtableId = new int[iStems];
            for (iWf = 0; iWf < lWordforms; ++iWf)
            {
                for (int iS = 0; iS < iStems; ++iS)
                {
                    bMatch = arr_sParadigm[iWf].bRegexMatch((*parr_sStems)[iS] + L"(.*)");
                    if (bMatch == true)
                    {
                        sEnding = arr_sParadigm[iWf].sGetRegexMatch(0);
                        // TODO: Several stress positions
                        int iStressPos = iDeleteStress(sEnding);
                        if ((*parr_sStems)[iS].sSubstr((*parr_sStems)[iS].uiLength() - 1, 1) == L"<")
                        {
                            iStressPos = 0;
                        }
                        // Write the ending and its grammatical meaning 
                        // to the temporary "endings table" no. i_s
                        arr_m_Endings[iS].iAddEnding(arr_iHash[iWf],
                            sEnding, iStressPos);
                        break;
                    }
                }
            }
            // Write the "endings tables" to the db if necessary;
            // get their subtable_id's
            for (int iS = 0; iS < iStems; ++iS)
            {
                arr_iSubtableId[iS] = arr_m_Endings[iS].iDbWrite(vecDb);
            }
            // Add the stems with links to the corresponding subtables to the db
            vInsertStemsAndLinks(parr_sStems, OLE2W(bstrLemma), arr_iSubtableId, iStems, llLexemeId);

            // Delete the arrays
            for (int i_ = 0; i_ < iStems; ++i_)
            {
                arr_m_Endings[i_].~CEndingsTable();
            }
            delete[] arr_sParadigm;
            delete[] arr_iHash;
            delete[] arr_iSubtableId;
            delete[] (*parr_sStems);
            delete parr_sStems;

            // Commit transactions approximately every 200 entries
            if (llLexemeId % 200 == 0)
            {
                m_pDb->CommitTransaction();
                m_pDb->BeginTransaction();
            }
        }
        catch (...)
        {
            ERROR_LOG (L"Exception thrown.");
            //return E_FAIL;
            continue;
        }
    } // for (llLexemeId = llStart; llLexemeId <= llEnd; ++llLexemeId)...
    m_pDb->CommitTransaction();
    m_pDb->BeginTransaction();
    for (int iTable = iDbSize; iTable < vecDb.size(); ++iTable)
    {
        vecDb[iTable].iDbWrite(m_pDb, false, iTable);
    }
    m_pDb->CommitTransaction();
    iDbSize = vecDb.size();

    sp_dict->Clear();

    CoUninitialize();

    return H_NO_ERROR;

} // PrepareLexemes(__int64 ll_start, __int64 ll_end)

ET_ReturnCode CT_LexPreprocessor::PrepareDictionary()
{
    return E_NOTIMPL;
}

int CT_LexPreprocessor::iClassifyStems()
// For every endings subtable, looks for the stems usable with it and
// stores up to NUM_SFX their longest common suffixes in the database
{
    if (m_pDb == NULL)
    {
        return -1;
    }
    const int MIN_NUMBER_OF_STEMS = 70;
    const int NUM_SFX = 5;
    const int MAX_NUM_SFX = 24;

    CEString sQuery, sStem;
    CEString *arr_sStems;
    CEString **parr_sSfx;
    vector<CEString> vecStems;
    int iLastSubtable = 0, iStem;

    iLastSubtable = m_pDb->iLastID(L"endings_meta");
    for (int iSubtable = 0; iSubtable <= iLastSubtable; ++iSubtable)
    {
        vecStems.clear();
        CEString sFirstLemma = L"";
        int iCutRight = 0;
        CEString sLemmaEnding = L"";

        vLongStemsBySubtable(iSubtable, 2, vecStems, sFirstLemma);
        if (vecStems.size() < MIN_NUMBER_OF_STEMS)
        {
            continue;
        }

        // Find the longest common prefix of the first stem and the corresponding lemma
        CEString* arr_sStemAndLemma;
        CEString** parr_sPfx;
        arr_sStemAndLemma = new CEString[2];
        arr_sStemAndLemma[0] = vecStems[0];
        arr_sStemAndLemma[1] = sFirstLemma;
        parr_sPfx = new CEString*;
        *parr_sPfx = new CEString[1];
        int iPfx = iLCP(arr_sStemAndLemma, parr_sPfx, 2, 1);
        if (iPfx <= 0)
        {
            continue;
        }
        CEString sCommonPfx = (*parr_sPfx)[0];
        iCutRight = vecStems[0].uiLength() - sCommonPfx.uiLength();
        if (iCutRight >= 4)
        {
            continue;
        }
        sLemmaEnding = sFirstLemma.sSubstr(sCommonPfx.uiLength(), sFirstLemma.uiLength() - sCommonPfx.uiLength());

        // Find longest common suffixes of the stems found
        iStem = 0;
        arr_sStems = new CEString[vecStems.size()];
        parr_sSfx = new CEString*;
        *parr_sSfx = new CEString[1];
        for (vector<CEString>::iterator iterStems = vecStems.begin();
            iterStems != vecStems.end();
            ++iterStems, ++iStem)
        {
            // We reverse the stem so that i_LCP could find suffixes
            // instead of prefixes
//            reverse((*iter_stems).begin(), (*iter_stems).end());
            (*iterStems).Reverse();
            arr_sStems[iStem] = *iterStems;
        }

        // several attemps
        int iSfx = 0;
        int iMaxSfx = NUM_SFX;
        while (iSfx <= 0 && iMaxSfx <= MAX_NUM_SFX)
        {
            delete[] *parr_sSfx;
            delete parr_sSfx;
            parr_sSfx = new CEString*;
            *parr_sSfx = new CEString[1];
            iSfx = iLCP(arr_sStems, parr_sSfx, vecStems.size(), iMaxSfx);
            if (iSfx == 1 && (*parr_sSfx)[0].uiLength() <= 0)
            {
                iSfx = 0;
            }
            iMaxSfx += 2;
        }
        vInsertCommonSfx(parr_sSfx, iSfx, iSubtable, vecStems.size(), iCutRight, sLemmaEnding);

        delete[] arr_sStems;
        delete[] *parr_sSfx;
        delete parr_sSfx;

        // TEST
        //if (i_subtable > 100)
        //{
        //    break;
        //}
    }
    return 0;
}

CEString CT_LexPreprocessor::sInsertStress(int iLetter, CEString s_)
{
    if (iLetter >= 0 && iLetter < (int)(s_.uiLength()))
    {
        return s_.sSubstr(0, iLetter) + L"<" + s_.sSubstr(iLetter, s_.uiLength() - iLetter);
    }
    return s_;
}

void CT_LexPreprocessor::vDeleteRepeats(vector<CEString>& vecStrings)
{
    if (vecStrings.empty())
    {
        return;
    }
    CEString sPrevious = L"";
    sort(vecStrings.begin(), vecStrings.end());
    for (vector<CEString>::iterator iter = vecStrings.begin();
        iter != vecStrings.end();
        ++iter)
    {
        if (*iter == sPrevious)
        {
            vector<CEString>::iterator iterErase = iter;
            --iter;
            if (iter == vecStrings.begin())
            {
                sPrevious = L"";
            }
            else
            {
                --iter;
                sPrevious = *iter;
                ++iter;
            }
            vecStrings.erase(iterErase);
        }
        else
        {
            sPrevious = *iter;
        }
    }
}

int CT_LexPreprocessor::iDeleteStress(CEString& s_)
// Deletes the stress and returns the number of the letter it follows
{
    unsigned int uiStressPos = s_.uiFind(L"<");
    if (uiStressPos >= s_.uiLength())
    {
        return -1;
    }
    s_ = s_.sSubstr(0, uiStressPos) 
        + s_.sSubstr(uiStressPos + 1, s_.uiLength() - uiStressPos - 1);
    return (int)uiStressPos;
}

int CT_LexPreprocessor::iLCP(CEString* arr_sWords, CEString** parr_sPfx, int iWords, int iPfx)
{
    // Divides the input into up to i_pfx groups and finds their longest common prefixes.
    // They may differ in length, but don't exceed the length of the shortest word.
    if (arr_sWords == NULL || iWords <= 0 || iPfx < 0)
    {
        return -1;
    }

    bool bClassChanged = false, bTryAgain = false, *arr_bDisabled;
    int iLen, iMinLen = 65536, iNumPfx = 1, iNumPfxPrev = 1, iWFinished = 0, *arr_iPfxClass, *arr_iBasedOn;
    CEString *arr_sCurPfx, *arr_sCurSymb;
    CEString str_s;
    arr_sCurPfx = new CEString[iPfx];
    arr_sCurSymb = new CEString[iPfx];
    arr_sCurPfx[0] = arr_sCurSymb[0] = L"";
    arr_iPfxClass = new int[iWords]; // Stores current prefix class number of the i_th string or -1 if corresponding prefix is finished
    arr_iBasedOn = new int[iPfx]; // Temporarily stores the number of class i_th class is based on
    arr_bDisabled = new bool[iWords];
    memset(arr_iPfxClass, 0, iWords * sizeof(int)); // Initially, there is only one prefix class
    memset(arr_iBasedOn, 0, iPfx * sizeof(int));
    memset(arr_bDisabled, 0, iWords * sizeof(bool)); // All the words are enabled

    // Find minimal word length
    for (int i_ = 0; i_ < iWords; ++i_)
    {
        iLen = arr_sWords[i_].uiLength();
        if (iMinLen > iLen)
        {
            iMinLen = iLen;
        }
    }

    // Find the prefixes
    for (int iL = 0; iL < iMinLen; ++iL)
    {
        for (int iW = 0; iW < iWords; ++iW)
        {
            if (arr_bDisabled[iW] == true)
            {
                continue; // The word is disabled
            }
            str_s = arr_sWords[iW][iL]; // current symbol of the current word
            if (arr_sCurSymb[arr_iPfxClass[iW]].uiLength() == 0)
            {
                arr_sCurSymb[arr_iPfxClass[iW]] = str_s;
            }
            else
            {
                if (str_s != arr_sCurSymb[arr_iPfxClass[iW]])  // The current prefix needs to be split or finalized
                {
                    // Maybe we can just change its class to one of those recently introduced?
                    for (int i_ = arr_iPfxClass[iW] + 1; i_ < iNumPfx; ++i_)
                    {
                        if (arr_sCurSymb[i_] == str_s && arr_iBasedOn[i_] == arr_iPfxClass[iW])
                        {
                            arr_iPfxClass[iW] = i_;
                            bClassChanged = true;
                            break;
                        }
                    }
                    if (bClassChanged == true)
                    {
                        bClassChanged = false;
                        continue;
                    }

                    // if not:
                    if (++iNumPfx > iPfx)
                    {
                        // Finalize this prefix and initialize variables
                        for (int i_ = 0; i_ < iWords; ++i_)
                        {
                            if (arr_iBasedOn[arr_iPfxClass[i_]] == arr_iPfxClass[iW])
                            {
                                arr_bDisabled[i_] = true;
                                iWFinished++; // One more word finished
                            }
                            arr_iPfxClass[i_] = arr_iBasedOn[arr_iPfxClass[i_]];
                        }
                        for (int i_ = 0; i_ < iPfx; ++i_)
                        {
                            arr_sCurSymb[i_] = L"";
                        }
                        iNumPfx = iNumPfxPrev;
                        --iL;
                        bTryAgain = true; // Try again with the same symbol position, but more words disabled
                        break;
                    }
                    else
                    {
                        // Introduce a new prefix class
                        arr_sCurPfx[iNumPfx - 1] = arr_sCurPfx[arr_iPfxClass[iW]];
                        arr_sCurSymb[iNumPfx - 1] = str_s;
                        arr_iBasedOn[iNumPfx - 1] = arr_iPfxClass[iW];
                        arr_iPfxClass[iW] = iNumPfx - 1;
                    }
                }
            }
        }
        // Maybe we need to try the same position in the word again
        if (bTryAgain == true)
        {
            bTryAgain = false;
            if (iWFinished == iWords)
            {
                break;
            }
            else
            {
                continue;
            }
        }
        // If not, add recently obtained symbols to the prefixes
        for (int i_ = 0; i_ < iNumPfx; ++i_)
        {
            arr_sCurPfx[i_] += arr_sCurSymb[i_];
            arr_sCurSymb[i_] = L"";
        }
        // Let recently formed classes become independent
        for (int iPfxClass = 0; iPfxClass < iNumPfx; ++iPfxClass)
        {
            arr_iBasedOn[iPfxClass] = iPfxClass;
        }
        iNumPfxPrev = iNumPfx;
    }

    delete[] *parr_sPfx;
    *parr_sPfx = new CEString[iNumPfxPrev];
    for (int i_ = 0; i_ < iNumPfxPrev; ++i_)
    {
        (*parr_sPfx)[i_] = arr_sCurPfx[i_];
    }
    delete[] arr_sCurPfx;
    delete[] arr_sCurSymb;
    delete[] arr_iPfxClass;
    return iNumPfxPrev;
}
